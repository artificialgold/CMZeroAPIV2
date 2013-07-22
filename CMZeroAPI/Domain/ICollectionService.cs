using System.Collections.Generic;

using CMZero.API.Messages;

namespace CMZero.API.Domain
{
    public interface ICollectionService : IBaseService<Collection>
    {
        IList<Collection> GetCollectionsForApplication(string applicationId);

        Collection GetCollectionByApiKeyAndName(string apiKey, string collectionName);

        IEnumerable<Collection> GetCollectionsByApiKey(string apiKey);
    }
}