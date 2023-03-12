using Kompass.ViewModels;
using Microsoft.Maui.Devices.Sensors;

namespace Kompass.Views;


public partial class MagnetPage : ContentPage
{

	MagnetViewModel _magnetViewModel;
	public MagnetPage()
	{
        InitializeComponent();

        _magnetViewModel = new MagnetViewModel();
		BindingContext = _magnetViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        //this.CompassButton.Text = "Kompass anschalten";
        //this.CompassImage.Rotation = 0;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (Compass.Default.IsSupported)
            _ = _magnetViewModel.TurnOff_Compass();
    }
}