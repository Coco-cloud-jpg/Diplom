namespace RecordingService.DTOS
{
    public class GridResult<T> where T : class
    {
        public int Total { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
