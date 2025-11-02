namespace BandManager.Models;

public record LearnedSong
{
    public required int Id { get; init; }
    public required string SongName { get; init; }
    public required string BandName { get; init; }
    public required ushort PlayCount { get; set; }
    public required byte CurrentConfidence { get; set; }
    public required DateTime LastPlayed { get; set; }
}