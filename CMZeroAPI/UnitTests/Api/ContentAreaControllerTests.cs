using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Api.Controllers;

using CMZero.API.Domain;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.API.Messages.Exceptions.Collections;
using CMZero.API.Messages.Exceptions.ContentAreas;
using CMZero.API.Messages.Responses.ContentAreas;

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

        [TestFixture]
        public class When_I_call_put_with_a_valid_contentArea : Given_a_content_area_controller
        {
            private readonly ContentArea contentArea = new ContentArea();

            private readonly ContentArea updatedContentArea = new ContentArea { Updated = DateTime.Now };

            private PutContentAreaResponse result;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ContentAreaService.Stub(x => x.Update(contentArea)).Return(updatedContentArea);
                result = ContentAreaController.Put(contentArea);

            }

            [Test]
            public void it_should_return_content_area_in_the_response()
            {
                result.ContentArea.ShouldBe(updatedContentArea);
            }
        }

        [TestFixture]
        public class When_I_call_put_and_service_returns_ItemNotFoundException : Given_a_content_area_controller
        {
            private ContentArea contentArea = new ContentArea();

            private HttpResponseException exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ContentAreaService.Stub(x => x.Update(contentArea)).Throw(new ItemNotFoundException());
                try
                {
                    ContentAreaController.Put(contentArea);
                }
                catch (HttpResponseException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_throw_HttpResponseException_with_status_code_NotFound()
            {
                exception.Response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            }
        }

        [TestFixture]
        public class When_I_call_put_and_service_returns_ApplicationIdNotValidException :
            Given_a_content_area_controller
        {
            private ContentArea contentArea = new ContentArea();

            private HttpResponseException exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ContentAreaService.Stub(x => x.Update(contentArea)).Throw(new ApplicationIdNotValidException());
                try
                {
                    ContentAreaController.Put(contentArea);
                }
                catch (HttpResponseException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void exception_should_have_status_code_bad_request()
            {
                exception.Response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            }
        }

        [TestFixture]
        public class When_I_call_Put_and_service_returns_CollectionIdNotPartOfApplicationException
        : Given_a_content_area_controller
        {
            private ContentArea contentArea = new ContentArea();

            private HttpResponseException exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ContentAreaService.Stub(x => x.Update(contentArea))
                                  .Throw(new CollectionIdNotPartOfApplicationException());

                try
                {
                    ContentAreaController.Put(contentArea);
                }
                catch (HttpResponseException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_throw_a_HttpResponseException_with_bad_request_status_code()
            {
                exception.Response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            }

            [Test]
            public void it_should_have_reason_phrase_collection_id_not_part_of_application()
            {
                exception.Response.ReasonPhrase.ShouldBe(ReasonPhrases.CollectionNotPartOfApplication);
            }
        }
    }
}