using AcceptanceTests.Helpers;
using AcceptanceTests.Helpers.ContentAreas;

using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions.Collections;
using CMZero.API.Messages.Exceptions.ContentAreas;

using Shouldly;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps.ContentAreas
{
    [Binding]
    public class CreateContentAreaSteps : StepBase
    {
        private ContentAreaResource resource = new Api().Resource<ContentAreaResource>();

        private string contentAreaKey = "contentAreaKey";

        [When(@"I create a content area for an existing collection")]
        public void WhenICreateAContentAreaForAnExistingCollection()
        {
            Remember(resource.NewContentArea(), contentAreaKey);
        }

        [Then(@"I should be able to get the content area")]
        public void ThenIShouldBeAbleToGetTheContentArea()
        {
            var contentAreaThatWasCreated = Recall<ContentArea>(contentAreaKey);

            var contentArea = resource.GetContentArea(contentAreaThatWasCreated.Id);

            contentArea.Name.ShouldBe(contentAreaThatWasCreated.Name);
            contentArea.Id.ShouldBe(contentAreaThatWasCreated.Id);
        }

        [When(@"I create a content area with a name that already exists for an existing collection")]
        public void WhenICreateAContentAreaWithANameThatAlreadyExistsForAnExistingCollection()
        {
            Remember(resource.NewContentAreaWithExistingName());
        }

        [Then(@"I should get a ContentAreaNameAlreadyExistsInCollectionException")]
        public void ThenIShouldGetAContentAreaNameAlreadyExistsInCollectionException()
        {
            var result = Recall<ContentAreaNameAlreadyExistsInCollectionException>();
            result.ShouldNotBe(null);
        }

        [When(@"I create a content area for a collection that does not exist")]
        public void WhenICreateAContentAreaForACollectionThatDoesNotExist()
        {
            Remember(resource.NewContentAreaForCollectionThatDoesNotExist());
        }

        [Then(@"I should get a CollectionIdNotValidException")]
        public void ThenIShouldGetACollectionIdNotValidException()
        {
            var result = Recall<CollectionIdNotValidException>();
            result.ShouldNotBe(null);
        }

        [When(@"I create a content area for an existing collection and specify a different applicationId")]
        public void WhenICreateAContentAreaForAnExistingCollectionAndSpecifyADifferentApplicationId()
        {
            Remember(resource.NewContentAreaWithInvalidApplicationId());
        }

        [Then(@"I should get a CollectionIdNotPartOfApplicationException")]
        public void ThenIShouldGetACollectionIdNotPartOfApplicationException()
        {
            Recall<CollectionIdNotPartOfApplicationException>();
        }

        [When(@"I create a content area for an existing collection and do not specify an applicationId")]
        public void WhenICreateAContentAreaForAnExistingCollectionAndDoNotSpecifyAnApplicationId()
        {
            Remember(resource.NewContentAreaWithBlankApplicationId());
        }

    }
}