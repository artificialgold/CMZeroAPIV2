using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.ApiKeys;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.API.Messages.Exceptions.Collections;
using CMZero.API.Messages.Exceptions.Organisations;

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

        [Then(@"I should get a OrganisationDoesNotExistException")]
        public void ThenIShouldGetAnOrganisationDoesNotException()
        {
            OrganisationDoesNotExistException result = Recall<OrganisationDoesNotExistException>();

            result.ShouldNotBe(null);
        }

        [Then(@"I should get a OrganisationIdNotValidException")]
        public void ThenIShouldGetAOrganisationIdNotValidException()
        {
            OrganisationIdNotValidException result = Recall<OrganisationIdNotValidException>();

            result.ShouldNotBe(null);
        }

        [Then(@"I should get an ApplicationIdNotValidException")]
        public void ThenIShouldGetAnApplicationIdNotValidException()
        {
            ApplicationIdNotValidException result = Recall<ApplicationIdNotValidException>();

            result.ShouldNotBe(null);
        }

        [Then(@"I should get an ApiKeyNotValidException")]
        public void ThenIShouldGetAnApiKeyNotValidException()
        {
            ApiKeyNotValidException result = Recall<ApiKeyNotValidException>();

            result.ShouldNotBe(null);
        }

        [Then(@"I should get a CollectionNameNotValidException")]
        public void ThenIShouldGetACollectionNameNotValidException()
        {
            CollectionNameNotValidException result = Recall<CollectionNameNotValidException>();

            result.ShouldNotBe(null);
        }
    }
}