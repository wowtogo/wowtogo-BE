using Bogus;
using Domain.Models;

namespace Infrastructure.Data.DataGenerator;
public static class StaffGenerator
{
    public static Staff[] GenerateStaff(User[] users, Event[] events)
    {
        Staff[] staffs = new Faker<Staff>()
        .UseSeed(1)
        .UseDateTimeReference(DateTime.UtcNow)
        .RuleFor(s => s.Id, f => f.PickRandom(users).Id)
        .RuleFor(s => s.EventId, f => f.PickRandom(events).Id)
        .RuleFor(s => s.CreatedAt, f => f.Date.Past())
        .RuleFor(s => s.UpdatedAt, f => f.Date.Past())
        .Generate(50)
        .ToArray();
        return staffs.DistinctBy(s => s.Id).ToArray();
    }
}