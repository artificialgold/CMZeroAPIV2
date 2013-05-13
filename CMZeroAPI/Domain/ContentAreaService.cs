using System;
using System.Collections.Generic;
using System.Linq;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.Collections;
using CMZero.API.Messages.Exceptions.ContentAreas;

namespace CMZero.API.Domain
{
    public class ContentAreaService : BaseService<ContentArea>, IContentAreaService
    {
        private readonly IContentAreaRepository _contentAreaRepository;

        private readonly ICollectionService _collectionService;

        public ContentAreaService(IContentAreaRepository contentAreaRepository, ICollectionService collectionService)
        {
            _contentAreaRepository = contentAreaRepository;
            _collectionService = collectionService;
            Repository = contentAreaRepository;
        }

        public new ContentArea Create(ContentArea contentArea)
        {
            if (NameExistsInCollection(contentArea.CollectionId, contentArea.Name)) {throw new ContentAreaNameAlreadyExistsInCollectionException();}

            Collection collection;

            try
            {
               collection = _collectionService.GetById(contentArea.CollectionId);
            }
            catch (ItemNotFoundException)
            {
                throw new CollectionIdNotValidException();
            }

            if (collection.ApplicationId != contentArea.ApplicationId) throw new CollectionIdNotPartOfApplicationException();

            return base.Create(contentArea);
        }

        private bool NameExistsInCollection(string collectionId, string name)
        {
            IList<ContentArea> contentAreas = _contentAreaRepository.ContentAreasInCollection(collectionId);

            return (from ca in contentAreas where ca.Name == name select ca).Any();
        }
    }
}