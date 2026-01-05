using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BandManager.Models;

namespace BandManager;

public partial class Band : ContentPage
{
    public ObservableCollection<BandMember> BandMembers { get; set; } = new ObservableCollection<BandMember>()
    {
        // Sample data, will pull from backend once implemented
        new BandMember { UserId = Guid.NewGuid(), Name = "Alice", Role = "Vocalist" },
        new BandMember { UserId = Guid.NewGuid(), Name = "Bob", Role = "Guitarist" },
        new BandMember { UserId = Guid.NewGuid(), Name = "Charlie", Role = "Drummer" }
    };
    public string BandName { get; set; } = "The Rockers";

    public Band()
    {
        InitializeComponent();
        BindingContext = this;
    }
    
    private void OnChangeLogoClicked(object sender, EventArgs e)
    {
        // Logic to change band logo
        DisplayAlertAsync("Change Logo", "Band logo change functionality is not implemented yet.", "OK");
    }

    private void OnEditNameClicked(object sender, EventArgs e)
    {
        // Logic to edit band name
        DisplayAlertAsync("Edit Name", "Band name edit functionality is not implemented yet.", "OK");
    }
    
    private void OnAddMemberClicked(object sender, EventArgs e)
    {
        // Logic to add a new band member
        DisplayAlertAsync("Add Member", "Add band member functionality is not implemented yet.", "OK");
    }
    
    private void OnDeleteMemberClicked(object sender, EventArgs e)
    {
        // Logic to remove a band member
        DisplayAlertAsync("Remove Member", "Remove band member functionality is not implemented yet.", "OK");
    }
    
    private void OnEditMemberClicked(object sender, EventArgs e)
    {
        // Logic to edit band member details
        DisplayAlertAsync("Edit Member", "Edit band member functionality is not implemented yet.", "OK");
    }
}