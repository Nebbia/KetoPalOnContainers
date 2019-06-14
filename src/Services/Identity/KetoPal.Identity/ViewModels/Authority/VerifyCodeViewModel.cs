using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KetoPal.Identity.ViewModels.Authority
{
    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }
        [Required(ErrorMessage = "CODE_REQUIRED")]
        [DataType(DataType.Text)]
        [Display(Name = "Authenticator code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }
        [Display(Name ="Remember this machine")]
        public bool RememberBrowser { get; set; }
        public bool RememberMe { get; set; }
    }
}
