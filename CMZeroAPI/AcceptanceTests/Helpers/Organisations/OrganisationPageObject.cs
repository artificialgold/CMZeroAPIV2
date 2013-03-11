using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Http;

using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Responses;
using CMZero.API.ServiceAgent;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Helpers.Organisations
{
    public class OrganisationResource : IResource
    {
        IOrganisationsServiceAgent _organisationsServiceAgent;

        public OrganisationResource(IOrganisationsServiceAgent organisationsServiceAgent)
        {
            _organisationsServiceAgent = organisationsServiceAgent;
        }

        public Organisation NewOrganisation()
        {
            return NewOrganisationWithSpecifiedName(
                   string.Format("Test{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture)));
        }

        public Organisation NewOrganisationWithSpecifiedName(string name)
        {
            PostOrganisationResponse response =
                _organisationsServiceAgent.Post(
                    new Organisation
                    {
                        Active = true,
                        Name = name
                    });

            return response.Organisation;
        }

        public BadRequestException NewOrganisationWithUnspecifiedName()
        {
            try
            {
                _organisationsServiceAgent.Post(new Organisation { Active = true, Name = null });
            }
            catch (BadRequestException ex)
            {
                return ex;
            }

            return null;
        }

        public Organisation GetOrganisation(string id)
        {
            var result = _organisationsServiceAgent.Get(id);

            if (result != null) return result.Organisation;

            return null;
        }

        public ItemNotFoundException GetOrganisationThatDoesNotExist()
        {
            try
            {
                _organisationsServiceAgent.Get("IDoNotExist");
            }
            catch (ItemNotFoundException ex)
            {
                return ex;
            }

            throw new SpecFlowException("Expected HttpResponseException not thrown");
        }

        public IEnumerable<Organisation> GetOrganisations()
        {
            return _organisationsServiceAgent.Get().Organisations;
        }

        public Organisation UpdateOrganisation(Organisation organisation)
        {
            return _organisationsServiceAgent.Put(organisation).Organisation;
        }
    }

    public class Api
    {
        private readonly IList<IResource> _knownResourceObjects;

        public Api()
        {
            _knownResourceObjects = new List<IResource> { new OrganisationResource(new OrganisationsServiceAgent(ConfigurationManager.AppSettings["BaseUri"])) };
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

    public interface IResource
    {
    }
}
