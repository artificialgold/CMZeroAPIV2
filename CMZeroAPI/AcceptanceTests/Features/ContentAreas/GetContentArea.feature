Feature: Getting Content Areas

@ContentAreas
Scenario: Get existing content area
	When I request an existing content area
	Then the content area should be returned

@ContentAreas
Scenario: Get non-existent content area
	When I request a non-existing content area
	Then not found exception should be returned

@ContentAreas
Scenario: Get content areas by collectionId that does not exist
	When I request content areas for a collectionId that does not exist
	Then I should get a CollectionIdNotValidException

@ContentAreas
Scenario: Get content areas by collectionId that does exist
	When I request content areas for a collectionId that does exist
	Then I should get all content areas in the that collection

@ContentAreas
Scenario: Get content areas by apikey and name where apikey does not exist
	When I request content areas for an apikey that does not exist
	Then I should get an ApiKeyNotValidException