using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace dufeksoft.lib.Mail
{
    /// <summary>
    /// Enables to sendig emails
    /// </summary>
    public class MailProvider
    {
        /// <summary>
        /// Gets the mailer domain
        /// </summary>
        public string MailerDomain { get; private set; }
        /// <summary>
        /// Gets the mailer identification
        /// </summary>
        public string MailerID { get; private set; }
        /// <summary>
        /// Gets the smtp server host for sending emails
        /// </summary>
        public string SmtpHost { get; private set; }
        /// <summary>
        /// Gets the smtp port for sending emails
        /// </summary>
        public int SmtpPort { get; private set; }
        /// <summary>
        /// Gets whether to use SSL for sending emails
        /// </summary>
        public bool SmtpUseSsl { get; private set; }
        /// <summary>
        /// Gets the user name for sending emails
        /// </summary>
        public string SmtpUser { get; private set; }
        /// <summary>
        /// Gets the password for sending emails
        /// </summary>
        public string SmtpPassword { get; private set; }
        /// <summary>
        /// Admin Email address to send email to admin
        /// </summary>
        public string SendToAdmin { get; private set; }
        /// <summary>
        /// Email address to send email from
        /// </summary>
        public string SendFromMail { get; private set; }
        /// <summary>
        /// Email address to send email from
        /// </summary>
        public string SendFromName { get; private set; }
        /// <summary>
        /// Email address to send email to as blind copy
        /// </summary>
        public string SendToBcc { get; set; }
        /// <summary>
        /// Internal identifier for Email address to send email to as blind copy for a specific mail service
        /// </summary>
        public string SendToBccServiceId { get; private set; }
        /// <summary>
        /// Email address to send email to as blind copy for a specific mail service
        /// </summary>
        public string SendToBccService { get; private set; }
        /// <summary>
        /// Whether email body is HTML
        /// </summary>
        public bool IsBodyHtml { get; private set; }

        /// <summary>
        /// Whether send bcc
        /// </summary>
        public bool UseSendToBcc { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailProvider"/> class.
        /// </summary>
        /// <param name="bccServiceId">Blind copy email service ID</param>
        public MailProvider(string bccServiceId)
            : this(bccServiceId, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailProvider"/> class.
        /// </summary>
        /// <param name="aIsBodyHtml">Whether email body is HTML</param>
        public MailProvider(string bccServiceId, bool aIsBodyHtml)
        {
            this.SendToBccServiceId = bccServiceId;
            this.IsBodyHtml = aIsBodyHtml;
            this.UseSendToBcc = true;
            LoadCfg();
        }

        /// <summary>
        /// Clears the configuration settings
        /// </summary>
        public void ClearCfg()
        {
            this.MailerDomain = string.Empty;
            this.MailerID = string.Empty;
            this.SmtpHost = string.Empty;
            this.SmtpPort = -1;
            this.SmtpUseSsl = false;
            this.SmtpUser = string.Empty;
            this.SmtpPassword = string.Empty;
            this.SendToAdmin = string.Empty;
            this.SendFromMail = string.Empty;
            this.SendFromName = string.Empty;
            this.SendToBcc = string.Empty;
            this.SendToBccService = string.Empty;
        }

        /// <summary>
        /// Loads configuration settings
        /// </summary>
        public void LoadCfg()
        {
            ClearCfg();

            // Mailer domain
            this.MailerDomain = ConfigurationManager.AppSettings["mailerDomain"];
            // Mailer ID
            this.MailerID = ConfigurationManager.AppSettings["mailerID"];
            // SMTP host
            this.SmtpHost = ConfigurationManager.AppSettings["smtpHost"];
            if (ConfigurationManager.AppSettings["smtpPort"] != null)
            {
                this.SmtpPort = int.Parse(ConfigurationManager.AppSettings["smtpPort"]);
            }
            if (ConfigurationManager.AppSettings["smtpUseSsl"] != null)
            {
                this.SmtpUseSsl = ConfigurationManager.AppSettings["smtpUseSsl"] == "true";
            }
            // SMTP user
            this.SmtpUser = ConfigurationManager.AppSettings["smtpUser"];
            // SMTP password
            this.SmtpPassword = ConfigurationManager.AppSettings["smtpPassword"];
            // Send to admin mail address
            this.SendToAdmin = ConfigurationManager.AppSettings["sendToAdmin"];
            // Send from
            this.SendFromMail = ConfigurationManager.AppSettings["sendFrom"];
            this.SendFromName = ConfigurationManager.AppSettings["sendFromName"];
            // Send to Bcc
            this.SendToBcc = ConfigurationManager.AppSettings["sendToBcc"];
            // Send to Bcc
            if (!string.IsNullOrEmpty(this.SendToBccServiceId))
            {
                this.SendToBccService = ConfigurationManager.AppSettings[this.SendToBccServiceId];
            }
        }

        /// <summary>
        /// Send an email
        /// </summary>
        /// <param name="mailSubject">Mail subject</param>
        /// <param name="mailBody">Mail body</param>
        /// <param name="sendTo">Send to email address</param>
        public void SendMail(string mailSubject, string mailBody, string sendTo, List<string> attachementList, Attachment attachment)
        {
            bool sendAsBcc = false;

            List<string> sendToList = new List<string>();
            string[] items = sendTo.Replace(',', '|').Replace(';', '|').Replace(' ', '|').Split('|');
            foreach (string item in items)
            {
                if (item == Mailer.MT_SendAsBcc)
                {
                    sendAsBcc = true;
                }
                else
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        sendToList.Add(item);
                    }
                }
            }
            SendMail(mailSubject, mailBody, sendToList, sendAsBcc, false, attachementList, attachment);
        }

        /// <summary>
        /// Send an email to the specified recepients
        /// </summary>
        /// <param name="mailSubject">Mail subject</param>
        /// <param name="mailBody">Mail body</param>
        /// <param name="sendToList">The list of recepients</param>
        /// <param name="asBcc">Whether the parameter <paramref name="sendToList"/> should be treated as BCC list</param>
        /// <param name="sendSeparately">Whether to send email separately to each recepient</param>
        /// <param name="attachementList">The list of attachements</param>
        public void SendMail(string mailSubject, string mailBody, List<string> sendToList, bool asBcc, bool sendSeparately, List<string> attachementList, Attachment attachment)
        {
            if (string.IsNullOrEmpty(this.SmtpHost))
                // No mail support
                return;

            if (sendToList == null || (sendToList != null && sendToList.Count == 0))
                // Empty receivers list
                return;

            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(this.SendFromMail, this.SendFromName);

            if (!sendSeparately)
            {
                // Send as one message to all recepients
                if (!string.IsNullOrEmpty(this.SendToBcc))
                {
                    AddToMailAddressCollection(mail.Bcc, this.SendToBcc);
                }
                if (!string.IsNullOrEmpty(this.SendToBccService))
                {
                    AddToMailAddressCollection(mail.Bcc, this.SendToBccService);
                }
                foreach (string sender in sendToList)
                {
                    if (!string.IsNullOrEmpty(sender))
                    {
                        if (asBcc)
                            AddToMailAddressCollection(mail.Bcc, sender);
                        else
                            AddToMailAddressCollection(mail.To, sender);
                    }
                }
            }

            mail.Headers["X-Mailer"] = this.MailerID;// "madosoft.sk mailer system";
            mail.Subject = mailSubject;
            mail.IsBodyHtml = this.IsBodyHtml;
            mail.SubjectEncoding = Encoding.UTF8;
            mail.BodyEncoding = Encoding.UTF8;
            mail.Body = mailBody;

            if (attachementList != null)
            {
                foreach (string attPath in attachementList)
                {
                    Attachment attItem = new Attachment(attPath);
                    mail.Attachments.Add(attItem);
                }
            }

            if (attachment != null)
            {
                mail.Attachments.Add(attachment);
            }

            try
            {
                SmtpClient c = GetSmtpClient();

                if (!sendSeparately)
                {
                    try
                    {
                        // Send as one message to all recipients
                        c.Send(mail);
                    }
                    catch (SmtpFailedRecipientException ex)
                    {
                        // Log the error and handle it gracefully
                        Console.WriteLine("Failed to send email to recipient: " + ex.FailedRecipient + " - " + ex.Message);
                    }
                    catch (SmtpException ex)
                    {
                        // Handle other SMTP-related errors
                        Console.WriteLine("SMTP error occurred while sending email: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        // Handle any other unexpected errors
                        Console.WriteLine("An unexpected error occurred while sending email: " + ex.Message);
                    }
                }
                else
                {
                    // Send individual message to each recepient
                    foreach (string sender in sendToList)
                    {
                        mail.Bcc.Clear();
                        mail.To.Clear();
                        if (!string.IsNullOrEmpty(sender))
                        {
                            if (asBcc)
                                AddToMailAddressCollection(mail.Bcc, sender);
                            else
                                AddToMailAddressCollection(mail.To, sender);
                            c.Send(mail);
                        }
                    }

                    if (this.UseSendToBcc)
                    {
                        // Send message to system bcc
                        mail.Bcc.Clear();
                        mail.To.Clear();
                        if (!string.IsNullOrEmpty(this.SendToBcc))
                        {
                            if (asBcc)
                                AddToMailAddressCollection(mail.Bcc, this.SendToBcc);
                            else
                                AddToMailAddressCollection(mail.To, this.SendToBcc);
                            c.Send(mail);
                        }
                    }
                    // Send message to service bcc
                    mail.Bcc.Clear();
                    mail.To.Clear();
                    if (!string.IsNullOrEmpty(this.SendToBccService))
                    {
                        if (asBcc)
                            AddToMailAddressCollection(mail.Bcc, this.SendToBccService);
                        else
                            AddToMailAddressCollection(mail.To, this.SendToBccService);
                        c.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SendMailException("Chyba pri odosielaní e-mailu", ex);
            }
        }

        /// <summary>
        /// Send an email to the specified recepients
        /// </summary>
        /// <param name="mailSubject">Mail subject</param>
        /// <param name="mailBody">Mail body</param>
        /// <param name="sendToList">The list of recepients</param>
        /// <param name="asBcc">Whether the parameter <paramref name="sendToList"/> should be treated as BCC list</param>
        /// <param name="sendSeparately">Whether to send email separately to each recepient</param>
        public void SendAdminMail(string mailSubject, string mailBody)
        {
            if (string.IsNullOrEmpty(this.SmtpHost))
                // No mail support
                return;

            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(this.SendFromMail, this.SendFromName);

            mail.Headers["X-Mailer"] = this.MailerID;// "madosoft.sk mailer system";
            mail.Subject = mailSubject;
            mail.IsBodyHtml = this.IsBodyHtml;
            mail.SubjectEncoding = Encoding.UTF8;
            mail.BodyEncoding = Encoding.UTF8;
            mail.Body = mailBody;
            AddToMailAddressCollection(mail.To, this.SendToAdmin);

            try
            {
                SmtpClient c = GetSmtpClient();

                c.Send(mail);
            }
            catch (Exception ex)
            {
                throw new SendMailException("Error sending e-mail.", ex);
            }
        }

        SmtpClient GetSmtpClient()
        {
            SmtpClient client = new SmtpClient(this.SmtpHost);
            if (this.SmtpUseSsl)
            {
                client.EnableSsl = true;
            }
            if (this.SmtpPort > 0)
            {
                client.Port = this.SmtpPort;
            }
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(this.SmtpUser, this.SmtpPassword);

            return client;
        }

        private void AddToMailAddressCollection(MailAddressCollection list, string mailAddress)
        {
            string[] mailItems = mailAddress.Split(';');
            foreach (string mailItem in mailItems)
            {
                list.Add(mailItem);
            }
        }
    }

    public class SendMailException : Exception
    {
        public string Subject { get; set; }
        public string Body { get; set; }

        // Summary:
        //     Initializes a new instance of the System.Exception class.
        public SendMailException()
            : base()
        {
        }

        //
        // Summary:
        //     Initializes a new instance of the System.Exception class with a specified
        //     error message.
        //
        // Parameters:
        //   message:
        //     The message that describes the error.
        public SendMailException(string message)
            : base(message)
        {
        }

        //
        // Summary:
        //     Initializes a new instance of the System.Exception class with a specified
        //     error message and a reference to the inner exception that is the cause of
        //     this exception.
        //
        // Parameters:
        //   message:
        //     The error message that explains the reason for the exception.
        //
        //   innerException:
        //     The exception that is the cause of the current exception, or a null reference
        //     (Nothing in Visual Basic) if no inner exception is specified.
        public SendMailException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    public class MailAddressHelper
    {
        public static string GetFirstEmail(string email)
        {
            string str = email.Replace(",", ";");
            string[] items = str.Split(';');

            return items[0].Trim();
        }
    }

    /// <summary>
    /// Mailer class
    /// </summary>
    public abstract class Mailer
    {
        public const string MT_SendAsBcc = "{SendAsBcc}";

        public static string AddTagToSendTo(string sendTo, string sendToTag)
        {
            return string.Format("{0}|{1}", sendTo, sendToTag);
        }

        /// <summary>
        /// Send an email
        /// </summary>
        /// <param name="mailSubject">Mail subject</param>
        /// <param name="mailBody">Mail body</param>
        /// <param name="sendTo">Send to email address</param>
        public static void SendMail(string mailSubject, string mailBody, string sendTo)
        {
            MailProvider mailProvider = new MailProvider(null);
            mailProvider.SendMail(GetFullTitle(mailSubject, mailProvider), mailBody, sendTo, null, null);
        }

        /// <summary>
        /// Send an email to the specified recepients
        /// </summary>
        /// <param name="mailSubject">Mail subject</param>
        /// <param name="mailBody">Mail body</param>
        /// <param name="sendToList">The list of recepients</param>
        /// <param name="asBcc">Whether the parameter <paramref name="sendToList"/> should be treated as BCC list</param>
        public static void SendMail(string mailSubject, string mailBody, List<string> sendToList, bool asBcc)
        {
            MailProvider mailProvider = new MailProvider(null);
            mailProvider.SendMail(GetFullTitle(mailSubject, mailProvider), mailBody, sendToList, asBcc, !asBcc, null, null);
        }

        /// <summary>
        /// Send an email using HTML template
        /// </summary>
        /// <param name="mailSubject">Mail subject</param>
        /// <param name="mailBody">Mail body</param>
        /// <param name="sendTo">Send to email address</param>
        public static void SendMailTemplate(string mailSubject, string mailBody, string sendTo, string cultureId, List<string> attachementList, Attachment attachment, string templateName = null)
        {
            SendMailTemplate(mailSubject, mailBody, sendTo, null, cultureId, attachementList, attachment, templateName);
        }

        /// <summary>
        /// Send an email using HTML template
        /// </summary>
        /// <param name="mailSubject">Mail subject</param>
        /// <param name="mailBody">Mail body</param>
        /// <param name="sendTo">Send to email address</param>
        /// <param name="bccServiceId">Blind copy email service ID</param>
        public static void SendMailTemplate(string mailSubject, string mailBody, string sendTo, string bccServiceId, string cultureId, List<string> attachementList, Attachment attachment, string templateName = null)
        {
            List<TextTemplateParam> paramList = new List<TextTemplateParam>();
            paramList.Add(new TextTemplateParam("EMAIL_SUBJ", mailSubject));
            paramList.Add(new TextTemplateParam("EMAIL_MSG", mailBody));
            paramList.Add(new TextTemplateParam("EMAIL_TO", sendTo));

            string toUseTemplate = string.IsNullOrEmpty(templateName) ? "EmailOne" : templateName;
            MailProvider mailProvider = new MailProvider(bccServiceId);
            mailProvider.SendMail(GetFullTitle(mailSubject, mailProvider), TextTemplate.GetTemplateText(toUseTemplate + cultureId, paramList), sendTo, attachementList, attachment);
        }

        /// <summary>
        /// Send an email to the specified recepients using HTML template
        /// </summary>
        /// <param name="mailSubject">Mail subject</param>
        /// <param name="mailBody">Mail body</param>
        /// <param name="sendToList">The list of recepients</param>
        /// <param name="asBcc">Whether the parameter <paramref name="sendToList"/> should be treated as BCC list</param>
        public static void SendMailTemplate(string mailSubject, string mailBody, List<string> sendToList, bool asBcc, string cultureId)
        {
            List<TextTemplateParam> paramList = new List<TextTemplateParam>();
            paramList.Add(new TextTemplateParam("EMAIL_SUBJ", mailSubject));
            paramList.Add(new TextTemplateParam("EMAIL_MSG", mailBody));

            MailProvider mailProvider = new MailProvider(null);
            mailProvider.SendMail(GetFullTitle(mailSubject, mailProvider), TextTemplate.GetTemplateText("EmailMore" + cultureId, paramList), sendToList, asBcc, !asBcc, null, null);
        }

        /// <summary>
        /// Send an email using HTML template
        /// </summary>
        /// <param name="mailSubject">Mail subject</param>
        /// <param name="mailBody">Mail body</param>
        /// <param name="sendTo">Send to email address</param>
        /// <param name="bccServiceId">Blind copy email service ID</param>
        public static void SendInternalMailTemplate(string mailSubject, string mailBody, string sendTo, string cultureId, List<string> attachementList, Attachment attachment)
        {
            List<TextTemplateParam> paramList = new List<TextTemplateParam>();
            paramList.Add(new TextTemplateParam("EMAIL_SUBJ", mailSubject));
            paramList.Add(new TextTemplateParam("EMAIL_MSG", mailBody));
            paramList.Add(new TextTemplateParam("EMAIL_TO", sendTo));

            MailProvider mailProvider = new MailProvider(null);
            mailProvider.UseSendToBcc = false;
            mailProvider.SendMail(GetFullTitle(mailSubject, mailProvider), TextTemplate.GetTemplateText("EmailOne" + cultureId, paramList), sendTo, attachementList, attachment);
        }

        /// <summary>
        /// Send an email using HTML template
        /// </summary>
        /// <param name="mailSubject">Mail subject</param>
        /// <param name="mailBody">Mail body</param>
        /// <param name="sendTo">Send to email address</param>
        public static void SendMailTemplateWithoutBcc(string mailSubject, string mailBody, string sendTo, string cultureId, List<TextTemplateParam> extraParamList = null, string template = null, Attachment attachement = null)
        {
            List<TextTemplateParam> paramList = new List<TextTemplateParam>();
            paramList.Add(new TextTemplateParam("EMAIL_SUBJ", mailSubject));
            paramList.Add(new TextTemplateParam("EMAIL_MSG", mailBody));
            paramList.Add(new TextTemplateParam("EMAIL_TO", sendTo));
            if (extraParamList != null)
            {
                foreach (TextTemplateParam ep in extraParamList)
                {
                    paramList.Add(ep);
                }
            }

            string templateName = string.IsNullOrEmpty(template) ? "EmailOne" : template;
            MailProvider mailProvider = new MailProvider(null);
            mailProvider.UseSendToBcc = false;
            mailProvider.SendToBcc = null;
            mailProvider.SendMail(GetFullTitle(mailSubject, mailProvider), TextTemplate.GetTemplateText(templateName + cultureId, paramList), sendTo, null, attachement);
        }

        private static string GetFullTitle(string mailSubject, MailProvider mailProvider)
        {
            if (string.IsNullOrEmpty(mailProvider.SendFromName))
            {
                return string.Format("{0}: {1}", mailProvider.MailerDomain, mailSubject);
            }
            else
            {
                return mailSubject;
            }
        }

        /// <summary>
        /// Send an email to admin
        /// </summary>
        /// <param name="mailSubject">Mail subject</param>
        /// <param name="mailBody">Mail body</param>
        /// <param name="sendTo">Send to email address</param>
        public static void SendAdminMail(string mailSubject, string mailBody)
        {
            MailProvider mailProvider = new MailProvider(null, false);
            mailProvider.SendAdminMail(GetFullTitle(mailSubject, mailProvider), mailBody);
        }
    }
}
