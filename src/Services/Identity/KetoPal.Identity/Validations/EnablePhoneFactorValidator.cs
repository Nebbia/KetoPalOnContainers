using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using KetoPal.Identity.Areas.Identity.Pages.ManageAccount;

namespace KetoPal.Identity.Validations
{
    public class EnablePhoneFactorValidator: AbstractValidator<EnablePhoneFactorModel>
    {
        public EnablePhoneFactorValidator()
        {
            RuleFor(x => x.Input.PhoneNumber).NotEmpty().When(x => x.PendingVerification == false);
            RuleFor(x => x.Input.Code).NotEmpty().When(x => x.PendingVerification == true);
        }
    }
}
