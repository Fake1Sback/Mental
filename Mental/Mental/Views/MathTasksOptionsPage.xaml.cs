using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.ViewModels;
using Mental.Models;
using System.Diagnostics;

namespace Mental.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MathTasksOptionsPage : ContentPage
    {            
        public MathTasksOptionsPage()
        {
            InitializeComponent();
            BindingContext = new MathTasksOptionsVM(this.Navigation);
        }

        public MathTasksOptionsPage(MathTasksOptions FavouriteMathTaskOptions)
        {
            InitializeComponent();
            BindingContext = new MathTasksOptionsVM(this.Navigation, FavouriteMathTaskOptions);
        }

        private void OperationsExpandMoreButton_Clicked(object sender, EventArgs e)
        {
            OperationsFrame.IsVisible = !OperationsFrame.IsVisible;
        }

        private void ChainLengthExpandMoreButton_Clicked(object sender, EventArgs e)
        {
            ChainLengthFrame.IsVisible = !ChainLengthFrame.IsVisible;
        }

        private void GeneratedValueExpandMoreButton_Clicked(object sender, EventArgs e)
        {
            GeneratedValuesFrame.IsVisible = !GeneratedValuesFrame.IsVisible;
        }

        private void NumbersTypeExpandMoreButton_Clicked(object sender, EventArgs e)
        {
            NumbersTypeFrame.IsVisible = !NumbersTypeFrame.IsVisible;
        }

        private void TaskTypeExpandMoreButton_Clicked(object sender, EventArgs e)
        {
            TaskTypeFrame.IsVisible = !TaskTypeFrame.IsVisible;
        }    
    }
}