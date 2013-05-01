using System.Collections.Generic;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Domain;
using CMZero.API.Messages;
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

            protected IContentAreaRepository contentAreaRepository;

            [SetUp]
            public virtual void SetUp()
            {
                contentAreaRepository = MockRepository.GenerateMock<IContentAreaRepository>();

                ContentAreaService = new ContentAreaService(contentAreaRepository);
            }
        }

        [TestFixture]
        public class When_calling_create_with_a_content_area_whos_name_already_exists_in_collection : Given_a_content_area_service
        {
            private string collectionId = "collectionId";

            private ContentArea result;

            private ContentAreaNameAlreadyExistsInCollectionException exception;

            [SetUp]
            public virtual void SetUp()
            {
                base.SetUp();
                string alreadyexists = "alreadyExists";
                ContentArea contentArea = new ContentArea { Name = alreadyexists, CollectionId = collectionId };
                contentAreaRepository.Stub(x => x.ContentAreasInCollection(collectionId)).Return(new List<ContentArea>{new ContentArea{CollectionId = collectionId, Name = alreadyexists}});
                try
                {
                    ContentAreaService.Create(contentArea);
                }
                catch(ContentAreaNameAlreadyExistsInCollectionException ex)
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
    }
}