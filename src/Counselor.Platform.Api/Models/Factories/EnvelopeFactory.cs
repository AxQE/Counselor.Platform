using Mapster;
using System;
using System.Net;
using System.Reflection;

namespace Counselor.Platform.Api.Models.Factories
{
	public static class EnvelopeFactory
	{
		private static readonly TypeAdapterConfig _config;

		static EnvelopeFactory()
		{
			var config = new TypeAdapterConfig();
			config.Scan(Assembly.GetExecutingAssembly());
			_config = config;
		}

		/// <summary>
		/// Создать объект ответа сервиса с маппингом даннных к переданному типу dto
		/// </summary>
		/// <typeparam name="T">Тип транспортной dto</typeparam>
		/// <param name="httpStatus">Код ответа</param>
		/// <param name="data">Данные для маппинга к dto</param>
		/// <param name="errorMessage">Сообщение об ошибке</param>
		/// <returns></returns>
		public static Envelope<T> Create<T>(HttpStatusCode httpStatus, object data = null, string errorMessage = null, Guid? errorId = null) where T : class
		{
			return new Envelope<T>
			{
				Payload = data?.Adapt<T>(_config),
				Error = !string.IsNullOrEmpty(errorMessage) ? new Error { Message = errorMessage, Id = errorId } : null,
				HttpStatus = httpStatus
			};
		}
	}
}
