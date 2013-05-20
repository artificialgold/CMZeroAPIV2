Feature: Update Content Area

@contentarea
Scenario: Update an existing content area
	Given an existing content area
	When I update the content area name
	Then the content area should have the new name
	And the content area should have the new updated date

@contentarea
Scenario: Update an existing content area to have no name
	Given an existing content area
	When I update the content area name to no name
	Then I should get a BadRequestException

@contentarea
Scenario: Update a content area that does not exist
	When I update a content area that does not exist
	Then I should get an ItemNotFoundException

@contentarea
Scenario: Update a content area to have a different applicationId 
	When I update a content area to have a different applicationId
	Then I should get an ApplicationIdNotValidException

@contentarea
Scenario: Update a content area to have a different collectionId not in the application
	When I update a content area to have a different collectionId that is not part of the application
	Then I should get a CollectionIdNotValidException
