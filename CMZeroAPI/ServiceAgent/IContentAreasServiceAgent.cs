using CMZero.API.Messages;
using CMZero.API.Messages.Responses.ContentAreas;

namespace CMZero.API.ServiceAgent
{
    public interface IContentAreasServiceAgent
    {
        ContentArea Get(string id);

        ContentArea Post(ContentArea contentArea);

        ContentArea Put(ContentArea contentArea);
    }
}