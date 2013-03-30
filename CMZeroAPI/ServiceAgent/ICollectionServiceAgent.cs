using CMZero.API.Messages;
using CMZero.API.Messages.Responses.Collections;

namespace CMZero.API.ServiceAgent
{
    public interface ICollectionServiceAgent
    {
        GetCollectionResponse Get(string id, string applicationId);

        PostCollectionResponse Post(Collection collection);

        PutCollectionResponse Put(Collection collection);
    }
}