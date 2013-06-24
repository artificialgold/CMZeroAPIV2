using System.Collections.Generic;

using CMZero.API.Messages;

namespace CMZero.API.ServiceAgent
{
    public interface IContentAreasServiceAgent
    {
        ContentArea Get(string id);

        ContentArea Post(ContentArea contentArea);

        ContentArea Put(ContentArea contentArea);

        IEnumerable<ContentArea> GetByCollection(string collectionId);
    }
}