using Kompass.ViewModels;

namespace Kompass.Views;

public partial class KompassPage : ContentPage
{
    private readonly KompassViewModel _kompassViewModel;

    public KompassPage()
    {
        InitializeComponent();

        _kompassViewModel = new KompassViewModel();
        BindingContext = _kompassViewModel;
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
            _ = _kompassViewModel.TurnOff_Compass();
    }
}