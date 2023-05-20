namespace BL.Helpers
{
    public class SecondsConverter
    {
        public static string ToString(long seconds)
        {
            var h = (int)Math.Floor(seconds / (decimal)3600);
            var m = (int)Math.Floor(seconds % 3600 / (decimal)60);
            var s = (int)Math.Floor(seconds % 3600 % (decimal)60);

            return $"{h.ToString("00")}:{m.ToString("00")}:{s.ToString("00")}";
        }
    }
}
