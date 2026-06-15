namespace GymDesk.API.Models;

public class TrainingSession
{
    public int Id { get; set; }

    public int ClientId { get; set; }
    public Client? Client { get; set; }

    public int TrainerId { get; set; }
    public Trainer? Trainer { get; set; }

    public DateOnly SessionDate { get; set; }
    public TimeOnly SessionTime { get; set; } 
    public string? Notes { get; set; } 
}