using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zadanie.Models.DTO;
using zadanie.Services;

namespace zadanie.Controllers
{
    [Route("api/")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IDbService _dbService;
        public PrescriptionsController(IDbService DbService)
        {
            _dbService = DbService;
        }
 

        [HttpGet("prescriptions")]
        public async Task<IActionResult> GetPrescription(GetPrescriptionCommand request)
        {
            return await _dbService.GetPrescription(request);
        }
    }
}
