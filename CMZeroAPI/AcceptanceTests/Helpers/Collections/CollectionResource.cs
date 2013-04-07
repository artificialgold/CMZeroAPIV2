using System;

using AcceptanceTests.Helpers.Applications;

using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.API.Messages.Exceptions.Collections;
using CMZero.API.Messages.Responses.Collections;
using CMZero.API.ServiceAgent;

namespace AcceptanceTests.Helpers.Collections
{
    public class CollectionResource : IResource
    {
        private ICollectionServiceAgent _collectionServiceAgent;
        private ApplicationResource applicationResource = new Api().Resource<ApplicationResource>();


        public CollectionResource(ICollectionServiceAgent collectionServiceAgent)
        {
            _collectionServiceAgent = collectionServiceAgent;
        }

        public Collection NewCollectionWithSpecifiedName(string name)
        {
            var newApplication = applicationResource.NewApplication();
            var applicationId = newApplication.Id;
            var organisationId = newApplication.OrganisationId;

            PostCollectionResponse response =
                _collectionServiceAgent.Post(
                    new Collection
                    {
                        Active = true,
                        Name = name,
                        ApplicationId = applicationId,
                        OrganisationId = organisationId
                    });

            return response.Collection;
        }

        public Collection GetCollection(string id, string applicationId)
        {
            return _collectionServiceAgent.Get(id, applicationId).Collection;
        }

        public BadRequestException NewCollectionWithUnspecifiedName()
        {
            var application = applicationResource.NewApplication();
            var applicationId = application.Id;
            var organisationId = application.OrganisationId;

            try
            {
                _collectionServiceAgent.Post(
                    new Collection
                    {
                        Active = true,
                        ApplicationId = applicationId,
                        OrganisationId = organisationId,
                    });
            }
            catch (BadRequestException ex)
            {
                return ex;
            }

            return null;
        }

        public ApplicationIdNotPartOfOrganisationException NewCollectionWithApplicationIdNotPartOfOrganisationId()
        {
            var application = applicationResource.NewApplication();
            var applicationId = application.Id;
            var organisationId = application.OrganisationId + "xx";

            try
            {
                _collectionServiceAgent.Post(
                    new Collection
                        {
                            Name = "testNameForBadOrganisationId",
                            Active = true,
                            ApplicationId = applicationId,
                            OrganisationId = organisationId
                        });
            }
            catch (ApplicationIdNotPartOfOrganisationException exception)
            {
                return exception;
            }

            return null;
        }

        public BadRequestException NewCollectionWithNoApplicationId()
        {
            try
            {
                _collectionServiceAgent.Post(
                    new Collection
                    {
                        Active = true,
                        Name = "name not to be created",
                        OrganisationId = "organisationId"
                    });
            }
            catch (BadRequestException exception)
            {
                return exception;
            }

            return null;
        }

        public BadRequestException NewCollectionWithNoOrganisationId()
        {
            var application = applicationResource.NewApplication();
            var applicationId = application.Id;

            try
            {
                _collectionServiceAgent.Post(
                    new Collection
                    {
                        Name = "testNameForNoOrganisationId",
                        Active = true,
                        ApplicationId = applicationId,
                    });
            }
            catch (BadRequestException exception)
            {
                return exception;
            }

            return null;
        }

        public CollectionNameAlreadyExistsException NewCollectionWithExistingNameInApplication()
        {
            var application = applicationResource.NewApplication();
            var applicationId = application.Id;
            var organisationId = application.OrganisationId;

            string collectionNameToBeDuplicated = "testNameForCollection" + DateTime.UtcNow.ToString("yyyyMMddSSmm");
            _collectionServiceAgent.Post(
                new Collection
                    {
                        Name = collectionNameToBeDuplicated,
                        Active = true,
                        ApplicationId = applicationId,
                        OrganisationId = organisationId
                    });

            try
            {
                _collectionServiceAgent.Post(
                    new Collection
                    {
                        Name = collectionNameToBeDuplicated,
                        Active = true,
                        ApplicationId = applicationId,
                        OrganisationId = organisationId
                    });
            }
            catch (CollectionNameAlreadyExistsException exception)
            {
                return exception;
            }

            return null;

        }
    }
}