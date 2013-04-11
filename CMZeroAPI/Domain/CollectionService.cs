using System;
using System.Collections.Generic;
using System.Linq;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.API.Messages.Exceptions.Collections;

namespace CMZero.API.Domain
{
    public class CollectionService : BaseService<Collection>, ICollectionService
    {
        private readonly IApplicationService applicationService;

        public CollectionService(ICollectionRepository collectionRepository, IApplicationService applicationService)
        {
            this.applicationService = applicationService;
            Repository = collectionRepository;
        }

        public new Collection Create(Collection collection)
        {
            if (NameExistsInApplication(collection))
                throw new CollectionNameAlreadyExistsException();

            if (!ApplicationInOrganisation(collection))
                throw new ApplicationIdNotPartOfOrganisationException();

            return base.Create(collection);
        }

        private bool NameExistsInApplication(Collection collection)
        {
            var collectionRepository = (ICollectionRepository)Repository;

            var collectionsInApplication = collectionRepository.GetCollectionsForApplication(
                collection.ApplicationId, collection.OrganisationId);

            return (from c in collectionsInApplication where c.Name == collection.Name select c).Any();
        }

        private bool ApplicationInOrganisation(Collection collection)
        {
            IList<Application> applicationsInOrganisation = applicationService.GetApplicationsForOrganisation(
                collection.OrganisationId);

            return (from a in applicationsInOrganisation
                             where a.Id == collection.ApplicationId
                             select a).Count() == 1;
        }
    }
}