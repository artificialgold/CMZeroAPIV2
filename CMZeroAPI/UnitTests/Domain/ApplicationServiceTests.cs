using System.Collections.Generic;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Domain;
using CMZero.API.Domain.ApiKey;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions.Organisations;

using NUnit.Framework;

using Rhino.Mocks;

using Shouldly;

namespace UnitTests.Domain
{
    public class ApplicationServiceTests
    {
        public class Given_an_application_service
        {
            protected ApplicationService ApplicationService;

            protected IApplicationRepository ApplicationRepository;

            protected IOrganisationService OrganisationService;

            protected IApiKeyCreator ApiKeyCreator;

            [SetUp]
            public virtual void SetUp()
            {
                OrganisationService = MockRepository.GenerateMock<IOrganisationService>();
                ApplicationRepository = MockRepository.GenerateMock<IApplicationRepository>();
                ApiKeyCreator = MockRepository.GenerateMock<IApiKeyCreator>();
                ApplicationService = new ApplicationService(ApplicationRepository, OrganisationService, ApiKeyCreator);
            }
        }

        public class When_I_call_create_with_an_organisationId_that_does_not_exist : Given_an_application_service
        {
            private string organisationIdThatDoesNotExist = "doesnotexist";

            private OrganisationDoesNotExistException exception;

            [SetUp]
            public new virtual void SetUp()
            {
                OrganisationService.Stub(x => x.IdExists(organisationIdThatDoesNotExist)).Return(false);
                Application application = new Application
                                              {
                                                  Name = "NewName",
                                                  OrganisationId = organisationIdThatDoesNotExist
                                              };
                try
                {
                    ApplicationService.Create(application);
                }
                catch (OrganisationDoesNotExistException exception)
                {
                    this.exception = exception;
                }
            }

            [Test]
            public void it_should_return_OrganisationIdNotValid_exception()
            {
                Assert.NotNull(exception);
            }
        }

        [TestFixture]
        public class When_I_call_with_an_organisationId_that_exists : Given_an_application_service
        {
            private Application application;

            private string organisationIdThatExists = "IExist";

            private Application result;

            private string apiKeyFromCreator = "createdApiKey";

            [SetUp]
            public new virtual void SetUp()
            {
                application = new Application { OrganisationId = organisationIdThatExists };
                OrganisationService.Stub(x => x.IdExists(organisationIdThatExists)).Return(true);
                ApiKeyCreator.Stub(x => x.Create()).Return(apiKeyFromCreator);
                result = ApplicationService.Create(application);
            }

            [Test]
            public void it_should_return_created_application()
            {
                Assert.AreEqual(result, application);
            }

            [Test]
            public void it_should_set_apikey_to_value_from_apikeycreator()
            {
                result.ApiKey.ShouldBe(apiKeyFromCreator);
            }
        }

        [TestFixture]
        public class When_I_call_GetApplicationsForOrganisation : Given_an_application_service
        {
            private string organisationId = "orgId";

            private IList<Application> result;

            private List<Application> objToReturn;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                objToReturn = new List<Application>();
                ApplicationRepository.Stub(x=>x.GetApplicationsForOrganisation(organisationId)).Return(objToReturn);
                result = ApplicationService.GetApplicationsForOrganisation(organisationId);
            }

            [Test]
            public void it_should_return_result_from_repository()
            {
               Assert.AreEqual(result, objToReturn);
            }
        }

        [TestFixture]
        public class When_I_call_update_with_different_organisationId_from_original : Given_an_application_service
        {
            private OrganisationIdNotValidException exception;

            private readonly Application applicationToUpdate=new Application{Id=ApplicationId, OrganisationId = "newId"};

            private const string ApplicationId = "toUpdate";

            [SetUp]
            public new virtual void SetUp()
            {
                ApplicationRepository.Stub(x => x.GetById(applicationToUpdate.Id))
                                     .Return(new Application { Id = ApplicationId, OrganisationId = "original" });
                try
                {
                    ApplicationService.Update(applicationToUpdate);
                }
                catch (OrganisationIdNotValidException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_throw_OrganisationIdNotValidException()
            {
                exception.ShouldNotBe(null);
            }
        }
    }
}