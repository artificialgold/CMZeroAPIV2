using System;

using AcceptanceTests.Helpers;
using AcceptanceTests.Helpers.Organisations;

using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions.Organisations;
using Shouldly;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps.Organisations
{
    [Binding]
    public class CreateOrganisationSteps : StepBase
    {
        private const string OrganisationName = "organisationName";

        private const string OrganisationIdKey = "organisationId";
        private readonly OrganisationResource _resource = new Api().Resource<OrganisationResource>();

        [Given(@"I create a valid organisation")]
        public void GivenICreateAValidOrganisation()
        {
            string name = string.Format("knownName{0}", DateTime.UtcNow.ToString("yyyyMMddSSmm"));
            string id = _resource.NewOrganisationWithSpecifiedName(name).Id;
            _resource.GetOrganisation(id).Name.ShouldNotBe(null);
            Remember(id, OrganisationIdKey);
            Remember(name, OrganisationName);
        }

        [Then(@"I should be able to get the organisation")]
        public void ThenIShouldBeAbleToGetTheOrganisation()
        {
            Organisation organisation = _resource.GetOrganisation(Recall<string>(OrganisationIdKey));
            organisation.Id.ShouldBe(Recall<string>(OrganisationIdKey));
            organisation.Name.ShouldBe(Recall<string>(OrganisationName));
        }

        [Given(@"I create an organisation without a name")]
        public void GivenICreateAnOrganisationWithoutAName()
        {
            Remember(_resource.NewOrganisationWithUnspecifiedName());
        }

        [Given(@"I create an organisation with a name that already exists")]
        public void GivenICreateAnOrganisationWithANameThatAlreadyExists()
        {
            Remember(_resource.NewOrganisationWithExistingName());
        }

        [Then(@"I should get a OrganisationNameAlreadyExistsException")]
        public void ThenIShouldGetAOrganisationNameAlreadyExistsException()
        {
            var exception = Recall<OrganisationNameAlreadyExistsException>();
            exception.ShouldNotBe(null);
        }
    }
}