using System;
using System.Collections.Generic;

using AcceptanceTests.Helpers;
using AcceptanceTests.Helpers.Organisations;

using CMZero.API.Messages;

using Shouldly;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps.Organisations
{
    [Binding]
    public class GetOrganisationSteps : StepBase    
    {
        private readonly OrganisationResource resource = new Api().Resource<OrganisationResource>();

        [When(@"I request an existing organisation")]
        public void WhenIRequestAnExistingOrganisation()
        {
            string id = resource.NewOrganisation().Id;
            resource.GetOrganisation(id).Name.ShouldNotBe(null);
            Remember(id, "organisationId");
        }

        [When(@"I request a non-existing organisation")]
        public void WhenIRequestANon_ExistingOrganisation()
        {
            Remember(resource.GetOrganisationThatDoesNotExist());
        }
        
        [Then(@"organisation should be returned")]
        public void ThenOrganisationShouldBeReturned()
        {
            string id = Recall<string>("organisationId");
            Organisation result = resource.GetOrganisation(id);

            result.Name.ShouldNotBe(null);
            result.Id.ShouldBe(id);
            result.Created.ShouldNotBe(DateTime.MinValue);
            result.Updated.ShouldNotBe(DateTime.MinValue);
        }

        [When(@"I request all organisations")]
        public void WhenIRequestAllOrganisations()
        {
            Remember(resource.GetOrganisations());
        }

        [Then(@"I should get a list of organisations")]
        public void ThenIShouldGetAListOfOrganisations()
        {
            IEnumerable<Organisation> result = Recall<IEnumerable<Organisation>>();
        
            result.ShouldNotBeEmpty();
        }

    }
}
