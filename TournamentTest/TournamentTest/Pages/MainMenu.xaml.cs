using System;
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
    public partial class MainMenu : MasterDetailPage
    {
        public List<MainMenuItem> MainMenuItems { get; set; }

        public MainMenu()
        {
            // Set the binding context to this code behind.
            BindingContext = this;

            // Build the Menu
            MainMenuItems = new List<MainMenuItem>()
            {
                //new MainMenuItem() { Title = "Page One", Icon = "menu_inbox.png", TargetType = typeof(PageOne) },
                //new MainMenuItem() { Title = "Page Two", Icon = "menu_stock.png", TargetType = typeof(PageTwo) },
                new MainMenuItem() { Title = "Players", Icon = "menu_inbox.png", TargetType = typeof(Players_Main) },
                new MainMenuItem() { Title = "Tournaments", Icon = "menu_stock.png", TargetType = typeof(Tournaments_Main) }
            };

            // Set the default page, this is the "home" page.
            Detail = new NavigationPage(new Players_Main());

            InitializeComponent();
        }

        // When a MenuItem is selected.
        public void MainMenuItem_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainMenuItem;
            if (item != null)
            {
                switch (item.Title.ToString())
                {
                    //case "Page One":
                    //    Detail = new NavigationPage(new PageOne());
                    //    break;

                    //case "Page Two":
                    //    Detail = new NavigationPage(new PageTwo());
                    //    break;

                    case "Players":
                        Detail = new NavigationPage(new Players_Main());
                        break;

                    case "Tournaments":
                        Detail = new NavigationPage(new Tournaments_Main());
                        break;
                }

                MenuListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}