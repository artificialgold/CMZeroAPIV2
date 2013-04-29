Feature: Create Content Area

@ContentArea
Scenario: Create a valid content area in a known collection
	When I create a content area for an existing collection
	Then I should be able to get the content area

@ContentArea
Scenario: Create a content area with name that already exists in collection
	When I create a content area with a name that already exists for an existing collection
	Then I should get a ContentAreaNameAlreadyExistsInCollectionException

@ContentArea
Scenario: Create a content area for a collection that does not exist
	When I create a content area for a collection that does not exist
	Then I should get a CollectionIdNotValidException

@ContentArea
Scenario: Create a content area for a collection that is not part of the application specified
	When I create a content area for an existing collection and specify a different applicationId
	Then I should get a CollectionIdNotPartOfApplicationException

@ContentArea
Scenario: Create a content area with blank application specified
	When I create a content area for an existing collection and do not specify an applicationId
	Then I should get an ApplicationIdNotValidException