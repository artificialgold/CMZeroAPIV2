﻿using System.Collections.Generic;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Domain;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
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

            [SetUp]
            public virtual void SetUp()
            {
                ContentAreaRepository = MockRepository.GenerateMock<IContentAreaRepository>();
                CollectionService = MockRepository.GenerateMock<ICollectionService>();

                ContentAreaService = new ContentAreaService(ContentAreaRepository, CollectionService);
            }
        }

        [TestFixture]
        public class When_calling_create_with_a_content_area_whos_name_already_exists_in_collection : Given_a_content_area_service
        {
            private string collectionId = "collectionId";

            private ContentAreaNameAlreadyExistsInCollectionException exception;

            [SetUp]
            public virtual void SetUp()
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
    }
}