using CMZero.API.Messages;

namespace CMZero.API.DataAccess.RepositoryInterfaces
{
    public interface IOrganisationRepository : IRepository<Organisation>
    {
        Organisation GetByName(string nameToSearchBy);
    }
}