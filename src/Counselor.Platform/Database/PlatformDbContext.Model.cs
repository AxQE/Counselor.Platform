using Counselor.Platform.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Database
{
	public partial class PlatformDbContext : DbContext
	{
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ErrorCode>().HasData(new ErrorCode { Id = 1, Description = "" });
		}
	}
}
