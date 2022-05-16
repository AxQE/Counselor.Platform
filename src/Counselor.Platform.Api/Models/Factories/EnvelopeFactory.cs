using Mapster;
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
		/// <param name="message">Сообщение об ошибке</param>
		/// <returns></returns>
		public static Envelope<T> Create<T>(HttpStatusCode httpStatus, object data = null, string message = null) where T : class
		{
			return new Envelope<T>
			{
				Data = data?.Adapt<T>(),
				Message = message,
				HttpStatus = httpStatus
			};
		}
	}
}
