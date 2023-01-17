using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
