using System;

using AcceptanceTests.Helpers;
using AcceptanceTests.Helpers.Applications;
using AcceptanceTests.Helpers.Organisations;

using CMZero.API.Messages;

using Shouldly;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps.Applications
{
    [Binding]
    public class CreateApplicationSteps : StepBase
    {
        private const string ApplicationName = "applicationName";

        private const string ApplicationIdKey = "applicationId";

        [Given(@"I create a valid application")]
        public void GivenICreateAValidApplication()
        {
            ApplicationResource resource = new Api().Resource<ApplicationResource>();
            string name = string.Format("knownName{0}", DateTime.UtcNow.ToString("yyyyMMddSSmm"));
            string id = resource.NewApplicationWithSpecifiedName(name).Id;
            resource.GetApplication(id).Name.ShouldNotBe(null);
            Remember(id, ApplicationIdKey);
            Remember(name, ApplicationName);
        }

        [Then(@"I should be able to get the application")]
        public void ThenIShouldBeAbleToGetTheApplication()
        {
          ApplicationResource resource = new Api().Resource<ApplicationResource>();
            Application organisation = resource.GetApplication(Recall<string>(ApplicationIdKey));
            organisation.Id.ShouldBe(Recall<string>(ApplicationIdKey));
            organisation.Name.ShouldBe(Recall<string>(ApplicationName));
        }


        [Given(@"I create an application without a name")]
        public void GivenICreateAnApplicationWithoutAName()
        {
            ApplicationResource resource = new Api().Resource<ApplicationResource>();
            Remember(resource.NewApplicationWithUnspecifiedName());
        }
    }
}