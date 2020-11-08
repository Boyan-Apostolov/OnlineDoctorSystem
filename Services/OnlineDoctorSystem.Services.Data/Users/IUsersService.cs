using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Services.Data.Users
{
    public interface IUsersService
    {
        Task<ApplicationUser> GetUserByUsername(string username);

        Task<bool> AddUserToRole(string username, string role);

        Task<bool> RemoveUserFromRole(string name, string role);

        Task<IEnumerable<ApplicationUser>> GetUsersByRole(string role);
    }
}
