namespace BandManager.Models;

public class LearnedSong
{
    public int id { get; set; }
    public string SongName { get; set; }
    public string BandName { get; set; }
    public ushort PlayCount { get; set; }
    public byte CurrentConfidence { get; set; }
    public DateTime LastPlayed { get; set; }
}