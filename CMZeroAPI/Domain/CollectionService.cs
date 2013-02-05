using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Messages;

namespace CMZero.API.Domain
{
    public class CollectionService : BaseService<Collection>, ICollectionService
    {
        public CollectionService(ICollectionRepository collectionRepository)
        {
            Repository = collectionRepository;
        }
    }
}