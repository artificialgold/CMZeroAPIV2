Feature: Getting Applications

@Application
Scenario: Get Existing Application
	When I request an existing application
	Then the application should be returned

@Application
Scenario: Get Non-existent Application
	When I request a non-existing application
	Then not found exception should be returned

@Application
Scenario: Get all applications
	When I request all applications
	Then I should get a list of applications

@Application
Scenario: Get applications for an organisation that does not exist
	When I request applications for an organisation that does not exist
	Then I should get a OrganisationIdNotValidException

@Application
Scenario: Get applications for an existing organisation
	When I request applications for an organisation that exists
	Then I should get all applications for that organisation