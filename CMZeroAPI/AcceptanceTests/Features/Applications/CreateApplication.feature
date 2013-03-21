Feature: Create Application

@Application
Scenario: Creating an application with correct parameters	
	Given I create a valid application
	Then I should be able to get the application

@Application
Scenario: Creating an application without name
	Given I create an application without a name
	Then I should get a BadRequestException