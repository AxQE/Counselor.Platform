using Mapster;
using System;
using System.Net;

namespace Counselor.Platform.Api.Models.Factories
{
	public static class EnvelopeFactory
	{
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
				Payload = data?.Adapt<T>(),
				Error = !string.IsNullOrEmpty(errorMessage) ? new Error { Message = errorMessage, Id = errorId } : null,
				HttpStatus = httpStatus
			};
		}
	}
}
