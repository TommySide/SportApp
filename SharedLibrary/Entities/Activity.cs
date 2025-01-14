using System.Text.Json.Serialization;

namespace SharedLibrary.Entities;

public class Activity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // názov
    public string Description { get; set; } = string.Empty; // popis
    public DateTime StartTime { get; set; } // začiatok
    public DateTime EndTime { get; set; } // koniec
    public int Capacity { get; set; } // max. pocet zakaznikov
    public Decimal Price { get; set; }

    public int TrainerId { get; set; } 
    public User Trainer { get; set; } 

    [JsonIgnore]
    public List<Reservation> Reservations { get; set; } = new(); // rezervacie
}