using CMZero.API.DataAccess.Repositories;
using CMZero.API.DataAccess.RepositoryInterfaces;

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Api.WindsorInstallers
{
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
            container.Register(
                Component.For<IContentAreaRepository>().ImplementedBy<ContentAreaRepository>().LifeStyle.Transient);
        }
    }
}