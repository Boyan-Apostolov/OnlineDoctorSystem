namespace OnlineDoctorSystem.Services.Data.Users
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Models;

    public interface IUsersService
    {
        Task<ApplicationUser> GetUserByUsername(string username);

        Task<bool> AddUserToRole(string username, string role);

        Task<bool> RemoveUserFromRole(string name, string role);

        Task<IEnumerable<ApplicationUser>> GetUsersByRole(string role);
    }
}
