using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using EventCloud.Authorization;
using EventCloud.Events.Dtos;
using EventCloud.Repository.Entity;
using System.Reflection;

namespace EventCloud
{
    [DependsOn(
        typeof(EventCloudCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class EventCloudApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<EventCloudAuthorizationProvider>();

            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
            {
                cfg.CreateMap<Location, CreateEventLocation>()
                      .ForMember(evt => evt.Address, opt => opt.MapFrom(src => src.RawAddress))
                      .ForMember(evt => evt.PostCode, opt => opt.Ignore());
            });
        }

        public override void Initialize()
        {
            //IocManager.RegisterAssemblyByConvention(Assembly.Load("EventCloud.Repository"));
            IocManager.Register<EventCloud.Repository.IEventRepository, EventCloud.Repository.MongoEventRepository>(DependencyLifeStyle.Transient);

            var thisAssembly = typeof(EventCloudApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }
    }
}
