using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Kompass.ViewModels
{
    public class GeographicViewModel : ViewModelBase
    {
        private double _compassRotation;

        public ICommand GetCompass { get; set; }


        public GeographicViewModel()
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

        private void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            // Update UI Label with compass state
            var heading = e.Reading.HeadingMagneticNorth;
            double rotationAngle = (360 - heading) % 360;
            CompassRotation = Convert.ToDouble(rotationAngle);
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
