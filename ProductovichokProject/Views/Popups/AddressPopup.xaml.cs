using CommunityToolkit.Maui.Views;
using Microsoft.EntityFrameworkCore;
using ProductovichokProject.Data;
using ProductovichokProject.Data.Models;
using ProductovichokProject.Services;

namespace ProductovichokProject.Views.Popups;

public partial class AddressPopup : Popup
{
	private readonly ProductovichokContext _context = new ProductovichokContext();
	User UserInfo;

    public AddressPopup(User userInfo)
	{
		InitializeComponent();
		UserInfo = userInfo;
        _context.Streets.ToList();
        _context.Addresses.ToList();
        StreetPick.ItemsSource = _context.Streets.Select(x => x.StreetName).ToList();
		
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		if(HousePick.SelectedItem != null)
		{
            Close(new UserAddress()
            {
                AddressId = _context.Addresses.SingleOrDefault(x => x.Street.StreetName == StreetPick.SelectedItem.ToString() && x.HouseId == (int?)HousePick.SelectedItem).AddressId,
                UserId = UserInfo.UserId,
                Appartament = string.IsNullOrWhiteSpace(AppartamentEntry.Text) ? null : Int32.Parse(AppartamentEntry.Text),
                Floor = string.IsNullOrWhiteSpace(FloorEntry.Text) ? null : Int32.Parse(FloorEntry.Text),
                Entrance = string.IsNullOrWhiteSpace(EntranceEntry.Text) ? null : Int32.Parse(EntranceEntry.Text)
            });
        }
    }

    private async void StreetPick_SelectedIndexChanged(object sender, EventArgs e)
    {
        HousePick.ItemsSource = await _context.Addresses.Where(x => x.Street.StreetName == StreetPick.SelectedItem.ToString()).Select(x => x.HouseId).ToListAsync();
    }
}