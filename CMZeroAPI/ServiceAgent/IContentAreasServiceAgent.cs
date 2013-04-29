using CMZero.API.Messages;
using CMZero.API.Messages.Responses.ContentAreas;

namespace CMZero.API.ServiceAgent
{
    public interface IContentAreasServiceAgent
    {
        GetContentAreaResponse Get(string id);

        PostContentAreaResponse Post(ContentArea contentArea);

        PutContentAreaResponse Put(ContentArea contentArea);
    }
}