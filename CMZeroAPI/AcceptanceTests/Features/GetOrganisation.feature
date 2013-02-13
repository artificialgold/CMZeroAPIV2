Feature: Get Organisation

@Organisation
Scenario: Get Existing Organisation
	When I request an existing organisation
	Then organisation should be returned
