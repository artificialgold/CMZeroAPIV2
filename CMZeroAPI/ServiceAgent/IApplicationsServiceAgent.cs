using System.Collections.Generic;

using CMZero.API.Messages;

namespace CMZero.API.ServiceAgent
{
    public interface IApplicationsServiceAgent
    {
        Application Get(string id);

        IEnumerable<Application> Get();

        Application Post(Application application);

        Application Put(Application application);

        IEnumerable<Application> GetByOrganisation(string organisationId);
    }
}