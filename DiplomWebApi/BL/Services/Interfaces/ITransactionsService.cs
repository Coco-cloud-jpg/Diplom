using DAL.DTOS;

namespace BL.Services
{
    public interface ITransactionsService
    {
        Task<List<TransactionReadDTO>> GetAllTransactions(Guid id, CancellationToken cancellationToken);
        Task<Guid> Create(TransactionCreateDTO model, CancellationToken cancellationToken);
    }
}
