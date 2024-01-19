using FinalProject.Back.Contexts;
using FinalProject.Data.Dtos;
using FinalProject.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : Controller
    {
        private readonly CertificationDbContext _context;

        public CandidateController(CertificationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<CandidateDto>>> GetAllCertificates()
        {
            var result = await _context.Candidates.Include(x => x.User)
                .Select(x => CandidateDto.FromEntity(x))
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<CandidateDto>> GetCandidateById(int id)
        {
            var candidate = await _context.Candidates.Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (candidate == null)
            {
                return NotFound();
            }

            return Ok(CandidateDto.FromEntity(candidate));
        }

        [HttpPost]
        public async Task<ActionResult<CandidateDto>> CreateCandidate(CandidateDto candidateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var passwordHasher = new PasswordHasher<User>();
            var encryptedPassword = passwordHasher.HashPassword(new User(), candidateDto.Password);
            var user = new User()
            {
                FirstName = candidateDto.FirstName,
                LastName = candidateDto.LastName,
                Email = candidateDto.Email,
                Phone = candidateDto.Phone ?? "",
                Address = candidateDto.Address,
                Password = encryptedPassword,
                Role = "candidate",
                CreatedAt = DateTime.Now
            };

            var candidate = new Candidate()
            {
                Number = Guid.NewGuid(),
                BirthDate = candidateDto.BirthDate,
                User = user
            };

            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();

            return Ok(candidateDto);
        }

        [HttpPost]
        [Route("id")]
        public async Task<ActionResult<CandidateDto>> UpdateCandidate(int id, CandidateDto candidateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var candidate = await _context.Candidates.Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (candidate == null)
            {
                return NotFound();
            }

            candidate.User.FirstName = candidateDto.FirstName;
            candidate.User.LastName = candidateDto.LastName;
            candidate.User.Email = candidateDto.Email;
            candidate.User.Phone = candidateDto.Phone;
            candidate.User.Address = candidateDto.Address;
            candidate.BirthDate = candidateDto.BirthDate;

            await _context.SaveChangesAsync();

            return Ok(CandidateDto.FromEntity(candidate));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<CandidateDto>> DeleteCandidate(int id)
        {
            var candidate = await _context.Candidates.Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (candidate == null)
            {
                return NotFound();
            }

            _context.Candidates.Remove(candidate);
            await _context.SaveChangesAsync();

            return Ok(CandidateDto.FromEntity(candidate));
        }
    }
}
