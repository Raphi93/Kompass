using Kompass.ViewModels;

namespace Kompass;

public partial class App : Application
{
    private KompassViewModel magnetViewModel = new KompassViewModel();

    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }

    // Does not work because of new Instance of ViewModel
    //protected override void OnResume()
    //{
    //    base.OnResume();
    //    magnetViewModel.ButtonText = "OK";
    //}

    //protected override void OnSleep()
    //{
    //    magnetViewModel.TurnOff_Compass();
    //    base.OnSleep();
    //}
}