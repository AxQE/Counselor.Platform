using Counselor.Platform.Core;
using Counselor.Platform.Database;
using Counselor.Platform.Entities;
using Counselor.Platform.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Chat = Telegram.Bot.Types.Chat;

namespace Counselor.Platform.Worker.Systems.Telegram
{
	class TelegramWorker : BackgroundService
	{
		private readonly Transport _transport;
		private readonly ILogger<TelegramWorker> _logger;
		private readonly TelegramOptions _options;
		private readonly TelegramBotClient _client;
		private readonly IPipelineExecutor _pipelineExecutor;
		private readonly IPlatformDatabase _database;

		public TelegramWorker(
			ILogger<TelegramWorker> logger,
			IOptions<TelegramOptions> options,
			IPipelineExecutor pipelineExecutor,
			//todo: нужно изменить работу с бд и освобождать после запроса, сервис живет постоянно и подключение будет слишком тяжелым при такой реализации
			IPlatformDatabase database
			)
		{
			_logger = logger;
			_options = options.Value;
			_pipelineExecutor = pipelineExecutor;
			_database = database;

			_client = new TelegramBotClient(_options.Token);
			BotInitialize();

			_transport = _database.Transports.FirstOrDefault(x => x.Name.Equals(TelegramOptions.TransportSystemName));

			if (_transport is null)
			{
				_transport = new Transport
				{
					Name = TelegramOptions.TransportSystemName
				};

				_database.SaveChangesAsync();
			}
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			if (_options.IsEnabled)
			{
				_logger.LogInformation("Telegram service worker is starting.");
				while (!stoppingToken.IsCancellationRequested)
				{
					_client.StartReceiving();
					await Task.Delay(Timeout.Infinite, stoppingToken);
				}
			}
			else
			{
				_logger.LogInformation("Telegram service worker is disabled.");
			}
		}

		private void BotInitialize()
		{
			_client.OnMessage += OnMessage;
			_client.OnReceiveError += OnReceiveError;
		}

		private async void OnReceiveError(object sender, global::Telegram.Bot.Args.ReceiveErrorEventArgs e)
		{
			throw new NotImplementedException();
		}

		private async void OnMessage(object sender, global::Telegram.Bot.Args.MessageEventArgs e)
		{
			try
			{
				if (!string.IsNullOrEmpty(e.Message.Text))
				{					
					await _pipelineExecutor.Run(await BuildDialog(e.Message.Chat, e.Message.Text));
				}
			}
			catch (Exception ex)
			{
				//todo: serialize message
				_logger.LogError(ex, "Can't process incoming telegram message.");
			}
		}

		private async Task<IDialog> BuildDialog(Chat chat, string payload)
		{
			return new Dialog
			{
				Id = new Guid(),
				User = await FindUser(chat),
				Messages = new List<Message>
				{
					new Message
					{
						Id = new Guid(),
						Payload = payload
					}
				}
			};
		}

		private async Task<User> FindUser(Chat chat)
		{
			var user =
				(await _database.UserTransports
					.Include(x => x.User)
					.FirstOrDefaultAsync(x => x.Transport.Id == _transport.Id))?.User;

			if (user is null)
			{
				_database.UserTransports.Add(
					new UserTransport
					{
						Transport = _transport,
						User = new User
						{
							Username = chat.Username
						}
					});

				await _database.SaveChangesAsync();
			}

			return user;
		}
	}
}
