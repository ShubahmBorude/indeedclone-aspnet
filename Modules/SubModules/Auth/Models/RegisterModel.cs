using IndeedClone.Modules.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndeedClone.Modules.SubModules.Auth.Models
{
    [Table("users")]
    public class RegisterModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

     // # Unique ref_no (relation with GoogleAuth table, column ref_no)
        [Column("ref_no")]
        public string RefNo { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("email")]
        public string Email { get; set; }

     // # Hashed Password
        [Column("password")]
        public string? Password { get; set; }

        // # Future use
        [Column("field1")]
        public string? Field1 { get; set; }  
      
        [Column("field2")]
        public string? Field2 { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        [Column("edited")]
        public DateTime? Edited { get; set; }

        [Column("deleted")]
        public DateTime? Deleted { get; set; }

        [Column("status")]
        public AccountStatus Status { get; set; }       // # enum → ACTIVE = 1, INACTIVE = 0, TEMPORARY_BLOCKED = 3, BLOCKED = -3, DELETED = -2, etc.

        [Column("otp")]
        public string? OTP { get; set; }

        [Column("is_email_verified")]
        public bool IsEmailVerified { get; set; } = false;

        [Column("verification_otp_expiry")]
        public DateTime? VerificationOTPExpiry { get; set; }

        [Column("verification_attempt_count")]
        public int VerificationAttemptCount { get; set; } = 0;

        [Column("blocked_until")]
        public DateTime? BlockedUntil { get; set; }

        [Column("row_version")]
        [Timestamp]
        public byte[]? RowVersion { get; set; }    // #  Concurrency token

        [Column("resend_otp_at")]
        public DateTime? ResendOtpAt { get; set; }

    }
}
