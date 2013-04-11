using System.Collections.Generic;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Domain;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.API.Messages.Exceptions.Collections;

using NUnit.Framework;

using Rhino.Mocks;

namespace UnitTests.Domain
{
    public class CollectionServiceTests
    {
        public class Given_a_CollectionService
        {
            protected CollectionService collectionService;

            protected ICollectionRepository collectionRepository;

            protected IApplicationService applicationService;

            [SetUp]
            public virtual void SetUp()
            {
                collectionRepository = MockRepository.GenerateMock<ICollectionRepository>();

                applicationService = MockRepository.GenerateMock<IApplicationService>();
                collectionService = new CollectionService(collectionRepository, applicationService);
            }
        }

        [TestFixture]
        public class When_I_call_create_with_an_application_not_in_the_organisation : Given_a_CollectionService
        {
            private string applicationIdNotInReturnedApplications = "appNotValidId";

            private string organisationIdToQueryWith = "organisationId";

            private ApplicationIdNotPartOfOrganisationException exceptionReturned;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                collectionRepository.Stub(x => x.GetCollectionsForApplication("appid", "orgid"))
                    .IgnoreArguments()
                    .Return(new List<Collection> { new Collection() });
                applicationService.Stub(x => x.GetApplicationsForOrganisation(organisationIdToQueryWith)).Return(new List<Application>());
                try
                {
                    collectionService.Create(
                        new Collection
                            {
                                Name = "I do not yet exist",
                                ApplicationId = applicationIdNotInReturnedApplications,
                                OrganisationId = organisationIdToQueryWith
                            });
                }
                catch (ApplicationIdNotPartOfOrganisationException exception)
                {

                    exceptionReturned = exception;
                }
            }

            [Test]
            public void it_should_throw_exception_ApplicationIdNotPartOfOrganisationException()
            {
                Assert.NotNull(exceptionReturned);
            }
        }

        [TestFixture]
        public class When_I_call_Create_with_a_collection_name_that_already_exists_in_the_collection : Given_a_CollectionService
        {
            private CollectionNameAlreadyExistsException exception;

            private string applicationId = "ghjkl";
            private string organisationId = "hjkjnjk";

            private const string nameThatAlreadyExists = "nameThatExists";

            [SetUp]
            public new virtual void SetUp()
            {
                collectionRepository.Stub(x => x.GetCollectionsForApplication(applicationId, organisationId))
                                    .Return(new List<Collection> { new Collection { Name = nameThatAlreadyExists } });

                try
                {
                    collectionService.Create(new Collection { Name = nameThatAlreadyExists, ApplicationId = applicationId, OrganisationId = organisationId });
                }
                catch (CollectionNameAlreadyExistsException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_throw_CollectionNameAlreadyExistsException()
            {
                Assert.NotNull(exception);
            }
        }
    }
}