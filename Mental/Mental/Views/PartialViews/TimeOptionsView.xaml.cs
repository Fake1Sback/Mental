using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mental.Views.PartialViews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TimeOptionsView : ContentView
	{
		public TimeOptionsView ()
		{
			InitializeComponent ();
		}

        private void TimeOptionsExpandMoreButton_Clicked(object sender, EventArgs e)
        {
            TimeOptionsFrame.IsVisible = !TimeOptionsFrame.IsVisible;
        }
    }
}