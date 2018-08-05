using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TournamentTest.Pages.Tournaments
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Tournaments_RoundInfo : ContentPage
	{
		public Tournaments_RoundInfo (string strTitle)
		{
			InitializeComponent ();
            Title = strTitle;
		}
	}
}