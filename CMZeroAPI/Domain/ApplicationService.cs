using System.Collections.Generic;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Domain.ApiKey;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.Organisations;

namespace CMZero.API.Domain
{
    public class ApplicationService : BaseService<Application>, IApplicationService
    {
        private readonly IOrganisationService organisationService;

        private IApiKeyCreator _apiKeyCreator;

        public ApplicationService(IApplicationRepository applicationRepository, IOrganisationService organisationService, IApiKeyCreator apiKeyCreator)
        {
            this.organisationService = organisationService;
            _apiKeyCreator = apiKeyCreator;
            Repository = applicationRepository;
        }

        public new Application Create(Application application)
        {
            if (!organisationService.IdExists(application.OrganisationId))
                throw new OrganisationDoesNotExistException();

            application.ApiKey = _apiKeyCreator.Create();

            base.Create(application);

            return application;
        }

        public IList<Application> GetApplicationsForOrganisation(string organisationId)
        {
            try
            {
                organisationService.GetById(organisationId);
            }
            catch (ItemNotFoundException)
            {
                throw new OrganisationIdNotValidException();
            }

            IApplicationRepository applicationRepository = (IApplicationRepository)Repository;

            return applicationRepository.GetApplicationsForOrganisation(organisationId);
        }

        public Application GetApplicationByApiKey(string apiKey)
        {
            IApplicationRepository applicationRepository = (IApplicationRepository)Repository;

            return applicationRepository.GetByApiKey(apiKey);
        }

        public new Application Update(Application application)
        {
            var applicationToCheck = GetById(application.Id);
            if (applicationToCheck.OrganisationId != application.OrganisationId)
                throw new OrganisationIdNotValidException();

            base.Update(application);

            return application;
        }
    }
}