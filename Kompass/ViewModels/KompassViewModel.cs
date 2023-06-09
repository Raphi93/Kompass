﻿using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Kompass.ViewModels
{
    public class KompassViewModel : ViewModelBase
    {
        private double _compassRotation;
        private bool _isLight;
        public ICommand GetCompass { get; set; }

        public KompassViewModel()
        {
            ButtonText = "Kompass anschalten";
            GetCompass = new AsyncRelayCommand(ToggleCompass, CanToggleCompass);
        }

        /// <summary>
        /// Schalten den Kompass ein und oder aus
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Prüft ob es Supported ist
        /// </summary>
        /// <returns></returns>
        public bool CanToggleCompass()
        {
            if (!Compass.Default.IsSupported)
            {
                ButtonText = "Kompass nicht unterstützt";
                return false;
            }
            else
            {
                ButtonText = "Kompass anschalten";
                return true;
            }
        }

        /// <summary>
        /// Schalten den Kompass aus
        /// </summary>
        /// <returns></returns>
        public async Task TurnOff_Compass()
        {
            CompassRotation = 0;
            ButtonText = "Kompass anschalten";
            Compass.Default.Stop();
            await Flashlight.TurnOffAsync();
            Compass.Default.ReadingChanged -= Compass_ReadingChanged;
        }

        /// <summary>
        /// Schaltet den Kompasss aus
        /// </summary>
        /// <returns></returns>
        public async Task TurnOn_Compass()
        {
            Compass.Default.ReadingChanged += Compass_ReadingChanged;
            Compass.Default.Start(SensorSpeed.UI);
            ButtonText = "Kompass ausschalten";
        }

        /// <summary>
        /// ändert die richtung
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            var heading = e.Reading.HeadingMagneticNorth;
            double rotationAngle = (360 - heading) % 360;
            CompassRotation = Convert.ToDouble(rotationAngle);
            if ((rotationAngle >= -5) && (rotationAngle <= 5) && (IsLight))
            {
                await FlashLightsNorth();
            }
            if ((rotationAngle >= 175) && (rotationAngle <= 185) && (IsLight))
            {
                await FlashLightsSouth();
            }
        }

        /// <summary>
        /// Flashlight schaltet es ein im süden
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Flashlights im Norden
        /// </summary>
        /// <returns></returns>
        private async Task FlashLightsNorth()
        {
            await Flashlight.Default.TurnOnAsync();
            await Task.Delay(1000);
            await Flashlight.Default.TurnOffAsync();
        }

        public double CompassRotation
        {
            get => _compassRotation;
            set => SetProperty(ref _compassRotation, value);
        }

        private string _buttonText = "Kompass anschalten";

        public string ButtonText
        {
            get => _buttonText;
            set => SetProperty(ref _buttonText, value);
        }

        public bool IsLight
        {
            get => _isLight;
            set => SetProperty(ref _isLight, value);
        }
    }
}