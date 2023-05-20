using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DAL.DTOS;
using BL.Services;

namespace DiplomWebApi.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private ITransactionsService _transactionsService;
        public TransactionsController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }
        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> GetAllTransactions(Guid id, CancellationToken cancellationToken) =>
            Ok(await _transactionsService.GetAllTransactions(id, cancellationToken));
        [HttpPost]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> Create(TransactionCreateDTO model, CancellationToken cancellationToken) =>
            Ok(await _transactionsService.Create(model, cancellationToken));
    }
}
