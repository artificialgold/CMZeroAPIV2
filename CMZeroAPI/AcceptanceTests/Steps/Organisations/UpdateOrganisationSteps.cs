using System;

using AcceptanceTests.Helpers;
using AcceptanceTests.Helpers.Organisations;

using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;

using Shouldly;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps.Organisations
{
    [Binding]
    public class UpdateOrganisationSteps : StepBase
    {
        private readonly OrganisationResource resource = new Api().Resource<OrganisationResource>();

        private string updateName = "updatedName";

        private string updateStartKey = "updateStartKey";

        private string updateEndKey = "updateEndKey";

        private string organisationidkey = "organisationIdKey";

        [Given(@"an existing organisation")]
        public void GivenAnExistingOrganisation()
        {
            string id = resource.NewOrganisationWithSpecifiedName("preUpdate").Id;
            Remember(id, organisationidkey);
        }

        [When(@"I update the organisation name with a valid name")]
        public void WhenIUpdateTheOrganisationNameWithAValidName()
        {
            var organisation = resource.GetOrganisation(Recall<string>(organisationidkey));
            organisation.Name = updateName;
            DateTime startUpdateTime = DateTime.UtcNow;
            resource.UpdateOrganisation(organisation);
            DateTime endUpdateTime = DateTime.UtcNow;
            Remember(startUpdateTime, updateStartKey);
            Remember(endUpdateTime, updateEndKey);
        }

        [When(@"I update the organisation name with no name")]
        public void WhenIUpdateTheOrganisationNameWithNoName()
        {
            var organisation = resource.GetOrganisation(Recall<string>(organisationidkey));
            var exception = resource.UpdateOrganisationWithUnspecifiedName(organisation);
            Remember(exception);
        }


        [Then(@"the organisation should have the new name")]
        public void ThenTheOrganisationShouldHaveTheNewName()
        {
            var organisation = resource.GetOrganisation(Recall<string>(organisationidkey));
            organisation.Name.ShouldBe(updateName);
        }

        [Then(@"the organisation should have the new updated date")]
        public void ThenTheOrganisationShouldHaveTheNewUpdatedDate()
        {
            var organisation = resource.GetOrganisation(Recall<string>(organisationidkey));
            organisation.Updated.ShouldBeGreaterThanOrEqualTo(Recall<DateTime>(updateStartKey));
            organisation.Updated.ShouldBeLessThan(Recall<DateTime>(updateEndKey).AddTicks(1));
        }

        [When(@"I update an organisation that does not exist")]
        public void WhenIUpdateAnOrganisationThatDoesNotExist()
        {
            Remember(resource.UpdateOrganisationThatDoesNotExist());
        }

        [Then(@"I should get an ItemNotFoundException")]
        public void ThenIShouldGetAnItemNotFoundException()
        {
            var result = Recall<ItemNotFoundException>();
            result.ShouldNotBe(null);
        }
    }
}