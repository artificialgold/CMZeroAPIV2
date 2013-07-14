using CMZero.API.Domain;
using CMZero.API.Domain.ApiKey;

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Api.WindsorInstallers
{
    public class ServicesWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IApplicationService>().ImplementedBy<ApplicationService>().LifeStyle.Transient);
            container.Register(
                Component.For<ICollectionService>().ImplementedBy<CollectionService>().LifeStyle.Transient);
            container.Register(
                Component.For<IOrganisationService>().ImplementedBy<OrganisationService>().LifeStyle.Transient);
            container.Register(
                Component.For<IContentAreaService>().ImplementedBy<ContentAreaService>().LifeStyle.Transient);
            container.Register(
                Component.For<IApiKeyCreator>().ImplementedBy<ApiKeyCreator>().LifeStyle.Transient);

            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
        }
    }
}