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

@Application
Scenario: Update Application that does not exist
	When I update an application that does not exist
	Then I should get an ItemNotFoundException

@Application
Scenario: Update Application to different organisationId
	Given an existing application
	When I update the application with a different organisationId
	Then I should get a OrganisationIdNotValidException