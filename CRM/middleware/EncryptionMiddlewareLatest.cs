using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CRM.middleware
{
    public class EncryptionMiddlewareLatest
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public EncryptionMiddlewareLatest(RequestDelegate next, IConfiguration configuration)
        {
            _configuration = configuration;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.Body = EncryptStream(httpContext.Response.Body,_configuration);
            httpContext.Request.Body = DecryptStream(httpContext.Request.Body,_configuration);

            if (httpContext.Request.QueryString.HasValue)
            {
                string decryptedString = DecryptString(httpContext.Request.QueryString.Value.Substring(1),_configuration);
                httpContext.Request.QueryString = new QueryString($"?{decryptedString}");
            }
            await _next(httpContext);
            await httpContext.Request.Body.DisposeAsync();
            await httpContext.Response.Body.DisposeAsync();
        }

        private static CryptoStream EncryptStream(Stream responseStream, IConfiguration _configuration)
        {
            Aes aes = GetEncryptionAlgorithm(_configuration);

            ToBase64Transform base64Transform = new ToBase64Transform();
            CryptoStream base64EncodedStream = new CryptoStream(responseStream, base64Transform, CryptoStreamMode.Write);
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            CryptoStream cryptoStream = new CryptoStream(base64EncodedStream, encryptor, CryptoStreamMode.Write);

            return cryptoStream;
        }

        private static Aes GetEncryptionAlgorithm(IConfiguration _configuration)
        {
            var audienceConfig = _configuration.GetSection("Audience");
            string key = audienceConfig["sec"].Trim();
            string iv = audienceConfig["sec"].Trim();

            var secret_key = key;
            var initialization_vector = iv;

            Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(secret_key);
            aes.IV = Encoding.UTF8.GetBytes(initialization_vector);

            return aes;
        }

        private static Stream DecryptStream(Stream cipherStream,IConfiguration _configuration)
        {
            Aes aes = GetEncryptionAlgorithm(_configuration);

            FromBase64Transform base64Transform = new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces);
            CryptoStream base64DecodedStream = new CryptoStream(cipherStream, base64Transform, CryptoStreamMode.Read);
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            CryptoStream decryptedStream = new CryptoStream(base64DecodedStream, decryptor, CryptoStreamMode.Read);
            return decryptedStream;
        }

        private static string DecryptString(string cipherText,IConfiguration _configuration)
        {
            Aes aes = GetEncryptionAlgorithm(_configuration);
            byte[] buffer = Convert.FromBase64String(cipherText);

            using MemoryStream memoryStream = new MemoryStream(buffer);
            using ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }
    }
}