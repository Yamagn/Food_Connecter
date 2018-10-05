using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Food_Connecter
{
    public partial class ChatPage : ContentPage
    {
        public ChatPage()
        {
            InitializeComponent();

            for (var i = 0; i < 100; i++)
            {
                Stack.Children.Add(new Label
                {
                    Text = "aaa"
                });
            }
        }
    }
}
