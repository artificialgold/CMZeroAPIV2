using System.Net.Http;
using Api.Controllers;
using CMZero.API.Domain;
using CMZero.API.Messages;
using NUnit.Framework;
using Rhino.Mocks;

namespace UnitTests.Api
{
    public class OrganisationsControllerTests
    {
        public class Given_an_OrganisationController
        {
            protected OrganisationsController OrganisationsController;
            protected IOrganisationService OrganisationService;

            [SetUp]
            public void SetUp()
            {
                OrganisationService = MockRepository.GenerateMock<IOrganisationService>();
                OrganisationsController = new OrganisationsController(OrganisationService);
            }
        }

        [TestFixture]
        public class When_I_call_add : Given_an_OrganisationController
        {
            protected HttpResponseMessage Outcome;
            protected Organisation OrganisationIntoService;
            private readonly Organisation OrganisationFromService = new Organisation{Name = "hkj"};

            [SetUp]
            public new void SetUp()
            {
                base.SetUp();

                OrganisationIntoService = new Organisation();

                Outcome = OrganisationsController.Post(OrganisationIntoService);
            }

            [Test]
            public void it_should_return_the_organisation_from_organisation_service_in_the_response()
            {
                Assert.AreEqual(Outcome.Content, OrganisationFromService);
            }
        }
    }
}
