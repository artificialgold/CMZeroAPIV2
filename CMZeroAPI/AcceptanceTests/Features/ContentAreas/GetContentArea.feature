Feature: Getting Content Areas

@ContentAreas
Scenario: Get existing content area
	When I request an existing content area
	Then the content area should be returned

@ContentAreas
Scenario: Get non-existent content area
	When I request a non-existing content area
	Then not found exception should be returned