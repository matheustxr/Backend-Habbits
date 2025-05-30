﻿namespace Habits.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public List<Habit> Habits { get; set; } = new();
    public List<HabitCategory>? HabitCategories { get; set; }
}
