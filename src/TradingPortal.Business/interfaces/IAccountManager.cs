using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingPortal.Core.Domain.Identity;
using TradingPortal.Core.ViewModels;
using TradingPortal.Infrastructure.ComplexTypes;

namespace TradingPortal.Business.interfaces
{
    public interface IAccountManager
    {

        Task<bool> CheckPasswordAsync(Customer user, string password);
        Task<Tuple<bool, string[]>> CreateRoleAsync(Role role, IEnumerable<string> claims);
        Task<Tuple<bool, string[]>> CreateUserAsync(Customer user, IEnumerable<string> roles, string password);
        Task<Tuple<bool, string[]>> DeleteRoleAsync(Role role);
        Task<Tuple<bool, string[]>> DeleteRoleAsync(string roleName);
        Task<Tuple<bool, string[]>> DeleteUserAsync(Customer user);
        Task<Tuple<bool, string[]>> DeleteUserAsync(string userId);
        Task<Role> GetRoleByIdAsync(string roleId);
        Task<Role> GetRoleByNameAsync(string roleName);
        Task<Role> GetRoleLoadRelatedAsync(string roleName);
        Task<List<Role>> GetRolesLoadRelatedAsync(int page, int pageSize);
        Task<Tuple<Customer, string[]>> GetUserAndRolesAsync(int userId);
        Task<Customer> GetUserByEmailAsync(string email);
        Task<Customer> GetUserByIdAsync(string userId);
        Task<Customer> GetUserByUserNameAsync(string userName);
        Task<IList<string>> GetUserRolesAsync(Customer user);
        Task<List<Tuple<Customer, string[]>>> GetUsersAndRolesAsync(int page, int pageSize);
        Task<Tuple<bool, string[]>> ResetPasswordAsync(Customer user, string newPassword);
        Task<bool> TestCanDeleteRoleAsync(string roleId);
        Task<bool> TestCanDeleteUserAsync(string userId);
        Task<Tuple<bool, string[]>> UpdatePasswordAsync(Customer user, string currentPassword, string newPassword);
        Task<Tuple<bool, string[]>> UpdateRoleAsync(Role role, IEnumerable<string> claims);
        Task<Tuple<bool, string[]>> UpdateUserAsync(Customer user);
        Task<Tuple<bool, string[]>> UpdateUserAsync(Customer user, IEnumerable<string> roles);
        //Task<UsersGridDto> SearchUsersAsync(UsersGridCommand command);
        Task<SearchCustomerGridDto> SearchUsersAsync(UsersGridCommand command);
        
    }
}
