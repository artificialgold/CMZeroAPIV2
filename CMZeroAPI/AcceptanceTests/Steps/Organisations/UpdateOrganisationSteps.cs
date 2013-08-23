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
        private readonly OrganisationResource _resource = new Api().Resource<OrganisationResource>();
        private readonly string _updateName = "updatedName" + DateTime.Now.Ticks;
        private const string UpdateStartKey = "updateStartKey";
        private const string UpdateEndKey = "updateEndKey";
        private const string Organisationidkey = "organisationIdKey";

        [Given(@"an existing organisation")]
        public void GivenAnExistingOrganisation()
        {
            string id = _resource.NewOrganisationWithSpecifiedName("preUpdate" + DateTime.Now.Ticks).Id;
            Remember(id, Organisationidkey);
        }

        [When(@"I update the organisation name with a valid name")]
        public void WhenIUpdateTheOrganisationNameWithAValidName()
        {
            var organisation = _resource.GetOrganisation(Recall<string>(Organisationidkey));
            organisation.Name = _updateName;
            DateTime startUpdateTime = DateTime.UtcNow;
            _resource.UpdateOrganisation(organisation);
            DateTime endUpdateTime = DateTime.UtcNow;
            Remember(startUpdateTime, UpdateStartKey);
            Remember(endUpdateTime, UpdateEndKey);
        }

        [When(@"I update the organisaton name to an existing name")]
        public void WhenIUpdateTheOrganisatonNameToAnExistingName()
        {
            Remember(_resource.UpdateOrganisationWithExistingName());
        }

        [When(@"I update the organisation name with no name")]
        public void WhenIUpdateTheOrganisationNameWithNoName()
        {
            var organisation = _resource.GetOrganisation(Recall<string>(Organisationidkey));
            var exception = _resource.UpdateOrganisationWithUnspecifiedName(organisation);
            Remember(exception);
        }


        [Then(@"the organisation should have the new name")]
        public void ThenTheOrganisationShouldHaveTheNewName()
        {
            var organisation = _resource.GetOrganisation(Recall<string>(Organisationidkey));
            organisation.Name.ShouldBe(_updateName);
        }

        [Then(@"the organisation should have the new updated date")]
        public void ThenTheOrganisationShouldHaveTheNewUpdatedDate()
        {
            var organisation = _resource.GetOrganisation(Recall<string>(Organisationidkey));
            organisation.Updated.ShouldBeGreaterThanOrEqualTo(Recall<DateTime>(UpdateStartKey));
            organisation.Updated.ShouldBeLessThan(Recall<DateTime>(UpdateEndKey).AddTicks(1));
        }

        [When(@"I update an organisation that does not exist")]
        public void WhenIUpdateAnOrganisationThatDoesNotExist()
        {
            Remember(_resource.UpdateOrganisationThatDoesNotExist());
        }

        [Then(@"I should get an ItemNotFoundException")]
        public void ThenIShouldGetAnItemNotFoundException()
        {
            var result = Recall<ItemNotFoundException>();
            result.ShouldNotBe(null);
        }
    }
}