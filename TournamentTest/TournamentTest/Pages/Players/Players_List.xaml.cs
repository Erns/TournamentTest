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
	public partial class Players_List : ContentPage
	{
        private bool blnActive;

		public Players_List (string strTitle, bool blnActive)
		{
			InitializeComponent ();
            Title = strTitle;
            this.blnActive = blnActive;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Player>();

                var lstPlayers = conn.Query<Player>("SELECT * FROM Player WHERE Active = ? AND DateDeleted IS NULL", blnActive);
                playersListView.ItemsSource = lstPlayers;
            }

            //var layout = new RelativeLayout();

            //var normalFab = new FAB.Forms.FloatingActionButton();
            //normalFab.Source = "ic_add_white_24dp.png";
            //normalFab.Size = FabSize.Normal;

            //layout.Children.Add(
            //    normalFab,
            //    xConstraint: Constraint.RelativeToParent((parent) => { return (parent.Width - normalFab.Width) - 16; }),
            //    yConstraint: Constraint.RelativeToParent((parent) => { return (parent.Height - normalFab.Height) - 16; })
            //);

        }

        public void OpenPlayer(TextCell sender, EventArgs e)
        {
            Navigation.PushAsync(new Players_AddEdit(Convert.ToInt32(sender.CommandParameter.ToString())));
        }

    }
}