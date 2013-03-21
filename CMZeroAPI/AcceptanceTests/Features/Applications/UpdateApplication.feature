Feature: Updating applications

@Application
Scenario: Update Application
	Given an existing application
	When I update the application name with a valid name
	Then the application should have the new name
	And the application should have the new updated date

@Application
Scenario: Update Application to have no name
	Given an existing application
	When I update the application name with no name
	Then I should get a BadRequestException