using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.ViewModels;
using Mental.Models;

namespace Mental.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchulteTableTaskPage : ContentPage
    {
        private SchulteTableVM SchulteTableVM;
        private Random random;
        private List<int> ListOfSchulteTableNumbers;

        private int[] FontSizes = new int[]
        {
            30,28,26,24,22
        };

        public SchulteTableTaskPage(SchulteTableTaskOptions _schulteTableTaskOptions, ITimeOption _timeOption)
        {
            InitializeComponent();
            ListOfSchulteTableNumbers = new List<int>(Enumerable.Range(1, (int)Math.Pow(_schulteTableTaskOptions.GridSize, 2)));
            random = new Random();
            SchulteTableVM = new SchulteTableVM(_schulteTableTaskOptions, _timeOption, this.Navigation,this);

            this.BindingContext = SchulteTableVM;
            SchulteTableGrid.ColumnSpacing = 0;
            SchulteTableGrid.RowSpacing = 0;


            for (int i = 0; i < _schulteTableTaskOptions.GridSize; i++)
            {
                SchulteTableGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                SchulteTableGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int i = 0; i < _schulteTableTaskOptions.GridSize; i++)
            {
                for (int j = 0; j < _schulteTableTaskOptions.GridSize; j++)
                {
                    Button button = new Button();
                    int randomNumberFromList = random.Next(0, ListOfSchulteTableNumbers.Count);
                    button.Text = ListOfSchulteTableNumbers[randomNumberFromList].ToString();
                    ListOfSchulteTableNumbers.RemoveAt(randomNumberFromList);
                    button.Style = (Style)this.Resources["GridButton"];
                    button.Command = SchulteTableVM.SchulteTableButtonClicked;
                    button.CommandParameter = button;
                    button.FontSize = FontSizes[_schulteTableTaskOptions.GridSize - 3];
                    SchulteTableGrid.Children.Add(button, i, j);
                }
            }

            this.Title = "Schulte Table " + _schulteTableTaskOptions.GridSize.ToString() + " x " + _schulteTableTaskOptions.GridSize.ToString();
        }

        public async void HideTaskFrame()
        {
            await TaskFrame.FadeTo(0, 750);
            TaskFrame.IsVisible = false;
            AfterTaskFrame.IsVisible = true;
            await AfterTaskFrame.FadeTo(1, 750);
        }

        public async void ShowTaskFrame()
        {
            await AfterTaskFrame.FadeTo(0, 750);
            AfterTaskFrame.IsVisible = false;
            TaskFrame.IsVisible = true;
            await TaskFrame.FadeTo(1, 750);
        }

        public void ButtonsColorToDefault()
        {
            for(int i = 0;i < SchulteTableGrid.Children.Count;i++)
            {
                Button button = SchulteTableGrid.Children[i] as Button;
                button.BackgroundColor = Color.FromHex("#6699ff");
            }
        }
    }
}