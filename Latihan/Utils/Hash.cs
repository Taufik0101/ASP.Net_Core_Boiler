using System.Net.Http.Headers;
using Latihan.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Latihan.Utils;

public interface IHash
{
    Task<ResponseHashed> GenerateRandomString(ParamsRandPassword input);
    Task<ResponseVerifyPassword> VerifyPassword(ParamsVerifyPassword input);

    Task<string> HashPassword(string plainPassword);
}

public class Hash : IHash
{
    private static readonly HttpClient Client = new HttpClient();
    
    public async Task<ResponseHashed> GenerateRandomString(ParamsRandPassword input)
    {
        var content = new StringContent(JsonConvert.SerializeObject(input));
        
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        
        var response = await Client.PostAsync(
            Environment.GetEnvironmentVariable("JS_URL") + "/generateRandomPassword", 
            content);
        
        var value = await response.Content.ReadAsStringAsync();
            
        var json = JObject.Parse(value);
            
        Console.WriteLine(json["data"]!["password"]);
            
        var result = JsonConvert.DeserializeObject<ResponseHashed>(json["data"]!.ToString());

        return result;
    }

    public async Task<ResponseVerifyPassword> VerifyPassword(ParamsVerifyPassword input)
    {
        var content = new StringContent(JsonConvert.SerializeObject(input));
            
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var response = await Client.PostAsync(
            Environment.GetEnvironmentVariable("JS_URL") + "/verifyPassword", 
            content);
            
        var value = await response.Content.ReadAsStringAsync();
            
        var result = JsonConvert.DeserializeObject<ResponseVerifyPassword>(value);

        return result;
    }

    public async Task<string> HashPassword(string plainPassword)
    {
        var content = new StringContent(JsonConvert.SerializeObject(new
        {
            Password = plainPassword
        }));
        
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        
        var response = await Client.PostAsync(
            Environment.GetEnvironmentVariable("JS_URL") + "/hashPassword", 
            content);
        
        var value = await response.Content.ReadAsStringAsync();
        
        var json = JObject.Parse(value);

        return json["data"]!.ToString();
    }
}