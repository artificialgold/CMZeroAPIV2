using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using AcceptanceTests.Helpers.Applications;
using AcceptanceTests.Helpers.Organisations;

using CMZero.API.ServiceAgent;

namespace AcceptanceTests.Helpers
{
    public class Api
    {
        private readonly IList<IResource> _knownResourceObjects;

        public Api()
        {
            _knownResourceObjects = new List<IResource> { new OrganisationResource(new OrganisationsServiceAgent(ConfigurationManager.AppSettings["BaseUri"])),
            new ApplicationResource(new ApplicationsServiceAgent(ConfigurationManager.AppSettings["BaseUri"]))};
        }

        public T Resource<T>() where T : IResource
        {
            try
            {
                return (T)_knownResourceObjects.First(s => s.GetType().Name == typeof(T).Name);
            }
            catch (Exception)
            {
                throw new ArgumentException(String.Format("Don't know how to make resource object of type '{0}'", typeof(T).Name));
            }
        }
    }
}