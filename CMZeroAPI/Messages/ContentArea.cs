using System.ComponentModel.DataAnnotations;

namespace CMZero.API.Messages
{
    public class ContentArea : BaseEntityWithName
    {
        public string Content { get; set; }

        public ContentAreaType? ContentType { get; set; }
       
        [Required]
        public string CollectionId { get; set; }
        
        [Required]
        public string ApplicationId { get; set; }
    }
}