using System.ComponentModel.DataAnnotations;

namespace dufeksoft.lib.Model.Contact
{
    public class ContactModel_De
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_De)]
        [Display(Name = "Vor- und Nachname")]
        public string Name { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_De)]
        [Email(ErrorMessage = ModelUtil.invalidEmailErrMessage_De)]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }
        /// <summary>
        /// Subject
        /// </summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_De)]
        [Display(Name = "Betreff")]
        public string Subject { get; set; }
        /// <summary>
        /// Text
        /// </summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_De)]
        [Display(Name = "Hiermit schreiben Sie Ihre Nachricht")]
        public string Text { get; set; }
        /// <summary>
        /// Captcha
        /// </summary>
        [Display(Name = "Captcha")]
        public string Captcha { get; set; }
    }
}