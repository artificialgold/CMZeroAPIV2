using System.ComponentModel.DataAnnotations;

namespace CMZero.API.Messages
{
    public class BaseEntityWithName : BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}