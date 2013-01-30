using CMZero.API.Messages;

namespace CMZero.API.Domain.RepositoryInterfaces
{
    public interface IOrganisationRepository : IRepository<Organisation>
    {
        Organisation GetByName(string nameToSearchBy);
    }
}