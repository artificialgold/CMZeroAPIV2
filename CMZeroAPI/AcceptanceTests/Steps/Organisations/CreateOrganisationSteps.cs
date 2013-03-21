using System;

using AcceptanceTests.Helpers;
using AcceptanceTests.Helpers.Organisations;

using CMZero.API.Messages;

using Shouldly;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps.Organisations
{
    [Binding]
    public class CreateOrganisationSteps : StepBase
    {
        private const string OrganisationName = "organisationName";

        private const string OrganisationIdKey = "organisationId";

        [Given(@"I create a valid organisation")]
        public void GivenICreateAValidOrganisation()
        {
            OrganisationResource resource = new Api().Resource<OrganisationResource>();
            string name = string.Format("knownName{0}", DateTime.UtcNow.ToString("yyyyMMddSSmm"));
            string id = resource.NewOrganisationWithSpecifiedName(name).Id;
            resource.GetOrganisation(id).Name.ShouldNotBe(null);
            Remember(id, OrganisationIdKey);
            Remember(name, OrganisationName);
        }

        [Then(@"I should be able to get the organisation")]
        public void ThenIShouldBeAbleToGetTheOrganisation()
        {
            OrganisationResource resource = new Api().Resource<OrganisationResource>();
            Organisation organisation = resource.GetOrganisation(Recall<string>(OrganisationIdKey));
            organisation.Id.ShouldBe(Recall<string>(OrganisationIdKey));
            organisation.Name.ShouldBe(Recall<string>(OrganisationName));
        }

        [Given(@"I create an organisation without a name")]
        public void GivenICreateAnOrganisationWithoutAName()
        {
            OrganisationResource resource = new Api().Resource<OrganisationResource>();
            Remember(resource.NewOrganisationWithUnspecifiedName());
        }
    }
}