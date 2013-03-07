using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Web.Http;

using AcceptanceTests.Helpers.Organisations;

using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;

using Shouldly;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps
{
    [Binding]
    public class GetOrganisationSteps : StepBase    
    {
        [When(@"I request an existing organisation")]
        public void WhenIRequestAnExistingOrganisation()
        {
            OrganisationResource resource = new Api().Resource<OrganisationResource>();
            string id = resource.NewOrganisation().Id;
            resource.GetOrganisation(id).Name.ShouldNotBe(null);
            Remember<string>(id, "organisationId");
        }

        [When(@"I request a non-existing organisation")]
        public void WhenIRequestANon_ExistingOrganisation()
        {
            OrganisationResource resource = new Api().Resource<OrganisationResource>();
            Remember(resource.GetOrganisationThatDoesNotExist());
        }
        
        [Then(@"organisation should be returned")]
        public void ThenOrganisationShouldBeReturned()
        {
            OrganisationResource resource = new Api().Resource<OrganisationResource>();
            string id = Recall<string>("organisationId");
            Organisation result = resource.GetOrganisation(id);

            result.Name.ShouldNotBe(null);
            result.Id.ShouldBe(id);
            result.Created.ShouldNotBe(DateTime.MinValue);
            result.Updated.ShouldNotBe(DateTime.MinValue);
        }

        [Then(@"not found exception should be returned")]
        public void ThenNotFoundExceptionShouldBeReturn()
        {
            OrganisationResource resource = new Api().Resource<OrganisationResource>();
            ItemNotFoundException result = Recall<ItemNotFoundException>();

            result.ShouldNotBe(null);
        }

        [When(@"I request all organisations")]
        public void WhenIRequestAllOrganisations()
        {
            OrganisationResource resource = new Api().Resource<OrganisationResource>();
            Remember<IEnumerable<Organisation>>(resource.GetOrganisations());
        }

        [Then(@"I should get a list of organisations")]
        public void ThenIShouldGetAListOfOrganisations()
        {
            OrganisationResource resource = new Api().Resource<OrganisationResource>();
            IEnumerable<Organisation> result = Recall<IEnumerable<Organisation>>();
        
            result.ShouldNotBeEmpty();
        }

    }
}
