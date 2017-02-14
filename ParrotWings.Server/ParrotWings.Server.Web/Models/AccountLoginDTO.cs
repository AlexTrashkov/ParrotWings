using System.ComponentModel.DataAnnotations;

namespace ParrotWings.Server.Web.Models
{
	public sealed class AccountLoginDTO
	{
		[Required]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }
	}
}