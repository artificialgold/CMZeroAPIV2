using CMZero.API.Messages;
using CMZero.API.ServiceAgent;

namespace AcceptanceTests.Helpers.Collections
{
    public class CollectionResource:IResource
    {
        private ICollectionServiceAgent collectionServiceAgent;

        public CollectionResource(ICollectionServiceAgent collectionServiceAgent)
        {
            collectionServiceAgent = collectionServiceAgent;
        }

        public Collection NewCollectionWithSpecifiedName(string name)
        {
            throw new System.NotImplementedException();
        }

        public Collection GetCollection(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
