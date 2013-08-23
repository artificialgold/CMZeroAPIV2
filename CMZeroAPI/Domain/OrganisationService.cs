using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions.Organisations;

namespace CMZero.API.Domain
{
    public class OrganisationService : BaseService<Organisation>, IOrganisationService
    {
        public OrganisationService(IOrganisationRepository organisationRepository)
        {
            Repository = organisationRepository;
        }

        public new Organisation Create(Organisation organisation)
        {
            CheckIfNameExists(organisation);

            return base.Create(organisation);
        }

        private void CheckIfNameExists(Organisation organisation)
        {
            var organisationRepository = (IOrganisationRepository)Repository;

            var organisationWithName = organisationRepository.GetByName(organisation.Name);
            bool shouldThrowException = organisationWithName != null && 
                                            organisation.Id != null && 
                                            organisationWithName.Id != organisation.Id;

            if (shouldThrowException) 
                throw new OrganisationNameAlreadyExistsException();
        }

        public new Organisation Update(Organisation organisation)
        {
            CheckIfNameExists(organisation);

            return base.Update(organisation);
        }
    }
}
