using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BandManager.Models;

namespace BandManager;

public partial class Songs : ContentPage
{
    public ObservableCollection<LearnedSong> LearnedSongs { get; set; } = new ObservableCollection<LearnedSong>();
    private List<LearnedSong> _allSongs;
    
    public Songs()
    {
        InitializeComponent();
        LoadSongsAsync();
        BindingContext = this;
    }
    
    private async void LoadSongsAsync()
    {
        LoadingSpinner.IsVisible = true;
        try
        {
            _allSongs = await RestService.GetSongsAsync();
            LearnedSongs = new ObservableCollection<LearnedSong>(_allSongs);
            OnPropertyChanged(nameof(LearnedSongs));
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
    
    private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = e.NewTextValue?.ToLower() ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(searchText))
        {
            LearnedSongs = new ObservableCollection<LearnedSong>(_allSongs);
        }
        else
        {
            var filtered = _allSongs.Where(s => 
                s.SongName.ToLower().Contains(searchText) || 
                s.BandName.ToLower().Contains(searchText));
            LearnedSongs = new ObservableCollection<LearnedSong>(filtered);
        }
        
        OnPropertyChanged(nameof(LearnedSongs));
    }
}