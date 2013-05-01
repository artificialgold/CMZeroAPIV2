using System;
using System.Collections.Generic;
using System.Linq;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions.ContentAreas;

namespace CMZero.API.Domain
{
    public class ContentAreaService : BaseService<ContentArea>, IContentAreaService
    {
        private readonly IContentAreaRepository _contentAreaRepository;

        public ContentAreaService(IContentAreaRepository contentAreaRepository)
        {
            _contentAreaRepository = contentAreaRepository;
        }

        public new ContentArea Create(ContentArea contentArea)
        {
            if (NameExistsInCollection(contentArea.CollectionId, contentArea.Name)) throw new ContentAreaNameAlreadyExistsInCollectionException();

            throw new NotImplementedException();
        }

        private bool NameExistsInCollection(string collectionId, string name)
        {
            IList<ContentArea> contentAreas = _contentAreaRepository.ContentAreasInCollection(collectionId);

            return (from ca in contentAreas where ca.Name == name select ca).Any();
        }
    }
}