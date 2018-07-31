﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentTest.Classes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TournamentTest.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Players_Main : TabbedPage
	{
		public Players_Main ()
		{
			InitializeComponent ();

            this.Children.Clear();
            Children.Add(new Pages.Players.Players_List("Active", true));
            Children.Add(new Pages.Players.Players_List("Inactive", false));
        }

       

        private void ToolbarItem_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Pages.Players.Players_AddEdit());
        }

    }
}