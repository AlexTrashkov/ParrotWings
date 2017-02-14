using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using ParrotWings.Server.Domain.Entities;
using ParrotWings.Server.Domain.Exceptions;
using ParrotWings.Server.Domain.Repositories;
using ParrotWings.Server.Domain.Services;
using ParrotWings.Server.Domain.Services.Implementations;
using ParrotWings.Server.Web.Extensions;
using ParrotWings.Server.Web.Models;

namespace ParrotWings.Server.Web.Controllers
{
	[RoutePrefix("api/Account")]
	public sealed class AccountController : ApiController
	{
		[NotNull] private readonly IUserRepository _userRepository;
		[NotNull] private readonly UserService _userService;
		[NotNull] private readonly ISecurityService _securityService;
		[NotNull] private readonly IMapper _mapper;

		public AccountController(
			[NotNull] IUserRepository userRepository,
			[NotNull] UserService userService,
			[NotNull] ISecurityService securityService,
			[NotNull] IMapper mapper)
		{
			_userRepository = userRepository;
			_userService = userService;
			_securityService = securityService;
			_mapper = mapper;
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("Login")]
		public async Task<HttpResponseMessage> Login([FromBody] AccountLoginDTO model)
		{
			var user = await _userRepository.FindByAsync(model.Email, model.Password);

			if (user == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound, "Incorrect email or password");
			}

			var identity = await _userService.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

			Authentication.SignIn(new AuthenticationProperties {IsPersistent = false}, identity);
			Authentication.SignOut(new AuthenticationProperties {IsPersistent = false});
			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("Logout")]
		public IHttpActionResult Logout()
		{
			Authentication.SignOut(new AuthenticationProperties {IsPersistent = false});
			return Ok();
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("Info")]
		public async Task<HttpResponseMessage> Info()
		{
			if (!Authentication.User.Identity.IsAuthenticated)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound, "You are not authorized");
			}
			
			var currentUserId = Authentication.User.Identity.GetUserIdAsGuid();
			var currentUser = await _userRepository.GetByAsync(currentUserId);

			return Request.CreateResponse(_mapper.Map<User, AccountInfoDTO>(currentUser));
		}


		[HttpPost]
		[AllowAnonymous]
		[Route("Register")]
		public async Task<HttpResponseMessage> Register([FromBody] AccountRegisterDTO model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return Request.CreateResponse(HttpStatusCode.BadRequest);
				}

				var salt = _securityService.CreateSalt();
				var hashedPass = _securityService.CalculateHash(model.Password, salt);
				var createdUser = new User(model.UserName, model.Email, hashedPass, salt);

				await _userRepository.CreateAsync(createdUser);
				return Request.CreateResponse(HttpStatusCode.OK);
			}
			catch (CurrentEmailIsAlreadyUsed e)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, "The email address you have entered is already registered");
			}
			catch (InvalidEmailException e)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, "Incorrect email");
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		private IAuthenticationManager Authentication => Request.GetOwinContext().Authentication;
	}
}