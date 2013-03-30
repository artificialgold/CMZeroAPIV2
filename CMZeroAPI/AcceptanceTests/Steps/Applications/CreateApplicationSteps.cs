using System;
using System.Net;
using System.Web.Http;

using AcceptanceTests.Helpers;
using AcceptanceTests.Helpers.Applications;
using AcceptanceTests.Helpers.Organisations;

using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.Organisations;

using Shouldly;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps.Applications
{
    [Binding]
    public class CreateApplicationSteps : StepBase
    {
        private ApplicationResource resource = new Api().Resource<ApplicationResource>();

        private const string ApplicationNameKey = "applicationName";

        private const string ApplicationIdKey = "applicationId";

        [Given(@"I create a valid application")]
        public void GivenICreateAValidApplication()
        {
            string name = string.Format("knownName{0}", DateTime.UtcNow.ToString("yyyyMMddSSmm"));
            string id = resource.NewApplicationWithSpecifiedName(name).Id;
            resource.GetApplication(id).Name.ShouldNotBe(null);
            Remember(id, ApplicationIdKey);
            Remember(name, ApplicationNameKey);
        }

        [Then(@"I should be able to get the application")]
        public void ThenIShouldBeAbleToGetTheApplication()
        {
            Application organisation = resource.GetApplication(Recall<string>(ApplicationIdKey));
            organisation.Id.ShouldBe(Recall<string>(ApplicationIdKey));
            organisation.Name.ShouldBe(Recall<string>(ApplicationNameKey));
        }
        [Given(@"I create an application without a name")]
        public void GivenICreateAnApplicationWithoutAName()
        {
            Remember(resource.NewApplicationWithUnspecifiedName());
        }

        [Given(@"I create an application with an organisationId that does not exist")]
        public void GivenICreateAnApplicationWithAnOrganisationIdThatDoesNotExist()
        {
            Remember(resource.NewApplicationWithNonExistentOrganisation());
        }

        [Then(@"I should get a BadResponseException")]
        public void ThenIShouldGetABadResponseException()
        {
            var exception = Recall<OrganisationDoesNotExistException>();
            exception.ShouldNotBe(null);
        }

    }
}