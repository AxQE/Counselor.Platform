using Counselor.Platform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Data.Database
{
	public partial class PlatformDbContext : DbContext, IPlatformDatabase
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Transport> Transports { get; set; }
		public DbSet<UserTransport> UserTransports { get; set; }
		public DbSet<Dialog> Dialogs { get; set; }
		public DbSet<Script> Scripts { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<ErrorCode> ErrorCodes { get; set; }
		public DbSet<Bot> Bots { get; set; }
		public DbSet<Command> Commands { get; set; }

		public PlatformDbContext(DbContextOptions<PlatformDbContext> options) : base(options)
		{
			base.Database.EnsureCreated();
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			SetModificationTime();
			return base.SaveChangesAsync(cancellationToken);
		}

		public override int SaveChanges()
		{
			SetModificationTime();
			return base.SaveChanges();
		}

		private void SetModificationTime()
		{
			var entities = ChangeTracker
				.Entries()
				.Where(e => e.Entity is EntityBase
				&& (e.State == EntityState.Modified || e.State == EntityState.Added))
				.Select(e => e.Entity)
				.Cast<EntityBase>();

			var now = DateTime.Now;

			foreach (var entity in entities)
			{
				if (entity.CreatedOn == default) entity.CreatedOn = now;

				entity.ModifiedOn = now;
			}
		}
	}
}
