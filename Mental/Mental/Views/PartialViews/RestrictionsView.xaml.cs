using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.ViewModels.PartialViewModels;
using Mental.Models;

namespace Mental.Views.PartialViews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RestrictionsView : ContentView
	{
        //public static readonly BindableProperty RestrictionsPartialViewModelProperty = BindableProperty.Create(nameof(RestrictionsPartialViewModel), typeof(RestrictionsPVM), typeof(RestrictionsView));

        //public RestrictionsPVM RestrictionsPartialViewModel
        //{
        //    get
        //    {
        //        return (RestrictionsPVM)GetValue(RestrictionsPartialViewModelProperty);
        //    }
        //    set
        //    {
        //        this.BindingContext = value;
        //        SetValue(RestrictionsPartialViewModelProperty, value);
        //    }
        //}

		public RestrictionsView ()
		{
            //this.BindingContext = new RestrictionsPVM(new Models.TaskRestrictions());
			InitializeComponent ();          
		}
	}
}