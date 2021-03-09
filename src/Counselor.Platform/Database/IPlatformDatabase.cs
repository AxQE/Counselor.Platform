﻿using Counselor.Platform.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Database
{
	public interface IPlatformDatabase
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Transport> Transports { get; set; }
		public DbSet<UserTransport> UserTransports { get; set; }
		public DbSet<Dialog> Dialogs { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<ErrorCode> ErrorCodes { get; set; }
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
		int SaveChanges();
	}
}
