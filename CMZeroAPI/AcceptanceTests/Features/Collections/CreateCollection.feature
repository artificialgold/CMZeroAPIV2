Feature: Create Collection

@Collection
Scenario: Create new collection for existing application
	When I post a valid collection
	Then I should be able to retrieve the collection

@Collection
Scenario: Create a new collection without a name
	When I post a collection with no name
	Then I should get a BadRequestException

@Collection
Scenario: Create a new collection with applicationId that is not part of the organisation
	When I post a collection with applicationId not for the same organisationId
	Then I should get a ApplicationNotInOrganisationException

@Collection
Scenario: Create a new collection with applicationId blank
	When I post a collection with no applicationId
	Then I should get a BadRequestException

@Collection
Scenario: Create a new collection with organisationId blank
	When I post a collection with no organisationId
	Then I should get a BadRequestException

@Collection
Scenario: Create a new collection with same name as existing collection in application
	When I post a collection with existing name in application
	Then I should get a CollectionNameAlreadyExistsException
