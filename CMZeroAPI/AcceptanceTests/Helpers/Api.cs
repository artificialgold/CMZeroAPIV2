using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using AcceptanceTests.Helpers.Applications;
using AcceptanceTests.Helpers.Collections;
using AcceptanceTests.Helpers.ContentAreas;
using AcceptanceTests.Helpers.Organisations;

using CMZero.API.ServiceAgent;

namespace AcceptanceTests.Helpers
{
    public class Api
    {
        private readonly IList<IResource> _knownResourceObjects;

        public Api()
        {
            string baseUri = ConfigurationManager.AppSettings["BaseUri"];
            _knownResourceObjects = new List<IResource> { 
                new OrganisationResource(new OrganisationsServiceAgent(baseUri)),
            new ApplicationResource(new ApplicationsServiceAgent(baseUri)),
            new CollectionResource(new CollectionsServiceAgent(baseUri)),
            new ContentAreaResource(new ContentAreasServiceAgent(baseUri), new ApplicationsServiceAgent(baseUri))};
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