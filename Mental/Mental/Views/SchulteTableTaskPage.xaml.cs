﻿using System;
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
            22,20,18,16,13,10,7
        };

        public SchulteTableTaskPage(SchulteTableTaskOptions _schulteTableTaskOptions, ITimeOption _timeOption)
        {
            InitializeComponent();
            ListOfSchulteTableNumbers = new List<int>(Enumerable.Range(1, (int)Math.Pow(_schulteTableTaskOptions.GridSize, 2)));
            random = new Random();
            SchulteTableVM = new SchulteTableVM(_schulteTableTaskOptions,_timeOption,this.Navigation);

            this.BindingContext = SchulteTableVM;
            SchulteTableGrid.ColumnSpacing = 0;
            SchulteTableGrid.RowSpacing = 0;
         

            for(int i = 0;i < _schulteTableTaskOptions.GridSize;i++)
            {
                SchulteTableGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                SchulteTableGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }

            for(int i = 0;i < _schulteTableTaskOptions.GridSize;i++)
            {
                for(int j = 0;j < _schulteTableTaskOptions.GridSize;j++)
                {
                    Button button = new Button();
                    int randomNumberFromList = random.Next(0, ListOfSchulteTableNumbers.Count);
                    button.Text = ListOfSchulteTableNumbers[randomNumberFromList].ToString();
                    ListOfSchulteTableNumbers.RemoveAt(randomNumberFromList);
                    button.Command = SchulteTableVM.SchulteTableButtonClicked;
                    button.CommandParameter = button;
                    button.FontSize = FontSizes[_schulteTableTaskOptions.GridSize - 2];
                    button.BorderWidth = 1;
                    button.BorderColor = Color.Black;
                    button.Margin = new Thickness(0, 0, 0, 0);
                    button.Padding = new Thickness(0, 0, 0, 0);
                    SchulteTableGrid.Children.Add(button, i, j);
                }
            }
        }
    }
}