Feature: CreateOrganisation

@Organisation
Scenario: Creating an organisation with correct parameters
	Given I create a valid organisation
	Then I should be able to get the organisation

@Organisation
Scenario: Creating an organisation without name
	Given I create an organisation without a name
	Then I should get a BadRequestException