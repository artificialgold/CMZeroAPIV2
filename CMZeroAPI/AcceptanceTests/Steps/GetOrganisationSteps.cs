using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps
{
    [Binding]
    public class GetOrganisationSteps
    {
        [When(@"I request an existing organisation")]
        public void WhenIRequestAnExistingOrganisation()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"organisation should be returned")]
        public void ThenOrganisationShouldBeReturned()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
