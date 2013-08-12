using System.Collections.Generic;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Domain;
using CMZero.API.Domain.ApiKey;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.ApiKeys;
using CMZero.API.Messages.Exceptions.Applications;
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
            private const string OrganisationIdThatDoesNotExist = "doesnotexist";

            private OrganisationDoesNotExistException _exception;

            [SetUp]
            public new virtual void SetUp()
            {
                OrganisationService.Stub(x => x.IdExists(OrganisationIdThatDoesNotExist)).Return(false);
                Application application = new Application
                                              {
                                                  Name = "NewName",
                                                  OrganisationId = OrganisationIdThatDoesNotExist
                                              };
                try
                {
                    ApplicationService.Create(application);
                }
                catch (OrganisationDoesNotExistException ex)
                {
                    _exception = ex;
                }
            }

            [Test]
            public void it_should_return_OrganisationIdNotValid_exception()
            {
                Assert.NotNull(_exception);
            }
        }

        [TestFixture]
        public class When_I_call_create_with_application_name_that_exists_for_that_organisation : Given_an_application_service
        {
            private ApplicationNameAlreadyExistsException _exception;
            private const string NameThatAlreadyExists = "nameThatAlreadyExists";
            private const string OrganisationId = "orgId";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                OrganisationService.Stub(x => x.IdExists(OrganisationId)).Return(true);
                ApplicationRepository.Stub(x => x.GetApplicationsForOrganisation(OrganisationId))
                                     .Return(new List<Application> {new Application {Name = NameThatAlreadyExists, OrganisationId = OrganisationId}});
                try
                {
                    ApplicationService.Create(new Application {Name = NameThatAlreadyExists, OrganisationId = OrganisationId});
                }
                catch (ApplicationNameAlreadyExistsException ex)
                {
                    _exception = ex;
                }
            }

            [Test]
            public void it_should_throw_ApplicationNameAlreadyExistsException()
            {
                _exception.ShouldNotBe(null);
            }
        }

        [TestFixture]
        public class When_I_call_create_with_a_valid_application : Given_an_application_service
        {
            private Application _application;

            private const string OrganisationIdThatExists = "IExist";

            private Application _result;

            private const string ApiKeyFromCreator = "createdApiKey";

            [SetUp]
            public new virtual void SetUp()
            {
                _application = new Application { Name="name", OrganisationId = OrganisationIdThatExists };
                OrganisationService.Stub(x => x.IdExists(OrganisationIdThatExists)).Return(true);
                ApplicationRepository.Stub(x => x.GetApplicationsForOrganisation(OrganisationIdThatExists)).Return(new List<Application>());
                ApiKeyCreator.Stub(x => x.Create()).Return(ApiKeyFromCreator);
                _result = ApplicationService.Create(_application);
            }

            [Test]
            public void it_should_return_created_application()
            {
                Assert.AreEqual(_result, _application);
            }

            [Test]
            public void it_should_set_apikey_to_value_from_apikeycreator()
            {
                _result.ApiKey.ShouldBe(ApiKeyFromCreator);
            }
        }

        [TestFixture]
        public class When_I_call_GetApplicationsForOrganisation : Given_an_application_service
        {
            private const string OrganisationId = "orgId";

            private IList<Application> _result;

            private List<Application> _objToReturn;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                _objToReturn = new List<Application>();
                ApplicationRepository.Stub(x => x.GetApplicationsForOrganisation(OrganisationId)).Return(_objToReturn);
                _result = ApplicationService.GetApplicationsForOrganisation(OrganisationId);
            }

            [Test]
            public void it_should_return_result_from_repository()
            {
                Assert.AreEqual(_result, _objToReturn);
            }
        }

        [TestFixture]
        public class When_I_call_GetApplicationsForOrganisation_with_not_existent_organisationId : Given_an_application_service
        {
            private OrganisationIdNotValidException _exception;
            private const string OrganisationId = "doesNotExist";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                OrganisationService.Stub(x => x.GetById(OrganisationId)).Throw(new ItemNotFoundException());
                try
                {
                    ApplicationService.GetApplicationsForOrganisation(OrganisationId);
                }
                catch (OrganisationIdNotValidException ex)
                {
                    _exception = ex;
                }
            }

            [Test]
            public void it_should_throw_organisationIdNotValidException()
            {
                _exception.ShouldNotBe(null);
            }
        }

        [TestFixture]
        public class When_I_call_update_with_different_organisationId_from_original : Given_an_application_service
        {
            private OrganisationIdNotValidException exception;

            private readonly Application applicationToUpdate = new Application { Id = ApplicationId, OrganisationId = "newId" };

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

        [TestFixture]
        public class When_I_call_GetByApiKey_with_invalid_api_key : Given_an_application_service
        {
            private string apiKeyThatDoesNotExist = "doesNotExist";

            private ApiKeyNotValidException exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                try
                {
                    ApplicationRepository.Stub(x => x.GetByApiKey(apiKeyThatDoesNotExist)).Throw(new ApiKeyNotValidException());
                    ApplicationService.GetApplicationByApiKey(apiKeyThatDoesNotExist);
                }
                catch (ApiKeyNotValidException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_throw_a_ApiKeyNotValidException()
            {
                exception.ShouldNotBe(null);
            }
        }

        [TestFixture]
        public class When_I_call_GetApplicationByApiKey_with_valid_apikey : Given_an_application_service
        {
            private string apiKey = "apiKeyThatIsValid";

            private Application result;

            private Application ApplicationToReturn = new Application();

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ApplicationRepository.Stub(x => x.GetByApiKey(apiKey)).Return(ApplicationToReturn);
                result = ApplicationService.GetApplicationByApiKey(apiKey);
            }

            [Test]
            public void it_should_return_application_from_repository()
            {
                result.ShouldBe(ApplicationToReturn);
            }
        }
    }
}