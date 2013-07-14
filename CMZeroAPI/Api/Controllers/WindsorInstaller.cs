using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Api.Controllers
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<OrganisationController>().ImplementedBy<OrganisationController>().LifeStyle.Transient);

            container.Register(
                Component.For<ApplicationController>().ImplementedBy<ApplicationController>().LifeStyle.Transient);

            container.Register(
                Component.For<CollectionController>().ImplementedBy<CollectionController>().LifeStyle.Transient);

            container.Register(
                Component.For<ApplicationCollectionController>()
                         .ImplementedBy<ApplicationCollectionController>()
                         .LifeStyle.Transient);

            container.Register(
                Component.For<ContentAreaController>().ImplementedBy<ContentAreaController>().LifeStyle.Transient);
        }
    }
}