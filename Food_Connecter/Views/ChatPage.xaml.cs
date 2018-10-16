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

        protected override void OnAppearing()
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

        void refreshChat(object sender, EventArgs e)
        {
            RefreshChat();
        }

        void entryFocused(object sender, EventArgs e)
        {
            AbsoluteLayout.SetLayoutFlags(ChatText, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(ChatText, new Rectangle(0, 0.6, 1, 0.1));
        }

        void entryUnFocused(object sender, EventArgs e)
        {
            AbsoluteLayout.SetLayoutFlags(ChatText, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(ChatText, new Rectangle(0, 1, 1, 0.1));
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
                    Text = i.userName,
                    FontSize = 10
                };
                Label chat = new Label
                {
                    Text = i.Text,
                    FontSize = 20
                };
                Label date = new Label
                {
                    Text = i.datetime,
                    FontSize = 10
                };

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
