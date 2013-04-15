using System;
using System.ComponentModel.DataAnnotations;

namespace CMZero.API.Messages
{
    public class ContentArea
    {
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        [Required]
        public string Name { get; set; }
        public string Content { get; set; }
        [Required]
        public ContentAreaType ContentType { get; set; }
    }
}