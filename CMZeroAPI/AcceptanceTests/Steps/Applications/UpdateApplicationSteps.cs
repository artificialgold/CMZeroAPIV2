using System;

using AcceptanceTests.Helpers;
using AcceptanceTests.Helpers.Applications;
using AcceptanceTests.Helpers.Organisations;

using Shouldly;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps.Applications
{
    [Binding]
    public class UpdateApplicationsSteps : StepBase
    {
        private readonly ApplicationResource resource = new Api().Resource<ApplicationResource>();

        private string updateName = "updatedName";

        private string updateStartKey = "updateStartKey";

        private string updateEndKey = "updateEndKey";

        private string applicationIdKey = "applicationIdKey";

        [Given(@"an existing application")]
        public void GivenAnExistingApplication()
        {
            string id = resource.NewApplicationWithSpecifiedName("preUpdate").Id;
            Remember(id, applicationIdKey);
        }

        [When(@"I update the application name with a valid name")]
        public void WhenIUpdateTheApplicationNameWithAValidName()
        {
            var application = resource.GetApplication(Recall<string>(applicationIdKey));
            application.Name = updateName;
            DateTime startUpdateTime = DateTime.UtcNow;
            resource.UpdateOrganisation(application);
            DateTime endUpdateTime = DateTime.UtcNow;
            Remember(startUpdateTime, updateStartKey);
            Remember(endUpdateTime, updateEndKey);
        }

        [When(@"I update the application name with no name")]
        public void WhenIUpdateTheApplicationNameWithNoName()
        {
            var application = resource.GetApplication(Recall<string>(applicationIdKey));
            var exception = resource.UpdateApplicationWithUnspecifiedName(application);
            Remember(exception);
        }


        [Then(@"the application should have the new name")]
        public void ThenTheApplicationShouldHaveTheNewName()
        {
            var organisation = resource.GetApplication(Recall<string>(applicationIdKey));
            organisation.Name.ShouldBe(updateName);
        }

        [Then(@"the application should have the new updated date")]
        public void ThenTheOrganisationShouldHaveTheNewUpdatedDate()
        {
            var application = resource.GetApplication(Recall<string>(applicationIdKey));
            application.Updated.ShouldBeGreaterThanOrEqualTo(Recall<DateTime>(updateStartKey));
            application.Updated.ShouldBeLessThan(Recall<DateTime>(updateEndKey).AddTicks(1));
        }
    }
}