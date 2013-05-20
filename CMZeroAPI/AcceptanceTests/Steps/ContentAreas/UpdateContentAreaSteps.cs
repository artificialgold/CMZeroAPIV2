using System;

using AcceptanceTests.Helpers;
using AcceptanceTests.Helpers.ContentAreas;

using Shouldly;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps.ContentAreas
{
    [Binding]
    public class UpdateContentAreaSteps : StepBase
    {
        private ContentAreaResource resource = new Api().Resource<ContentAreaResource>();

        private string contentAreaIdKey = "contentAreaIdKey";

        private string applicationIdKey = "applicationIdKey";

        private string updateName = "updateName";

        private string updateStartKey = "updateStartKey";

        private string updateEndKey = "updateEndKey";

        [Given(@"an existing content area")]
        public void GivenAnExistingContentArea()
        {
            var contentArea = resource.NewContentAreaWithSpecifiedName("preUpdate");
            var id = contentArea.Id;
            var applicationId = contentArea.ApplicationId;
            Remember(id, contentAreaIdKey);
            Remember(applicationId, applicationIdKey);
        }

        [When(@"I update the content area name")]
        public void WhenIUpdateTheContentAreaName()
        {
            var contentArea = resource.GetContentArea(Recall<string>(contentAreaIdKey));
            contentArea.Name = updateName;
            DateTime startUpdateTime = DateTime.UtcNow;
            resource.UpdateContentArea(contentArea);
            DateTime endUpdateTime = DateTime.UtcNow;
            Remember(startUpdateTime, updateStartKey);
            Remember(endUpdateTime, updateEndKey);
        }

        [Then(@"the content area should have the new name")]
        public void ThenTheContentAreaShouldHaveTheNewName()
        {
            var contentArea = resource.GetContentArea(Recall<string>(contentAreaIdKey));
            contentArea.Name.ShouldBe(updateName);
        }

        [Then(@"the content area should have the new updated date")]
        public void ThenTheContentAreaShouldHaveTheNewUpdatedDate()
        {
            var contentArea = resource.GetContentArea(Recall<string>(contentAreaIdKey));
            contentArea.Updated.ShouldBeGreaterThanOrEqualTo(Recall<DateTime>(updateStartKey));
            contentArea.Updated.ShouldBeLessThan(Recall<DateTime>(updateEndKey).AddTicks(1));
        }

        [When(@"I update the content area name to no name")]
        public void WhenIUpdateTheContentAreaNameToNoName()
        {
            var contentArea = resource.GetContentArea(Recall<string>(contentAreaIdKey));
            var exception = resource.UpdateContentAreaWithUnspecifiedName(contentArea);
            Remember(exception);
        }

        [When(@"I update a content area that does not exist")]
        public void WhenIUpdateAContentAreaThatDoesNotExist()
        {
            Remember(resource.UpdateContentAreaThatDoesNotExist());
        }

        [When(@"I update a content area to have a different applicationId")]
        public void WhenIUpdateAContentAreaToHaveADifferentApplicationId()
        {
            Remember(resource.UpdateContentAreaToHaveDifferentApplicationId());
        }

        [When(@"I update a content area to have a different collectionId that is not part of the application")]
        public void WhenIUpdateAContentAreaToHaveADifferentCollectionIdThatIsNotPartOfTheApplication()
        {
            Remember(resource.UpdateContentAreaToHaveCollectionIdNotPartOfApplicationId());
        }
    }
}