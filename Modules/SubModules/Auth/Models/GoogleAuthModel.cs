using IndeedClone.Modules.Shared.DateFormat;
using IndeedClone.Modules.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndeedClone.Modules.SubModules.Auth.Models
{
    [Table("user_google_auth")]
    public class GoogleAuthModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; } // PK

        [Column("ref_no")]
        public string RefNo { get; set; } = string.Empty; // FK → Users.RefNo

        [Column("issuer")]
        public string Issuer { get; set; } = string.Empty;
  
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("provider_user_id")]
        public string UserId { get; set; } = string.Empty; // Google `sub`

        [Column("picture_link")]
        public string PictureLink { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("email_verified")]
        public bool EmailVerified { get; set; } = false;

        [Column("provider")]
        public AuthProvider ProvidedId { get; set; } // Google = 1

        [Column("created")]
        public DateTime Created { get; set; }

        [Column("edited")]
        public DateTime? Edited { get; set; }

        [Column("deleted")]
        public DateTime? Deleted { get; set; }

        [Column("status")]
        public GoogleAuthStatus Status { get; set; } = GoogleAuthStatus.INACTIVE;
    }
}
