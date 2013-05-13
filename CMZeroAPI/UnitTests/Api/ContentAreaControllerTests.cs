using System.Net;
using System.Net.Http;
using System.Web.Http;

using Api.Controllers;

using CMZero.API.Domain;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.Collections;
using CMZero.API.Messages.Exceptions.ContentAreas;

using NUnit.Framework;

using Rhino.Mocks;

using Shouldly;

namespace UnitTests.Api
{
    public class ContentAreaControllerTests
    {
        public class Given_a_content_area_controller
        {
            protected ContentAreaController ContentAreaController;

            protected IContentAreaService ContentAreaService;

            [SetUp]
            public virtual void SetUp()
            {
                ContentAreaService = MockRepository.GenerateMock<IContentAreaService>();

                ContentAreaController = new ContentAreaController(ContentAreaService);
            }
        }

        [TestFixture]
        public class When_the_contentAreaService_throws_a_ContentAreaNameAlreadyExistsInCollectionException :
            Given_a_content_area_controller
        {
            private HttpResponseException exception;

            [SetUp]
            public virtual void SetUp()
            {
                ContentArea contentAreaToCreate = new ContentArea();
                ContentAreaService.Stub(x => x.Create(contentAreaToCreate))
                                  .Throw(new ContentAreaNameAlreadyExistsInCollectionException());

                try
                {
                    ContentAreaController.Post(contentAreaToCreate);
                }
                catch (HttpResponseException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_throw_a_HttpResponseMessage_with_bad_request_status_code()
            {
                exception.Response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
                exception.Response.ReasonPhrase.ShouldBe(ReasonPhrases.ContentAreaNameAlreadyExistsInCollection);
            }
        }

        [TestFixture]
        public class When_the_content_area_service_throws_a_collectionIdDoesNotExist_Exception :
            Given_a_content_area_controller
        {
            private readonly ContentArea contentArea = new ContentArea();
            private HttpResponseException exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ContentAreaService.Stub(x => x.Create(contentArea)).Throw(new CollectionIdNotValidException());
                try
                {
                    ContentAreaController.Post(contentArea);
                }
                catch (HttpResponseException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_throw_a_HttpResponseMessage_with_bad_request_status_code()
            {
                exception.Response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
                exception.Response.ReasonPhrase.ShouldBe(ReasonPhrases.CollectionIdDoesNotExist);
            }
        }

        [TestFixture]
        public class When_the_content_area_service_throws_a_CollectionIdNotPartOfApplicationException :
            Given_a_content_area_controller
        {
            private ContentArea contentArea = new ContentArea();

            private HttpResponseException exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ContentAreaService.Stub(x => x.Create(contentArea))
                                  .Throw(new CollectionIdNotPartOfApplicationException());

                try
                {
                    ContentAreaController.Post(contentArea);
                }
                catch (HttpResponseException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_throw_a_HttpResponseMessage_with_bad_request_status_code()
            {
                exception.Response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
                exception.Response.ReasonPhrase.ShouldBe(ReasonPhrases.CollectionNotPartOfApplication);
            }
        }
    }
}