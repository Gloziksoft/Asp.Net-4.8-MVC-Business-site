using dufeksoft.lib.Localization;
using dufeksoft.lib.Mail;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Resources;

namespace dufeksoft.lib.Model.Contact
{
    public class ContactModel
    {
        static ResourceManager _rm = null;

        public static ResourceManager RM
        {
            get
            {
                if (_rm == null)
                {
                    _rm = new ResourceManager(typeof(dufeksoft.lib.Localization.DufeksoftLibResource));
                }

                return _rm;
            }
        }

        /// <summary>
        /// Name
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(DufeksoftLibResource), ErrorMessageResourceName = "RequiredName")]
        [Display(ResourceType = typeof(DufeksoftLibResource), Name = "LabelName")]
        public string Name { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(DufeksoftLibResource), ErrorMessageResourceName = "RequiredEmail")]
        [Email(ErrorMessageResourceType = typeof(DufeksoftLibResource), ErrorMessageResourceName = "InvalidEmail")]
        [Display(ResourceType = typeof(DufeksoftLibResource), Name = "LabelEmail")]
        public string Email { get; set; }
        /// <summary>
        /// Subject
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(DufeksoftLibResource), ErrorMessageResourceName = "RequiredSubject")]
        [Display(ResourceType = typeof(DufeksoftLibResource), Name = "LabelSubject")]
        public string Subject { get; set; }
        /// <summary>
        /// Text
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(DufeksoftLibResource), ErrorMessageResourceName = "RequiredMsgText")]
        [Display(ResourceType = typeof(DufeksoftLibResource), Name = "LabelMsgText")]
        public string Text { get; set; }
        /// <summary>
        /// Captcha
        /// </summary>
        [Display(Name = "Captcha")]
        public string Captcha { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        [Display(ResourceType = typeof(DufeksoftLibResource), Name = "LabelPassword")]
        public string Password { get; set; }
        /// <summary>
        /// Confirm password
        /// </summary>
        [Display(ResourceType = typeof(DufeksoftLibResource), Name = "LabelConfirmPassword")]
        public string ConfirmPassword { get; set; }

        public bool SendContactRequest()
        {
            List<TextTemplateParam> paramList = new List<TextTemplateParam>();
            paramList.Add(new TextTemplateParam("NAME", this.Name));
            paramList.Add(new TextTemplateParam("EMAIL", this.Email));
            paramList.Add(new TextTemplateParam("SUBJECT", this.Subject));
            paramList.Add(new TextTemplateParam("TEXT", this.Text));

            // Odoslanie uzivatelovi
            Mailer.SendMailTemplate(
                string.Format(ContactModel.RM.GetString("ContactRequestSubject"), this.Subject),
                TextTemplate.GetTemplateText(ContactModel.RM.GetString("ContactRequestTemplate"), paramList),
                this.Email, ContactModel.RM.GetString("ContactRequestCultureId"), null, null);

            return true;
        }

        public string LabelSubject = ContactModel.RM.GetString("LabelSubject");
        public string LabelName = ContactModel.RM.GetString("LabelName");
        public string LabelEmail = ContactModel.RM.GetString("LabelEmail");
        public string LabelMsgText = ContactModel.RM.GetString("LabelMsgText");
        public string LabelSend = ContactModel.RM.GetString("LabelSend");
        public string LabelErrorInfo = ContactModel.RM.GetString("LabelErrorInfo");
        public string LabelHeader = ContactModel.RM.GetString("LabelHeader");
        public string LabelThankYou = ContactModel.RM.GetString("LabelThankYou");
        public string LabelMsgSent = ContactModel.RM.GetString("LabelMsgSent");
        public string LabelSeeYouSoon = ContactModel.RM.GetString("LabelSeeYouSoon");
        public string LabelPassword = ContactModel.RM.GetString("LabelPassword");
        public string LabelConfirmPassword = ContactModel.RM.GetString("LabelConfirmPassword");
    }
}