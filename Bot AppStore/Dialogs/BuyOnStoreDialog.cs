using System;
using System.Threading.Tasks;
using Bot_AppStore.Azure;
using Bot_AppStore.Model;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;

namespace Bot_AppStore.Dialogs
{
    [Serializable]
    [LuisModel("d3a52e98-fcdf-4539-bcea-bc932310997f", "ebe798bf76be4f5b88a052a0b0001e20")]
    public class BuyOnStoreDialog : LuisDialog<object>
    {

        [LuisIntent("saudar")]
        public async Task Greetings(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Olá, eu sou um bot e estou aqui para ajudar você a encontrar um bom violino! Como posso lhe ajudar?");
        }

        [LuisIntent("instrumentosDisponiveis")]
        public async Task AvaiableInstruments(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Tenho estes instrumentos disponíveis em estoque:");
            var actv = await GetViolinCarousel(context);
            await context.PostAsync(actv);
        }

        [LuisIntent("comprarViolino")]
        public async Task BuyInstrument(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Qual violino você deseja comprar?");
        }

        [LuisIntent("realizarCompraInstrumento")]
        public async Task PurchaseInstrument(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Ok, compra realizada com sucesso!");
        }

        [LuisIntent("agradecimento")]
        public async Task acknowledgment(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Ok, sempre que precisar estarei aqui para lhe ajudar. Tenha um bom dia :)");
        }

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Desculpe, não entendi.");
        }
        private async Task<List<InstrumentModel>> GetViolinKinds()
        {
            return await StorageManger.GetInstrumentsFromAPI();
        }
        private async Task<IMessageActivity> GetViolinCarousel(IDialogContext context)
        {
            var list = await GetViolinKinds();
            var response = context.MakeMessage();
            foreach (var item in list)
            {
                response.Attachments.Add(new HeroCard
                {
                    Title = item.Descricao,
                    Subtitle = item.Preco,
                    Images = new List<CardImage> { new CardImage(url: item.Image) }
                }.ToAttachment());
            };
            response.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            return response;
        }
    }
}