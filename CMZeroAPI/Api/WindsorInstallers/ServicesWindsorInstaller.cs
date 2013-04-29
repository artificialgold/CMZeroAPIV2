using CMZero.API.DataAccess.Repositories;
using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Domain;

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Api.Infrastructure
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

            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
        }
    }

    public class DataAccessWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IApplicationRepository>().ImplementedBy<ApplicationRepository>().LifeStyle.Transient);
            container.Register(
                Component.For<ICollectionRepository>().ImplementedBy<CollectionRepository>().LifeStyle.Transient);
            container.Register(
                Component.For<IOrganisationRepository>().ImplementedBy<OrganisationRepository>().LifeStyle.Transient);
        }
    }
}