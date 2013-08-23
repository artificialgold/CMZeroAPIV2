using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Api.Controllers;
using CMZero.API.Domain;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.Organisations;
using NUnit.Framework;
using Rhino.Mocks;
using Shouldly;

namespace UnitTests.Api
{
    public class OrganisationsControllerTests
    {
        public class Given_an_OrganisationController
        {
            protected OrganisationController OrganisationsController;
            protected IOrganisationService OrganisationService;

            [SetUp]
            public void SetUp()
            {
                OrganisationService = MockRepository.GenerateMock<IOrganisationService>();
                OrganisationsController = new OrganisationController(OrganisationService);
            }
        }

        [TestFixture]
        public class When_I_call_Post : Given_an_OrganisationController
        {
            protected HttpResponseMessage Outcome;
            protected Organisation OrganisationIntoService;
            private readonly Organisation organisationFromService = new Organisation { Name = "hkj" };

            [SetUp]
            public new void SetUp()
            {
                base.SetUp();

                OrganisationIntoService = new Organisation();
                OrganisationService.Stub(x => x.Create(OrganisationIntoService)).Return(organisationFromService);
                Outcome = OrganisationsController.Post(OrganisationIntoService);
            }

            [Ignore("How to deal with the Request object. Need to mock or delete test")]
            [Test]
            public void it_should_return_the_organisation_from_organisation_service_in_the_response()
            {
                Assert.AreEqual(Outcome.Content, organisationFromService);
            }
        }

        [TestFixture]
        public class When_I_call_Create_with_name_that_already_exists : Given_an_OrganisationController
        {
            private const string OrganisationThatAlreadyExists = "nameAlreadyExists";
            private Organisation _organisation;
            private HttpResponseException _exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                _organisation = new Organisation { Name = OrganisationThatAlreadyExists };
                OrganisationService.Stub(x => x.Create(_organisation))
                                   .Throw(new OrganisationNameAlreadyExistsException());
                try
                {
                    OrganisationsController.Post(_organisation);
                }
                catch (HttpResponseException ex)
                {
                    _exception = ex;
                }
            }

            [Test]
            public void it_should_throw_exception()
            {
                _exception.ShouldNotBe(null);
            }

            [Test]
            public void it_should_throw_exception_with_reason_phrase_OrganisationNameAlreadyExists()
            {
                _exception.Response.ReasonPhrase.ShouldBe(ReasonPhrases.OrganisationNameAlreadyExists);
            }

            [Test]
            public void it_should_throw_exception_with_status_code_BadRequest()
            {
                _exception.Response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            }
        }

        [TestFixture]
        public class When_I_call_Get_with_existing_id : Given_an_OrganisationController
        {
            private Organisation organisationFromService = new Organisation();

            private HttpResponseMessage outcome;

            private const string Id = "ghj";

            [SetUp]
            public new virtual void SetUp()
            {
                OrganisationService.Stub(x => x.GetById(Id)).Return(organisationFromService);

                outcome = OrganisationsController.Get(Id);
            }

            [Ignore("How to deal with the Request as a mock. Delete if necessary")]
            [Test]
            public void it_should_return_the_organisation_from_organisation_service_in_the_response()
            {
                Assert.AreEqual(outcome.Content, organisationFromService);
            }
        }

        [TestFixture]
        public class When_I_call_Get_with_non_existent_id_and_OrganisationService_throws_exception : Given_an_OrganisationController
        {
            private string Id = "ghj";

            private Exception exception;

            [SetUp]
            public new virtual void SetUp()
            {
                OrganisationService = new OrganisationServiceThatThrowsExceptionWhenNotKnownId();
                OrganisationsController = new OrganisationController(OrganisationService);

                try
                {
                    OrganisationsController.Get(Id);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_return_a_not_found_exception()
            {
                Assert.That(exception, Is.TypeOf(typeof(HttpResponseException)));
            }
        }

        public class OrganisationServiceThatThrowsExceptionWhenNotKnownId : IOrganisationService
        {
            public Organisation Create(Organisation entity)
            {
                throw new NotImplementedException();
            }

            public Organisation Update(Organisation entity)
            {
                throw new NotImplementedException();
            }

            public Organisation GetById(string id)
            {
                throw new ItemNotFoundException();
            }

            public IEnumerable<Organisation> GetAll()
            {
                throw new NotImplementedException();
            }

            public bool IdExists(string id)
            {
                throw new NotImplementedException();
            }
        }

        [TestFixture]
        public class When_I_call_GetAllOrganisations : Given_an_OrganisationController
        {
            private HttpResponseMessage outcome;

            private IEnumerable<Organisation> organisations = new List<Organisation>();

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                OrganisationService.Stub(x => x.GetAll()).Return(organisations);
                outcome = OrganisationsController.Get();
            }

            [Ignore("How to deal with the Request object. Need to mock or delete test")]
            [Test]
            public void it_should_return_result_from_OrganisationService_in_the_response()
            {
                Assert.AreEqual(outcome.Content, organisations);
            }
        }

        [Ignore("How to deal with the Request object. Need to mock or delete test")]
        [TestFixture]
        public class When_I_call_put_with_a_valid_organisation : Given_an_OrganisationController
        {
            private Organisation organisationToUpdate = new Organisation { Name = "preUpdate" };

            private Organisation updatedOrganisation = new Organisation { Name = "afterUpdate" };

            private HttpResponseMessage outcome;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                OrganisationService.Stub(x => x.Update(organisationToUpdate)).Return(updatedOrganisation);
                outcome = OrganisationsController.Put(organisationToUpdate);
            }

            [Test]
            public void it_should_return_response_with_organisation_from_service()
            {
                Assert.AreEqual(outcome.Content, updatedOrganisation);
            }
        }

        [TestFixture]
        public class When_I_update_an_organisation_with_an_existing_name : Given_an_OrganisationController
        {
            private HttpResponseException _exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                const string organisationNameExists = "nameExists";
                var organisation = new Organisation { Name = organisationNameExists };
                OrganisationService.Stub(x => x.Update(organisation)).Throw(new OrganisationNameAlreadyExistsException());
                try
                {
                    OrganisationsController.Put(organisation);
                }
                catch (HttpResponseException ex)
                {
                    _exception = ex;
                }
            }

            [Test]
            public void it_should_return_HttpResponseException()
            {
                _exception.ShouldNotBe(null);
            }

            [Test]
            public void it_should_return_status_code_bad_request()
            {
                _exception.Response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            }

            [Test]
            public void it_shouldd_return_reason_phrase_organisation_name_already_exists()
            {
                _exception.Response.ReasonPhrase.ShouldBe(ReasonPhrases.OrganisationNameAlreadyExists);
            }
        }
    }
}
