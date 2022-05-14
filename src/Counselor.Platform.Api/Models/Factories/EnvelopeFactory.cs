using Mapster;
using System.Net;

namespace Counselor.Platform.Api.Models.Factories
{
	public static class EnvelopeFactory
	{
		/// <summary>
		/// Создать объект ответа сервиса с маппингом даннных к переданному типу dto
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="httpStatus"></param>
		/// <param name="data"></param>
		/// <param name="message"></param>
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
