Feature: Getting Collection

@Collection
Scenario: Get existing collection 
	When I request an existing collection
	Then the collection should be returned

@Collection
Scenario: Get non-existent collection
	When I request a non-existing collection
	Then not found exception should be returned

@Collection
Scenario: Get collections by valid apikey
	When I request collections for a valid apikey
	Then the collections should be returned

@Collection
Scenario: Get collections by invalid apikey
	When I request collections for an invalid apikey
	Then I should get an ApiKeyNotValidException