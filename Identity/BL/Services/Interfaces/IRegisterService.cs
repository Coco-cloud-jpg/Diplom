using DAL.DTO;

namespace BL.Services
{
    public interface IRegisterService
    {
        Task Register(UserCreateDTO model);
        Task Register(AdminCreate model);
        Task<bool> CheckEmail(string email);
        Task<string> GetEmail(Guid companyId);
        Task RegisterCompany(string refererUrl, CompanyCreate model);
        Task<IEnumerable<ShortEntityModel<Guid>>> GetCountries(CancellationToken cancellationToken);
        Task PasswordResetRequest(string refererUrl, ResetPasswordRequest model);
        Task ResetPassword(ResetPasswordSubmit model);
    }
}
