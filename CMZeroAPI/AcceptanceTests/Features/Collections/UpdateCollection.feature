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

@Collection
Scenario: Update a collection that does not exist
	When I update a collection that does not exist
	Then I should get an ItemNotFoundException

@Collection
Scenario: Update a collection to have a different applicationId 
	When I update a collection to have a different applicationId
	Then I should get an ApplicationIdNotValidException

@Collection
Scenario: Update a collection to have a different organisationId
	When I update a collection to have a different organisationId
	Then I should get a OrganisationIdNotValidException
