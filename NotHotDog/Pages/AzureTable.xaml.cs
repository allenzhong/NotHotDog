using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace NotHotDog
{
    public partial class AzureTable : ContentPage
    {
        Geocoder geoCoder;
		MobileServiceClient client = AzureManager.AzureManagerInstance.AzureClient;
        public AzureTable()
        {
            InitializeComponent();
            geoCoder = new Geocoder();
        }

        async void Handle_ClickedAsync(object sender, System.EventArgs e)
        {
            List<NotHotDogModel> notHotDogInformation = await AzureManager.AzureManagerInstance.GetHotDogInformation();

            //HotDogList.ItemsSource = notHotDogInformation;
            foreach(NotHotDogModel model in notHotDogInformation)
            {
                var position = new Position(model.Latitude, model.Longitude);
                var possibleAdresses = await geoCoder.GetAddressesForPositionAsync(position);
                foreach(var address in possibleAdresses)
                {
                    model.City = address;
                }

            }

            HotDogList.ItemsSource = notHotDogInformation;
        }
    }
}
