using Counselor.Platform.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Database
{
	public class ApplicationDbContext : DbContext, IApplicationDatabase
	{
		public ApplicationDbContext()
		{
			base.Database.EnsureCreated();
		}

		public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
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

			return await base.SaveChangesAsync(cancellationToken);
		}
	}
}
