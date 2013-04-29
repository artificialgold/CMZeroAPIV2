using System;
using System.Linq;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Messages;

using Raven.Client.Linq;

namespace CMZero.API.DataAccess.Repositories
{
    public class OrganisationRepository : RepositoryBase<Organisation>, IOrganisationRepository
    {
        //TODO: Make this more efficient if possible, first or default seems poor but might be ok
        public Organisation GetByName(string nameToSearchBy)
        {
            try
            {
                using (var session = GetSession())
                {
                    var organisations = (from c in session.Query<Organisation>()
                                        where c.Name == nameToSearchBy
                                        select c);
                    return organisations.ToArray().FirstOrDefault();
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
