using System.Collections.Generic;
using System.Linq;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.API.Messages.Exceptions.Collections;
using CMZero.API.Messages.Exceptions.Organisations;

namespace CMZero.API.Domain
{
    public class CollectionService : BaseService<Collection>, ICollectionService
    {
        private readonly IApplicationService _applicationService;

        public CollectionService(ICollectionRepository collectionRepository, IApplicationService applicationService)
        {
            _applicationService = applicationService;
            Repository = collectionRepository;
        }

        public new Collection Create(Collection collection)
        {
            if (DoesNameExistInApplication(collection))
                throw new CollectionNameAlreadyExistsException();

            if (!IsApplicationInOrganisation(collection))
                throw new ApplicationIdNotPartOfOrganisationException();

            return base.Create(collection);
        }

        private bool DoesNameExistInApplication(Collection collection)
        {
            var collectionRepository = (ICollectionRepository)Repository;

            var collectionsInApplication = collectionRepository.GetCollectionsForApplication(
                collection.ApplicationId, collection.OrganisationId);

            return (from c in collectionsInApplication where c.Name == collection.Name select c).Any();
        }

        private bool IsApplicationInOrganisation(Collection collection)
        {
            IList<Application> applicationsInOrganisation = _applicationService.GetApplicationsForOrganisation(
                collection.OrganisationId);

            return (from a in applicationsInOrganisation
                    where a.Id == collection.ApplicationId
                    select a).Count() == 1;
        }

        public new Collection Update(Collection collection)
        {
            var originalCollection = GetById(collection.Id);
            if (collection.ApplicationId != originalCollection.ApplicationId) throw new ApplicationIdNotValidException();
            if (collection.OrganisationId != originalCollection.OrganisationId) throw new OrganisationIdNotValidException();

            return base.Update(collection);
        }

        public IList<Collection> GetCollectionsForApplication(string applicationId)
        {
            var collectionRepository = (ICollectionRepository)Repository;

            var application = _applicationService.GetById(applicationId);

            return collectionRepository.GetCollectionsForApplication(applicationId, application.OrganisationId);
        }

        public Collection GetCollectionByApiKeyAndName(string apiKey, string collectionName)
        {
            var application = _applicationService.GetApplicationByApiKey(apiKey);
            
            var collectionRepository = (ICollectionRepository)Repository;
            var collections = collectionRepository.GetCollectionsForApplication(application.Id, application.OrganisationId);
            var collectionsWithName = (from c in collections where c.Name == collectionName select c);

            if (!collectionsWithName.Any()) throw new CollectionNameNotValidException();

            return collectionsWithName.First();
        }

        public IEnumerable<Collection> GetCollectionsByApiKey(string apiKey)
        {
            var application = _applicationService.GetApplicationByApiKey(apiKey);

            var collectionRepository = (ICollectionRepository) Repository;
            return collectionRepository.GetCollectionsForApplication(application.Id, application.OrganisationId);
        }
    }
}