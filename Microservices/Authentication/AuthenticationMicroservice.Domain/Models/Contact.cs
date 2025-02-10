using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AuthenticationMicroservice.Domain.Models
{
    public class Contact : Base.Entity
    {
        public Contact() : base() { }

        [Required]
        [MaxLength(254)]
        public string Fullname { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(254)]
        public string Email { get; set; }

        [AllowNull]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
