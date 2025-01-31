using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AuthenticationMicroservice.Domain.Models
{
    public class UserInformation : Base.Entity
    {
        public UserInformation(): base()
        {
        }

        //private readonly ILazyLoader _loader;
        //public UserInformation(ILazyLoader loader)
        //{
        //    _loader = loader;
        //}

        //private User _user;
        //[Required]
        //public virtual User User
        //{
        //    get => _loader.Load(this, ref _user);
        //    set => _user = value;
        //}

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [AllowNull]
        public string NationalId { get; set; }

        [Required]
        public Enums.Gender Gender { get; set; }

        [Required]
        public System.DateTime Birthdate { get; set; }

        [AllowNull]
        public string Bio { get; set; }

        [Required]
        public string Email { get; set; }

        [AllowNull]
        public string Phone { get; set; }

        [AllowNull]
        public string Equipment { get; set; }

        [AllowNull]
        public string SocialMediaLinks { get; set; }

        [AllowNull]
        public string Certifications { get; set; }

        [AllowNull]
        public string Language { get; set; }

        [Required]
        public System.Guid UserId { get; set; }

        public virtual User User { get; set; }
    }
}
