using NUnit.Framework;

namespace IntegrationTests.DataAccess
{
    public class ApplicationRepositoryTests
    {
        protected ApplicationRepository ApplicationRepository;

        [SetUp]
        public virtual void SetUp()
        {
            ApplicationRepository = new ApplicationRepository();
        }
    }

    [TestFixture]
    public class ApplicationRepository
    {
        [SetUp]
        public virtual void SetUp()
        {
            
        }
    }
}