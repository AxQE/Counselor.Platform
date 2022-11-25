using Counselor.Platform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Data.Database
{
	public interface IPlatformDatabase : IDisposable
	{
		DbSet<User> Users { get; set; }
		DbSet<Transport> Transports { get; set; }
		DbSet<UserTransport> UserTransports { get; set; }
		DbSet<Dialog> Dialogs { get; set; }
		DbSet<Script> Scripts { get; set; }
		DbSet<Message> Messages { get; set; }
		DbSet<ErrorCode> ErrorCodes { get; set; }
		DbSet<Bot> Bots { get; set; }
		DbSet<Command> Commands { get; set; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
		int SaveChanges();
	}
}
