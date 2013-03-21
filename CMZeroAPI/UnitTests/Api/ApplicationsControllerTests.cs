﻿using System;
using System.Collections.Generic;
using System.Web.Http;

using Api.Controllers;
using CMZero.API.Domain;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Responses;

using NUnit.Framework;
using Rhino.Mocks;

namespace UnitTests.Api
{
    public class ApplicationsControllerTests
    {
        public class Given_an_ApplicationsController
        {
            protected ApplicationController ApplicationsController;
            protected IApplicationService ApplicationService;

            [SetUp]
            public void SetUp()
            {
                ApplicationService = MockRepository.GenerateMock<IApplicationService>();
                ApplicationsController = new ApplicationController(ApplicationService);
            }
        }

        [TestFixture]
        public class When_I_call_Post : Given_an_ApplicationsController
        {
            protected PostApplicationResponse Outcome;
            protected Application ApplicationIntoService;
            private readonly Application applicationFromService = new Application { Name = "hkj" };

            [SetUp]
            public new void SetUp()
            {
                base.SetUp();

                ApplicationIntoService = new Application();
                ApplicationService.Stub(x => x.Create(ApplicationIntoService)).Return(applicationFromService);
                Outcome = ApplicationsController.Post(ApplicationIntoService);
            }

            [Test]
            public void it_should_return_the_application_from_application_service_in_the_response()
            {
                Assert.AreEqual(Outcome.Application, applicationFromService);
            }
        }

        [TestFixture]
        public class When_I_call_Get_with_existing_id : Given_an_ApplicationsController
        {
            private Application applicationFromService = new Application();

            private GetApplicationResponse outcome;

            private const string Id = "ghj";

            [SetUp]
            public new virtual void SetUp()
            {
                ApplicationService.Stub(x => x.GetById(Id)).Return(applicationFromService);

                outcome = ApplicationsController.Get(Id);
            }

            [Test]
            public void it_should_return_the_organisation_from_organisation_service_in_the_response()
            {
                Assert.AreEqual(outcome.Application, applicationFromService);
            }
        }

        [TestFixture]
        public class When_I_call_Get_with_non_existent_id_and_OrganisationService_throws_exception : Given_an_ApplicationsController
        {
            private string Id = "ghj";

            private Exception exception;

            [SetUp]
            public new virtual void SetUp()
            {
                ApplicationService = new ApplicationServiceThatThrowsExceptionWhenNotKnownId();
                ApplicationsController = new ApplicationController(ApplicationService);
                
                try
                {
                    ApplicationsController.Get(Id);
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

        public class ApplicationServiceThatThrowsExceptionWhenNotKnownId : IApplicationService
        {
            public Application Create(Application entity)
            {
                throw new NotImplementedException();
            }

            public Application Update(Application entity)
            {
                throw new NotImplementedException();
            }

            public Application GetById(string id)
            {
                throw new ItemNotFoundException();
            }

            public IEnumerable<Application> GetAll()
            {
                throw new NotImplementedException();
            }
        }

        [TestFixture]
        public class When_I_call_GetAllOrganisations : Given_an_ApplicationsController
        {
            private GetApplicationsResponse outcome;

            private IEnumerable<Application> applications = new List<Application>();

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ApplicationService.Stub(x => x.GetAll()).Return(applications);
                outcome = ApplicationsController.Get();
            }

            [Test]
            public void it_should_return_result_from_OrganisationService_in_the_response()
            {
                Assert.AreEqual(outcome.Applications, applications);
            }
        }

        [TestFixture]
        public class When_I_call_put_with_a_valid_application : Given_an_ApplicationsController
        {
            private Application applicationToUpdate= new Application{Name = "preUpdate"};

            private Application updatedApplication= new Application{Name = "afterUpdate"};

            private PutApplicationResponse outcome;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ApplicationService.Stub(x => x.Update(applicationToUpdate)).Return(updatedApplication);
                outcome = ApplicationsController.Put(applicationToUpdate);
            }

            [Test]
            public void it_should_return_response_with_organisation_from_service()
            {
                Assert.AreEqual(outcome.Application, updatedApplication);
            }
        }
    }
}