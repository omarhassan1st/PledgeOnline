using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Web_FirstApplication.Models.IdentityModel;
using static Web_FirstApplication.Conest.Settings;

namespace Web_FirstApplication.Const
{
    public class Services
    {
        public static class Location
        {
            public static string GetUserCountryByIp(string ip)
            {
                IpInfo ipInfo = new();
                try
                {
                    string info = new WebClient().DownloadString($"http://ipinfo.io/{ip}");
                    ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                    RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                    ipInfo.Country = myRI1.EnglishName;
                }
                catch (Exception)
                {
                    ipInfo.Country = "0";
                }

                return ipInfo.Country;
            }
        }
        public class Mailing
        {
            private readonly Email _mmailSettings;
            public Mailing(IOptions<Email> mmailSettings)
            {
                _mmailSettings = mmailSettings.Value;
            }
            public bool SendEmail(string email, string subject, string message)
            {
                string EmailBody = message;
                SmtpClient Client = new SmtpClient(_mmailSettings.Host, _mmailSettings.Port);
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(email);
                mailMessage.To.Add(email);
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                Client.UseDefaultCredentials = false;
                Client.EnableSsl = true;
                Client.Credentials = new NetworkCredential(_mmailSettings.ID, _mmailSettings.PW);
                mailMessage.Body = EmailBody;
                Client.Send(mailMessage);
                return true;
            }
        }

    }
}
