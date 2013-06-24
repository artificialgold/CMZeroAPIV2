using System.Collections.Generic;
using System.Linq;

using AcceptanceTests.Helpers;
using AcceptanceTests.Helpers.ContentAreas;

using CMZero.API.Messages;

using Shouldly;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps.ContentAreas
{
    [Binding]
    public class GetContentAreaSteps : StepBase
    {
        private ContentAreaResource resource = new Api().Resource<ContentAreaResource>();

        private string contentAreaIdKey = "contentAreaIdKey";

        [When(@"I request an existing content area")]
        public void WhenIRequestAnExistingContentArea()
        {
            var contentArea = resource.NewContentArea();
            Remember(contentArea.Id, contentAreaIdKey);
        }

        [Then(@"the content area should be returned")]
        public void ThenTheContentAreaShouldBeReturned()
        {
            var contentAreaId = Recall<string>(contentAreaIdKey);
            var contentArea = resource.GetContentArea(contentAreaId);
            contentArea.CollectionId.ShouldNotBe(null);
            contentArea.Name.ShouldNotBe(null);
        }

        [When(@"I request a non-existing content area")]
        public void WhenIRequestANon_ExistingContentArea()
        {
            Remember(resource.GetContentAreaThatDoesNotExist());
        }

        [When(@"I request content areas for a collectionId that does not exist")]
        public void WhenIRequestContentAreasForACollectionIdThatDoesNotExist()
        {
            Remember(resource.GetContentAreasForACollectionThatDoesNotExist());
        }

        [When(@"I request content areas for a collectionId that does exist")]
        public void WhenIRequestContentAreasForACollectionIdThatDoesExist()
        {
            Remember(resource.GetContentAreasForValidCollection());
        }

        [Then(@"I should get all content areas in the that collection")]
        public void ThenIShouldGetAllContentAreasInTheThatCollection()
        {
            var contentAreas = Recall<IEnumerable<ContentArea>>();
            contentAreas.ShouldNotBe(null);
            contentAreas.Count().ShouldBe(1);
        }
    }
}