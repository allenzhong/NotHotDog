using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace NotHotDog
{
    public class AzureManager
    {
        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<NotHotDogModel> notHotDogTable;
        private const string MOBILE_APP_URL = "https://azurehotdog.azurewebsites.net/";

        private AzureManager()
        {
            //CurrentPlatform.Init();
            this.client = new MobileServiceClient(MOBILE_APP_URL);
            this.notHotDogTable = this.client.GetTable<NotHotDogModel>();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static AzureManager AzureManagerInstance
        {
            get 
            {
                if(instance == null)
                {
                    instance = new AzureManager();
                }

                return instance;
            }
        }

        public async Task<List<NotHotDogModel>> GetHotDogInformation()
        {
            return await this.notHotDogTable.ToListAsync();
        }
    }
}
