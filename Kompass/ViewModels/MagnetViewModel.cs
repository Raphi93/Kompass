using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    Compass.Default.ReadingChanged -= Compass_ReadingChanged;
                }
            }
        }

        private void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            // Update UI Label with compass state
            var heading = e.Reading.HeadingMagneticNorth;
            double rotationAngle = (360 - heading) % 360;
            CompassRotation =  Convert.ToDouble(rotationAngle);
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
