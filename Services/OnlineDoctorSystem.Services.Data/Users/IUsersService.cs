namespace OnlineDoctorSystem.Services.Data.Users
{
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Models;

    public interface IUsersService
    {
        ApplicationUser GetUserByUsername(string username);

        Task<bool> AddUserToRole(string username, string role);
    }
}
