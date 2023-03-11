using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Kompass.ViewModels
{
    public class MagnetViewModel : ViewModelBase
    {

        private double _compassRotation; 

        public ICommand GetCompass { get; set; }


        public MagnetViewModel()
        {
            GetCompass = new AsyncRelayCommand(ToggleCompass);
        }

        private async Task ToggleCompass()
        {
            if (Compass.Default.IsSupported)
            {
                if (!Compass.Default.IsMonitoring)
                {
                    // Turn on compass
                    Compass.Default.ReadingChanged += Compass_ReadingChanged;
                    Compass.Default.Start(SensorSpeed.UI);
                }
                else
                {
                    // Turn off compass
                    Compass.Default.Stop();
                    CompassRotation = 0;
                    Compass.Default.ReadingChanged -= Compass_ReadingChanged;
                }
            }
        }

        private async void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            // Update UI Label with compass state
            var heading = e.Reading.HeadingMagneticNorth;
            double rotationAngle = (360 - heading) % 360;
            CompassRotation =  Convert.ToDouble(rotationAngle);
            if ((rotationAngle >= -5) && (rotationAngle <= 5))
            {
                await FlashLightsNorth();
            }
            else if ((rotationAngle >= 175) && (rotationAngle <= 185))
            {
                await FlashLightsSouth();
            }
        }

        private async Task FlashLightsSouth()
        {
            await Flashlight.Default.TurnOnAsync();
            await Task.Delay(333);
            await Flashlight.Default.TurnOffAsync();
            await Task.Delay(333);
            await Flashlight.Default.TurnOnAsync();
            await Task.Delay(333);
            await Flashlight.Default.TurnOffAsync();
        }

        private async Task FlashLightsNorth()
        {
            await Flashlight.Default.TurnOnAsync();
            await Task.Delay(1000);
            await Flashlight.Default.TurnOffAsync();
        }

        public double CompassRotation
        {
            get { return _compassRotation; }
            set
            {
                SetProperty(ref _compassRotation, value);
            }
        }
    }
}
