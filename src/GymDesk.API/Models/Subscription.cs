namespace GymDesk.API.Models;

public class Subscription
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }

    public string Type { get; set; } = string.Empty; 
    public DateOnly StartDate { get; set; }          
    public DateOnly EndDate { get; set; }            
    public decimal Price { get; set; }
}