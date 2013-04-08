using Api.Controllers;

using CMZero.API.Domain;
using CMZero.API.Messages;
using CMZero.API.Messages.Responses.Collections;

using NUnit.Framework;

using Rhino.Mocks;

namespace UnitTests.Api
{
    public class CollectionControllerTests
    {
        public class Given_a_CollectonController
        {
            protected CollectionController CollectionController;

            protected ICollectionService CollectionService;

            [SetUp]
            public virtual void SetUp()
            {
                CollectionService = MockRepository.GenerateMock<ICollectionService>();
                CollectionController = new CollectionController(CollectionService);
            }
        }

        public class When_I_call_Get : Given_a_CollectonController
        {
            private string id = "collectionId";

            private GetCollectionResponse result;

            private Collection collectionFromService;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                CollectionService.Stub(x => x.GetById(id)).Return(collectionFromService);
                result = CollectionController.Get(id);
            }

            [Test]
            public void it_should_return_collection_from_collection_service_in_response()
            {
                Assert.AreEqual(result.Collection, collectionFromService);
            }
        }

        [TestFixture]
        public class When_I_call_Post : Given_a_CollectonController
        {
            protected PostCollectionResponse Outcome;
            protected Collection CollectionIntoService;
            private readonly Collection collectionFromService = new Collection { Name = "hkj" };

            [SetUp]
            public new void SetUp()
            {
                base.SetUp();

                CollectionIntoService = new Collection();
                CollectionService.Stub(x => x.Create(CollectionIntoService)).Return(collectionFromService);
                Outcome = CollectionController.Post(CollectionIntoService);
            }

            [Test]
            public void it_should_return_the_collection_from_collection_service_in_the_response()
            {
                Assert.AreEqual(Outcome.Collection, collectionFromService);
            }
        }
    }
}