using System;

using AcceptanceTests.Helpers;
using AcceptanceTests.Helpers.Collections;

using Shouldly;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps.Collections
{
    [Binding]
    public class CreateCollectionSteps : StepBase
    {
        private CollectionResource resource = new Api().Resource<CollectionResource>();

        private const string CollectionNameKey = "collectionNameKey";

        private const string CollectionIdKey = "collectionIdKey";

        [When(@"I post a valid collection")]
        public void WhenIPostAValidCollection()
        {
            string name = string.Format("knownName{0}", DateTime.UtcNow.ToString("yyyyMMddSSmm"));
            string id = resource.NewCollectionWithSpecifiedName(name).Id;
            resource.GetCollection(id).Name.ShouldNotBe(null);
            Remember(id, CollectionIdKey);
            Remember(name, CollectionNameKey);
        }

    }
}
