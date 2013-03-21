Feature: UpdateOrganisation

@Organisation
Scenario: Update Organisation
	Given an existing organisation
	When I update the organisation name with a valid name
	Then the organisation should have the new name
	And the organisation should have the new updated date

@Organisation
Scenario: Update Organisation to have no name
	Given an existing organisation
	When I update the organisation name with no name
	Then I should get a BadRequestException