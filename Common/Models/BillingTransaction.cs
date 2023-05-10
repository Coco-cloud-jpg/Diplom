namespace Common.Models
{
    public class BillingTransaction: BaseEntity
    {
        public Guid CompanyId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public short Currency { get; set; }
        public virtual Company Company { get; set; }
    }
}
