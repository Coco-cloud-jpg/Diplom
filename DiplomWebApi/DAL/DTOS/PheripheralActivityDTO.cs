namespace DAL.DTOS
{
    public class PheripheralActivityDTO
    {
        public Guid RecorderId { get; set; }
        public double MouseActivity { get; set; }
        public double KeyboardActivity { get; set; }
    }
}
