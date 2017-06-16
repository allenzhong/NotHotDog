using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;

namespace NotHotDog
{
    public partial class AzureTable : ContentPage
    {
		MobileServiceClient client = AzureManager.AzureManagerInstance.AzureClient;
        public AzureTable()
        {
            InitializeComponent();
        }

        async void Handle_ClickedAsync(object sender, System.EventArgs e)
        {
            List<NotHotDogModel> notHotDogInformation = await AzureManager.AzureManagerInstance.GetHotDogInformation();
            HotDogList.ItemsSource = notHotDogInformation;
        }
    }
}
