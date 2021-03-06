﻿namespace CMZero.API.Messages.Exceptions
{
    public class ReasonPhrases
    {
        public const string OrganisationNameAlreadyExists= "Organisation name already exists";
        
        public const string ApplicationNameAlreadyExists="Application name already exists for this organisation";

        public const string CollectionNameNotValidException = "Collection Name is not valid";

        public const string ApiKeyNotValid = "ApiKey is not valid";

        public const string CollectionNotPartOfApplication = "CollectionId is not part of the application specified";

        public const string ContentAreaNameAlreadyExistsInCollection = "Content area name already exists in collection";

        public const string CollectionIdDoesNotExist = "CollectionId is not valid";

        public const string ApplicationIdNotValid = "ApplicationId is not valid";

        public const string OrganisationIdNotValid = "OrganisationId is not valid";

        public const string ApplicationNotPartOfOrganisation = "ApplicationId is not part of the Organisation specified";

        public const string OrganisationIdDoesNotExist = "OrganisationId does not exist";

        public const string CollectionNameAlreadyExists = "Collection Name already exists for that application";
    }
}