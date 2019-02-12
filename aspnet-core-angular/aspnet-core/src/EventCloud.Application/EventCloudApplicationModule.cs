using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.ModelBuilder;
using Castle.MicroKernel.Registration;
using EventCloud.Authorization;
using EventCloud.Events;
using EventCloud.Events.Dtos;
using EventCloud.Interceptor;
using EventCloud.Repository.Entity;
using System.ComponentModel;
using System.Linq;
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
            IocManager.Register<Repository.IEventRepository, Repository.MongoEventRepository>(DependencyLifeStyle.Transient);

            var thisAssembly = typeof(EventCloudApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );

            //IocManager.IocContainer.Register(
            //    Component.For<IEventAppService>().ImplementedBy<EventAppService>()
            //        .Interceptors(typeof(MongoEventInterceptor)).LifeStyle.Transient
            //    );

            IocManager.IocContainer.Kernel.ComponentModelBuilder.AddContributor(new ContributeComponentConstruct());
        }
    }

    public class ContributeComponentConstruct : IContributeComponentModelConstruction
    {
        public void ProcessModel(IKernel kernel, ComponentModel model)
        {
            if (model.Services.Any(s => s == typeof(IEventAppService)))
            {
                model.Interceptors.Add(InterceptorReference.ForType<MongoEventInterceptor>());
            }
        }
    }
}
