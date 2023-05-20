using DAL.DTOS;

namespace BL.Services
{
    public interface ICompaniesService
    {
        Task<List<CompanyDTO>> GetAllCompanies(bool includeDeleted, CancellationToken cancellationToken);
        Task ToggleStatus(Guid id, CancellationToken cancellationToken);
        Task<CompanyDTO> GetCompanyInfo(Guid id, CancellationToken cancellationToken);
        Task<CompanyBillingResponse> GetCompanysPackages(Guid id, CancellationToken cancellationToken);
    }
}
