using System;

using AcceptanceTests.Helpers;
using AcceptanceTests.Helpers.Collections;

using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.API.Messages.Exceptions.Collections;

using Shouldly;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps.Collections
{
    [Binding]
    public class CreateCollectionSteps : StepBase
    {
        private CollectionResource resource = new Api().Resource<CollectionResource>();

        private const string ApplicationIdKey = "ApplicationIdKey";
        private const string CollectionNameKey = "collectionNameKey";
        private const string CollectionIdKey = "collectionIdKey";

        [When(@"I post a valid collection")]
        public void WhenIPostAValidCollection()
        {
            string name = string.Format("knownName{0}", DateTime.UtcNow.ToString("yyyyMMddSSmm"));
            var collection = resource.NewCollectionWithSpecifiedName(name);
            var id = collection.Id;
            var applicationId = collection.ApplicationId;
            resource.GetCollection(id, applicationId).Name.ShouldNotBe(null);
            Remember(id, CollectionIdKey);
            Remember(applicationId, ApplicationIdKey);
            Remember(name, CollectionNameKey);
        }

        [Then(@"I should be able to retrieve the collection")]
        public void ThenIShouldBeAbleToRetrieveTheCollection()
        {
            Collection collection = resource.GetCollection(Recall<string>(CollectionIdKey), Recall<string>(ApplicationIdKey));
            collection.Id.ShouldBe(Recall<string>(CollectionIdKey));
            collection.ApplicationId.ShouldBe(Recall<string>(ApplicationIdKey));
            collection.Name.ShouldBe(Recall<string>(CollectionNameKey));
        }

        [When(@"I post a collection with no name")]
        public void WhenIPostACollectionWithNoName()
        {
            Remember(resource.NewCollectionWithUnspecifiedName());
        }

        [When(@"I post a collection with applicationId not for the same organisationId")]
        public void WhenIPostACollectionWithApplicationIdNotForTheSameOrganisationId()
        {
            Remember(resource.NewCollectionWithApplicationIdNotPartOfOrganisationId());
        }

        [Then(@"I should get a ApplicationNotInOrganisationException")]
        public void ThenIShouldGetAApplicationNotInOrganisationException()
        {
            var result = Recall<ApplicationIdNotPartOfOrganisationException>();
            result.ShouldNotBe(null);
        }

        [When(@"I post a collection with no applicationId")]
        public void WhenIPostACollectionWithNoApplicationId()
        {
            Remember(resource.NewCollectionWithNoApplicationId());
        }

        [When(@"I post a collection with a non-existent organisationId")]
        public void WhenIPostACollectionWithANon_ExistentOrganisationId()
        {
            Remember(resource.NewCollectionWithNonExistentOrganisationId());
        }


        [When(@"I post a collection with no organisationId")]
        public void WhenIPostACollectionWithNoOrganisationId()
        {
           Remember(resource.NewCollectionWithNoOrganisationId());
        }

        [When(@"I post a collection with existing name in application")]
        public void WhenIPostACollectionWithExistingNameInApplication()
        {
            Remember(resource.NewCollectionWithExistingNameInApplication());
        }

        [Then(@"I should get a CollectionNameAlreadyExistsException")]
        public void ThenIShouldGetACollectionNameAlreadyExistsException()
        {
            var result = Recall<CollectionNameAlreadyExistsException>();
            result.ShouldNotBe(null);
        }
    }
}