using Counselor.Platform.Core;
using Counselor.Platform.Database;
using Counselor.Platform.Entities;
using Counselor.Platform.Repositories;
using Counselor.Platform.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Chat = Telegram.Bot.Types.Chat;

namespace Counselor.Platform.Worker.Systems.Telegram
{
	class TelegramWorker : IngoingServiceBase
	{
		private readonly Transport _transport;
		private readonly ILogger<TelegramWorker> _logger;
		private readonly TelegramOptions _options;
		private readonly TelegramBotClient _client;				

		protected override Action StartTransport => ExecuteTelegram;

		public TelegramWorker(
			ILogger<TelegramWorker> logger,
			IOptions<TelegramOptions> options,
			IPipelineExecutor pipelineExecutor,
			//todo: нужно изменить работу с бд и освобождать после запроса, сервис живет постоянно и подключение будет слишком тяжелым при такой реализации
			IPlatformDatabase database,
			ConnectionsRepository connections,
			DialogsRepository dialogs
			) 
			: base(logger, options, pipelineExecutor, database, connections, dialogs)
		{
			_logger = logger;
			_options = options.Value;

			_client = new TelegramBotClient(_options.Token);
			_client.OnMessage += OnMessageAsync;
			_client.OnReceiveError += OnReceiveErrorAsync;
		}

		private void ExecuteTelegram()
		{
			_client.StartReceiving();
		}

		private async void OnReceiveErrorAsync(object sender, global::Telegram.Bot.Args.ReceiveErrorEventArgs e)
		{
			throw new NotImplementedException();
		}

		private async void OnMessageAsync(object sender, global::Telegram.Bot.Args.MessageEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.Message.Text))
			{
				await HandleMessageAsync(e.Message.Chat.Id.ToString(), e.Message.Chat.Username, e.Message.Text);
			}
		}		
	}
}
