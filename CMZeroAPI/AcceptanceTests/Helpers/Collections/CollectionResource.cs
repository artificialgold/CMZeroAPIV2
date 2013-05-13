using System;
using System.Globalization;

using AcceptanceTests.Helpers.Applications;

using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.API.Messages.Exceptions.Collections;
using CMZero.API.Messages.Exceptions.Organisations;
using CMZero.API.Messages.Responses.Collections;
using CMZero.API.ServiceAgent;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Helpers.Collections
{
    public class CollectionResource : IResource
    {
        private ICollectionServiceAgent _collectionServiceAgent;

        public CollectionResource(ICollectionServiceAgent collectionServiceAgent)
        {
            _collectionServiceAgent = collectionServiceAgent;
        }

        public Collection NewCollection()
        {
            return NewCollectionWithSpecifiedName(
                string.Format("Test{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture)));
        }

        public Collection NewCollectionWithSpecifiedName(string name)
        {
            ApplicationResource applicationResource = new Api().Resource<ApplicationResource>();
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
            return _collectionServiceAgent.Get(id).Collection;
        }

        public BadRequestException NewCollectionWithUnspecifiedName()
        {
            ApplicationResource applicationResource = new Api().Resource<ApplicationResource>();
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

            throw new SpecFlowException("Expected BadRequestException was not caught");
        }

        public ApplicationIdNotPartOfOrganisationException NewCollectionWithApplicationIdNotPartOfOrganisationId()
        {
            ApplicationResource applicationResource = new Api().Resource<ApplicationResource>();
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
            ApplicationResource applicationResource = new Api().Resource<ApplicationResource>();
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
            ApplicationResource applicationResource = new Api().Resource<ApplicationResource>();
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

            throw new SpecFlowException("Did not get expected CollectionNameAlreadyExistsException");
        }

        public ItemNotFoundException GetCollectionThatDoesNotExist()
        {
            try
            {
                _collectionServiceAgent.Get("IDoNotExist");
            }
            catch (ItemNotFoundException ex)
            {
                return ex;
            }

            throw new SpecFlowException("Expected HttpResponseException not thrown");
        }

        public Collection UpdateCollection(Collection collection)
        {
            return _collectionServiceAgent.Put(collection).Collection;
        }

        public BadRequestException UpdateCollectionWithUnspecifiedName(Collection collection)
        {
            try
            {
                collection.Name = string.Empty;
                _collectionServiceAgent.Put(collection);
            }
            catch (BadRequestException ex)
            {
                return ex;
            }

            throw new SpecFlowException("Expected Bad Request Exception not caught");
        }

        public ItemNotFoundException UpdateCollectionThatDoesNotExist()
        {
            try
            {
                _collectionServiceAgent.Put(
                    new Collection
                        {
                            Id = "notExisting",
                            Name = "IDoNotExist",
                            ApplicationId = "pp",
                            OrganisationId = "jhj"
                        });
            }
            catch (ItemNotFoundException ex)
            {
                return ex;
            }

            throw new SpecFlowException("Expected ItemNotFoundException was not caught");
        }

        public ApplicationIdNotValidException UpdateCollectionToHaveDifferentApplicationId()
        {
            try
            {
                var collection = NewCollection();
                collection.ApplicationId = "newApplicationId";
                _collectionServiceAgent.Put(collection);
            }
            catch (ApplicationIdNotValidException ex)
            {
                return ex;
            }

            throw new SpecFlowException("Expected ApplicationIdNotValidException was not caught");
        }

        public OrganisationIdNotValidException UpdateCollectionToHaveDifferentOrganisationId()
        {
            try
            {
                var collection = NewCollection();
                collection.OrganisationId = "newOrganisationId";
                _collectionServiceAgent.Put(collection);
            }
            catch (OrganisationIdNotValidException ex)
            {
                return ex;
            }

            throw new SpecFlowException("Expected OrganisationIdNotValidException was not caught");
        }
    }
}