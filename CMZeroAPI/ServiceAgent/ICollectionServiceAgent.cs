using System.Collections.Generic;
using CMZero.API.Messages;

namespace CMZero.API.ServiceAgent
{
    public interface ICollectionServiceAgent
    {
        Collection Get(string id);

        Collection Post(Collection collection);

        Collection Put(Collection collection);
        IEnumerable<Collection> GetByApiKey(string apiKey);
    }
}