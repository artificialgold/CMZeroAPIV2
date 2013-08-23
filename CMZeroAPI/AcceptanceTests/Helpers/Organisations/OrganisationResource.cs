using System;
using System.Collections.Generic;
using System.Globalization;

using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.Organisations;
using CMZero.API.ServiceAgent;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Helpers.Organisations
{
    public class OrganisationResource : IResource
    {
        readonly IOrganisationsServiceAgent _organisationsServiceAgent;

        public OrganisationResource(IOrganisationsServiceAgent organisationsServiceAgent)
        {
            _organisationsServiceAgent = organisationsServiceAgent;
        }

        public Organisation NewOrganisation()
        {
            return NewOrganisationWithSpecifiedName(
                   string.Format("Test{0}{1}", DateTime.Now.ToString(CultureInfo.InvariantCulture), DateTime.Now.Ticks));
        }

        public Organisation NewOrganisationWithSpecifiedName(string name)
        {
            Organisation response =
                _organisationsServiceAgent.Post(
                    new Organisation
                    {
                        Active = true,
                        Name = name
                    });

            return response;
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

        public BadRequestException UpdateOrganisationWithUnspecifiedName(Organisation organisation)
        {
            try
            {
                organisation.Name = string.Empty;
                _organisationsServiceAgent.Put(organisation);
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

            if (result != null) return result;

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
            return _organisationsServiceAgent.Get();
        }

        public Organisation UpdateOrganisation(Organisation organisation)
        {
            return _organisationsServiceAgent.Put(organisation);
        }

        public ItemNotFoundException UpdateOrganisationThatDoesNotExist()
        {
            try
            {
                _organisationsServiceAgent.Put(
                    new Organisation { Name = "test", Id = "newId" + DateTime.Now.ToBinary() });
            }
            catch (ItemNotFoundException ex)
            {
                return ex;
            }

            throw new SpecFlowException("Expected ItemNotFoundException was not caught");
        }

        public OrganisationNameAlreadyExistsException NewOrganisationWithExistingName()
        {
            var organisationNameThatWillBeUsedTwice = "orgisationNameThatExists" +DateTime.Now.Ticks;
            NewOrganisationWithSpecifiedName(organisationNameThatWillBeUsedTwice);

            try
            {
                NewOrganisationWithSpecifiedName(organisationNameThatWillBeUsedTwice);
            }
            catch (OrganisationNameAlreadyExistsException ex)
            {
                return ex;
            }

            throw new SpecFlowException("Expected OrganisationNameAlreadyExistsException was not caught");
        }

        public OrganisationNameAlreadyExistsException UpdateOrganisationWithExistingName()
        {
            string organisationNameToDuplicate = "organisationNameToDuplicate" + DateTime.Now.Ticks;
            NewOrganisationWithSpecifiedName(organisationNameToDuplicate);
            var organisationToUpdate = NewOrganisation();
            organisationToUpdate.Name = organisationNameToDuplicate;

            try
            {
                _organisationsServiceAgent.Put(organisationToUpdate);
            }
            catch(OrganisationNameAlreadyExistsException ex)
            {
                return ex;
            }

            throw new SpecFlowException("Expected OrganisationNameAlreadyExistsException was not caught");
        }
    }
}
