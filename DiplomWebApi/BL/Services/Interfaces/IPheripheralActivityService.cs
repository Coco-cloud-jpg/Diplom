using Common.Models;
using DAL.DTOS;

namespace BL.Services
{
    public interface IPheripheralActivityService
    {
        Task AddEntry(PheripheralActivityDTO model);
    }
}
