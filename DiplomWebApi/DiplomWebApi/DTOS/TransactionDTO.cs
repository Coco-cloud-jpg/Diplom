namespace RecordingService.DTOS
{
    public class TransactionReadDTO
    {
        public Guid Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Sum { get; set; }
        public short Currency { get; set; }
    }

    public class TransactionCreateDTO
    {
        public decimal Sum { get; set; }
        public short Currency { get; set; }
        public Guid CompanyId { get; set; }
    }

    public class BillingByCurrency
    {
        public short Currency { get; set; }
        public decimal Sum { get; set; }
    }
}
