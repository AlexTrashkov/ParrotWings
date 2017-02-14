using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using ParrotWings.Server.Domain.Entities;
using ParrotWings.Server.Domain.Exceptions;
using ParrotWings.Server.Domain.Repositories;
using ParrotWings.Server.Domain.Services;
using ParrotWings.Server.Storage.DTO;

namespace ParrotWings.Server.Storage.Repositories
{
	internal sealed class UserRepository : IUserRepository
	{
		[NotNull] private readonly ParrotWingsDataContext _dataContext;
		[NotNull] private readonly IMapper _mapper;
		[NotNull] private readonly ISecurityService _securityService;

		public UserRepository(
			[NotNull] ParrotWingsDataContext dataContext,
			[NotNull] IMapper mapper,
			[NotNull]ISecurityService securityService)
		{
			_dataContext = dataContext;
			_mapper = mapper;
			_securityService = securityService;
		}

		public async Task<User> GetByAsync(Guid userId)
		{
			var result = await _dataContext.Users.FirstAsync(x => x.Id == userId);
			return _mapper.Map<UserDTO, User>(result);
		}

		public async Task<User> FindByAsync(string email)
		{
			var result = await _dataContext.Users.FirstOrDefaultAsync(x => x.Email == email);

			return result != null
				? _mapper.Map<UserDTO, User>(result)
				: null;
		}

		public async Task<User> FindByAsync(string email, string password)
		{
			var result = await _dataContext.Users.FirstOrDefaultAsync(x => x.Email == email);

			if (result == null)
				return null;

			var verificationResult = _securityService.CalculateHash(password, result.Salt) == result.HashedPass;

			return verificationResult
				? _mapper.Map<UserDTO, User>(result)
				: null;
		}

		public async Task<IReadOnlyCollection<User>> ListByAsync(string searchString, uint offset, uint count)
		{
			var result = await _dataContext.Users.Where(x =>
				x.Email.ToLower().Contains(searchString.ToLower()) ||
				x.UserName.ToLower().Contains(searchString.ToLower()))
				.OrderBy(x => x.UserName)
				.Skip((int) offset)
				.Take((int) count)
				.ToArrayAsync();

			return result
				.Select(_mapper.Map<UserDTO, User>)
				.ToArray();
		}

		public async Task CreateAsync(User user)
		{
			var duplicatedUser = await FindByAsync(user.Email);

			if (duplicatedUser != null)
			{
				throw new CurrentEmailIsAlreadyUsed();
			}

			_dataContext.Users.Add(_mapper.Map<User, UserDTO>(user));
			await _dataContext.SaveChangesAsync();
		}

		public Task UpdateAsync(User user)
		{
			throw new NotImplementedException();
		}

		public async Task DeleteAsync(User user)
		{
			_dataContext.Users.Remove(_mapper.Map<User, UserDTO>(user));
			await _dataContext.SaveChangesAsync();
		}

		public async Task<User> FindByIdAsync(Guid userId)
		{
			var result = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
			return _mapper.Map<UserDTO, User>(result);
		}

		public Task<User> FindByNameAsync(string userName)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{

		}
	}
}