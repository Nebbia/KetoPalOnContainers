using System.Threading.Tasks;

namespace KetoPal.Identity.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string phoneNumber, string v);
    }
}