using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KetoPal.Identity.Services
{
    public interface IRedirectService
    {
        string ExtractRedirectUriFromReturnUrl(string url);
    }
}
