using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using zadanie.Models.DTO;
using zadanie.Services;

namespace zadanie.Controllers
{
    [Route("api/")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDbService _dbService;
        public DoctorsController(IDbService DbService)
        {
            _dbService = DbService;
        }

        [HttpGet("doctor")]
        public async Task<IActionResult> GetDoctor()
        {
            return await _dbService.GetDoctors();
        }
        [HttpPost("doctor")]
        public async Task<IActionResult> AddDoctor(AddDoctorCommand request)
        {
            return await _dbService.AddDoctor(request);
        }
        [HttpDelete("doctor/{idDoctor}")]
        public async Task<IActionResult> DeleteDoctor(int IdDoctor)
        {
            return await _dbService.DeleteDoctor(IdDoctor);
        }
        [HttpPut("doctor/{idDoctor}")]
        public async Task<IActionResult> ModifyDoctor(UpdateDoctorCommand request)
        {
            return await _dbService.ModifyDoctor(request);
        }
    }
}
