using System.Collections.Generic;
using System.Linq;
using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Domain.ApiKey;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.API.Messages.Exceptions.Organisations;

namespace CMZero.API.Domain
{
    public class ApplicationService : BaseService<Application>, IApplicationService
    {
        private readonly IOrganisationService _organisationService;

        private readonly IApiKeyCreator _apiKeyCreator;

        public ApplicationService(IApplicationRepository applicationRepository, IOrganisationService organisationService, IApiKeyCreator apiKeyCreator)
        {
            _organisationService = organisationService;
            _apiKeyCreator = apiKeyCreator;
            Repository = applicationRepository;
        }

        public new Application Create(Application application)
        {
            if (!_organisationService.IdExists(application.OrganisationId))
                throw new OrganisationDoesNotExistException();

            IList<Application> applications = GetApplicationsForOrganisation(application.OrganisationId);
            bool nameExists = (from a in applications
                               where a.Name == application.Name
                               select a).Any();

            if (nameExists) throw new ApplicationNameAlreadyExistsException();

            application.ApiKey = _apiKeyCreator.Create();

            base.Create(application);

            return application;
        }

        public IList<Application> GetApplicationsForOrganisation(string organisationId)
        {
            //TODO: Change to IdExists
            try
            {
                _organisationService.GetById(organisationId);
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