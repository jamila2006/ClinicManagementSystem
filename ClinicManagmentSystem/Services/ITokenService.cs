using ClinicManagementSystem.Models;

namespace ClinicManagementSystem.Services
{
    public interface ITokenService
    {
        string CreateToken(AppUser user, IList<string> roles);
    }
}
