using Kompass.ViewModels;
using Microsoft.Maui.Devices.Sensors;

namespace Kompass.Views;


public partial class MagnetPage : ContentPage
{

	MagnetViewModel _magnetViewModel;
	public MagnetPage()
	{
		_magnetViewModel = new MagnetViewModel();
		BindingContext = _magnetViewModel;
		InitializeComponent();
    }
}