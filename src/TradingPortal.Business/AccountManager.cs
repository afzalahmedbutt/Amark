using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TradingPortal.Business.interfaces;
using TradingPortal.Core;
using TradingPortal.Core.Constants;
using TradingPortal.Core.Domain;
using TradingPortal.Core.Domain.Identity;
using TradingPortal.Core.Infrastructure;
using TradingPortal.Core.ViewModels;
using TradingPortal.Infrastructure;
using TradingPortal.Infrastructure.ComplexTypes;
using TradingPortal.Infrastructure.DatabaseContexts;
using TradingPortal.Infrastructure.Services.Interfaces;

namespace TradingPortal.Business
{
    public class AccountManager : IAccountManager
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Customer> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IGenericAttributeManager _genericAttributeManager;
        private readonly IStoreCommandRepository _storeRepository;

        public AccountManager(
            ApplicationDbContext context,
            UserManager<Customer> userManager,
            RoleManager<Role> roleManager,
            IGenericAttributeManager genericAttributeManager,
            IHttpContextAccessor httpAccessor,
            IStoreCommandRepository storeRepository)
        {
            _context = context;
            _context.CurrentUserId = httpAccessor.HttpContext?.User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value?.Trim();
            _userManager = userManager;
            _roleManager = roleManager;
            _genericAttributeManager = genericAttributeManager;
            _storeRepository = storeRepository;
        }


        public Task<bool> CheckPasswordAsync(Customer user, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<Tuple<bool, string[]>> CreateRoleAsync(Role role, IEnumerable<string> claims)
        {
            if (claims == null)
                claims = new string[] { };

            string[] invalidClaims = claims.Where(c => ApplicationPermissions.GetPermissionByValue(c) == null).ToArray();
            if (invalidClaims.Any())
                return Tuple.Create(false, new[] { "The following claim types are invalid: " + string.Join(", ", invalidClaims) });


            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());


            role = await _roleManager.FindByNameAsync(role.Name);

            foreach (string claim in claims.Distinct())
            {
                result = await this._roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, ApplicationPermissions.GetPermissionByValue(claim)));

                if (!result.Succeeded)
                {
                    await DeleteRoleAsync(role);
                    return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
                }
            }

            return Tuple.Create(true, new string[] { });
        }

        public async Task<Tuple<bool, string[]>> CreateUserAsync(Customer user, IEnumerable<string> roles, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());


            user = await _userManager.FindByNameAsync(user.Email);

            try
            {
                result = await this._userManager.AddToRolesAsync(user, roles.Distinct());
            }
            catch(Exception ex)
            {
                await DeleteUserAsync(user);
                throw ex;
            }

            if (!result.Succeeded)
            {
                await DeleteUserAsync(user);
                return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
            }

            return Tuple.Create(true, new string[] { });
        }

        public Task<Tuple<bool, string[]>> DeleteRoleAsync(Role role)
        {

            throw new NotImplementedException();
        }

        public Task<Tuple<bool, string[]>> DeleteRoleAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<Tuple<bool, string[]>> DeleteUserAsync(Customer user)
        {
            var result = await _userManager.DeleteAsync(user);
            return Tuple.Create(result.Succeeded, result.Errors.Select(e => e.Description).ToArray());
        }

        public async Task<Tuple<bool, string[]>> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
                return await DeleteUserAsync(user);

            return Tuple.Create(true, new string[] { });
        }

        public Task<Role> GetRoleByIdAsync(string roleId)
        {
            throw new NotImplementedException();
        }

        public Task<Role> GetRoleByNameAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<Role> GetRoleLoadRelatedAsync(string roleName)
        {
            var role = await _context.Roles
                //.Include(r => r.Claims)
                .Include(r => r.Users)
                .Where(r => r.Name == roleName)
                .FirstOrDefaultAsync();

            return role;
        }

        public async Task<List<Role>> GetRolesLoadRelatedAsync(int page, int pageSize)
        {
            IQueryable<Role> rolesQuery = _context.Roles
                //.Include(r => r.Claims)
                .Include(r => r.Users)
                .OrderBy(r => r.Name);

            if (page != -1)
                rolesQuery = rolesQuery.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                rolesQuery = rolesQuery.Take(pageSize);

            var roles = await rolesQuery.ToListAsync();

            return roles;
        }

        public async Task<Tuple<Customer, string[]>> GetUserAndRolesAsync(int userId)
        {
            var userT = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var user = await _context.Users
                .Include(u => u.Roles)
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
                return null;

            var userRoleIds = user.Roles.Select(r => r.RoleId).ToList();

            var roles = await _context.Roles
                .Where(r => userRoleIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToArrayAsync();
            //var vendors = await _vendorsRepository.GetAll()
            //              .Where(v => v.Active && !v.Deleted)
            //              .OrderBy(v => v.Name)
            //              .Select(v => new SelectListItem
            //              {
            //                  Text = v.Name,
            //                  Value = v.Id.ToString()
            //              }).ToListAsync();

            return Tuple.Create(user, roles);
        }

        

        public async Task<Customer> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<Customer> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<Customer> GetUserByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<IList<string>> GetUserRolesAsync(Customer user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<List<Tuple<Customer, string[]>>> GetUsersAndRolesAsync(int page, int pageSize)
        {
            IQueryable<Customer> usersQuery = _context.Users
                .Include(u => u.Roles)
                .OrderBy(u => u.UserName);

            if (page != -1)
                usersQuery = usersQuery.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                usersQuery = usersQuery.Take(pageSize);
            //usersQuery = usersQuery
            //    .Join(_genericAttributeManager.GetAll(), x => x.Id, y => y.EntityId, (x, y) => new { Customer = x, Attribute = y })
            //        .Where((z => z.Attribute.KeyGroup == "Customer" &&
            //            z.Attribute.Key == SystemCustomerAttributeNames.Company &&
            //            z.Attribute.Value.Contains(company)))
            //        .Select(z => z.Customer);

            var users = await usersQuery.ToListAsync();

            var userRoleIds = users.SelectMany(u => u.Roles.Select(r => r.RoleId)).ToList();

            var roles = await _context.Roles
                .Where(r => userRoleIds.Contains(r.Id))
                .ToArrayAsync();

            return users.Select(u => Tuple.Create(u,
                roles.Where(r => u.Roles.Select(ur => ur.RoleId).Contains(r.Id)).Select(r => r.Name).ToArray()))
                .ToList();
        }

        public async Task<SearchCustomerGridDto> SearchUsersAsync(UsersGridCommand command)
        {
            var roleIds = "";
            if (command.CustomerRoleIds.Length > 0)
            {
                roleIds = String.Join(',', command.CustomerRoleIds);
            }
            try
            {
                var customers = await _storeRepository.CreateStoreProcedureCommand("dbo.spSearchCustomers")
                .AddParameter("@FirstName", command.FirstName ?? "")
                .AddParameter("@LastName", command.LastName ?? "")
                .AddParameter("@Email", command.Email ?? "")
                .AddParameter("@FirstRow", command.FirstRow)
                .AddParameter("@LastRow", command.LastRow)
                .AddParameter("@roleIds", roleIds)
                .ExecuteToListAsync<SearchCustomerViewModel>();
                SearchCustomerGridDto customerDto = new SearchCustomerGridDto();
                customerDto.Customers = customers.ToList();
                customerDto.TotalCustomers = customers.Count() > 0 ? customers.First().TotalCustomers.Value : 0;

                return customerDto;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UsersGridDto> SearchUsersAsync1(UsersGridCommand command)
        {
            command.PageNumber = 1;
            command.PageSize = 15;
            var roleIds = "1";
            if (command.CustomerRoleIds.Length > 0)
            {
                roleIds = String.Join(',', command.CustomerRoleIds);
            }
            

            
            List<SearchCustomerViewModel> searchCustomers = null;
            try
            {
                var result = _storeRepository.CreateStoreProcedureCommand("dbo.spSearchCustomers")
                .AddParameter("@FirstName", command.FirstName ?? "")
                .AddParameter("@LastName", command.FirstName ?? "")
                .AddParameter("@Email", command.FirstName ?? "")
                .AddParameter("@FirstRow", command.PageNumber)
                .AddParameter("@LastRow", command.PageSize)
                .AddParameter("@roleIds", roleIds)
                .ExecuteToListAsync<SearchCustomerViewModel>().Result.ToList();
                searchCustomers =  await _context.Query<SearchCustomerViewModel>().FromSql($"Execute [dbo].[spSearchCustomers] {command.FirstName ?? ""} {command.LastName ?? ""} {command.Email ?? ""} {command.Company ?? ""} {command.PageNumber} {command.PageSize} {roleIds}]").ToListAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            IQueryable<Customer> usersQuery = _context.Users
                .Include(u => u.Roles)
                .OrderBy(u => u.UserName);
            if(command.CustomerRoleIds != null && command.CustomerRoleIds.Length > 0)
            {
                //usersQuery = usersQuery.Where(u => u.Roles.Select(r => r.RoleId).Intersect(command.CustomerRoleIds).Any());
                usersQuery = usersQuery.Where(u => u.Roles.Any(r => command.CustomerRoleIds.Any(cid => cid == r.RoleId)));
            }
            if(!String.IsNullOrEmpty(command.Email))
            {
                usersQuery = usersQuery.Where(u => u.Email.Contains(command.Email));
            }
            if (!String.IsNullOrEmpty(command.FirstName))
            {
                usersQuery = usersQuery
                    .Join(_genericAttributeManager.GetAll(), x => x.Id, y => y.EntityId, (x, y) => new { Customer = x, Attribute = y })
                    .Where((z => z.Attribute.KeyGroup == "Customer" &&
                        z.Attribute.Key == SystemCustomerAttributeNames.FirstName &&
                        z.Attribute.Value.Contains(command.FirstName)))
                    .Select(z => z.Customer);
            }
            if (!String.IsNullOrEmpty(command.LastName))
            {
                usersQuery = usersQuery
                    .Join(_genericAttributeManager.GetAll(), x => x.Id, y => y.EntityId, (x, y) => new { Customer = x, Attribute = y })
                    .Where((z => z.Attribute.KeyGroup == "Customer" &&
                    z.Attribute.Key == SystemCustomerAttributeNames.LastName &&
                    z.Attribute.Value.Contains(command.LastName)))
                    .Select(z => z.Customer);
            }
            if (!String.IsNullOrWhiteSpace(command.Company))
            {
                usersQuery = usersQuery
                    .Join(_genericAttributeManager.GetAll(), x => x.Id, y => y.EntityId, (x, y) => new { Customer = x, Attribute = y })
                    .Where((z => z.Attribute.KeyGroup == "Customer" &&
                    z.Attribute.Key == SystemCustomerAttributeNames.Company &&
                    z.Attribute.Value.Contains(command.Company)))
                    .Select(z => z.Customer);
            }
                       
            try
            {
                var result = new UsersGridDto();
                //int count = await usersQuery.CountAsync();
                usersQuery = usersQuery.Skip((command.PageNumber - 1) * command.PageSize);
                usersQuery = usersQuery.Take(command.PageSize);


                var users = await usersQuery.ToListAsync();


                var userRoleIds = users.SelectMany(u => u.Roles.Select(r => r.RoleId)).ToList();

                var roles = await _context.Roles
                    .Where(r => userRoleIds.Contains(r.Id))
                    .ToArrayAsync();

                result.Customers = users.Select(u => Tuple.Create(u,
                   roles.Where(r => u.Roles.Select(ur => ur.RoleId).Contains(r.Id)).Select(r => r.Name).ToArray()))
                    .ToList();
                result.Count = 100;
                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public Task<Tuple<bool, string[]>> ResetPasswordAsync(Customer user, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TestCanDeleteRoleAsync(string roleId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TestCanDeleteUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<bool, string[]>> UpdatePasswordAsync(Customer user, string currentPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<bool, string[]>> UpdateRoleAsync(Role role, IEnumerable<string> claims)
        {
            throw new NotImplementedException();
        }

        public async Task<Tuple<bool, string[]>> UpdateUserAsync(Customer user)
        {
            return await UpdateUserAsync(user, null);
        }

        public async Task<Tuple<bool, string[]>> UpdateUserAsync(Customer user, IEnumerable<string> roles)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());


            if (roles != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var rolesToRemove = userRoles.Except(roles).ToArray();
                var rolesToAdd = roles.Except(userRoles).Distinct().ToArray();

                if (rolesToRemove.Any())
                {
                    result = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    if (!result.Succeeded)
                        return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
                }

                if (rolesToAdd.Any())
                {
                    result = await _userManager.AddToRolesAsync(user, rolesToAdd);
                    if (!result.Succeeded)
                        return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
                }
            }

            return Tuple.Create(true, new string[] { });
        }

    }
}
