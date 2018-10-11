using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;

using Xamarin.Forms;

namespace Food_Connecter
{
    public partial class ChatPage : ContentPage
    {
        public ChatPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            RefreshChat();
        }

        async void SendMessage(object sender, EventArgs e)
        {
            if (ChatContent.Text == null)
            {
                return;
            }
            postChatModel pm = new postChatModel();
            pm.User = kanriPage.userInfo.UserName;
            pm.ToUser = (string)this.BindingContext;
            pm.Text = ChatContent.Text;

            var json = JsonConvert.SerializeObject(pm);
            var content = new StringContent(json);
            content.Headers.ContentType.MediaType = "application/json";
            var res = await App.client.PostAsync(Constants.ApplicationURL + "/api/postchat", content);
            Console.WriteLine(res.StatusCode);
            ChatContent.Text = null;
            RefreshChat();
        }

        async void refreshChat(object sender, EventArgs e)
        {
            RefreshChat();
        }

        public async void RefreshChat()
        {
            Stack.Children.Clear();
            Console.WriteLine(kanriPage.userInfo.UserName);
            var res = await App.client.GetAsync(Constants.ApplicationURL + "/api/getchat?user=" + kanriPage.userInfo.UserName + "&touser=" + (string)BindingContext);
            var json = res.Content.ReadAsStringAsync().Result;
            Console.WriteLine(json);
            List<getChatModel> history = new List<getChatModel>();
            try
            {
                history = JsonConvert.DeserializeObject<List<getChatModel>>(json);
            }
            catch
            {
                return;
            }
            foreach (var i in history)
            {
                Label label = new Label
                {
                    Text = i.userName
                };
                label.FontSize = 20;
                Label chat = new Label
                {
                    Text = i.Text
                };
                chat.FontSize = 20;
                Label date = new Label
                {
                    Text = i.datetime
                };
                label.FontSize = 5;

                if(i.Flag)
                {
                    label.HorizontalOptions = LayoutOptions.Start;
                    chat.HorizontalOptions = LayoutOptions.Start;
                    date.HorizontalOptions = LayoutOptions.Start;
                }
                else
                {
                    label.HorizontalOptions = LayoutOptions.End;
                    chat.HorizontalOptions = LayoutOptions.End;
                    date.HorizontalOptions = LayoutOptions.End;
                }

                Stack.Children.Add(label);
                Stack.Children.Add(chat);
                Stack.Children.Add(date);
            }
        }
    }
}
