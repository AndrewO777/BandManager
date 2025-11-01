using BandManager.Controls;
using BandManager.Models;

namespace BandManager;

public partial class MainPage : ContentPage
{
    private List<LearnedSong> songs = new List<LearnedSong>();
    private LearnedSong recommendedSong;

    public MainPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        LoadingSpinner.IsVisible = true;
        try
        {
            //Get songs from api here
            //Temp data for testing
            recommendedSong = new LearnedSong()
            {
                id = 0,
                SongName = "Test Song",
                BandName = "Test Band",
                CurrentConfidence = 75,
                LastPlayed = DateTime.Now.AddDays(-10),
                PlayCount = 5
            };
            CalculateSong();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        } 
    }
    
    private void CalculateSong()
    {
        int len = songs.Count;
        // No songs so return
        if (len == 0)
            return;
        
        // Shuffle songs
        while (len > 1)
        {
            int k = Random.Shared.Next(len--);
            LearnedSong temp = songs[k];
            songs[len] = songs[k];
            songs[k] = temp;
        }
        
        var weightedSongs = songs.Select(song => new
        {
            Song = song,
            Weight = CalculateWeight(song)
        }).ToList();

        double totalWeight = weightedSongs.Sum(ws => ws.Weight);
        double randomWeight = Random.Shared.NextDouble() * totalWeight;
        double cumulativeWeight = 0;
        foreach (var ws in weightedSongs)
        {
            cumulativeWeight += ws.Weight;
            if (randomWeight <= cumulativeWeight)
            {
                recommendedSong = ws.Song;
                break;
            }
        }
        SongLbl.Text = recommendedSong.SongName + ", " + recommendedSong.BandName;
        ConfidenceSlider.Value = recommendedSong.CurrentConfidence;
        RatingValueLbl.Text = "Rating: " + recommendedSong.CurrentConfidence;
    }

    private double CalculateWeight(LearnedSong song)
    {
        double daysSinceLastPlayed = (DateTime.Now - song.LastPlayed).TotalDays;
        double confidenceFactor = 101 - song.CurrentConfidence;
        return daysSinceLastPlayed * (confidenceFactor/10);
    }

    private void OnNewSongClicked(object sender, EventArgs e)
    {
        CalculateSong();
        SongLbl.Text = recommendedSong.SongName + ", " + recommendedSong.BandName;
    }

    private void OnUpdateClicked(object sender, EventArgs e)
    {
        if (PlayedCheckBox.IsChecked)
        {
            recommendedSong.PlayCount++;
            recommendedSong.LastPlayed = DateTime.Now;
        }

        recommendedSong.CurrentConfidence = (byte)ConfidenceSlider.Value;
        //Update song in database here via api
    }
    
    private void OnRateSliderChanged(object sender, ValueChangedEventArgs e)
    {
        ConfidenceSlider.Value = Math.Round(e.NewValue);
        RatingValueLbl.Text = "Rating: " + ConfidenceSlider.Value;
    }
    /*private void OnCounterClicked(object? sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }*/
}