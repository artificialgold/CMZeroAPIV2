using System;
using System.Globalization;

using AcceptanceTests.Helpers.Collections;

using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.API.Messages.Exceptions.Collections;
using CMZero.API.Messages.Exceptions.ContentAreas;
using CMZero.API.Messages.Responses.ContentAreas;
using CMZero.API.ServiceAgent;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Helpers.ContentAreas
{
    public class ContentAreaResource : IResource
    {
        private readonly IContentAreasServiceAgent _contentAreasServiceAgent;

        public ContentAreaResource(IContentAreasServiceAgent contentAreasServiceAgent)
        {
            _contentAreasServiceAgent = contentAreasServiceAgent;
        }

        public ContentArea NewContentArea()
        {
            return
                NewContentAreaWithSpecifiedName(
                    string.Format("Test{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture)));
        }

        private ContentArea NewContentAreaWithSpecifiedName(string name)
        {
            CollectionResource collectionResource = new Api().Resource<CollectionResource>();
            var newCollection = collectionResource.NewCollection();
            var collectionId = newCollection.Id;
            var applicationId = newCollection.ApplicationId;

            PostContentAreaResponse response =
                _contentAreasServiceAgent.Post(
                    new ContentArea
                    {
                        Active = true,
                        Name = name,
                        ApplicationId = applicationId,
                        Content = "testContentArea",
                        ContentType = ContentAreaType.HtmlArea,
                        CollectionId = collectionId
                    });

            return response.ContentArea;
        }

        public ContentArea GetContentArea(string contentAreaId)
        {
            return _contentAreasServiceAgent.Get(contentAreaId).ContentArea;
        }

        public ContentAreaNameAlreadyExistsInCollectionException NewContentAreaWithExistingName()
        {
            try
            {
                CollectionResource collectionResource = new Api().Resource<CollectionResource>();
                var newCollection = collectionResource.NewCollection();
                var collectionId = newCollection.Id;
                var applicationId = newCollection.ApplicationId;

                _contentAreasServiceAgent.Post(
                    new ContentArea
                    {
                        Active = true,
                        Name = "sameName",
                        ApplicationId = applicationId,
                        Content = "testContentArea",
                        ContentType = ContentAreaType.HtmlArea,
                        CollectionId = collectionId
                    });

                _contentAreasServiceAgent.Post(
                    new ContentArea
                    {
                        Active = true,
                        Name = "sameName",
                        ApplicationId = applicationId,
                        Content = "testContentArea",
                        ContentType = ContentAreaType.Label,
                        CollectionId = collectionId
                    });
            }
            catch (ContentAreaNameAlreadyExistsInCollectionException ex)
            {
                return ex;
            }

            throw new SpecFlowException("Expected ContentAreaNameAlreadyExistsInCollectionException was not caught");
        }

        public CollectionIdNotValidException NewContentAreaForCollectionThatDoesNotExist()
        {
            try
            {
                const string CollectionId = "madeUpCollectionId";
                const string ApplicationId = "madeUpApplicationId";

                _contentAreasServiceAgent.Post(
                    new ContentArea
                    {
                        Active = true,
                        Name = "name",
                        ApplicationId = ApplicationId,
                        Content = "testContentArea",
                        ContentType = ContentAreaType.HtmlArea,
                        CollectionId = CollectionId
                    });
            }
            catch (CollectionIdNotValidException ex)
            {
                return ex;
            }

            throw new SpecFlowException("Expected CollectionIdNotValidException was not caught");
        }

        public CollectionIdNotPartOfApplicationException NewContentAreaWithInvalidApplicationId()
        {
            try
            {
                CollectionResource collectionResource = new Api().Resource<CollectionResource>();
                var newCollection = collectionResource.NewCollection();
                var collectionId = newCollection.Id;
                var applicationId = newCollection.ApplicationId;

                _contentAreasServiceAgent.Post(
                    new ContentArea
                        {
                            Active = true,
                            Name = "name",
                            ApplicationId = applicationId + "partToMakeApplicationIdDifferent",
                            Content = "testContentArea",
                            ContentType = ContentAreaType.HtmlArea,
                            CollectionId = collectionId
                        });
            }
            catch (CollectionIdNotPartOfApplicationException ex)
            {
                return ex;
            }
            throw new SpecFlowException("Expected CollectionIdNotPartOfApplicationException was not caught");
        }

        public BadRequestException NewContentAreaWithBlankApplicationId()
        {
            try
            {
                _contentAreasServiceAgent.Post(
                    new ContentArea
                        {
                            Active = true,
                            Name = "name",
                            ApplicationId = null,
                            Content = "testContentArea",
                            ContentType = ContentAreaType.HtmlArea,
                            CollectionId = "doesNotExistButDoesNotMatterAsApplicationIdIsBlank"
                        });
            }
            catch (BadRequestException ex)
            {
                return ex;
            }

            throw new SpecFlowException("Expected exception ApplicationIdNotValidException was not caught");
        }

        public ItemNotFoundException GetContentAreaThatDoesNotExist()
        {
            try
            {
                _contentAreasServiceAgent.Get("IDoNotExist");
            }
            catch (ItemNotFoundException ex)
            {
                return ex;
            }

            throw new SpecFlowException("Expected HttpResponseException not thrown");
        }
    }
}