using System;
using System.Collections.Generic;
using System.Linq;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions.ApiKeys;

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

        public Application GetByApiKey(string apiKey)
        {
            try
            {
                using (var session = GetSession())
                {
                    var result = (from a in session.Query<Application>() where a.ApiKey == apiKey select a);

                    if (result.Count() == 1) return result.First();
                    throw new ApiKeyNotValidException();
                }
            }
            catch (Exception ex)
            {
                //TODO: Log and get correct exceptions
                throw ex;
            }
        }
    }
}