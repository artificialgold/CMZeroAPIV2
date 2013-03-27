using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Http;

using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.Organisations;
using CMZero.API.Messages.Responses;
using CMZero.API.Messages.Responses.Applications;
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
            PostApplicationResponse response =
                _applicationsServiceAgent.Post(
                    new Application
                        {
                            Active = true,
                            Name = name
                        });

            return response.Application;
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

        public Application GetApplication(string id)
        {
            var result = _applicationsServiceAgent.Get(id);

            if (result != null) return result.Application;

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
            return _applicationsServiceAgent.Get().Applications;
        }

        public Application UpdateOrganisation(Application application)
        {
            return _applicationsServiceAgent.Put(application).Application;
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

        public BadResponseException NewApplicationWithNonExistentOrganisation()
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
            catch (BadResponseException exception)
            {
                return exception;
            }

            return null;
        }
    }
}