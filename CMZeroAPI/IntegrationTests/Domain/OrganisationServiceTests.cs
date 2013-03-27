using System;
using System.Globalization;
using System.Linq;

using CMZero.API.DataAccess.Repositories;
using CMZero.API.Domain;
using CMZero.API.Messages;

using NUnit.Framework;

namespace IntegrationTests.Domain
{
    public class OrganisationServiceTests
    {
        public class Given_an_organisation_service
        {
            protected OrganisationService OrganisationService;

            [SetUp]
            public virtual void SetUp()
            {
                OrganisationService = new OrganisationService(new OrganisationRepository());
            }
        }

        [TestFixture]
        public class When_I_call_try_to_get_a_newly_created_organisation : Given_an_organisation_service
        {
            private string id;

            private string name;

            private Organisation outcome;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();

                name = string.Format("test{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                id = OrganisationService.Create(new Organisation { Active = true, Name = name }).Id;

                outcome = OrganisationService.GetById(id);
            }

            [Test]
            public void it_should_return_organisation_with_correct_name()
            {
                Assert.AreEqual(outcome.Name, name);
            }
        }

        [TestFixture]
        public class When_I_call_IdExists_with_an_Id_that_exists : Given_an_organisation_service
        {
            private bool result;

            [SetUp]
            public virtual void SetUp()
            {
                base.SetUp();

                var organisationThatExists = OrganisationService.GetAll().FirstOrDefault();
                
                result = OrganisationService.IdExists(organisationThatExists.Id);
            }

            [Test]
            public void it_should_return_true()
            {
                Assert.True(result);
            }
        }
    }
}