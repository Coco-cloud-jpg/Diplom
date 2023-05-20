using BL.Services.Interfaces;
using Common.Emails;
using Common.Models;
using Common.Services.Interfaces;
using DAL.DTO;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BL.Services
{
    public class UsersService: IUsersService
    {

        private readonly ICryptoService _cryptoService;
        private IIdentityUnitOfWork _unitOfWork;
        private IEmailSender _emailSender;
        public UsersService(IIdentityUnitOfWork unitOfWork, IEmailSender emailSender, ICryptoService cryptoService)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _cryptoService = cryptoService;
        }
        public async Task<List<UserReadDTO>> GetAllUsers(Guid companyId, bool includeDeleted, CancellationToken cancellationToken) =>
            await _unitOfWork.UserRepository.DbSet
                    .Include(item => item.Role)
                    .AsNoTracking()
                    .Where(item => item.CompanyId == companyId && (includeDeleted ? true : item.IsActive))
                    .Select(item => new UserReadDTO
                    {
                        Id = item.Id,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Email = item.Email,
                        Role = item.Role.Name,
                        IsActive = item.IsActive
                    })
                    .ToListAsync(cancellationToken);
        public async Task<bool> ToggleStatus(Guid userIdToPerformAction, Guid id, CancellationToken cancellationToken)
        {
            var userToDisable = await _unitOfWork.UserRepository.GetById(id, cancellationToken);

            if (userToDisable.Password == String.Empty)
                throw new ArgumentNullException("User was never activated!");

            userToDisable.IsActive = !userToDisable.IsActive;
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            return userIdToPerformAction == id;
        }
        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            await _unitOfWork.UserRepository.Delete(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
        public async Task Create(Guid companyId, string refererUrl, UserUpdateDTO model, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.UserRepository.DbSet.FirstOrDefaultAsync(item => item.Email == model.Email) != null)
                throw new ArgumentNullException("This email already exists in system!");

            if (!await HasCompanyFreePlaces(companyId))
                throw new ArgumentNullException("Your plan doesn't support new users!");

            var createdId = Guid.NewGuid();

            var dbModel = new User
            {
                Id = createdId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                IsActive = false,
                RoleId = model.Role,
                CompanyId = companyId,
                Password = String.Empty
            };

            await _unitOfWork.UserRepository.Create(dbModel, cancellationToken);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            var template = System.IO.File.ReadAllText("../BL/Templates/Email_User_Adding.html");

            template = template.Replace("{{url}}", $"{refererUrl}user-register/{companyId}/{createdId}");

            var message = new Message(new string[] { model.Email }, "User Registration!", template);
            _emailSender.SendEmail(message);
        }
        public async Task Submit(SubmitUserRegisration model, CancellationToken cancellationToken)
        {
            var itemToUpdate = await _unitOfWork.UserRepository.GetById(model.UserId, cancellationToken);

            if (itemToUpdate.CompanyId != model.CompanyId)
                throw new ArgumentNullException();

            itemToUpdate.Password = _cryptoService.ComputeSHA256(model.Password);
            itemToUpdate.IsActive = true;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task<string> GetEmail(Guid companyId, Guid userId, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetById(userId, cancellationToken);

            if ((user.CompanyId != companyId) || (user.Password != String.Empty))
                throw new ArgumentNullException();

            return user.Email;
        }
        public async Task Update(Guid id, UserUpdateDTO model, CancellationToken cancellationToken)
        {
            var itemToUpdate = await _unitOfWork.UserRepository.GetById(id, cancellationToken);

            if (itemToUpdate == null)
                throw new ArgumentNullException();

            itemToUpdate.FirstName = model.FirstName;
            itemToUpdate.LastName = model.LastName;
            itemToUpdate.Email = model.Email;
            itemToUpdate.RoleId = model.Role;

            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
        public async Task<List<Role>> GetRoles(CancellationToken cancellationToken) =>
            await _unitOfWork.RoleRepository.DbSet.AsNoTracking()
                .Where(item => item.Id != Guid.Parse(Common.Constants.Role.SystemAdmin)).ToListAsync(cancellationToken);

        private async Task<bool> HasCompanyFreePlaces(Guid companyId)
        {
            var maxUsersCount = await _unitOfWork.PackageTypeCompanyRepository.DbSet.Include(item => item.PackageType)
                .AsNoTracking().Where(item => item.CompanyId == companyId).SumAsync(item => (int)(item.Count * item.PackageType.MaxUsersCount));

            var currentUsersCount = await _unitOfWork.UserRepository.DbSet.AsNoTracking().Where(item => item.CompanyId == companyId).CountAsync();

            return maxUsersCount > currentUsersCount;
        }
    }
}
