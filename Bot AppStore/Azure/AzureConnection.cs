using System.Net.Http;
using Bot_AppStore.Model;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Bot_AppStore.Azure
{
    public static class StorageManger
    {
        private const string endpoint = "http://webapiinstrument.azurewebsites.net/api/Home/GetAllInstruments";
        public static async Task<List<InstrumentModel>> GetInstrumentsFromAPI()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(endpoint);
                    return  await response.Content.ReadAsAsync<List<InstrumentModel>>();
            }
        }
    }
}