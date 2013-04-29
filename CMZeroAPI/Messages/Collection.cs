using System.ComponentModel.DataAnnotations;

namespace CMZero.API.Messages
{
    public class Collection : BaseEntityWithName
    {
        [Required]
        public string OrganisationId { get; set; }

        [Required]
        public string ApplicationId { get; set; }
    }
}
