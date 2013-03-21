using System;
using System.Collections.Generic;

using AcceptanceTests.Helpers;
using AcceptanceTests.Helpers.Applications;
using AcceptanceTests.Helpers.Organisations;

using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;

using Shouldly;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps.Applications
{
    [Binding]
    public class GetApplicationSteps : StepBase    
    {
        private readonly ApplicationResource resource = new Api().Resource<ApplicationResource>();

        private const string ApplicationIdKey = "applicationId";

        [When(@"I request an existing application")]
        public void WhenIRequestAnExistingApplication()
        {
            string id = resource.NewApplication().Id;
            resource.GetApplication(id).Name.ShouldNotBe(null);
            Remember(id, ApplicationIdKey);
        }

        [When(@"I request a non-existing application")]
        public void WhenIRequestANon_ExistingApplication()
        {
            Remember(resource.GetApplicationThatDoesNotExist());
        }
        
        [Then(@"the application should be returned")]
        public void ThenApplicationShouldBeReturned()
        {
            string id = Recall<string>(ApplicationIdKey);
            Application result = resource.GetApplication(id);

            result.Name.ShouldNotBe(null);
            result.Id.ShouldBe(id);
            result.Created.ShouldNotBe(DateTime.MinValue);
            result.Updated.ShouldNotBe(DateTime.MinValue);
        }

        [When(@"I request all applications")]
        public void WhenIRequestAllApplications()
        {
            Remember<IEnumerable<Application>>(resource.GetApplications());
        }

        [Then(@"I should get a list of applications")]
        public void ThenIShouldGetAListOfApplications()
        {
            IEnumerable<Application> result = Recall<IEnumerable<Application>>();
        
            result.ShouldNotBeEmpty();
        }

    }
}
