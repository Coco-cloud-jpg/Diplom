using Common.Models;
using System.ComponentModel.DataAnnotations;

namespace RecordingService.DTOS
{
    public class PackageTypeDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public ushort MaxUsersCount { get; set; }

        [Required]
        public ushort MaxRecordersCount { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public short CurrencyShort { get; set; }

    }
    public class PackageTypeReadDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ushort MaxUsersCount { get; set; }
        public ushort MaxRecordersCount { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public short CurrencyShort { get; set; }

    }
    public class PackageTypeCompaniesReadDTO
    {
        public short Count { get; set; }
        public string CompanyName { get; set; }
        public Guid CompanyId { get; set; }
    }
}
