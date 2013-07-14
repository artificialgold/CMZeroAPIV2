using CMZero.API.Domain.ApiKey;

using NUnit.Framework;

using Shouldly;

namespace UnitTests.Domain.ApiKey
{
    public class ApiKeyCreatorTests
    {
         public class Given_an_ApiKeyCreator
         {
             protected ApiKeyCreator ApiKeyCreator;

             [SetUp]
             public virtual void SetUp()
             {
                 ApiKeyCreator = new ApiKeyCreator();
             }
         }

        [TestFixture]
        public class When_I_call_create : Given_an_ApiKeyCreator
        {
            private string _result;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                _result = ApiKeyCreator.Create();
            }

            [Test]
            public void it_should_return_a_string_value()
            {
                _result.ShouldNotBe(null);
            }
        }
    }
}