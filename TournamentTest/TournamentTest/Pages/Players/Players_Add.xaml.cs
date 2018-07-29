using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentTest.Classes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TournamentTest.Pages.Players
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Players_Add : ContentPage
	{
		public Players_Add ()
		{
			InitializeComponent ();         
		}

        private void Button_Clicked(object sender, EventArgs e)
        {
            Player player = new Player()
            {
                Name = nameEntry.Text,
                Email = emailEntry.Text
            };

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Player>();
                var numberOfRows = conn.Insert(player);
                if (numberOfRows > 0)
                {
                    DisplayAlert("Success", "Player successfully inserted", "Great!");
                }
                else
                {
                    DisplayAlert("Failure", "Player failed to be inserted", "Oops!");
                }
            }
        }
    }
}