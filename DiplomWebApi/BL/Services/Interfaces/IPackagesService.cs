using DAL.DTOS;

namespace BL.Services
{
    public interface IPackagesService
    {
        Task<List<PackageTypeReadDTO>> GetAllPackages(CancellationToken cancellationToken);
        Task<Guid> Create(PackageTypeDTO model, CancellationToken cancellationToken);
        Task<Guid> Update(Guid id, PackageTypeDTO model, CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
        Task<List<PackageTypeCompaniesReadDTO>> GetCompanies(Guid id, CancellationToken cancellationToken);
        IEnumerable<ShortEntityModelShort> GetCurrencies();
    }
}
