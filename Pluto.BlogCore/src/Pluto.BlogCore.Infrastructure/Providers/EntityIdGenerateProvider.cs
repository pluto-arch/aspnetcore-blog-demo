using System;

namespace Pluto.BlogCore.Infrastructure.Providers
{
	public static class EntityIdGenerateProvider
	{
		public static long GenerateLongId()
		{
			byte[] buffer = Guid.NewGuid().ToByteArray();
			return BitConverter.ToInt64(buffer, 0);
		}
		
		public static long GenerateIntId()
		{
			byte[] buffer = Guid.NewGuid().ToByteArray();
			return BitConverter.ToInt32(buffer, 0);
		}
	}
}