using CMZero.API.Messages.Exceptions;

using Shouldly;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps.Shared
{
    [Binding]
    public class ExceptionSteps:StepBase
    {
        [Then(@"I should get a BadRequestException")]
        public void ThenIShouldGetABadRequestException()
        {
            var result = Recall<BadRequestException>();
            result.ShouldNotBe(null);
        }

        [Then(@"not found exception should be returned")]
        public void ThenNotFoundExceptionShouldBeReturn()
        {
            ItemNotFoundException result = Recall<ItemNotFoundException>();

            result.ShouldNotBe(null);
        }
    }
}