﻿using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Kompass.ViewModels
{
    public class MagnetViewModel : ViewModelBase
    {

        private double _compassRotation;

        private bool _isLight;
        public ICommand GetCompass { get; set; }


        public MagnetViewModel()
        {
            ButtonText = "Kompass anschalten";
            GetCompass = new AsyncRelayCommand(ToggleCompass, CanToggleCompass);
        }

        public bool CanToggleCompass()
        {
            if (!Compass.Default.IsSupported)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public double CompassRotation
        {
            get { return _compassRotation; }
            set
            {
                SetProperty(ref _compassRotation, value);
            }
        }

        private string _buttonText = "Kompass anschalten";
        public string ButtonText
        {
            get { return _buttonText; }
            set
            {
                SetProperty(ref _buttonText, value);
            }
        }


        private async Task ToggleCompass()
        {

            if (!Compass.Default.IsMonitoring)
            {
                await TurnOn_Compass();
            }
            else
            {
                await TurnOff_Compass();
            }


        }

        public async Task TurnOff_Compass()
        {
            CompassRotation = 0;
            ButtonText = "Kompass anschalten";
            Compass.Default.Stop();
            await Flashlight.TurnOffAsync();
            Compass.Default.ReadingChanged -= Compass_ReadingChanged;
        }

        public async Task TurnOn_Compass()
        {
            Compass.Default.ReadingChanged += Compass_ReadingChanged;
            Compass.Default.Start(SensorSpeed.UI);
            ButtonText = "Kompass ausschalten";
        }

        private async void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            var heading = e.Reading.HeadingMagneticNorth;
            double rotationAngle = (360 - heading) % 360;
            CompassRotation =  Convert.ToDouble(rotationAngle);
            if ((rotationAngle >= -5) && (rotationAngle <= 5) && (IsLight))
            {
                await FlashLightsNorth();
            }
            else if ((rotationAngle >= 175) && (rotationAngle <= 185) && (IsLight))
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

        public bool IsLight
        {
            get { return _isLight; }
            set
            {
                SetProperty(ref _isLight, value);
            }
        }
    }
}
