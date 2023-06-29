using CommunityToolkit.Maui.Views;
using Microsoft.EntityFrameworkCore;
using ProductovichokProject.Data;
using ProductovichokProject.Data.Models;
using ProductovichokProject.Services;

namespace ProductovichokProject.Views.Popups;

public partial class AddressPopup : Popup
{
    private readonly ProductovichokContext _context;
	User UserInfo;
    List<UserAddress> UserAddresses;

    public AddressPopup(User userInfo, List<UserAddress> userAddresses, ProductovichokContext context)
	{
		InitializeComponent();
		UserInfo = userInfo;
        UserAddresses = userAddresses;
        _context = context;
        _context.Streets.ToList();
        _context.Addresses.ToList();
        StreetPick.ItemsSource = _context.Streets.Select(x => x.StreetName).ToList();
		
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		if(HousePick.SelectedItem != null)
		{
            if(!UserAddresses.Any(x => x.Address.HouseId == (int?)HousePick.SelectedItem))
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
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Данный адресс уже добавлен. Если вы хотите его изменить, то перейдите в профиль.", "Окей");
            }
        }
    }

    private async void StreetPick_SelectedIndexChanged(object sender, EventArgs e)
    {
        HousePick.ItemsSource = await _context.Addresses.Where(x => x.Street.StreetName == StreetPick.SelectedItem.ToString()).Select(x => x.HouseId).ToListAsync();
    }
}