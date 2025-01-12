﻿using SharedLibrary.Entities;

public class Reservation
{
    public int Id { get; set; } 
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    
    public int ActivityId { get; set; } 
    public Activity Activity { get; set; } = null!;

    public DateTime ReservationDate { get; set; } = DateTime.Now; 
}