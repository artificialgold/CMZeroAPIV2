Feature: UpdateOrganisation

@Organisation
Scenario: Add two numbers
	Given an existing organisation
	When I update the organisation name with a valid name
	Then the organisation should have the new name
	And the organisation should have the new updated date
