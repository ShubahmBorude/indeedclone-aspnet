namespace IndeedClone.Emails.EmailTemplates
{
    public class AuthEmailTemplates
    {
        public static string BuidVerificationOTP(string name, string otp)
        {
            return $@"
                <div style='font-family:Arial;padding:20px'>
                    <h2>Email Verification</h2>
                    <p>Hello <b>{name}</b>,</p>

                    <p>Your verification OTP is:</p>

                    <h1 style='letter-spacing:6px;
                                background:#255AA8;
                                color:white;
                                padding:10px;
                                display:inline-block'>
                        {otp}
                    </h1>

                    <p>This OTP expires in 15 minutes.</p>

                    <br/>
                    <small>
                        If you didn't request this,
                        please ignore this email.
                    </small>
                </div>";
        }
    }
}
