using System.Net;
using System.Net.Http;
using System.Web.Http;

using Api.Controllers;

using CMZero.API.Domain;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.ApiKeys;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.API.Messages.Exceptions.Organisations;

using NUnit.Framework;

using Rhino.Mocks;

using Shouldly;

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

        [Ignore("Figure out how to mock Request or delete the test")]
        [TestFixture]
        public class When_I_call_Get : Given_a_CollectonController
        {
            private string id = "collectionId";

            private HttpResponseMessage result;

            private Collection collectionFromService = new Collection();

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
                Assert.AreEqual(result.Content, collectionFromService);
            }
        }

        [Ignore("Figure out how to deal with mocking Request or delete")]
        [TestFixture]
        public class When_I_call_Post : Given_a_CollectonController
        {
            protected HttpResponseMessage Outcome;
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
                Assert.AreEqual(Outcome.Content, collectionFromService);
            }
        }

        [TestFixture]
        public class When_I_call_Put_with_collection_that_does_not_exist : Given_a_CollectonController
        {
            private Collection collectionThatDoesNotExist = new Collection();

            private HttpResponseException exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                CollectionService.Stub(x => x.Update(collectionThatDoesNotExist)).Throw(new ItemNotFoundException());
                try
                {
                    CollectionController.Put(collectionThatDoesNotExist);
                }
                catch (HttpResponseException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_throw_exception_with_statuscode_notfound()
            {
                exception.Response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            }
        }

        [TestFixture]
        public class When_I_call_Put_with_collection_that_has_applicationId_changed : Given_a_CollectonController
        {
            private Collection collection = new Collection();

            private HttpResponseException exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                CollectionService.Stub(x => x.Update(collection)).Throw(new ApplicationIdNotValidException());
                try
                {
                    CollectionController.Put(collection);
                }
                catch (HttpResponseException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_throw_exception_with_BadRequest_status_code()
            {
                exception.Response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            }

            [Test]
            public void it_should_throw_exception_with_ReasonPhrase_application_id_not_valid()
            {
                exception.Response.ReasonPhrase.ShouldBe(ReasonPhrases.ApplicationIdNotValid);
            }
        }

        [TestFixture]
        public class When_I_call_Put_with_collection_that_has_organisationId_changed : Given_a_CollectonController
        {
            private Collection collection = new Collection();

            private HttpResponseException exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                CollectionService.Stub(x => x.Update(collection)).Throw(new OrganisationIdNotValidException());
                try
                {
                    CollectionController.Put(collection);
                }
                catch (HttpResponseException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_throw_exception_with_BadRequest_status_code()
            {
                exception.Response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            }

            [Test]
            public void it_should_throw_exception_with_ReasonPhrase_application_id_not_valid()
            {
                exception.Response.ReasonPhrase.ShouldBe(ReasonPhrases.OrganisationIdNotValid);
            }
        }

        [TestFixture]
        public class When_I_call_GetByApiKey_with_invalid_apiKey : Given_a_CollectonController
        {
            private HttpResponseException exception;
            private const string ApiKey = "apiKey";

            [SetUp]
            public void SetUp()
            {
                base.SetUp();
                CollectionService.Stub(x => x.GetCollectionsByApiKey(ApiKey)).Throw(new ApiKeyNotValidException());

                try
                {
                    CollectionController.GetByApiKey(ApiKey);
                }
                catch (HttpResponseException ex)
                {
                    exception = ex;
                }
            }

            [Test]3
            public void it_should_return_HttpResponseException_with_status_code_bad_request()
            {
                exception.Response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            }

            [Test]
            public void it_should_return_HttpResponseException_with_reason_phrase_ApiKeyNotValid()
            {
                exception.Response.ReasonPhrase.ShouldBe(ReasonPhrases.ApiKeyNotValid);
            }
        }
    }
}
