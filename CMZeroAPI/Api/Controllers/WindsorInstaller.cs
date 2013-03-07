using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Api.Controllers
{
    public class WindsorInstaller:IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<OrganisationController>().ImplementedBy<OrganisationController>().LifeStyle.Transient);

        }
    }
}