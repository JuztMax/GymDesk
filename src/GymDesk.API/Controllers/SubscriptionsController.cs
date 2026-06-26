using GymDesk.API.Data;
using GymDesk.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymDesk.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly GymDeskDbContext _context;

    public SubscriptionsController(GymDeskDbContext context)
    {
        _context = context;
    }

    // GET: api/Subscriptions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Subscription>>> GetSubscriptions()
    {
        // 👇 ДОБАВЛЕНО .Include(s => s.Client) — теперь клиент загружается вместе с абонементом
        var subscriptions = await _context.Subscriptions
            .Include(s => s.Client)
            .ToListAsync();

        return Ok(subscriptions);
    }

    // GET: api/Subscriptions/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Subscription>> GetSubscription(int id)
    {
        var subscription = await _context.Subscriptions.FindAsync(id);
        if (subscription == null) return NotFound();
        return Ok(subscription);
    }

    // POST: api/Subscriptions
    [HttpPost]
    public async Task<ActionResult<Subscription>> CreateSubscription(Subscription subscription)
    {
        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSubscription), new { id = subscription.Id }, subscription);
    }

    // PUT: api/Subscriptions/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSubscription(int id, Subscription subscription)
    {
        if (id != subscription.Id) return BadRequest();

        _context.Entry(subscription).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SubscriptionExists(id)) return NotFound();
            else throw;
        }

        return NoContent();
    }

    // DELETE: api/Subscriptions/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubscription(int id)
    {
        var subscription = await _context.Subscriptions.FindAsync(id);
        if (subscription == null) return NotFound();

        _context.Subscriptions.Remove(subscription);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool SubscriptionExists(int id)
    {
        return _context.Subscriptions.Any(e => e.Id == id);
    }
}