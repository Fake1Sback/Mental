using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.ViewModels;

namespace Mental.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SchulteTableTaskOptionsPage : ContentPage
	{
		public SchulteTableTaskOptionsPage ()
		{
			InitializeComponent ();
            BindingContext = new SchulteTableTaskOptionsVM(this.Navigation);
		}
	}
}