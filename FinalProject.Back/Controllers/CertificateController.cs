using FinalProject.Back.Contexts;
using FinalProject.Data.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace FinalProject.Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly CertificationDbContext _context;

        public CertificateController(CertificationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<CertificateDto>>> GetAllCertificates()
        {
            var result = await _context.Certificates
                .Select(x=>CertificateDto.FromEntity(x))
                .ToListAsync();

            return Ok(result);
        }
    }
}
