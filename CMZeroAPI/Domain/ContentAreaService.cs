using System;
using System.Collections.Generic;
using System.Linq;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.Applications;
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
            if (NameExistsInCollection(contentArea.CollectionId, contentArea.Name)) { throw new ContentAreaNameAlreadyExistsInCollectionException(); }

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

        public new ContentArea Update(ContentArea contentArea)
        {
            ContentArea existingContentArea = _contentAreaRepository.GetById(contentArea.Id);
            if (existingContentArea == null) throw new ItemNotFoundException();
            if (existingContentArea.ApplicationId != contentArea.ApplicationId) throw new ApplicationIdNotValidException();

            var collections = _collectionService.GetCollectionsForApplication(contentArea.ApplicationId);
            var x = (from c in collections where c.Id == contentArea.CollectionId select c).Any();
            if (!x) throw new CollectionIdNotPartOfApplicationException();

            return base.Update(contentArea);
        }

        public IEnumerable<ContentArea> GetByCollection(string collectionId)
        {
            try
            {
                var collection = _collectionService.GetById(collectionId);
                if (collection == null)
                    throw new CollectionIdNotValidException();

                return _contentAreaRepository.ContentAreasInCollection(collectionId);
            }
            catch (ItemNotFoundException)
            {
                throw new CollectionIdNotValidException();
            }
        }
    }
}