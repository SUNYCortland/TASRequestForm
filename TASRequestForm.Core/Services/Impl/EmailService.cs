using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Extensions;

namespace TASRequestForm.Core.Services.Impl
{
    public class EmailService : IEmailService
    {
        private readonly ITemplateService _templateService;
        private const string TEMPLATE_PATH = "/Emailer/Templates/";

        public EmailService(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        public void SendEmail<T>(EmailTemplate template, T model, string emailAddress, string subject)
        {
            if (String.IsNullOrWhiteSpace(emailAddress))
                throw new ArgumentNullException("You must specify a valid email address");

            string templateContents = GetTemplate(template);

            string renderedTemplate = _templateService.Parse(templateContents, model, null, template.ToFriendlyString());

            var message = new MailMessage();
            message.To.Add(emailAddress);
            message.Subject = subject;
            message.From = new MailAddress("tas@cortland.edu");
            message.Body = renderedTemplate;
            message.IsBodyHtml = true;

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Send(message);
            }
        }

        private string GetTemplate(EmailTemplate template)
        {
            ObjectCache cache = MemoryCache.Default;

            string fileContents = cache[template.ToFriendlyString()] as string;

            if (fileContents == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(30);

                var filePaths = new List<string>();
                string cachedFilePath = GetTemplatePath(template.ToFriendlyString() + ".cshtml");
                filePaths.Add(cachedFilePath);

                policy.ChangeMonitors.Add(new HostFileChangeMonitor(filePaths));

                fileContents = File.ReadAllText(cachedFilePath);

                cache.Set(template.ToFriendlyString(), fileContents, policy);
            }

            return fileContents;
        }

        private string GetTemplatePath(string fileName)
        {
            var appDomain = System.AppDomain.CurrentDomain;
            var basePath = appDomain.RelativeSearchPath ?? appDomain.BaseDirectory;

            return Path.Combine(basePath, "Emailer", "Templates", fileName);
        }
    }
}
