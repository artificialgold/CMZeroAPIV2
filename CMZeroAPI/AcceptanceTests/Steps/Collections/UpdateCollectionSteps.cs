﻿using System;

using AcceptanceTests.Helpers;
using AcceptanceTests.Helpers.Collections;

using Shouldly;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps.Collections
{
    [Binding]
    public class UpdateCollectionSteps : StepBase
    {
        private CollectionResource resource = new Api().Resource<CollectionResource>();

        private string collectionidkey = "collectionIdKey";

        private string applicationIdKey = "appIdKey";

        private string updateName = "updatedName";

        private string updateStartKey = "updateStartKey";

        private string updateEndKey = "updateEndKey";

        [Given(@"an existing collection")]
        public void GivenAnExistingCollection()
        {
            var collection = resource.NewCollectionWithSpecifiedName("preUpdate");
            var id = collection.Id;
            var applicationId = collection.ApplicationId;
            Remember(id, collectionidkey);
            Remember(applicationId, applicationIdKey);
        }

        [When(@"I update the collection name")]
        public void WhenIUpdateTheCollectionName()
        {
            var collection = resource.GetCollection(Recall<string>(collectionidkey), Recall<string>(applicationIdKey));
            collection.Name = updateName;
            DateTime startUpdateTime = DateTime.UtcNow;
            resource.UpdateCollection(collection);
            DateTime endUpdateTime = DateTime.UtcNow;
            Remember(startUpdateTime, updateStartKey);
            Remember(endUpdateTime, updateEndKey);
        }
        [Then(@"the collection should have the new name")]
        public void ThenTheCollectionShouldHaveTheNewName()
        {
            var collection = resource.GetCollection(Recall<string>(collectionidkey), Recall<string>(applicationIdKey));
            collection.Name.ShouldBe(updateName);
        }
        [Then(@"the collection should have the new updated date")]
        public void ThenTheCollectionShouldHaveTheNewUpdatedDate()
        {
            var collection = resource.GetCollection(Recall<string>(collectionidkey), Recall<string>(applicationIdKey));
            collection.Updated.ShouldBeGreaterThanOrEqualTo(Recall<DateTime>(updateStartKey));
            collection.Updated.ShouldBeLessThan(Recall<DateTime>(updateEndKey).AddTicks(1)); 
        }

        [When(@"I update the collection name to no name")]
        public void WhenIUpdateTheCollectionNameToNoName()
        {
            var collection = resource.GetCollection(Recall<string>(collectionidkey), Recall<string>(applicationIdKey));
            var exception = resource.UpdateCollectionWithUnspecifiedName(collection);
            Remember(exception);
        }

        [When(@"I update a collection that does not exist")]
        public void WhenIUpdateACollectionThatDoesNotExist()
        {
           Remember(resource.UpdateCollectionThatDoesNotExist());
        }

        [When(@"I update a collection to have a different applicationId")]
        public void WhenIUpdateACollectionToHaveADifferentApplicationId()
        {
            Remember(resource.UpdateCollectionToHaveDifferentApplicationId());
        }

        [When(@"I update a collection to have a different organisationId")]
        public void WhenIUpdateACollectionToHaveADifferentOrganisationId()
        {
            Remember(resource.UpdateCollectionToHaveDifferentOrganisationId());
        }

    }
}