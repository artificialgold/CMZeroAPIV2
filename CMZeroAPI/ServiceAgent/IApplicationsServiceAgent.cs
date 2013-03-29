using CMZero.API.Messages;
using CMZero.API.Messages.Responses.Applications;

namespace CMZero.API.ServiceAgent
{
    public interface IApplicationsServiceAgent
    {
        GetApplicationResponse Get(string id);

        GetApplicationsResponse Get();

        PostApplicationResponse Post(Application application);

        PutApplicationResponse Put(Application application);
    }
}