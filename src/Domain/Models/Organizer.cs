using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models.Shared;

namespace Domain.Models;
public class Organizer : BaseEntity
{
    public required string OrganizationName { get; set; }
    //----------------------------------------- 
    [ForeignKey(nameof(Id))]
    public User User { get; set; } = null!;
}