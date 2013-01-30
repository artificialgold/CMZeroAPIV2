using System;

using CMZero.API.Domain;
using CMZero.API.Domain.RepositoryInterfaces;
using CMZero.API.Messages;

using NUnit.Framework;

using Rhino.Mocks;

namespace UnitTests.Domain
{
    public class OrganisationServiceTests
    {
        public class Given_an_OrganisationService
        {
            protected OrganisationService OrganisationService;

            protected IOrganisationRepository OrganisationRepository;

            [SetUp]
            public virtual void SetUp()
            {
                OrganisationRepository = MockRepository.GenerateMock<IOrganisationRepository>();

                OrganisationService = new OrganisationService(OrganisationRepository);
            }
        }

        public class When_I_call_Create_organisation : Given_an_OrganisationService
        {
            private readonly Organisation organisation = new Organisation();

            private Organisation outcome;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                outcome = OrganisationService.Create(organisation);
            }

            [Test]
            public void it_should_call_OrganisationRepositoryAdd_and_parameter_as_return()
            {
                Assert.AreEqual(outcome, organisation);
            }
        }

        public class When_I_call_Update : Given_an_OrganisationService
        {
            private readonly Organisation organisation = new Organisation { Active = true, Name = "hjkljh" };

            private Organisation outcome;

            [SetUp]
            public virtual void SetUp()
            {
                base.SetUp();
                outcome = OrganisationService.Update(organisation);
            }

            [Test]
            public void it_should_return_outcome_of_update_operation()
            {
                Assert.AreEqual(outcome, organisation);
            }
        }

        public class When_call_GetById : Given_an_OrganisationService
        {
            private string id = "ghjkl";
            private Organisation outcome = new Organisation { Created = DateTime.Now };
            private readonly Organisation _organisationFromRepository = new Organisation { Active = true };

            [SetUp]
            public virtual void SetUp()
            {
                base.SetUp();
                OrganisationRepository.Stub(x => x.GetById(id)).Return(_organisationFromRepository);
                outcome = OrganisationService.GetById(id);
            }

            [Test]
            public void it_should_return_organisation_from_repository()
            {
                Assert.AreEqual(outcome, _organisationFromRepository);
            }
        }
    }
}
