﻿namespace CMZero.API.Messages.Exceptions
{
    public class ReasonPhrases
    {
        public const string ApplicationIdNotValid = "ApplicationId is not valid";

        public const string OrganisationIdNotValid = "OrganisationId is not valid";

        public const string ApplicationNotPartOfOrganisation = "ApplicationId is not part of the Organisation specified";

        public const string OrganisationIdDoesNotExist = "OrganisationId does not exist";

        public const string CollectionNameAlreadyExists = "Collection Name already exists for that application";
    }
}