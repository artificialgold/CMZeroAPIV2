using System;
using System.Collections.Generic;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Domain;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.Organisations;
using NUnit.Framework;

using Rhino.Mocks;
using Shouldly;

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

        public class When_I_call_Create_organisation_with_valid_parameters : Given_an_OrganisationService
        {
            private readonly Organisation _organisation = new Organisation();

            private DateTime _startTime;

            private DateTime _endTime;
            private Organisation _outcome;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                _startTime = DateTime.UtcNow;
                _outcome = OrganisationService.Create(_organisation);
                _endTime = DateTime.UtcNow;
            }

            [Test]
            public void it_should_call_OrganisationRepositoryAdd_and_parameter_as_return()
            {
                Assert.AreEqual(_outcome, _organisation);
            }

            [Test]
            public void it_should_set_created_to_current_datetime()
            {
                var testOutcome =
                    (_outcome.Created >= _startTime) && (_outcome.Created <= _endTime);
                Assert.True(testOutcome);
            }

            [Test]
            public void it_should_set_updated_to_current_datetime()
            {
                var testOutcome = (_outcome.Updated >= _startTime) && (_outcome.Updated <= _endTime);
                Assert.True(testOutcome);
            }
        }

        [TestFixture]
        public class When_I_call_create_with_name_that_already_exists : Given_an_OrganisationService
        {
            private OrganisationNameAlreadyExistsException _exception;
            private const string NameThatAlreadyExists = "alreadyExists";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                try
                {
                    OrganisationRepository.Stub(x => x.GetByName(NameThatAlreadyExists)).Return(new Organisation());
                    OrganisationService.Create(new Organisation { Name = NameThatAlreadyExists, Id = "newId" });
                }
                catch (OrganisationNameAlreadyExistsException ex)
                {
                    _exception = ex;
                }
            }

            [Test]
            public void it_should_throw_OrganisationNameAlreadyExistsException()
            {
                _exception.ShouldNotBe(null);
            }
        }

        public class When_I_call_Update_with_valid_parameters : Given_an_OrganisationService
        {
            private readonly Organisation _organisation = new Organisation { Active = true, Name = "hjkljh", Id = OrganisationId };
            private DateTime _startTime;
            private DateTime _endTime;
            private Organisation _outcome;
            private const string OrganisationId = "ghjkl";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                OrganisationRepository.Stub(x => x.GetById(OrganisationId)).Return(new Organisation());
                _startTime = DateTime.UtcNow;
                _outcome = OrganisationService.Update(_organisation);
                _endTime = DateTime.UtcNow;
            }

            [Test]
            public void it_should_return_outcome_of_update_operation()
            {
                Assert.AreEqual(_outcome, _organisation);
            }

            [Test]
            public void it_should_set_updated_to_current_datetime()
            {
                var testOutcome = (_outcome.Updated >= _startTime) && (_outcome.Updated <= _endTime);
                Assert.True(testOutcome);
            }
        }

        [TestFixture]
        public class When_I_call_update_with_existing_name_of_other_organisation : Given_an_OrganisationService
        {
            private OrganisationNameAlreadyExistsException _exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                const string nameThatAlreadyExists = "alreadyExists";
                OrganisationRepository.Stub(x => x.GetByName(nameThatAlreadyExists)).Return(new Organisation());
                const string organisationId = "organisationId";
                OrganisationRepository.Stub(x => x.GetById(organisationId))
                                      .Return(new Organisation { Id = organisationId });
                var organisation = new Organisation { Name = nameThatAlreadyExists, Id = organisationId };

                try
                {
                    OrganisationService.Update(organisation);
                }
                catch (OrganisationNameAlreadyExistsException ex)
                {
                    _exception = ex;
                }
            }

            [Test]
            public void it_should_return_organisation_name_already_exists_exception()
            {
                _exception.ShouldNotBe(null);
            }
        }


        [TestFixture]
        public class When_I_call_update_with_existing_name_of_this_organisation : Given_an_OrganisationService
        {
            private OrganisationNameAlreadyExistsException _exception;
            private Organisation _organisationReturned;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                const string organisationId = "organisationId";
                const string nameThatAlreadyExistsForThisOrganisation = "alreadyExistsForThisOrganisation";
                OrganisationRepository.Stub(x => x.GetByName(nameThatAlreadyExistsForThisOrganisation)).Return(new Organisation { Id = organisationId });
                OrganisationRepository.Stub(x => x.GetById(organisationId))
                                      .Return(new Organisation { Id = organisationId });
                var organisation = new Organisation { Name = nameThatAlreadyExistsForThisOrganisation, Id = organisationId };

                _organisationReturned = OrganisationService.Update(organisation);
            }

            [Test]
            public void it_should_return_updated_organisation()
            {
                _organisationReturned.ShouldNotBe(null);
            }
        }

        public class When_call_GetById_with_existing_id : Given_an_OrganisationService
        {
            private const string Id = "ghjkl";
            private Organisation _outcome;
            private readonly Organisation _organisationFromRepository = new Organisation { Active = true };

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                OrganisationRepository.Stub(x => x.GetById(Id)).Return(_organisationFromRepository);
                _outcome = OrganisationService.GetById(Id);
            }

            [Test]
            public void it_should_return_organisation_from_repository()
            {
                Assert.AreEqual(_outcome, _organisationFromRepository);
            }
        }

        public class When_call_GetByID_with_non_existent_id : Given_an_OrganisationService
        {
            private const string Id = "ghjklvchjk";
            private readonly Organisation _organisationFromRepository = null;
            private Exception _exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                OrganisationRepository.Stub(x => x.GetById(Id)).Return(_organisationFromRepository);
                try
                {
                    OrganisationService.GetById(Id);
                }
                catch (ItemNotFoundException ex)
                {
                    _exception = ex;
                }
            }

            [Test]
            public void it_should_throw_ItemNotFoundException()
            {
                Assert.NotNull(_exception);
            }
        }

        [TestFixture]
        public class When_I_call_GetAll : Given_an_OrganisationService
        {
            private IEnumerable<Organisation> _outcome;

            private readonly IEnumerable<Organisation> _organisationsToReturn = new Organisation[3];

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                OrganisationRepository.Stub(x => x.GetAll()).Return(_organisationsToReturn);
                _outcome = OrganisationService.GetAll();
            }

            [Test]
            public void it_should_return_organisations_from_repository()
            {
                Assert.AreEqual(_outcome, _organisationsToReturn);
            }
        }

        [TestFixture]
        public class When_I_call_IdExists : Given_an_OrganisationService
        {
            private bool _result;
            private string _shouldReturnAnOrganisation;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();

                _shouldReturnAnOrganisation = "should_return_an_organisation";
                OrganisationRepository.Stub(x => x.IdExists(_shouldReturnAnOrganisation)).Return(true);

                _result = OrganisationService.IdExists(_shouldReturnAnOrganisation);
            }

            [Test]
            public void it_should_return_value_from_Repository()
            {
                Assert.True(_result);
            }
        }
    }
}