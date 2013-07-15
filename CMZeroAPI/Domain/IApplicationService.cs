using System.Collections.Generic;

using CMZero.API.Messages;

namespace CMZero.API.Domain
{
    public interface IApplicationService : IBaseService<Application>
    {
        IList<Application> GetApplicationsForOrganisation(string organisationId);

        Application GetApplicationByApiKey(string apiKey);
    }
}