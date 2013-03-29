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