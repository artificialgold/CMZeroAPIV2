Feature: Create Application

@Application
Scenario: Creating an application with correct parameters	
	Given I create a valid application
	Then I should be able to get the application with a new apikey

@Application
Scenario: Creating an application without name
	Given I create an application without a name
	Then I should get a BadRequestException

@Application
Scenario: Creating an application with an organisationId that does not exist
	Given I create an application with an organisationId that does not exist
	Then I should get a OrganisationDoesNotExistException

@Application
Scenario: Creating an application with an application name that already exists for this organisation
	Given I create an application with a name already existing for an organisation
	Then I should get a ApplicationNameAlreadyExistsException