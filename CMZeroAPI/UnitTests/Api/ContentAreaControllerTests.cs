using Api.Controllers;

using CMZero.API.Domain;

using NUnit.Framework;

using Rhino.Mocks;

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
        public class When_the_contentAreaService_throws_a_ContentAreaNameAlreadyExistsException :
            Given_a_content_area_controller
        {
            [Test]
            public void Test()
            {
                Assert.Fail();
            }
        }
    }
}