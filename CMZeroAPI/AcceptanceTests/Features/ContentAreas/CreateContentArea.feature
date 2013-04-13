Feature: Create Content Area

@ContentArea
Scenario: Create a valid content area in a known collection
	Given I have a collectionId
	When I create a content area
	Then I should get the content area back in the list of content areas for the collection
