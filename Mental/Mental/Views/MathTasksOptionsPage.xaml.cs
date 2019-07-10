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
        private bool IsVisible = true;
             
        public MathTasksOptionsPage()
        {
            InitializeComponent();
            BindingContext = new MathTasksOptionsVM(this.Navigation);
        }

        private async void ShowHideDescription_Clicked(object sender, EventArgs e)
        {
            if (IsVisible)
            {
               // await OperationDescriptionFrame.TranslateTo(OperationDescriptionFrame.X, OperationDescriptionFrame.Y - 100, 500);
                await OperationDescriptionFrame.ScaleTo(0,500,Easing.SinOut);
                OperationDescriptionFrame.IsEnabled = false;
                OperationDescriptionFrame.IsVisible = false;
                IsVisible = false;
            }
            else
            {
                OperationDescriptionFrame.IsEnabled = true;
                OperationDescriptionFrame.IsVisible = true;
                await OperationDescriptionFrame.ScaleTo(1, 500);
                IsVisible = true;
            }
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