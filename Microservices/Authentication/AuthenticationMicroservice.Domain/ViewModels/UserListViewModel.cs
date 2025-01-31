namespace AuthenticationMicroservice.Domain.ViewModels
{
    public class UserListViewModel
    {
        public UserListViewModel()
        {
        }

        public System.Guid Id { get; set; }
        public string Username { get; set; }
        public System.DateTime RegisterDateTime { get; set; }
        public bool IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; }
        public Enums.Gender? Gender { get; set; }
        public System.DateTime? Birthdate { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Equipment { get; set; }
        public string SocialMediaLinks { get; set; }
        public string Certifications { get; set; }
        public string Language { get; set; }
    }
}
