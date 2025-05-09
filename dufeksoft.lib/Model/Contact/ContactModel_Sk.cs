using System.ComponentModel.DataAnnotations;

namespace dufeksoft.lib.Model.Contact
{
    public class ContactModel_Sk
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [Display(Name = "Meno")]
        public string Name { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [Email(ErrorMessage = ModelUtil.invalidEmailErrMessage_Sk)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        /// <summary>
        /// Subject
        /// </summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [Display(Name = "Predmet")]
        public string Subject { get; set; }
        /// <summary>
        /// Text
        /// </summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [Display(Name = "Sem napíšte správu")]
        public string Text { get; set; }
        /// <summary>
        /// Captcha
        /// </summary>
        [Display(Name = "Captcha")]
        public string Captcha { get; set; }
    }
}