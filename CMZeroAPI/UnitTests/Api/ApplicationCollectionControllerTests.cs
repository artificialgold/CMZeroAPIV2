using System.Collections.Generic;
using System.Net.Http;

using Api.Controllers;

using CMZero.API.Messages;

using NUnit.Framework;

using Shouldly;

namespace UnitTests.Api
{
    public class ApplicationCollectionControllerTests
    {
        public class Given_an_ApplicationCollectionController
        {
            protected ApplicationCollectionController ApplicationCollectionController;

            [SetUp]
            public virtual void SetUp()
            {
               // ApplicationCollectionController = new ApplicationCollectionController();
            }
        }

        [Ignore("happy path hard to test")]
        [TestFixture]
        public class When_I_call_Get_with_valid_appkey : Given_an_ApplicationCollectionController
        {
            private string apikey = "apikey";

            private HttpResponseMessage _result;

            private IList<Collection> collectionsFromService = new List<Collection>();

            [SetUp]
            public new virtual void SetUp()
            {
                _result = ApplicationCollectionController.Get(apikey);
            }

            [Test]
            public void it_should_return_HttpResponse_with_collections_from_service()
            {
               // _result.Content.ShouldBe<IList<Collection>>(collectionsFromService);
            }
        }
    }
}