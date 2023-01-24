namespace Latihan.Utils;

public class Generates
{
    public async Task<string> GeneratePassword(int specialChar, int numChar, int upperChar, int lowerChar)
    {
        var randomPassword = "";
        var randoms = new Random();
        for (var i = 0; i < numChar; i++)
        {
            randomPassword += Convert.ToChar(randoms.Next(48, 58));
        }
        
        for (var i = 0; i < specialChar; i++)
        {
            randomPassword += Convert.ToChar(randoms.Next(33, 48));
        }
        
        for (var i = 0; i < lowerChar; i++)
        {
            randomPassword += Convert.ToChar(randoms.Next(97, 123));
        }
        
        for (var i = 0; i < upperChar; i++)
        {
            randomPassword += Convert.ToChar(randoms.Next(65, 91));
        }

        return randomPassword;
    }
}