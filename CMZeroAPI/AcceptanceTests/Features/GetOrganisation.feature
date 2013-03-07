Feature: Get Organisation

@Organisation
Scenario: Get Existing Organisation
	When I request an existing organisation
	Then organisation should be returned

@Organisation
Scenario: Get Non-existent Organisation
	When I request a non-existing organisation
	Then not found exception should be returned

@Organisation
Scenario: Get all organisations
	When I request all organisations
	Then I should get a list of organisations