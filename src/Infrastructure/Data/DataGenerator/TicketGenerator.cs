using Bogus;
using Domain.Enums;
using Domain.Models;

namespace Infrastructure.Data.DataGenerator;
public static class TicketGenerator
{
    public static Ticket[] GenerateTickets(Attendee[] attendees, Order[] orders)
        => new Faker<Ticket>()
            .UseSeed(1)
            .UseDateTimeReference(DateTime.UtcNow)
            .RuleFor(t => t.Id, f => f.Random.Guid())
            .RuleFor(t => t.AttendeeId, f => f.PickRandom(attendees).Id)
            .RuleFor(t => t.OrderId, f => f.PickRandom(orders).Id)
            .RuleFor(t => t.Code, f => f.Random.AlphaNumeric(10).ToUpper())
            .RuleFor(t => t.UsedInFormat, f => f.PickRandom<UsedInFormatEnum>())
            .RuleFor(t => t.UsedAt, f => f.Date.Future())
            .Generate(200)
            .ToArray();
}