using GymDesk.API.Data;
using GymDesk.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymDesk.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly GymDeskDbContext _context;

    public ClientsController(GymDeskDbContext context)
    {
        _context = context;
    }

    // GET: api/Clients
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetClients()
    {
        var clients = await _context.Clients.ToListAsync();
        return Ok(clients);
    }

    // GET: api/Clients/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Client>> GetClient(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null) return NotFound();
        return Ok(client);
    }

    // POST: api/Clients
    [HttpPost]
    public async Task<ActionResult<Client>> CreateClient(Client client)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
    }

    // PUT: api/Clients/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClient(int id, Client client)
    {
        if (id != client.Id) return BadRequest();

        _context.Entry(client).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ClientExists(id)) return NotFound();
            else throw;
        }

        return NoContent();
    }

    // DELETE: api/Clients/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null) return NotFound();

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ClientExists(int id)
    {
        return _context.Clients.Any(e => e.Id == id);
    }
}