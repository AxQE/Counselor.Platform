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
		DbSet<DialogScript> DialogScripts { get; set; }
		DbSet<Message> Messages { get; set; }
		DbSet<ErrorCode> ErrorCodes { get; set; }
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
		int SaveChanges();
	}
}
