using System.Collections.Generic;

using CMZero.API.Messages;

namespace CMZero.API.Domain
{
    public interface IContentAreaService : IBaseService<ContentArea>
    {
        IEnumerable<ContentArea> GetByCollection(string collectionId);

        IEnumerable<ContentArea> GetByCollectionNameAndApiKey(string apiKey, string collectionName);
    }
}