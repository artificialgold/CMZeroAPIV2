using CMZero.API.Domain;

using NUnit.Framework;

namespace UnitTests.Domain
{
    public class ContentAreaServiceTests
    {
        public class Given_a_content_area_service
        {
            protected ContentAreaService ContentAreaService;

            [SetUp]
            public virtual void SetUp()
            {
                //ContentAreaRepository 

                ContentAreaService = new ContentAreaService();
            }
        } 
    }
}