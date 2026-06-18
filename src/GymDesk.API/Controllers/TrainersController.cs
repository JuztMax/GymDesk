using GymDesk.API.Data;
using GymDesk.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymDesk.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainersController : ControllerBase
{
    private readonly GymDeskDbContext _context;

    public TrainersController(GymDeskDbContext context)
    {
        _context = context;
    }

    // GET: api/Trainers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Trainer>>> GetTrainers()
    {
        var trainers = await _context.Trainers.ToListAsync();
        return Ok(trainers);
    }

    // GET: api/Trainers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Trainer>> GetTrainer(int id)
    {
        var trainer = await _context.Trainers.FindAsync(id);
        if (trainer == null) return NotFound();
        return Ok(trainer);
    }

    // POST: api/Trainers
    [HttpPost]
    public async Task<ActionResult<Trainer>> CreateTrainer(Trainer trainer)
    {
        _context.Trainers.Add(trainer);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTrainer), new { id = trainer.Id }, trainer);
    }

    // PUT: api/Trainers/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTrainer(int id, Trainer trainer)
    {
        if (id != trainer.Id) return BadRequest();

        _context.Entry(trainer).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TrainerExists(id)) return NotFound();
            else throw;
        }

        return NoContent();
    }

    // DELETE: api/Trainers/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrainer(int id)
    {
        var trainer = await _context.Trainers.FindAsync(id);
        if (trainer == null) return NotFound();

        _context.Trainers.Remove(trainer);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TrainerExists(int id)
    {
        return _context.Trainers.Any(e => e.Id == id);
    }
}