using System.Collections.Generic;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Domain;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions.ApiKeys;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.API.Messages.Exceptions.Collections;
using CMZero.API.Messages.Exceptions.Organisations;

using NUnit.Framework;

using Rhino.Mocks;

using Shouldly;

namespace UnitTests.Domain
{
    public class CollectionServiceTests
    {
        public class Given_a_CollectionService
        {
            protected CollectionService CollectionService;

            protected ICollectionRepository CollectionRepository;

            protected IApplicationService ApplicationService;

            [SetUp]
            public virtual void SetUp()
            {
                CollectionRepository = MockRepository.GenerateMock<ICollectionRepository>();

                ApplicationService = MockRepository.GenerateMock<IApplicationService>();
                CollectionService = new CollectionService(CollectionRepository, ApplicationService);
            }
        }

        [TestFixture]
        public class When_I_call_create_with_an_application_not_in_the_organisation : Given_a_CollectionService
        {
            private string applicationIdNotInReturnedApplications = "appNotValidId";

            private string organisationIdToQueryWith = "organisationId";

            private ApplicationIdNotPartOfOrganisationException exceptionReturned;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                CollectionRepository.Stub(x => x.GetCollectionsForApplication("appid", "orgid"))
                    .IgnoreArguments()
                    .Return(new List<Collection> { new Collection() });
                ApplicationService.Stub(x => x.GetApplicationsForOrganisation(organisationIdToQueryWith)).Return(new List<Application>());
                try
                {
                    CollectionService.Create(
                        new Collection
                            {
                                Name = "I do not yet exist",
                                ApplicationId = applicationIdNotInReturnedApplications,
                                OrganisationId = organisationIdToQueryWith
                            });
                }
                catch (ApplicationIdNotPartOfOrganisationException exception)
                {

                    exceptionReturned = exception;
                }
            }

            [Test]
            public void it_should_throw_exception_ApplicationIdNotPartOfOrganisationException()
            {
                Assert.NotNull(exceptionReturned);
            }
        }

        [TestFixture]
        public class When_I_call_Create_with_a_collection_name_that_already_exists_in_the_collection : Given_a_CollectionService
        {
            private CollectionNameAlreadyExistsException exception;

            private string applicationId = "ghjkl";
            private string organisationId = "hjkjnjk";

            private const string nameThatAlreadyExists = "nameThatExists";

            [SetUp]
            public new virtual void SetUp()
            {
                CollectionRepository.Stub(x => x.GetCollectionsForApplication(applicationId, organisationId))
                                    .Return(new List<Collection> { new Collection { Name = nameThatAlreadyExists } });

                try
                {
                    CollectionService.Create(new Collection { Name = nameThatAlreadyExists, ApplicationId = applicationId, OrganisationId = organisationId });
                }
                catch (CollectionNameAlreadyExistsException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_throw_CollectionNameAlreadyExistsException()
            {
                Assert.NotNull(exception);
            }
        }

        [TestFixture]
        public class When_I_call_update_with_applicationId_different_from_original : Given_a_CollectionService
        {
            private string collectionId = "collection_original";

            private ApplicationIdNotValidException exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                CollectionRepository.Stub(x => x.GetById(collectionId))
                                    .Return(new Collection { Id = collectionId, ApplicationId = "old" });
                try
                {
                    CollectionService.Update(new Collection { Id = collectionId, ApplicationId = "new" });
                }
                catch (ApplicationIdNotValidException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_throw_exception_ApplicationIdNotValidException()
            {
                exception.ShouldNotBe(null);
            }
        }

        [TestFixture]
        public class When_I_call_update_with_a_collection_having_organisationId_changed : Given_a_CollectionService
        {
            private string collectionId = "collection_original";

            private string applicationId = "applicationId";

            private OrganisationIdNotValidException exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                CollectionRepository.Stub(x => x.GetById(collectionId))
                                        .Return(new Collection { Id = collectionId, ApplicationId = applicationId, OrganisationId = "old" });
                try
                {
                    CollectionService.Update(new Collection { Id = collectionId, ApplicationId = applicationId, OrganisationId = "new" });
                }
                catch (OrganisationIdNotValidException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_throw_exception_OrganisationIdNotValidException()
            {
                exception.ShouldNotBe(null);
            }
        }

        [TestFixture]
        public class When_I_call_GetCollectionsForApplication : Given_a_CollectionService
        {
            private string applicationId = "appId";

            private string organisationId = "orgId";

            private IList<Collection> result;

            private List<Collection> listToReturn;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                listToReturn = new List<Collection>();
                ApplicationService.Stub(x => x.GetById(applicationId))
                                  .Return(new Application { OrganisationId = organisationId, Id = applicationId });
                CollectionRepository.Stub(x => x.GetCollectionsForApplication(applicationId, organisationId))
                                    .Return(listToReturn);
                result = CollectionService.GetCollectionsForApplication(applicationId);
            }

            [Test]
            public void it_should_return_value_from_repository_GetCollectionsForApplication()
            {
                result.ShouldBe(listToReturn);
            }
        }

        [TestFixture]
        public class When_I_call_get_collection_by_ApiKey_and_name_with_valid_parameters : Given_a_CollectionService
        {
            private const string apiKey = "apiKey";

            private const string CollectionName = "collectionName";

            private Collection _result;

            private const string ApplicationId = "applicationId";

            private const string OrganisationId = "organisationId";

            private Collection _collectionToReturn;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ApplicationService.Stub(x => x.GetApplicationByApiKey(apiKey)).Return(new Application { Id = ApplicationId, OrganisationId = OrganisationId });
                _collectionToReturn = new Collection { Name = CollectionName };
                CollectionRepository.Stub(x => x.GetCollectionsForApplication(ApplicationId, OrganisationId))
                                 .Return(new List<Collection> { _collectionToReturn });
                _result = CollectionService.GetCollectionByApiKeyAndName(apiKey, CollectionName);
            }

            [Test]
            public void it_should_return_collection_from_correct_application()
            {
                _result.ShouldBe(_collectionToReturn);
            }
        }

        [TestFixture]
        public class When_I_call_GetCollectionByApiKeyAndName : Given_a_CollectionService
        {
            private ApiKeyNotValidException exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                string apiKey = "badApiKey";
                ApplicationService.Stub(x => x.GetApplicationByApiKey(apiKey)).Throw(new ApiKeyNotValidException());
                try
                {
                    CollectionService.GetCollectionByApiKeyAndName(apiKey, "CollectionName");
                }
                catch (ApiKeyNotValidException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_throw_ApiKeyNotValidException()
            {
                exception.ShouldNotBe(null);
            }
        }

        [TestFixture]
        public class When_I_call_GetCollectionByApiKeyAndName_with_bad_collection_name : Given_a_CollectionService
        {
            private CollectionNameNotValidException exception;

            private string applicationId = "applicationId";

            private string organisationId = "organisationId";

            private const string CollectionName = "BadCollectionName";

            private const string ApiKey = "apiKey";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ApplicationService.Stub(x => x.GetApplicationByApiKey(ApiKey))
                                  .Return(new Application { Id = applicationId, OrganisationId = organisationId });
                CollectionRepository.Stub(x => x.GetCollectionsForApplication(applicationId, organisationId))
                                    .Return(new List<Collection>());

                try
                {
                    CollectionService.GetCollectionByApiKeyAndName(ApiKey, CollectionName);
                }
                catch (CollectionNameNotValidException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_return_CollectionNameNotValidException()
            {
                exception.ShouldNotBe(null);
            }
        }

        [TestFixture]
        public class When_I_call_GetCollectionsByApiKey_with_invalid_apiKey : Given_a_CollectionService
        {
            private string apiKey = "invalidApiKey";
            private ApiKeyNotValidException _exception;

            [SetUp]
            public new void SetUp()
            {
                base.SetUp();
                ApplicationService.Stub(x => x.GetApplicationByApiKey(apiKey)).Throw(new ApiKeyNotValidException());
                try
                {
                    CollectionService.GetCollectionsByApiKey(apiKey);
                }
                catch (ApiKeyNotValidException ex)
                {
                    _exception = ex;
                }
            }

            [Test]
            public void it_should_throw_a_ApiKeyNotValidException()
            {
                _exception.ShouldNotBe(null);
            }
        }

        [TestFixture]
        public class When_I_call_GetCollectionsByApiKey_with_valid_api_key : Given_a_CollectionService
        {
            private const string ApiKey = "validApiKey";
            private IEnumerable<Collection> _result;
            private readonly List<Collection> _collectionsToReturn = new List<Collection>();
            private string applicationId = "applicationId";
            private string organisationId = "orgId";

            [SetUp]
            public new void SetUp()
            {
                base.SetUp();
                ApplicationService.Stub(x => x.GetApplicationByApiKey(ApiKey))
                                  .Return(new Application { Id = applicationId, OrganisationId = organisationId });
                CollectionRepository.Stub(x => x.GetCollectionsForApplication(applicationId, organisationId)).Return(_collectionsToReturn);
                _result = CollectionService.GetCollectionsByApiKey(ApiKey);
            }

            [Test]
            public void it_should_return_results_from_repository()
            {
                _result.ShouldBe(_collectionsToReturn);
            }
        }
    }
}