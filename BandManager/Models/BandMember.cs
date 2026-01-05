namespace BandManager.Models;

public record BandMember()
{
    public required Guid UserId { get; set; }
    public required string Name { get; set; }
    public required string Role { get; set; }
}