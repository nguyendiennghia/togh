using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using EventCloud.Authorization.Users;
using EventCloud.Events.Dtos;
using Microsoft.EntityFrameworkCore;
using IExtensionRepository = EventCloud.Repository.IEventRepository;
using ExtEvent = EventCloud.Repository.Entity.Event;
using ExtLocation = EventCloud.Repository.Entity.Location;
using MongoDB.Bson;

namespace EventCloud.Events
{
    [AbpAuthorize]
    public class EventAppService : EventCloudAppServiceBase, IEventAppService
    {
        private readonly IEventManager _eventManager;
        private readonly IRepository<Event, Guid> _eventRepository;
        private readonly IExtensionRepository _extension;

        public EventAppService(
            IEventManager eventManager,
            IRepository<Event, Guid> eventRepository,
            IExtensionRepository extension)
        {
            _eventManager = eventManager;
            _eventRepository = eventRepository;
            _extension = extension;
        }

        public async Task<ListResultDto<EventListDto>> GetListAsync(GetEventListInput input)
        {
            var events = await _eventRepository
                .GetAll()
                .Include(e => e.Registrations)
                .WhereIf(!input.IncludeCanceledEvents, e => !e.IsCancelled)
                .OrderByDescending(e => e.CreationTime)
                .Take(64)
                .ToListAsync();

            var ext = await _extension.GetByAsync();

            // TODO: Merge
            var dtos = events.MapTo<List<EventListDto>>();
            //var dtos = (from e in events
            //            select new
            //            {
            //                e.Title,
            //                e.Description,
            //                e.Date,
            //                e.IsCancelled,
            //                e.MaxRegistrationCount,
            //                e.Registrations
            //            }).ToList().MapTo<List<EventListDto>>();
            //var dtos = (from e in events select e).ToList().MapTo<List<EventListDto>>();

            return new ListResultDto<EventListDto>(dtos);
        }

        public async Task<EventDetailOutput> GetDetailAsync(EntityDto<Guid> input)
        {
            var @event = await _eventRepository
                .GetAll()
                .Include(e => e.Registrations)
                .ThenInclude(r => r.User)
                .Where(e => e.Id == input.Id)
                .FirstOrDefaultAsync();

            if (@event == null)
            {
                throw new UserFriendlyException("Could not found the event, maybe it's deleted.");
            }

            return @event.MapTo<EventDetailOutput>();
        }

        public async Task CreateAsync(CreateEventInput input)
        {
            var @event = Event.Create(AbpSession.GetTenantId(), input.Title, input.Date, input.Description, input.MaxRegistrationCount);

            await _eventManager.CreateAsync(@event);

            // TODO: Apply AOP -> all extension out of this app service class
            // TODO: Put conversion Guid <-> ObjectId in helper class
            //ObjectId.TryParse(@event.Id.ToString(), out ObjectId externalId);
            await _extension.SaveAsync(new ExtEvent
            {
                ExternalId = @event.Id,
                // TODO: Smash it to sucking automapper pls
                Location = new ExtLocation { Latitude = input.Location.Latitude, Longtitude = input.Location.Longitude, RawAddress = input.Location.Address }
            });
        }

        public async Task CancelAsync(EntityDto<Guid> input)
        {
            var @event = await _eventManager.GetAsync(input.Id);
            _eventManager.Cancel(@event);
        }

        public async Task<EventRegisterOutput> RegisterAsync(EntityDto<Guid> input)
        {
            var registration = await RegisterAndSaveAsync(
                await _eventManager.GetAsync(input.Id),
                await GetCurrentUserAsync()
                );

            return new EventRegisterOutput
            {
                RegistrationId = registration.Id
            };
        }

        public async Task CancelRegistrationAsync(EntityDto<Guid> input)
        {
            await _eventManager.CancelRegistrationAsync(
                await _eventManager.GetAsync(input.Id),
                await GetCurrentUserAsync()
                );
        }

        private async Task<EventRegistration> RegisterAndSaveAsync(Event @event, User user)
        {
            var registration = await _eventManager.RegisterAsync(@event, user);
            await CurrentUnitOfWork.SaveChangesAsync();
            return registration;
        }
    }
}
