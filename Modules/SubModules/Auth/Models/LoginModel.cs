using System.ComponentModel.DataAnnotations.Schema;

namespace IndeedClone.Modules.SubModules.Auth.Models
{
    public class LoginModel
    {
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("password")]
        public string Password { get; set; } = string.Empty;

        [Column("status")]
        public bool Status { get; set; } = false;
    }
}
