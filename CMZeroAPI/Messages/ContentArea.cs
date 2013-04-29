using System;
using System.ComponentModel.DataAnnotations;

namespace CMZero.API.Messages
{
    public class ContentArea : BaseEntityWithName
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public ContentAreaType ContentType { get; set; }
        [Required]
        public string CollectionId { get; set; }
        [Required]
        public string ApplicationId { get; set; }
    }
}