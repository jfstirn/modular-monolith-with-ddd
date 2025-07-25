using OtpNet;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;

public class Authenticator
{
    public string GenerateAuthenticatorCode(string secretKey)
    {
        var totp = new Totp(Base32Encoding.ToBytes(secretKey));
        return totp.ComputeTotp();
    }
}