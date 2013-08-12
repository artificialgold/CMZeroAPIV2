using System;
using System.Collections.Generic;
using System.Globalization;

using AcceptanceTests.Helpers.Organisations;

using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.API.Messages.Exceptions.Organisations;
using CMZero.API.ServiceAgent;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Helpers.Applications
{
    public class ApplicationResource : IResource
    {
        IApplicationsServiceAgent _applicationsServiceAgent;

        public ApplicationResource(IApplicationsServiceAgent applicationsServiceAgent)
        {
            _applicationsServiceAgent = applicationsServiceAgent;
        }

        public Application NewApplication()
        {
            return NewApplicationWithSpecifiedName(
                string.Format("Test{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture)));
        }

        public Application NewApplicationWithSpecifiedName(string name)
        {
            OrganisationResource resource = new Api().Resource<OrganisationResource>();
            string organisationId = resource.NewOrganisation().Id;

            Application response =
                _applicationsServiceAgent.Post(
                    new Application
                        {
                            Active = true,
                            Name = name,
                            OrganisationId = organisationId
                        });

            return response;
        }

        public BadRequestException NewApplicationWithUnspecifiedName()
        {
            try
            {
                _applicationsServiceAgent.Post(new Application { Active = true, Name = null });
            }
            catch (BadRequestException ex)
            {
                return ex;
            }

            return null;
        }

        public ApplicationNameAlreadyExistsException NewApplicationWithExistingNameInOrganisation()
        {
            var organisationResource = new Api().Resource<OrganisationResource>();
            var organisationId = organisationResource.NewOrganisation().Id;

            try
            {
                const string alreadyExistingName = "nameThatWillAlreadyExist";
                _applicationsServiceAgent.Post(new Application {Active = true, Name = alreadyExistingName, OrganisationId = organisationId});
                _applicationsServiceAgent.Post(new Application {Active = true, Name = alreadyExistingName, OrganisationId = organisationId});
            }
            catch (ApplicationNameAlreadyExistsException ex)
            {
                return ex;
            }

            throw new SpecFlowException();
        }


        public Application GetApplication(string id)
        {
            var result = _applicationsServiceAgent.Get(id);

            if (result != null) return result;

            return null;
        }

        public ItemNotFoundException GetApplicationThatDoesNotExist()
        {
            try
            {
                _applicationsServiceAgent.Get("IDoNotExist");
            }
            catch (ItemNotFoundException ex)
            {
                return ex;
            }

            throw new SpecFlowException("Expected HttpResponseException not thrown");
        }

        public IEnumerable<Application> GetApplications()
        {
            return _applicationsServiceAgent.Get();
        }

        public Application UpdateApplication(Application application)
        {
            return _applicationsServiceAgent.Put(application);
        }

        public BadRequestException UpdateApplicationWithUnspecifiedName(Application application)
        {
            try
            {
                application.Name = string.Empty;
                _applicationsServiceAgent.Put(application);
            }
            catch (BadRequestException ex)
            {
                return ex;
            }

            return null;
        }

        public OrganisationDoesNotExistException NewApplicationWithNonExistentOrganisation()
        {
            try
            {
                Application application = new Application
                {
                    Name = "validName",
                    OrganisationId = "I do not exist"
                };
                _applicationsServiceAgent.Post(application);
            }
            catch (OrganisationDoesNotExistException exception)
            {
                return exception;
            }

            return null;
        }

        public OrganisationIdNotValidException UpdateApplicationWithDifferentOrganisationId(Application application)
        {
            try
            {
                application.OrganisationId = "different";
                _applicationsServiceAgent.Put(application);
            }
            catch (OrganisationIdNotValidException ex)
            {
                return ex;
            }

            throw new SpecFlowException("Expected OrganisationIdNotValidException not caught");
        }

        public ItemNotFoundException UpdateApplicationThatDoesNotExist()
        {
            try
            {
                _applicationsServiceAgent.Put(new Application { Id = "khjsassd", Name = "madeUp" });
            }
            catch (ItemNotFoundException ex)
            {
                return ex;
            }

            throw new SpecFlowException("Expected ItemNotFoundException not caught");
        }

        public OrganisationIdNotValidException GetApplicationsForOrganisationThatDoesNotExist()
        {
            try
            {
                _applicationsServiceAgent.GetByOrganisation("organisationIdThatDoesNoteExist");
            }
            catch(OrganisationIdNotValidException ex)
            {
                return ex;
            }

            throw new SpecFlowException("Expected OrganisationIdNotValidException was not caught");
        }

        public IEnumerable<Application> GetApplicationsForOrganisationThatExists()
        {
            var application = NewApplication();

            return _applicationsServiceAgent.GetByOrganisation(application.OrganisationId);
        }

        [Then(@"I should get a ApplicationNameAlreadyExistsException")]
        public void ThenIShouldGetAApplicationNameAlreadyExistsException()
        {

        }

    }
}