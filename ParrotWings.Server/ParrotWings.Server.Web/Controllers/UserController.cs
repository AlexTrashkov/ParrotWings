using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using JetBrains.Annotations;
using ParrotWings.Server.Domain.Entities;
using ParrotWings.Server.Domain.Repositories;
using ParrotWings.Server.Web.Models;

namespace ParrotWings.Server.Web.Controllers
{
	[Authorize]
	public sealed class UserController : ApiController
	{
		[NotNull] private readonly IUserRepository _userRepository;
		[NotNull] private readonly IMapper _mapper;

		public UserController([NotNull] IUserRepository userRepository, IMapper mapper)
		{
			_userRepository = userRepository;
			_mapper = mapper;
		}

		public async Task<IReadOnlyCollection<UserInfoDTO>> Get(string searchString = "", uint offset = 0, uint count = 100)
		{
			var users = await _userRepository.ListByAsync(searchString, offset, count);

			return users
				.Select(_mapper.Map<User, UserInfoDTO>)
				.ToArray();
		}

		public async Task<UserInfoDTO> Get(string email)
		{
			var user = await _userRepository.FindByAsync(email);
			return _mapper.Map<User, UserInfoDTO>(user);
		}
	}
}