using System;

using CMZero.API.DataAccess.Repositories;
using CMZero.API.Messages;

using NUnit.Framework;

namespace IntegrationTests.DataAccess
{
    public class OrganisationRepositoryTests
    {
        public class Given_an_OrganisationRepository
        {
            protected OrganisationRepository OrganisationRepository;

            [SetUp]
            public virtual void SetUp()
            {
                OrganisationRepository = new OrganisationRepository();
            }
        }

        public class When_I_add_a_new_organisation : Given_an_OrganisationRepository
        {
            private Organisation organisationToCreate;

            private Organisation outcome;

            private bool active = true;

            private DateTime dateTime;

            private Guid guid;

            private const string Name = "testName";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                active = true;
                dateTime = DateTime.Now;
                guid = new Guid();
                organisationToCreate = new Organisation { Name = Name, Active = active, Created = dateTime, Id = guid };
                OrganisationRepository.Create(organisationToCreate);
                outcome = OrganisationRepository.GetById(organisationToCreate.Id);
            }

            [Test]
            public void it_should_create_object_with_same_name()
            {
                Assert.AreEqual(outcome.Name, Name);
            }

            [Test]
            public void it_should_create_object_same_active()
            {
                Assert.AreEqual(outcome.Active, active);
            }

            [Test]
            public void it_should_create_object_with_same_created_date()
            {
                Assert.AreEqual(outcome.Created, dateTime);
            }

            [Test]
            public void it_should_create_object_with_same_id()
            {
                Assert.AreEqual(outcome.Id, organisationToCreate.Id);
            }
        }

        public class When_I_search_by_name : Given_an_OrganisationRepository
        {
            private string nameToSearchBy = string.Format("{0}name", DateTime.UtcNow);

            private Organisation result;

            private DateTime dateTime;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                dateTime = DateTime.UtcNow;
                OrganisationRepository.Create(new Organisation { Active = true, Created = dateTime, Name = nameToSearchBy });
                result = OrganisationRepository.GetByName(nameToSearchBy);
            }

            [Test]
            public void it_should_return_organisation()
            {
                Assert.AreEqual(result.Created, dateTime);
            }
        }

        public class When_I_update_an_organisation : Given_an_OrganisationRepository
        {
            private const bool Active = true;
            private readonly DateTime created = DateTime.UtcNow;
            private Organisation organisation;
            private const string NewName = "newName";
            private readonly string name = string.Format("{0}ghjk", DateTime.UtcNow);
            private Organisation retrievedOrganisation;
            private Organisation storedOrganisation;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                organisation = new Organisation { Active = Active, Created = created, Name = name };
                OrganisationRepository.Create(organisation);
                storedOrganisation = OrganisationRepository.GetById(organisation.Id);
                storedOrganisation.Active = false;
                storedOrganisation.Name = NewName;
                OrganisationRepository.Update(storedOrganisation);
                retrievedOrganisation = OrganisationRepository.GetById(organisation.Id);
            }

            [Test]
            public void it_should_store_updated_organisation()
            {
                Assert.AreEqual(retrievedOrganisation.Name, NewName);
                Assert.AreEqual(retrievedOrganisation.Active, false);
            }
        }
    }
}
