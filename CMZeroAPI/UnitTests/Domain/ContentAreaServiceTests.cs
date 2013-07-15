using System.Collections.Generic;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Domain;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.ApiKeys;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.API.Messages.Exceptions.Collections;
using CMZero.API.Messages.Exceptions.ContentAreas;

using NUnit.Framework;

using Rhino.Mocks;

using Shouldly;

namespace UnitTests.Domain
{
    public class ContentAreaServiceTests
    {
        public class Given_a_content_area_service
        {
            protected ContentAreaService ContentAreaService;

            protected ICollectionService CollectionService;

            protected IContentAreaRepository ContentAreaRepository;

            protected IApplicationService ApplicationService;

            [SetUp]
            public virtual void SetUp()
            {
                ContentAreaRepository = MockRepository.GenerateMock<IContentAreaRepository>();
                CollectionService = MockRepository.GenerateMock<ICollectionService>();
                ApplicationService = MockRepository.GenerateMock<IApplicationService>();
                ContentAreaService = new ContentAreaService(ContentAreaRepository, CollectionService);
            }
        }

        [TestFixture]
        public class When_calling_create_with_a_content_area_whos_name_already_exists_in_collection : Given_a_content_area_service
        {
            private string collectionId = "collectionId";

            private ContentAreaNameAlreadyExistsInCollectionException exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                const string Alreadyexists = "alreadyExists";
                var contentArea = new ContentArea { Name = Alreadyexists, CollectionId = collectionId };
                ContentAreaRepository.Stub(x => x.ContentAreasInCollection(collectionId)).Return(new List<ContentArea> { new ContentArea { CollectionId = collectionId, Name = Alreadyexists } });
                try
                {
                    ContentAreaService.Create(contentArea);
                }
                catch (ContentAreaNameAlreadyExistsInCollectionException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_throw_ContentAreaNameAlreadyExistsInCollectionException()
            {
                exception.ShouldNotBe(null);
            }
        }

        [TestFixture]
        public class When_calling_create_with_a_content_area_with_collectionId_that_is_not_part_of_the_application
            : Given_a_content_area_service
        {
            private const string CollectionIdThatIsNotPartOfApplication = "wrong";
            private const string ApplicationIdThatDoesNotHaveCollectionId = "jhjhjkl;j";

            private CollectionIdNotPartOfApplicationException exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                var contentArea = new ContentArea { Name = "test", CollectionId = CollectionIdThatIsNotPartOfApplication };
                ContentAreaRepository.Stub(x => x.ContentAreasInCollection(CollectionIdThatIsNotPartOfApplication))
                    .Return(new List<ContentArea>());
                CollectionService.Stub(x => x.GetById(CollectionIdThatIsNotPartOfApplication))
                                 .Return(new Collection { Id = CollectionIdThatIsNotPartOfApplication, ApplicationId = ApplicationIdThatDoesNotHaveCollectionId });

                try
                {
                    ContentAreaService.Create(contentArea);
                }
                catch (CollectionIdNotPartOfApplicationException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_throw_CollectionIdNotPartOfApplicationException()
            {
                exception.ShouldNotBe(null);
            }
        }

        [TestFixture]
        public class When_I_call_create_with_a_collectionId_that_is_not_valid :
            Given_a_content_area_service
        {
            private readonly ContentArea contentArea = new ContentArea { ApplicationId = "appId", CollectionId = CollectionId };

            private CollectionIdNotValidException exception;

            private const string CollectionId = "test";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ContentAreaRepository.Stub(x => x.ContentAreasInCollection(CollectionId))
                                     .Return(new List<ContentArea>());
                CollectionService.Stub(x => x.GetById(CollectionId))
                    .Throw(new ItemNotFoundException());

                try
                {
                    ContentAreaService.Create(contentArea);
                }
                catch (CollectionIdNotValidException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_throw_a_CollectionIdNotValidException()
            {
                exception.ShouldNotBe(null);
            }
        }

        [TestFixture]
        public class When_I_put_a_content_area_with_different_applicationId : Given_a_content_area_service
        {
            private readonly ContentArea contentArea = new ContentArea { ApplicationId = DifferentApplicationId, Id = ContentAreaId };
            private const string OriginalApplicationId = "original";
            private const string DifferentApplicationId = "new";
            private const string ContentAreaId = "contentAreaId";
            private readonly ContentArea contentAreaOriginal = new ContentArea { ApplicationId = OriginalApplicationId };
            private ApplicationIdNotValidException exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ContentAreaRepository.Stub(x => x.GetById(ContentAreaId)).Return(contentAreaOriginal);

                try
                {
                    ContentAreaService.Update(contentArea);
                }
                catch (ApplicationIdNotValidException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_throw_ApplicationIdNotValidException()
            {
                exception.ShouldNotBe(null);
            }
        }

        [TestFixture]
        public class When_I_call_put_with_a_collectionId_that_is_not_part_of_application : Given_a_content_area_service
        {
            private ContentArea contentArea = new ContentArea { Id = ContentAreaId, ApplicationId = ApplicationId, CollectionId = "collectionIdNotInReturnedCollection" };

            private CollectionIdNotPartOfApplicationException exception;

            private const string ContentAreaId = "contentAreaId";

            private const string ApplicationId = "applicationId";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ContentAreaRepository.Stub(x => x.GetById(ContentAreaId)).Return(new ContentArea { ApplicationId = ApplicationId });
                CollectionService.Stub(x => x.GetCollectionsForApplication(ApplicationId))
                                 .Return(new List<Collection> { new Collection { } });

                try
                {
                    ContentAreaService.Update(contentArea);
                }
                catch (CollectionIdNotPartOfApplicationException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_throw_a_CollectionIdNotPartOfApplicationException()
            {
                exception.ShouldNotBe(null);
            }
        }

        [TestFixture]
        public class When_I_call_GetByCollection_with_a_collectionId_that_does_not_exist : Given_a_content_area_service
        {
            private CollectionIdNotValidException _exception;

            private const string CollectionId = "collectionId";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                try
                {
                    CollectionService.Stub(x => x.GetById(CollectionId)).Throw(new ItemNotFoundException());
                    ContentAreaService.GetByCollection(CollectionId);
                }
                catch (CollectionIdNotValidException ex)
                {
                    _exception = ex;
                }
            }

            [Test]
            public void it_should_return_CollectionIdNotValidException()
            {
                _exception.ShouldNotBe(null);
            }
        }

        [TestFixture]
        public class When_I_call_GetByCollection_with_a_collectionId_that_exists : Given_a_content_area_service
        {
            private const string CollectionId = "collection";

            private IEnumerable<ContentArea> returned;

            private List<ContentArea> contentAreasFromRepository;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                CollectionService.Stub(x => x.GetById(CollectionId)).Return(new Collection { Id = CollectionId });
                contentAreasFromRepository = new List<ContentArea>();
                ContentAreaRepository.Stub(x => x.ContentAreasInCollection(CollectionId)).Return(contentAreasFromRepository);
                returned = ContentAreaService.GetByCollection(CollectionId);
            }

            [Test]
            public void it_should_return_content_areas_from_repository()
            {
                returned.ShouldBe(contentAreasFromRepository);
            }
        }

        [TestFixture]
        public class When_I_call_GetByCollectionNameAndApiKey_with_valid_parameters : Given_a_content_area_service
        {
            private List<ContentArea> _contentAreasToReturn;

            private IEnumerable<ContentArea> _result;

            private const string ApiKey = "apiKey";
            private const string CollectionName = "collectionName";
            private const string CollectionId = "collectionId";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                CollectionService.Stub(x => x.GetCollectionByApiKeyAndName(ApiKey, CollectionName))
                                 .Return(new Collection { Id = CollectionId });
                _contentAreasToReturn = new List<ContentArea>();
                ContentAreaRepository.Stub(x => x.ContentAreasInCollection(CollectionId))
                                     .Return(_contentAreasToReturn);
                _result=ContentAreaService.GetByCollectionNameAndApiKey(ApiKey, CollectionName);
            }

            [Test]
            public void it_should_return_value_from_repository()
            {
                _result.ShouldBe(_contentAreasToReturn);
            }
        }

        [TestFixture]
        public class When_I_call_GetByCollectionNameAndApiKey_with_invalid_api_key : Given_a_content_area_service
        {
            private string collectionName="collectionName";

            private ApiKeyNotValidException exception;

            private const string ApiKeyThatIsNotValid = "IDoNotExist";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                CollectionService.Stub(x=>x.GetCollectionByApiKeyAndName(ApiKeyThatIsNotValid, collectionName)).Throw(new ApiKeyNotValidException());

                try
                {
                    ContentAreaService.GetByCollectionNameAndApiKey(ApiKeyThatIsNotValid, collectionName);
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
        public class When_I_call_GetCollectionByApiKeyAndName : Given_a_content_area_service
        {
            private CollectionNameNotValidException exception;

            private const string ApiKey = "apiKey";

            private const string CollectionNameThatIsNotValid = "IAmNotValid";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                CollectionService.Stub(x => x.GetCollectionByApiKeyAndName(ApiKey, CollectionNameThatIsNotValid))
                                 .Throw(new CollectionNameNotValidException());

                try
                {
                    ContentAreaService.GetByCollectionNameAndApiKey(ApiKey, CollectionNameThatIsNotValid);
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
    }
}