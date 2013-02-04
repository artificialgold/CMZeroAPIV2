using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Messages;

namespace CMZero.API.Domain
{
    public class OrganisationService : BaseService<Organisation>
    {
        public OrganisationService(IOrganisationRepository organisationRepository)
        {
            Repository = organisationRepository;
        }
    }
}
