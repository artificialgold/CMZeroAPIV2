Feature: Create Collection

@Collection
Scenario: Create new collection for existing application
	When I post a valid collection
	Then I should be able to retrieve the collection
