using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Food_Connecter
{
    public partial class eventManagePage : ContentPage
    {
        public eventManagePage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var info = (eventModel)BindingContext;
            Console.WriteLine(info.Num);

            var res = await App.client.GetAsync(Constants.ApplicationURL + "/api/manageevent?id=" + App.Authenticator.user.UserId + "&eventnum=" + info.Num.ToString());
            var text = res.Content.ReadAsStringAsync().Result;
            Console.WriteLine(res.StatusCode);
            Console.WriteLine(text);
            try
            {
                var json = JsonConvert.DeserializeObject<List<ManageEvent>>(text)[0];
                eventName.Text = json.eventName;
                eventCity.Text = json.eventCity;
                eventPlace.Text = json.eventPlace;
                Date.Text = json.Date;
                foreach (var i in json.Menbers)
                {
                    EntryCell cell = new EntryCell();
                    cell.Text = i.userName;
                    cell.IsEnabled = false;
                    menber.Add(cell);
                }
                foreach (var i in json.Wanteds)
                {
                    TextCell cell = new TextCell();
                    cell.Text = i.foodName;
                    wanted.Add(cell);
                }
                foreach (var i in json.Foods)
                {
                    EntryCell cell = new EntryCell();
                    cell.Label = i.foodName;
                    cell.Text = i.userName;
                    cell.IsEnabled = false;
                    food.Add(cell);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        async void Wanted_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddWantedPage
            {
                BindingContext = (eventModel)this.BindingContext
            });
        }
    }
}
