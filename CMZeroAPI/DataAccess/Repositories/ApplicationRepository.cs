using System;
using System.Collections.Generic;
using System.Linq;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Messages;

namespace CMZero.API.DataAccess.Repositories
{
    public class ApplicationRepository : RepositoryBase<Application>, IApplicationRepository
    {
        public IList<Application> GetApplicationsForOrganisation(string organisationId)
        {
            try
            {
                using (var session = GetSession())
                {
                    var result =
                        (from a in session.Query<Application>() where a.OrganisationId == organisationId select a);

                    return result.ToList();
                }
            }
            catch (Exception)
            {
                //TODO: Get correct exceptions
                throw new Exception();
            }
        }
    }
}