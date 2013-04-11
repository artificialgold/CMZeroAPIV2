using System;
using System.ComponentModel.DataAnnotations;

namespace CMZero.API.Messages
{
    public class Collection : BaseEntityWithName
    {
        [Required]
        public string OrganisationId { get; set; }

        [Required]
        public string ApplicationId { get; set; }
        public ContentArea[] ContentAreas { get; set; }
    }

    public class ContentArea
    {
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public ContentAreaType ContentType { get; set; }
    }

    public enum ContentAreaType
    {
        Label,
        HtmlArea
    }
}
