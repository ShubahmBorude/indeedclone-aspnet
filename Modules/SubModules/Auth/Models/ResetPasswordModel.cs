using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndeedClone.Modules.SubModules.Auth.Models
{
    [Table("user_password_reset_tokens")]
    public class ResetPasswordModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("ref_no")]
        public string RefNo { get; set; }

        [Column("otp")]
        public string OTP { get; set; }

        [Column("expired_at")]
        public DateTime ExpiredAt { get; set; }

        [Column("is_verified")]
        public bool IsVerified { get; set; }

        [Column("verified_at")]
        public DateTime? VerifiedAt { get; set; }

        [Column("used_at")]
        public DateTime? UsedAt { get; set; }

        [Column("attempt_count")]
        public int AttemptCount { get; set; } = 0;

        [Column("Blocked_until")]
        public DateTime? BlockedUntil { get; set; }

        [Column("resend_allowed_at")]
        public DateTime? ResendAllowedAt { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }
    }
}
