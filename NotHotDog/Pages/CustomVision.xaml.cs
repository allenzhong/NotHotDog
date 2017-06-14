﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Linq;

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

            //file.Dispose();
            await MakePredictionRequest(file);
		}

        static byte[] GetImageAsByteArray(MediaFile file)
        {
            var stream = file.GetStream();
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }

        async Task MakePredictionRequest(MediaFile file)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Prediction-Key","306055b083f64c3ca25171b754c1f4b3");
            string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/9b3a082f-147a-42ce-943d-e0853a1391c5/image?iterationId=a729604d-0622-4507-afac-77a6e643d754";

            HttpResponseMessage response;

            byte[] byteData = GetImageAsByteArray(file);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);

                if(response.IsSuccessStatusCode) 
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    JObject rss = JObject.Parse(responseString);

                    var probability = from p in rss["Predictions"] select (double)p["Probability"];
                    var tag = from p in rss["Predictions"] select (string)p["Tag"];

                    foreach (var item in tag)
                    {
                        TagLabel.Text += item + " : \n";
                    }
                    foreach (var item in probability)
                    {
                        PredictionLabel.Text += item.ToString("F2") + "% \n";
                    }
                }

                file.Dispose();
            }
        }
    }
}
