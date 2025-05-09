using System.ComponentModel.DataAnnotations;

namespace dufeksoft.lib.Model.Contact
{
    public class ContactModel_Cs
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Cs)]
        [Display(Name = "Jméno")]
        public string Name { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Cs)]
        [Email(ErrorMessage = ModelUtil.invalidEmailErrMessage_Cs)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        /// <summary>
        /// Subject
        /// </summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Cs)]
        [Display(Name = "Předmět")]
        public string Subject { get; set; }
        /// <summary>
        /// Text
        /// </summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Cs)]
        [Display(Name = "Sem napište zprávu")]
        public string Text { get; set; }
        /// <summary>
        /// Captcha
        /// </summary>
        [Display(Name = "Captcha")]
        public string Captcha { get; set; }
    }
}