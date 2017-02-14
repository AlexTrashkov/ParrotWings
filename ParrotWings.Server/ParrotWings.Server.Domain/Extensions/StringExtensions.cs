namespace ParrotWings.Server.Domain.Extensions
{
	internal static class StringExtensions
	{
		public static bool IsValidEmail(this string value)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(value);
				return addr.Address == value;
			}
			catch
			{
				return false;
			}
		}
	}
}