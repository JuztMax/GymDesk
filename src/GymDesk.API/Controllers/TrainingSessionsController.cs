using GymDesk.API.Data;
using GymDesk.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymDesk.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainingSessionsController : ControllerBase
{
    private readonly GymDeskDbContext _context;

    public TrainingSessionsController(GymDeskDbContext context)
    {
        _context = context;
    }

    // GET: api/TrainingSessions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrainingSession>>> GetTrainingSessions()
    {
        // 👇 ДОБАВЛЕНО: Сортировка по Дате и Времени
        var sessions = await _context.TrainingSessions
            .Include(s => s.Client)
            .Include(s => s.Trainer)
            .OrderBy(s => s.SessionDate)   // 1. Сначала сортируем по дате (от старых к новым)
            .ThenBy(s => s.SessionTime)    // 2. Внутри одной даты сортируем по времени
            .ToListAsync();

        return Ok(sessions);
    }

    // GET: api/TrainingSessions/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TrainingSession>> GetTrainingSession(int id)
    {
        var session = await _context.TrainingSessions.FindAsync(id);
        if (session == null) return NotFound();
        return Ok(session);
    }

    // POST: api/TrainingSessions
    [HttpPost]
    public async Task<ActionResult<TrainingSession>> CreateTrainingSession(TrainingSession session)
    {
        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTrainingSession), new { id = session.Id }, session);
    }

    // PUT: api/TrainingSessions/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTrainingSession(int id, TrainingSession session)
    {
        if (id != session.Id) return BadRequest();

        _context.Entry(session).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TrainingSessionExists(id)) return NotFound();
            else throw;
        }

        return NoContent();
    }

    // DELETE: api/TrainingSessions/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrainingSession(int id)
    {
        var session = await _context.TrainingSessions.FindAsync(id);
        if (session == null) return NotFound();

        _context.TrainingSessions.Remove(session);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TrainingSessionExists(int id)
    {
        return _context.TrainingSessions.Any(e => e.Id == id);
    }
}