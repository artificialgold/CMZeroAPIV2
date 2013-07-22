using System.Collections.Generic;
using AcceptanceTests.Helpers;
using AcceptanceTests.Helpers.Collections;
using CMZero.API.Messages;
using Shouldly;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps.Collections
{
    [Binding]
    public class GetCollectionSteps : StepBase
    {
        private readonly CollectionResource _resource = new Api().Resource<CollectionResource>();

        private const string ApplicationIdKey = "ApplicationIdKey";

        private const string CollectionIdKey = "collectionIdKey";

        [When(@"I request an existing collection")]
        public void WhenIRequestAnExistingCollection()
        {
            var collection = _resource.NewCollection();
            Remember(collection.Id, CollectionIdKey);
            Remember(collection.ApplicationId, ApplicationIdKey);
        }

        [Then(@"the collection should be returned")]
        public void ThenTheCollectionShouldBeReturned()
        {
            var collectionId = Recall<string>(CollectionIdKey);
            var applicationId = Recall<string>(ApplicationIdKey);
            var collection = _resource.GetCollection(collectionId, applicationId);
            collection.ApplicationId.ShouldBe(applicationId);
            collection.Id.ShouldBe(collectionId);
            collection.Name.ShouldNotBe(null);
        }

        [When(@"I request a non-existing collection")]
        public void WhenIRequestANon_ExistingCollection()
        {
            Remember(_resource.GetCollectionThatDoesNotExist());
        }

        [When(@"I request collections for a valid apikey")]
        public void WhenIRequestCollectionsForAValidApikey()
        {
            Remember(_resource.GetCollectionsForValidApiKey());
        }

        [Then(@"the collections should be returned")]
        public void ThenTheCollectionsShouldBeReturned()
        {
            var result = Recall<IList<Collection>>();

            result.ShouldNotBe(null);
            result.Count.ShouldBe(2);
        }

        [When(@"I request collections for an invalid apikey")]
        public void WhenIRequestCollectionsForAnInvalidApikey()
        {
            Remember(_resource.GetCollectionsForInvalidApiKey());
        }
    }
}