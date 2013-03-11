using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;

using Api.Filters;

using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Api.Infrastructure
{
    public class BootStrapper
    {
        public static readonly IWindsorContainer Container = new WindsorContainer();

        public static void Init(Assembly applicationAssembly)
        {
            InitContainer(applicationAssembly);
            InitTasks();
            InitFilters();
        }

        private static void InitFilters()
        {
            GlobalConfiguration.Configuration.Filters.Add(new ValidationActionFilter());
        }

        private static void InitContainer(Assembly applicationAssembly)
        {
            Container.Install(FromAssembly.Instance(applicationAssembly));
            
            GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(Container);
        }

        private static void InitTasks()
        {
            var bootstrapperTasks = Container.ResolveAll<IBootstrapperTask>();

            foreach (IBootstrapperTask bootstrapperTask in bootstrapperTasks)
            {
                bootstrapperTask.Execute();
            }
        }
    }
}