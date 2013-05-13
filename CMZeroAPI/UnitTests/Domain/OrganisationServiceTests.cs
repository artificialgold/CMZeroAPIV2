using System;
using System.Collections.Generic;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Domain;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;

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

            private DateTime startTime;

            private DateTime endTime;
            private Organisation outcome;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                startTime = DateTime.UtcNow;
                outcome = OrganisationService.Create(organisation);
                endTime = DateTime.UtcNow;
            }

            [Test]
            public void it_should_call_OrganisationRepositoryAdd_and_parameter_as_return()
            {
                Assert.AreEqual(outcome, organisation);
            }

            [Test]
            public void it_should_set_created_to_current_datetime()
            {
                var testOutcome =
                    (outcome.Created >= startTime) && (outcome.Created <= endTime);
                Assert.True(testOutcome);
            }

            [Test]
            public void it_should_set_updated_to_current_datetime()
            {
                var testOutcome = (outcome.Updated >= startTime) && (outcome.Updated <= endTime);
                Assert.True(testOutcome);
            }
        }

        public class When_I_call_Update : Given_an_OrganisationService
        {
            private readonly Organisation organisation = new Organisation { Active = true, Name = "hjkljh",Id=organisationId };

            private DateTime startTime;

            private DateTime endTime;
            private Organisation outcome;

            private static string organisationId="ghjkl";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                OrganisationRepository.Stub(x => x.GetById(organisationId)).Return(new Organisation());
                startTime = DateTime.UtcNow;
                outcome = OrganisationService.Update(organisation);
                endTime = DateTime.UtcNow;
            }

            [Test]
            public void it_should_return_outcome_of_update_operation()
            {
                Assert.AreEqual(outcome, organisation);
            }

            [Test]
            public void it_should_set_updated_to_current_datetime()
            {
                var testOutcome = (outcome.Updated >= startTime) && (outcome.Updated <= endTime);
                Assert.True(testOutcome);
            }
        }

        public class When_call_GetById_with_existing_id : Given_an_OrganisationService
        {
            private string id = "ghjkl";
            private Organisation outcome;
            private readonly Organisation _organisationFromRepository = new Organisation { Active = true };

            [SetUp]
            public new virtual void SetUp()
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

        public class When_call_GetByID_with_non_existent_id : Given_an_OrganisationService
        {
            private string id = "ghjklvchjk";

            private Organisation outcome;

            private readonly Organisation _organisationFromRepository = null;

            private Exception exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                OrganisationRepository.Stub(x => x.GetById(id)).Return(_organisationFromRepository);
                try
                {
                    outcome = OrganisationService.GetById(id);
                }
                catch (ItemNotFoundException ex)
                {
                    exception = ex;
                }
            }

            [Test]
            public void it_should_return_organisation_from_repository()
            {
                Assert.NotNull(exception);
            }
        }

        [TestFixture]
        public class When_I_call_GetAll : Given_an_OrganisationService
        {
            private IEnumerable<Organisation> outcome;

            private readonly IEnumerable<Organisation> organisationsToReturn = new Organisation[3];

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                OrganisationRepository.Stub(x => x.GetAll()).Return(organisationsToReturn);
                outcome = OrganisationService.GetAll();
            }

            [Test]
            public void it_should_return_organisations_from_repository()
            {
                Assert.AreEqual(outcome, organisationsToReturn);
            }
        }

        [TestFixture]
        public class When_I_call_IdExists : Given_an_OrganisationService
        {
            private bool result;

            private string shouldReturnAnOrganisation;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();

                shouldReturnAnOrganisation = "should_return_an_organisation";
                OrganisationRepository.Stub(x=>x.IdExists(shouldReturnAnOrganisation)).Return(true);

                result = OrganisationService.IdExists(shouldReturnAnOrganisation);
            }

            [Test]
            public void it_should_return_value_from_Repository()
            {
                Assert.True(result);
            }
        }

    }
}
