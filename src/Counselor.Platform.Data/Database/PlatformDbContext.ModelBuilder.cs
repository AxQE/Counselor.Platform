using Counselor.Platform.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Counselor.Platform.Data.Database
{
	public partial class PlatformDbContext : DbContext
	{
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ErrorCode>().HasData(new ErrorCode { Id = 1, Description = "" });

			modelBuilder.Entity<Dialog>().Ignore(x => x.CurrentMessage);
		}
	}
}
