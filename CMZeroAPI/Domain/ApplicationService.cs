using System.Collections.Generic;

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

            base.Create(application);
            
            return application;
        }

        //private bool ApplicationIsInOrganisation(string applicationId, string organisationId)
        //{
        //    var result = appl
        //}

        public IList<Application> GetApplicationsForOrganisation(string organisationId)
        {
            IApplicationRepository applicationRepository = (IApplicationRepository)Repository;

            return applicationRepository.GetApplicationsForOrganisation(organisationId);
        }
    }
}