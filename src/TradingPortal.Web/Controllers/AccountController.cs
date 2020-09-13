using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TradingPortal.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenIddict.Validation;

using TradingPortal.Core.ViewModels;
using TradingPortal.Core.Domain.Identity;
using TradingPortal.Core.Interfaces;
using TradingPortal.Business.interfaces;
using TradingPortal.Infrastructure.Repositories;
using TradingPortal.Core.Domain;
using TradingPortal.Core;
using AutoMapper;
using TradingPortal.Web.Helpers;
using TradingPortal.Web.Authorization;
using TradingPortal.Core.Constants;

namespace TradingPortal.Web.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
    public class AccountController : Controller
    {
        private readonly IAccountManager _accountManager;
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IEmailManager _emailManager;
        private readonly IGenericAttributeManager _genericAttributeManager;
        private readonly IUserStore<Customer> _userStore;
        private readonly ICurrentUser _currentUser;
        private readonly IEncryptionService _encryptionService;
        private readonly IRepository<MessageTemplate> _messageTemplateRepository;
        private readonly IAuthorizationService _authorizationService;

        private const string GetUserByIdActionName = "GetUserById";
        private const string GetRoleByIdActionName = "GetRoleById";

        public AccountController(
            UserManager<Customer> userManager,
            SignInManager<Customer> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger,
            IEmailManager emailManager,
            IGenericAttributeManager genericAttributeManager,
            IUserStore<Customer> userStore,
            ICurrentUser currentUse,
            IEncryptionService encryptionService,
            IRepository<MessageTemplate> messageTemplateRepository,
            IAccountManager accountManager,
            IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _emailManager = emailManager;
            _genericAttributeManager = genericAttributeManager;
            _userStore = userStore;
            _currentUser = currentUse;
            _encryptionService = encryptionService;
            _messageTemplateRepository = messageTemplateRepository;
            _accountManager = accountManager;
            _authorizationService = authorizationService;
        }


        [Route("forgotpassword/{email}")]
        [AllowAnonymous]
        public async Task<bool> ForgotPassword(string email)
        {
            try
            {
                var currentUser = await _userManager.FindByNameAsync(email);
                if (currentUser == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return false;
                  
                }
                
                var code = Guid.NewGuid();
                var messageTemplate = await _messageTemplateRepository.FindAsync(mt => mt.Name == "Customer.AMarkPasswordRecovery");
                await _genericAttributeManager.SaveCustomerAttribute(currentUser.Id, "PasswordRecoveryToken", "Customer", code.ToString());
                var host = Request.Scheme + "://" + Request.Host;
                var callbackUrl = host + "/confirmpassword?userId=" + currentUser.Id + "&passwordResetCode=" + code;
                var confirmationLink = "<a class='btn-primary' href=\"" + callbackUrl + "\">Reset your password</a>";
                await SetPasswordRecoveryMessage(messageTemplate, currentUser.Id, callbackUrl);
                var emailAccount = _emailManager.GetEmailAccountById(messageTemplate.EmailAccountId);
                //await _emailSender.SendEmailAsync(messageTemplate, email, messageTemplate.Subject, emailAccount);
                await _emailSender.SendEmailAsync(messageTemplate, messageTemplate.Subject, emailAccount,new String[] { email });
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task SetPasswordRecoveryMessage(MessageTemplate messageTemplate,int userId,string callbackUrl)
        {
            var customerAttributes = await _genericAttributeManager.GetCustomerAttributes(userId);
            messageTemplate.Body = messageTemplate.Body.Replace("%Customer.FullName%", customerAttributes.FirstName + " " +customerAttributes.LastName);
            messageTemplate.Body = messageTemplate.Body.Replace("%Customer.PasswordRecoveryURL%", callbackUrl);
        }

        
        [AllowAnonymous]
        [HttpPost("resetpassword")]
        //[ValidateAntiForgeryToken]
        public async Task<IdentityResultCore> ResetPassword([FromBody]ResetPasswordViewModel model)
        {
            var identityResult = new IdentityResultCore { Succeeded = false };
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                identityResult.Errors.Add("Please enter a valid email address!!");
                return identityResult;
            }
            var resetPasswordCode = _genericAttributeManager.GetPasswordRecoveryToken(user.Id);
            if (model.Code == resetPasswordCode)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
                identityResult.Succeeded = result.Succeeded;
                if (!identityResult.Succeeded)
                {
                    identityResult.Errors = result.Errors.Select(e => e.Description).ToList();
                }

            }
            else
            {
                identityResult.Errors.Add("Invalid security token!");
            }
            return identityResult;
        }

        
        [HttpGet("getpasswordrecoverytoken")]
        public PasswordRecoveryToken GetPasswordRecoverytoken()
        {
            var currentUser = _currentUser.User;
            var code = Guid.NewGuid();
            _genericAttributeManager.SaveCustomerAttribute(currentUser.Id, "PasswordRecoveryToken", "Customer", code.ToString());
            PasswordRecoveryToken passwordRecoveryToken = new PasswordRecoveryToken
            {
                Token = code.ToString(),
                UserId = currentUser.Id
            };
            return passwordRecoveryToken;
        }

        
        [HttpPost("changepassword")]
        public async Task<IdentityResultCore> ChangePassword([FromBody]ResetPasswordViewModel model)
        {
            var user = _currentUser.User;
            var identityResult = new IdentityResultCore { Succeeded = false };
            if (_userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password)
                != PasswordVerificationResult.Failed)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                identityResult.Succeeded = result.Succeeded;
                if (!identityResult.Succeeded)
                {
                    identityResult.Errors = result.Errors.Select(e => e.Description).ToList();
                }
                return identityResult;
            }
            else
            {
                identityResult.Errors.Add("Password is not correct!");
            }
            return identityResult;

        }

        [HttpGet("users/me")]
        [ProducesResponseType(200, Type = typeof(UserViewModel))]
        public async Task<IActionResult> GetCurrentUser()
        {
            return await GetUserByUserName(this.User.Identity.Name);
        }


        [HttpGet("users/{id}", Name = GetUserByIdActionName)]
        [ProducesResponseType(200, Type = typeof(UserViewModel))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserById(int id)
        {
            //if (!(await _authorizationService.AuthorizeAsync(this.User, id, AccountManagementOperations.Read)).Succeeded)
            //    return new ChallengeResult();


            UserViewModel userVM = await GetUserViewModelHelper(id);
            
            if (userVM != null)
                return Ok(userVM);
            else
                return NotFound(id);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserEditViewModel user)
        {
            //if (!(await _authorizationService.AuthorizeAsync(this.User, Tuple.Create(user.Roles, new string[] { }), Authorization.Policies.AssignAllowedRolesPolicy)).Succeeded)
            //    return new ChallengeResult();
            user.CreatedOnUtc = DateTime.UtcNow;
            user.LastActivityDateUtc = DateTime.UtcNow;


            if (ModelState.IsValid)
            {
                if (user == null)
                    return BadRequest($"{nameof(user)} cannot be null");


                Customer customer = Mapper.Map<Customer>(user);
                customer.CustomerGuid = Guid.NewGuid();
                var result = await _accountManager.CreateUserAsync(customer, user.Roles, user.Password);
                if (result.Item1)
                {
                    await _genericAttributeManager.SaveAttribute(customer, SystemCustomerAttributeNames.FirstName, user.FirstName);
                    await _genericAttributeManager.SaveAttribute(customer, SystemCustomerAttributeNames.LastName, user.LastName);
                    await _genericAttributeManager.SaveAttribute(customer, SystemCustomerAttributeNames.AmarkTradingPartnerNumber, user.AmarkTradingPartnerNumber);
                    await _genericAttributeManager.SaveAttribute(customer, SystemCustomerAttributeNames.AmarkTPAPIKey, user.AmarkTPAPIKey);
                    await _genericAttributeManager.SaveAttribute(customer, SystemCustomerAttributeNames.Company, user.CompanyName);
                    UserViewModel userVM = await GetUserViewModelHelper(customer.Id);
                    return CreatedAtAction(GetUserByIdActionName, new { id = userVM.Id }, userVM);
                }

                AddErrors(result.Item2);
            }

            return BadRequest(ModelState);
        }


        [HttpGet("users/username/{userName}")]
        [ProducesResponseType(200, Type = typeof(UserViewModel))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            Customer appUser = await _accountManager.GetUserByUserNameAsync(userName);

            if (!(await _authorizationService.AuthorizeAsync(this.User, appUser?.Id ?? null, AccountManagementOperations.Read)).Succeeded)
                return new ChallengeResult();

            if (appUser == null)
                return NotFound(userName);

            return await GetUserById(appUser.Id);
        }


        [HttpGet("users")]
        //[Authorize(Authorization.Policies.ViewAllUsersPolicy)]
        //[Authorize(Roles = "Administrators")]
        [ProducesResponseType(200, Type = typeof(List<UserViewModel>))]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                return await GetUsers(-1, -1);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("users/{pageNumber:int}/{pageSize:int}")]
        [Authorize(Authorization.Policies.ViewAllUsersPolicy)]
        [ProducesResponseType(200, Type = typeof(List<UserViewModel>))]
        public async Task<IActionResult> GetUsers(int pageNumber, int pageSize)
        {
            var usersAndRoles = await _accountManager.GetUsersAndRolesAsync(pageNumber, pageSize);
            //var usersAndRoles = await _accountManager.GetUsersAndRolesAsync();
            List<UserViewModel> usersVM = new List<UserViewModel>();

            foreach (var item in usersAndRoles)
            {
                var userVM = Mapper.Map<UserViewModel>(item.Item1);
                var customerAttributes = await _genericAttributeManager.GetCustomerAttributes(userVM.Id);
                userVM.FullName = String.Format("{0} {1}",customerAttributes.FirstName,customerAttributes.LastName);
                userVM.CompanyName = customerAttributes.Company;

                userVM.Roles = item.Item2;

                usersVM.Add(userVM);
            }

            return Ok(usersVM);
        }

        [HttpPost("searchcustomers")]
        //[Authorize(Authorization.Policies.ViewAllUsersPolicy)]
        [ProducesResponseType(200, Type = typeof(List<UserViewModel>))]
        public async Task<IActionResult> SearchCustomers([FromBody]UsersGridCommand command)
        {
            var customerGridResponse = await _accountManager.SearchUsersAsync(command);
            return Ok(customerGridResponse);
           
        }

        [HttpPut("users/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserEditViewModel user)
        {
            Customer appUser = await _accountManager.GetUserByIdAsync(id);
            string[] currentRoles = appUser != null ? (await _accountManager.GetUserRolesAsync(appUser)).ToArray() : null;
            
            if (ModelState.IsValid)
            {
                if (user == null)
                    return BadRequest($"{nameof(user)} cannot be null");

                if (appUser == null)
                    return NotFound(id);
              

                bool isValid = true;

                if (isValid)
                {
                    Mapper.Map<UserViewModel, Customer>(user, appUser);

                    var result = await _accountManager.UpdateUserAsync(appUser, user.Roles);
                    if (result.Item1)
                    {
                        await _genericAttributeManager.SaveAttribute(appUser, SystemCustomerAttributeNames.FirstName, user.FirstName);
                        await _genericAttributeManager.SaveAttribute(appUser, SystemCustomerAttributeNames.LastName, user.LastName);
                        await _genericAttributeManager.SaveAttribute(appUser, SystemCustomerAttributeNames.AmarkTradingPartnerNumber, user.AmarkTradingPartnerNumber);
                        await _genericAttributeManager.SaveAttribute(appUser, SystemCustomerAttributeNames.AmarkTPAPIKey, user.AmarkTPAPIKey);
                        await _genericAttributeManager.SaveAttribute(appUser, SystemCustomerAttributeNames.Company, user.CompanyName);
                        if (!string.IsNullOrWhiteSpace(user.NewPassword))
                        {
                            if (!string.IsNullOrWhiteSpace(user.CurrentPassword))
                                result = await _accountManager.UpdatePasswordAsync(appUser, user.CurrentPassword, user.NewPassword);
                            else
                                result = await _accountManager.ResetPasswordAsync(appUser, user.NewPassword);
                        }

                        if (result.Item1)
                            return NoContent();
                    }

                    AddErrors(result.Item2);
                }
            }

            return BadRequest(ModelState);
        }


        [HttpPut("users/me")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UserEditViewModel user)
        {
            return await UpdateUser(Utilities.GetUserId(this.User), user);
        }

        [HttpDelete("users/{id}")]
        [ProducesResponseType(200, Type = typeof(UserViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (!(await _authorizationService.AuthorizeAsync(this.User, id, AccountManagementOperations.Delete)).Succeeded)
                return new ChallengeResult();

            if (!await _accountManager.TestCanDeleteUserAsync(id))
                return BadRequest("User cannot be deleted. Delete all orders associated with this user and try again");


            UserViewModel userVM = null;
            Customer appUser = await this._accountManager.GetUserByIdAsync(id);

            if (appUser != null)
                userVM = await GetUserViewModelHelper(appUser.Id);


            if (userVM == null)
                return NotFound(id);

            var result = await this._accountManager.DeleteUserAsync(appUser);
            if (!result.Item1)
                throw new Exception("The following errors occurred whilst deleting user: " + string.Join(", ", result.Item2));


            return Ok(userVM);
        }

        [HttpGet("roles")]
        //[Authorize(Authorization.Policies.ViewAllRolesPolicy)]
        [ProducesResponseType(200, Type = typeof(List<RoleViewModel>))]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                return await GetRoles(-1, -1);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
              


        

        [HttpGet("roles/{pageNumber:int}/{pageSize:int}")]
        //[Authorize(Authorization.Policies.ViewAllRolesPolicy)]
        [ProducesResponseType(200, Type = typeof(List<RoleViewModel>))]
        public async Task<IActionResult> GetRoles(int pageNumber, int pageSize)
        {
            var roles = await _accountManager.GetRolesLoadRelatedAsync(pageNumber, pageSize);
            return Ok(Mapper.Map<List<RoleViewModel>>(roles));
        }

        private async Task<UserViewModel> GetUserViewModelHelper(int userId)
        {
            var userAndRoles = await _accountManager.GetUserAndRolesAsync(userId);
            if (userAndRoles == null)
                return null;

            var userVM = Mapper.Map<UserViewModel>(userAndRoles.Item1);
            var customerAttributes = await _genericAttributeManager.GetCustomerAttributes(userVM.Id);
            userVM.FirstName = customerAttributes.FirstName;
            userVM.LastName = customerAttributes.LastName;
            userVM.CompanyName = customerAttributes.Company;
            userVM.AmarkTradingPartnerNumber = customerAttributes.AmarkTradingPartnerNumber;
            userVM.AmarkTPAPIKey = customerAttributes.AmarkTPAPIKey;

            userVM.Roles = userAndRoles.Item2;

            return userVM;
        }

        private async Task<RoleViewModel> GetRoleViewModelHelper(string roleName)
        {
            var role = await _accountManager.GetRoleLoadRelatedAsync(roleName);
            if (role != null)
                return Mapper.Map<RoleViewModel>(role);
            return null;
        }

        private void AddErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

    }
}