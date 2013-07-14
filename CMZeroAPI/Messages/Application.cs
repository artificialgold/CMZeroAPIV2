using System.ComponentModel.DataAnnotations;

namespace CMZero.API.Messages
{
    public class Application : BaseEntityWithName
    {
        //TODO: ensure organisation name is unique
        public string OrganisationId { get; set; }

        public string ApiKey { get; set; }
    }
}
