using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using OAuth2;

namespace Login2.Models.SendMail
{
    public class SendMail
    {
        public string Recipient { get; set; }
        public string Replyto { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        private static string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            // Special "url-safe" base64 encode.
            return Convert.ToBase64String(inputBytes)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");
        }
    }
}