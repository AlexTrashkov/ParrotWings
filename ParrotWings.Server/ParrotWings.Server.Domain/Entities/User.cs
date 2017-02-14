using System;
using JetBrains.Annotations;
using Microsoft.AspNet.Identity;
using ParrotWings.Server.Domain.Exceptions;
using ParrotWings.Server.Domain.Extensions;

namespace ParrotWings.Server.Domain.Entities
{
	public sealed class User : IUser<Guid>
	{
		private const decimal DEFAULT_BALANCE_FOR_NEW_USERS = 500m;

		public Guid Id { get; }
		public DateTime CreateDate { get; }

		[NotNull]
		public string UserName { get; set; }

		[NotNull]
		public string Email { get; }

		[NotNull]
		public string HashedPass { get; }

		[NotNull]
		public string Salt { get; }

		private decimal _currentBalance;
		public decimal CurrentBalance
		{
			get { return _currentBalance; }
			internal set
			{
				if (value < 0)
				{
					throw new UserBalanceLessThanZeroException();
				}

				_currentBalance = value;
			}
		}

		public User(
			[NotNull] string userName,
			[NotNull] string email,
			[NotNull] string hashedPass,
			[NotNull] string salt)
			: this(
				Guid.NewGuid(),
				DateTime.UtcNow,
				userName,
				email,
				hashedPass,
				salt,
				DEFAULT_BALANCE_FOR_NEW_USERS)
		{
		}

		public User(
			Guid id,
			DateTime createDate,
			[NotNull] string userName,
			[NotNull] string email,
			[NotNull] string hashedPass,
			[NotNull] string salt,
			decimal currentBalance)
		{
			Id = id;
			CreateDate = createDate;
			UserName = userName;

			if (!email.IsValidEmail())
			{
				throw new InvalidEmailException();
			}

			Email = email;
			HashedPass = hashedPass;
			Salt = salt;

			if (currentBalance < 0)
			{
				throw new UserBalanceLessThanZeroException();
			}

			CurrentBalance = currentBalance;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hash = 17;

				hash = hash*23 + Id.GetHashCode();
				hash = hash*23 + CreateDate.GetHashCode();
				// ReSharper disable once NonReadonlyMemberInGetHashCode
				hash = hash*23 + UserName.GetHashCode();
				hash = hash*23 + Email.GetHashCode();
				hash = hash*23 + HashedPass.GetHashCode();
				hash = hash*23 + Salt.GetHashCode();
				hash = hash*23 + CurrentBalance.GetHashCode();

				return hash;
			}
		}

		public override bool Equals(object value)
		{
			var valueAsUser = value as User;

			if (valueAsUser == null)
			{
				return false;
			}

			if (ReferenceEquals(this, valueAsUser))
			{
				return true;
			}

			return Equals(valueAsUser);
		}

		public bool Equals([CanBeNull] User value)
		{
			if (value == null)
			{
				return false;
			}

			return
				Id.Equals(value.Id) &&
				CreateDate.Equals(value.CreateDate) &&
				UserName.Equals(value.UserName) &&
				Email.Equals(value.Email) &&
				HashedPass.Equals(value.HashedPass) &&
				Salt.Equals(value.Salt) &&
				CurrentBalance.Equals(value.CurrentBalance);
		}
	}
}