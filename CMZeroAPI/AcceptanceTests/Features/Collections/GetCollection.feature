Feature: Getting Collection

@Collection
Scenario: Get existing collection 
	When I request an existing collection
	Then the collection should be returned

@Collection
Scenario: Get non-existent collection
	When I request a non-existing collection
	Then not found exception should be returned