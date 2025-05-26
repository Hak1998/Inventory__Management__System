using FluentValidation;
using InvTrack.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.DTOs;
using MyApp.Application.Enums;
using MyApp.Application.Services;

namespace InvTrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IValidator<TransactionDto> _validator;

        public TransactionsController(ITransactionService transactionService, IValidator<TransactionDto> validator)
        {
            _transactionService = transactionService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var response = await _transactionService.GetAllTransactionsAsync();

            return response.Status.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var response = await _transactionService.GetTransactionByIdAsync(id);

            if (!response.Status.Success)
                return BadRequest(response);

            return response.Result == null
                ? NotFound(response)
                : Ok(response);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Purchase(TransactionModel transactionModel)
        {
            var transactionDto = new TransactionDto
            {
                Notes = transactionModel.Notes,
                Quantity = transactionModel.Quantity,
                ProductId = transactionModel.ProductId,
                TransactionType = TransactionType.Purchase.ToString()
            };

            var validationResult = await _validator.ValidateAsync(transactionDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await _transactionService.CreateTransactionAsync(transactionDto);

            if (response.Status.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPost]
        public async Task<ActionResult> Sale(TransactionModel transactionModel)
        {
            var transactionDto = new TransactionDto
            {
                Notes = transactionModel.Notes,
                Quantity = transactionModel.Quantity,
                ProductId = transactionModel.ProductId,
                TransactionType = TransactionType.Sale.ToString()
            };

            var validationResult = await _validator.ValidateAsync(transactionDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await _transactionService.CreateTransactionAsync(transactionDto);

            if (response.Status.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
