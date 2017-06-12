using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace NotHotDog
{
    public partial class CustomVision : ContentPage
    {
        public CustomVision()
        {
            InitializeComponent();
        }

		private async void loadCamera(object sender, EventArgs e)
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{
				await DisplayAlert("No Camera", ":( No camera available.", "OK");
				return;
			}

			MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
			{
				PhotoSize = PhotoSize.Medium,
				Directory = "Sample",
				Name = $"{DateTime.UtcNow}.jpg"
			});

			if (file == null)
				return;

			image.Source = ImageSource.FromStream(() =>
			{
				return file.GetStream();
			});

			file.Dispose();
		}
    }
}
