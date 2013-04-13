Feature: UpdateCollection

@Collection
Scenario: Update an existing collection
	Given an existing collection
	When I update the collection name
	Then the collection should have the new name
	And the collection should have the new updated date

@Collection
Scenario: Update an existing collection to have no name
	Given an existing collection
	When I update the collection name to no name
	Then I should get a BadRequestException
