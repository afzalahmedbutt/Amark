using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using TradingPortal.Business.interfaces;
using TradingPortal.Core.Domain.Identity;
using TradingPortal.Core.Infrastructure;
using TradingPortal.Infrastructure.Services.Interfaces;

namespace TradingPortal.Web.Controllers
{
    [Route("api/[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly SignInManager<Customer> _signInManager;
        private readonly UserManager<Customer> _userManager;
        private readonly IGenericAttributeManager _genericAttributeManager;
        private readonly ICustomerManager _customerManager;
        public readonly ISettingsService _settingsService;


        public AuthorizationController(
            IOptions<IdentityOptions> identityOptions,
            SignInManager<Customer> signInManager,
            UserManager<Customer> userManager,
            IGenericAttributeManager genericAttributeManager,
            ICustomerManager customerManager,
            ISettingsService settingsService)
            
        {
            _identityOptions = identityOptions;
            _signInManager = signInManager;
            _userManager = userManager;
            _genericAttributeManager = genericAttributeManager;
            _customerManager = customerManager;
            _settingsService = settingsService;
        }


        [HttpPost("~/connect/token")]
        [Produces("application/json")]
        public async Task<IActionResult> Exchange(OpenIdConnectRequest request)
        {
            try
            {
                if (request.IsPasswordGrantType())
            {
                    var user = await _userManager.FindByEmailAsync(request.Username) ?? await _userManager.FindByNameAsync(request.Username);
                    //var roles = user.Roles;
                    if (user == null)
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.InvalidGrant,
                            ErrorDescription = "Please check that your email and password is correct"
                        });
                    }
                    
                    // Validate the username/password parameters and ensure the account is not locked out.
                    var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);

                    // Ensure the user is not already locked out.
                    if (result.IsLockedOut)
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.InvalidGrant,
                            ErrorDescription = "The specified user account has been suspended"
                        });
                    }

                    // Reject the token request if two-factor authentication has been enabled by the user.
                    if (result.RequiresTwoFactor)
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.InvalidGrant,
                            ErrorDescription = "Invalid login procedure"
                        });
                    }

                    // Ensure the user is allowed to sign in.
                    if (result.IsNotAllowed)
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.InvalidGrant,
                            ErrorDescription = "The specified user is not allowed to sign in"
                        });
                    }

                    if (!result.Succeeded)
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.InvalidGrant,
                            ErrorDescription = "Please check that your email and password is correct"
                        });
                    }

                    // Create a new authentication ticket.
                    var ticket = await CreateTicketAsync(request, user);

                    //Task.Run( async () =>
                    //{
                    //    await _customerManager.UpdatePortalProducts(user);
                    //});
                    await _customerManager.UpdatePortalProducts(user);

                    var userRoles = await _userManager.GetRolesAsync(user);
                    var signInResult =  SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
                    
                    //await _customerManager.UpdatePortalProducts(user);
                    return signInResult;

                }
            else if (request.IsRefreshTokenGrantType())
                {
                    // Retrieve the claims principal stored in the refresh token.
                    var info = await HttpContext.AuthenticateAsync(OpenIddictServerDefaults.AuthenticationScheme);

                    // Retrieve the user profile corresponding to the refresh token.
                    // Note: if you want to automatically invalidate the refresh token
                    // when the user password/roles change, use the following line instead:
                    // var user = _signInManager.ValidateSecurityStampAsync(info.Principal);
                    var user = await _userManager.GetUserAsync(info.Principal);
                    if (user == null)
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.InvalidGrant,
                            ErrorDescription = "The refresh token is no longer valid"
                        });
                    }

                    // Ensure the user is still allowed to sign in.
                    if (!await _signInManager.CanSignInAsync(user))
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.InvalidGrant,
                            ErrorDescription = "The user is no longer allowed to sign in"
                        });
                    }

                    // Create a new authentication ticket, but reuse the properties stored
                    // in the refresh token, including the scopes originally granted.
                    var ticket = await CreateTicketAsync(request, user);
                    

                    return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
                }
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                    ErrorDescription = "The specified grant type is not supported"
                });
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }

        private async Task<AuthenticationTicket> CreateTicketAsync(OpenIdConnectRequest request, Customer user)
        {
            // Create a new ClaimsPrincipal containing the claims that
            // will be used to create an id_token, a token or a code.
            ClaimsPrincipal principal;
            try
            {
                principal = await _signInManager.CreateUserPrincipalAsync(user);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            // Create a new authentication ticket holding the user identity.
            var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), OpenIddictServerDefaults.AuthenticationScheme);


            if (!request.IsRefreshTokenGrantType())
            {
                // Set the list of scopes granted to the client application.
                // Note: the offline_access scope must be granted
                // to allow OpenIddict to return a refresh token.
                ticket.SetScopes(new[]
            {
                    OpenIdConnectConstants.Scopes.OpenId,
                    OpenIdConnectConstants.Scopes.Email,
                    OpenIdConnectConstants.Scopes.Phone,
                    OpenIdConnectConstants.Scopes.Profile,
                    OpenIdConnectConstants.Scopes.OfflineAccess,
                    OpenIddictConstants.Scopes.Roles
            }.Intersect(request.GetScopes()));
            }

            

            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.

            foreach (var claim in ticket.Principal.Claims)
            {
                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                if (claim.Type == _identityOptions.Value.ClaimsIdentity.SecurityStampClaimType)
                    continue;


                var destinations = new List<string> { OpenIdConnectConstants.Destinations.AccessToken };

                // Only add the iterated claim to the id_token if the corresponding scope was granted to the client application.
                // The other claims will only be added to the access_token, which is encrypted when using the default format.
                if ((claim.Type == OpenIdConnectConstants.Claims.Subject && ticket.HasScope(OpenIdConnectConstants.Scopes.OpenId)) ||
                    (claim.Type == OpenIdConnectConstants.Claims.Name && ticket.HasScope(OpenIdConnectConstants.Scopes.Profile)) ||
                    (claim.Type == OpenIdConnectConstants.Claims.Role && ticket.HasScope(OpenIddictConstants.Claims.Roles)) ||
                    (claim.Type == CustomClaimTypes.Permission && ticket.HasScope(OpenIddictConstants.Claims.Roles)))
                {
                    destinations.Add(OpenIdConnectConstants.Destinations.IdentityToken);
                }


                claim.SetDestinations(destinations);
            }


            var identity = principal.Identity as ClaimsIdentity;

            var customerAttributes = await _genericAttributeManager.GetCustomerAttributes(user.Id);
            if (ticket.HasScope(OpenIdConnectConstants.Scopes.Profile))
            {
                //if (!string.IsNullOrWhiteSpace(user.JobTitle))
                //    identity.AddClaim(CustomClaimTypes.JobTitle, user.JobTitle, OpenIdConnectConstants.Destinations.IdentityToken);

                if (!string.IsNullOrWhiteSpace(customerAttributes.FirstName))
                    identity.AddClaim(CustomClaimTypes.FirstName, customerAttributes.FirstName, OpenIdConnectConstants.Destinations.IdentityToken);

                if (!string.IsNullOrWhiteSpace(customerAttributes.LastName))
                    identity.AddClaim(CustomClaimTypes.LastName, customerAttributes.LastName, OpenIdConnectConstants.Destinations.IdentityToken);
                //if (!string.IsNullOrWhiteSpace(user.Configuration))
                //    identity.AddClaim(CustomClaimTypes.Configuration, user.Configuration, OpenIdConnectConstants.Destinations.IdentityToken);
            }

            if (ticket.HasScope(OpenIdConnectConstants.Scopes.Email))
            {
                if (!string.IsNullOrWhiteSpace(user.Email))
                    identity.AddClaim(CustomClaimTypes.Email, user.Email, OpenIdConnectConstants.Destinations.IdentityToken);
            }

            if (ticket.HasScope(OpenIdConnectConstants.Scopes.Phone))
            {
                if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
                    identity.AddClaim(CustomClaimTypes.Phone, user.PhoneNumber, OpenIdConnectConstants.Destinations.IdentityToken);
            }

            if(ticket.HasScope(OpenIddictConstants.Scopes.Roles))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                identity.AddClaims(userRoles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));
            }

            var isStoreClosed = await _settingsService.GetSettingsByKeyAndStoreIdAsync<bool>("storeinformationsettings.storeclosed", storeId: 1);
            identity.AddClaim(CustomClaimTypes.Configuration, isStoreClosed ? "storeclosed" : "storeopen", OpenIdConnectConstants.Destinations.IdentityToken);

            return ticket;
        }
    }
}