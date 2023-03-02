namespace RecordingService.Extensions
{
    public static class StreamExtensions
    {
		public static string ToBase64String(this Stream stream)
		{
			byte[] buffer = new byte[stream.Length];
			stream.Read(buffer, 0, (int)stream.Length);
			return Convert.ToBase64String(buffer);
		}
	}
}
