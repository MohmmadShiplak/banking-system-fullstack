using BusinessLayer_BankManagementSystem.Services;
using DataAccessLayer_BankManagementSystem.Data;
using DataAccessLayer_BankManagementSystem.DTO;
using DataAccessLayer_BankManagementSystem.Entities;
using DataAccessLayer_BankManagementSystem.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_End_Bank_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        readonly IClientRepository<Client> _clientRepository;

        readonly ApplicationDbContext _context;

        public ClientController(IClientRepository<Client> clientRepository, ApplicationDbContext context)
        {
            _clientRepository = clientRepository;
            _context = context;
        }


        [HttpGet]

        public async Task < IActionResult> Get()
        {
            var clients =await  _clientRepository.GetAllClientsAsync();

            return Ok(clients);
        }

        [HttpGet("GetAllClientCount")]
        public async Task<IActionResult> GetAllClientCount()
        {

            var count = await _context.Clients.CountAsync();

            return Ok(new { totalClients = count });


        }
        [HttpGet("account/{accountNumber}")]
        public async Task<IActionResult> GetClient(string accountNumber)
        {
            try
            {
                var client = await _clientRepository.FindByAccountNumberAsync(accountNumber);
                if (client == null)
                    return NotFound();

                return Ok(client);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{accountNumber}/deposit")]
        public async Task<IActionResult> Deposit(string accountNumber, [FromBody] DepositRequest request)
        {
            try
            {
                var newBalance = await _clientRepository.DepositAsync(accountNumber, request.Amount);
                return Ok(new { balance = newBalance });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{accountNumber}/withdrew")]
        public async Task<IActionResult> Withdrew(string accountNumber, [FromBody] DepositRequest request)
        {
            try
            {
                var newBalance = await _clientRepository.WithdrewAsync(accountNumber, request.Amount);
                return Ok(new { balance = newBalance });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Transfer")]

        public async Task<IActionResult> Transfer([FromBody] TransferRequest request)
        {
            try
            {

                var result = await _clientRepository.TransferAsync(

                              request.FromAccount,
                              request.ToAccount,
                              request.Amount

                              );


                return Ok(result);


            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // 400 Bad Request (e.g., invalid amount)
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // 400 Bad Request (e.g., insufficient funds)
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal error occurred."); // 500 Server Error
            }



        }

        [HttpGet("GetTotallAccountBalance")]
public async Task<IActionResult> GetTotallAccountBalance()
{
  
    var totalBalance = await _context.Clients.SumAsync(a => a.AccountBalance);
    
    return Ok(new { 
        totalBalance = totalBalance
    });
}






        [HttpGet("{Id}")]
        public async Task <IActionResult> Get(int Id)
        {
            var client = await _clientRepository.GetByClientsinfoByIdAsync(Id);
            if (client == null) return NotFound();
            return Ok(client);
        }

        [HttpDelete("{id}")]
        public async Task <IActionResult> Delete(int id)
        {

            var client =await  _context.Clients.FindAsync(id);

            if (client == null)
                return NotFound(); // 404 if product doesn't exist

            // Delete the product
          await _clientRepository.DeleteClientAsync(client);

            // Return 200 OK with the deleted product in the response body
            return Ok(client);

        }

        [HttpPut]
        public async Task <IActionResult> Update(Client client)
        {


            var updatedClients =await _clientRepository.UpdateClientsAsync(client);

            return Ok(updatedClients);

        }
        [HttpPost]
        public async Task<IActionResult> Add(Client client)
        {

            var users = await _clientRepository.AddClientsAsync(client);

            return Ok(users);
        }
    }


    }

