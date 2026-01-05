using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BandManager.Models;

namespace BandManager;

public partial class Practice : ContentPage
{
    private List<LearnedSong> _songs;
    private LearnedSong? _recommendedSong;
    
    public Practice()
    {
        InitializeComponent();
        LoadSongsAsync();
    }

    private async void LoadSongsAsync()
    {
        LoadingSpinner.IsVisible = true;
        try
        {
            _songs = await RestService.GetSongsAsync();
            CalculateSong();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
        finally
        {
            LoadingSpinner.IsVisible = false;
        }
    }
    
    private void CalculateSong()
    {
        int len = _songs.Count;
        // No songs so return
        if (len == 0)
            return;
        
        // Shuffle songs
        while (len > 1)
        {
            int k = Random.Shared.Next(len--);
            (_songs[len],_songs[k]) = (_songs[k], _songs[len]);
        }
        
        var weightedSongs = _songs.Select(song => new
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
                _recommendedSong = ws.Song;
                PlayedSwitch.IsToggled = false;
                break;
            }
        }

        if (_recommendedSong == null)
        {
            return;
        }

        SongLbl.Text = _recommendedSong.SongName + ", " + _recommendedSong.BandName;
        ConfidenceSlider.Value = _recommendedSong.CurrentConfidence;
        RatingValueLbl.Text = "Rating: " + _recommendedSong?.CurrentConfidence;
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
    }

    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        if (_recommendedSong == null)
            return;
        
        if (PlayedSwitch.IsToggled)
        {
            _recommendedSong.PlayCount++;
            _recommendedSong.LastPlayed = DateTime.Now;
        }

        _recommendedSong.CurrentConfidence = (byte)ConfidenceSlider.Value;
        LoadingSpinner.IsVisible = true;
        try
        {
            await RestService.UpdateSong(_recommendedSong);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            await DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            LoadingSpinner.IsVisible = false;
        }
    }
    
    private void OnRateSliderChanged(object sender, ValueChangedEventArgs e)
    {
        ConfidenceSlider.Value = Math.Round(e.NewValue);
        RatingValueLbl.Text = "Rating: " + ConfidenceSlider.Value;
    }
}