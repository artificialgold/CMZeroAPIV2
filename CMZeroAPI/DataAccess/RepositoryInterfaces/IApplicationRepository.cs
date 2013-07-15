using System.Collections.Generic;

using CMZero.API.Messages;

namespace CMZero.API.DataAccess.RepositoryInterfaces
{
    public interface IApplicationRepository : IRepository<Application>
    {
        IList<Application> GetApplicationsForOrganisation(string organisationId);

        Application GetByApiKey(string apiKey);
    }
}