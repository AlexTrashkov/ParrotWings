using System.Data.Entity;
using ParrotWings.Server.Storage.DTO;
using System.Configuration;

namespace ParrotWings.Server.Storage
{
	internal sealed class ParrotWingsDataContext : DbContext
	{
		public DbSet<TransferDTO> Transfers { get; set; }
		public DbSet<UserDTO> Users { get; set; }

		public ParrotWingsDataContext() : base(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)
		{
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TransferDTO>()
				.HasRequired(x => x.UserTo)
				.WithMany()
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<TransferDTO>()
				.HasRequired(x => x.UserFrom)
				.WithMany()
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<TransferDTO>()
				.Property(x => x.Amount)
				.HasPrecision(20, 10);

			modelBuilder.Entity<TransferDTO>()
				.Property(x => x.UserFromBalanceBeforeTransfer)
				.HasPrecision(20, 10);

			modelBuilder.Entity<TransferDTO>()
				.Property(x => x.UserToBalanceBeforeTransfer)
				.HasPrecision(20, 10);

			modelBuilder.Entity<UserDTO>()
				.Property(x => x.CurrentBalance)
				.HasPrecision(20, 10);
		}
	}
}