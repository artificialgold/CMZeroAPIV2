using CMZero.API.Domain.RepositoryInterfaces;
using CMZero.API.Messages;

namespace CMZero.API.Domain
{
    public class CollectionService : BaseService<Collection>
    {
        public CollectionService(ICollectionRepository collectionRepository)
        {
            Repository = collectionRepository;
        }
    }
}