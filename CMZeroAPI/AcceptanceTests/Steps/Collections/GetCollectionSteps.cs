using AcceptanceTests.Helpers;
using AcceptanceTests.Helpers.Collections;

using Shouldly;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps.Collections
{
    [Binding]
    public class GetCollectionSteps : StepBase
    {
        private CollectionResource resource = new Api().Resource<CollectionResource>();

        private const string ApplicationIdKey = "ApplicationIdKey";

        private const string CollectionIdKey = "collectionIdKey";

        [When(@"I request an existing collection")]
        public void WhenIRequestAnExistingCollection()
        {
            var collection = resource.NewCollection();
            Remember(collection.Id, CollectionIdKey);
            Remember(collection.ApplicationId, ApplicationIdKey);
        }

        [Then(@"the collection should be returned")]
        public void ThenTheCollectionShouldBeReturned()
        {
            var collectionId = Recall<string>(CollectionIdKey);
            var applicationId = Recall<string>(ApplicationIdKey);
            var collection = resource.GetCollection(collectionId, applicationId);
            collection.ApplicationId.ShouldBe(applicationId);
            collection.Id.ShouldBe(collectionId);
            collection.Name.ShouldNotBe(null);
        }

        [When(@"I request a non-existing collection")]
        public void WhenIRequestANon_ExistingCollection()
        {
            Remember(resource.GetCollectionThatDoesNotExist());
        }

    }
}