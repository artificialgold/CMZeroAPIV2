using System;

using AcceptanceTests.Helpers.Organisations;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps
{
    [Binding]
    public class UpdateOrganisationSteps : StepBase
    {
        private string updateName = "updatedName";

        private DateTime updateStartKey;

        [Given(@"an existing organisation")]
        public void GivenAnExistingOrganisation()
        {
            OrganisationResource resource = new Api().Resource<OrganisationResource>();
            string id = resource.NewOrganisationWithSpecifiedName("preUpdate").Id;
            Remember<string>(id);
        }

        [When(@"I update the organisation name with a valid name")]
        public void WhenIUpdateTheOrganisationNameWithAValidName()
        {
            OrganisationResource resource = new Api().Resource<OrganisationResource>();
            var organisation = resource.GetOrganisation(Recall<string>());
            organisation.Name = updateName;
            DateTime startUpdateTime = DateTime.UtcNow;
            resource.UpdateOrganisation(organisation);
            DateTime endUpdateTime = DateTime.UtcNow;
            Remember<DateTime>(updateStartKey);
            Remember<DateTime>(updateEndKey);
        }

        [Then(@"the organisation should have the new name")]
        public void ThenTheOrganisationShouldHaveTheNewName()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the organisation should have the new updated date")]
        public void ThenTheOrganisationShouldHaveTheNewUpdatedDate()
        {
            ScenarioContext.Current.Pending();
        }
    }
}