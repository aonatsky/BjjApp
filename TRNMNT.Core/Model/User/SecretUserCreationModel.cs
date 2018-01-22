using System.ComponentModel.DataAnnotations;

namespace TRNMNT.Core.Model.User
{
    public class SecretUserCreationModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string AccessKey { get; set; }
    }
}
