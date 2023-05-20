using Common.Models;
using Newtonsoft.Json;

namespace DAL.DTOS
{
    public class CompanyDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime TimeToPay { get; set; }
    }

    public class CompanysPackagesDTO
    {
        public string Name { get; set; }
        public int Total { get; set; }
        [JsonIgnore]
        public int MaxUsersCount { get; set; }
        [JsonIgnore]
        public int MaxRecordersCount { get; set; }
        [JsonIgnore]
        public decimal Price { get; set; }
        [JsonIgnore]
        public Currency Currency { get; set; }
    }

    public class CompanyBillingResponse
    {
        public List<CompanysPackagesDTO> Packages { get; set; }
        public int MaxUsersCount { get; set; }
        public int UsersCount { get; set; }
        public int MaxRecordersCount { get; set; }
        public int RecordersCount { get; set; }
        public decimal MonthlyDollarsCharge { get; set; }
        public decimal MonthlyEurosCharge { get; set; }
        public decimal MonthlyUAHCharge { get; set; }
    }

    public class CompanyUsersAndRecordersCountDTO: BaseEntity
    {
        public int UsersCount { get; set; }
        public int RecordersCount { get; set; }
    }
}
