using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationMicroservice.Domain.ViewModels
{
    public class ProfileViewModel
    {
        [Required]
        public System.Guid UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public Enums.Gender Gender { get; set; }

        [Required]
        public System.DateTime Birthdate { get; set; }

        [Required]
        public string Bio { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [AllowNull]
        public string Equipment { get; set; }

        [AllowNull]
        public string SocialMediaLinks { get; set; }

        [AllowNull]
        public string Certifications { get; set; }

        [Required]
        public string Language { get; set; }
    }
}
