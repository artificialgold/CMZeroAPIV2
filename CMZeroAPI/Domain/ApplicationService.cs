using System;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions.Organisations;

namespace CMZero.API.Domain
{
    public class ApplicationService : BaseService<Application>, IApplicationService
    {
        private readonly IOrganisationService organisationService;

        public ApplicationService(IApplicationRepository applicationRepository, IOrganisationService organisationService)
        {
            this.organisationService = organisationService;
            Repository = applicationRepository;
        }

        public new Application Create(Application application)
        {
           if (!organisationService.IdExists(application.OrganisationId))
                throw new OrganisationDoesNotExistException();

            Repository.Create(application);
            
            return application;
        }
    }
}