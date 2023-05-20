namespace BL.Helpers
{
    public class ChartHelper
    {
        public static string GetThisDayHours() => $@"from 
            (SELECT 
                DATEADD(HOUR, number, DayStart) AS DatePart
            FROM 
                master..spt_values
                CROSS APPLY (SELECT '{DateTime.Today.ToString("yyyy-MM-dd")}')
	            AS StartingDates(DayStart)
            WHERE 
                type = 'P' 
                AND number BETWEEN 0 AND 23) as dateTable";
        public static string GetThisWeekDays() => $@"from 
            (SELECT 
                DATEADD(DAY, number, StartOfWeek) AS DatePart
            FROM 
                master..spt_values
                CROSS APPLY (SELECT '{GetWeekStart().ToString("yyyy-MM-dd")}')
	            AS StartingDates(StartOfWeek)
            WHERE 
                type = 'P' 
                AND number BETWEEN 0 AND 6) as dateTable";
        public static string GetThisMonthDays()
        {
            var monthStart = GetMonthStart();

            return $@"from 
            (SELECT 
                DATEADD(DAY, number, MonthStart) AS DatePart
            FROM 
                master..spt_values
                CROSS APPLY (SELECT '{monthStart.ToString("yyyy-MM-dd")}')
	            AS StartingDates(MonthStart)
            WHERE 
                type = 'P' 
                AND number BETWEEN 0 AND (datediff(day, '{monthStart.ToString("yyyy-MM-dd")}', dateadd(month, 1, '{monthStart.ToString("yyyy-MM-dd")}'))) - 1) as dateTable";
        }

        public static DateTime GetWeekStart()
        {
            int sundayOffset = DateTime.Today.DayOfWeek == 0 ? 7 : 0;
            return DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday - sundayOffset);
        }

        public static DateTime GetMonthStart()
        {
            var today = DateTime.Today;
            return new DateTime(today.Year, today.Month, 1);
        }
    }
}
