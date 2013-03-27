using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Domain;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions.Organisations;

using NUnit.Framework;

using Rhino.Mocks;

namespace UnitTests.Domain
{
    public class ApplicationServiceTests
    {
        public class Given_an_application_service
        {
            protected ApplicationService applicationService;

            protected IApplicationRepository applicationRepository;

            protected IOrganisationService organisationService;

            [SetUp]
            public virtual void SetUp()
            {
                organisationService = MockRepository.GenerateMock<IOrganisationService>();
                applicationRepository = MockRepository.GenerateMock<IApplicationRepository>();
                applicationService = new ApplicationService(applicationRepository, organisationService);
            }
        }

        public class When_I_call_create_with_an_organisationId_that_does_not_exist : Given_an_application_service
        {
            private string organisationIdThatDoesNotExist = "doesnotexist";

            private OrganisationDoesNotExistException Exception;

            [SetUp]
            public virtual void SetUp()
            {
                organisationService.Stub(x => x.IdExists(organisationIdThatDoesNotExist)).Return(false);
                Application application = new Application
                                              {
                                                  Name = "NewName",
                                                  OrganisationId = organisationIdThatDoesNotExist
                                              };
                try
                {
                    applicationService.Create(application);
                }
                catch (OrganisationDoesNotExistException exception)
                {
                    Exception = exception;
                }
            }

            [Test]
            public void it_should_return_OrganisationIdNotValid_exception()
            {
                Assert.NotNull(Exception);
            }
        }

        public class When_I_call_with_an_organisationId_that_exists : Given_an_application_service
        {
            private Application application;

            private string organisationIdThatExists = "IExist";

            private Application result;

            [SetUp]
            public virtual void SetUp()
            {
                application = new Application { OrganisationId = organisationIdThatExists };
                organisationService.Stub(x => x.IdExists(organisationIdThatExists)).Return(true);
                result = applicationService.Create(application);
            }

            [Test]
            public void it_should_return_created_application()
            {
                Assert.AreEqual(result, application);
            }
        }
    }
}