using System;
using System.Collections.Generic;
using System.Text;
using Mental.Models;
using Xamarin.Forms;
using Mental.Views;

namespace Mental.ViewModels
{
    public class StartingPageVM : BaseVM
    {
        private Color DefaultColor = Color.Transparent;
        private Color ActiveColor = Color.FromHex("#6699ff");

        private INavigation navigation;
        private List<Color> NavigationButtonColors = new List<Color>
        {
            Color.Transparent,
            Color.Transparent,
            Color.Transparent
        };

        private CarouselViewListItem _SelectedCarouselListItem;
        public CarouselViewListItem SelectedCarouselListItem
        {
            get
            {
                return _SelectedCarouselListItem;
            }
            set
            {
                _SelectedCarouselListItem = value;
                OnPropertyChanged("SelectedCarouselListItem");
                int selectedIndex = _CarouselViewList.FindIndex(t => t == _SelectedCarouselListItem);
                HighlightButton(selectedIndex);
            }
        }

        private List<CarouselViewListItem> _CarouselViewList = new List<CarouselViewListItem>();
        public List<CarouselViewListItem> CarouselViewList
        {
            get
            {
                return _CarouselViewList;
            }
            set
            {
                _CarouselViewList = value;
                OnPropertyChanged("CarouselViewList");
            }
        }

        public StartingPageVM(INavigation _navigation)
        {
            navigation = _navigation;
            InitializeList();
        }

        public string Caption
        {
            get
            {
                return _SelectedCarouselListItem.Caption;
            }
        }

        public Color FirstPoint
        {
            get
            {
                return NavigationButtonColors[0];
            }
            set
            {
                NavigationButtonColors[0] = value;
                OnPropertyChanged("FirstPoint");
            }
        }
        public Color SecondPoint
        {
            get
            {
                return NavigationButtonColors[1];
            }
            set
            {
                NavigationButtonColors[1] = value;
                OnPropertyChanged("SecondPoint");

            }
        }
        public Color ThirdPoint
        {
            get
            {
                return NavigationButtonColors[2];
            }
            set
            {
                NavigationButtonColors[2] = value;
                OnPropertyChanged("ThirdPoint");
            }
        }

        public void UpdateView()
        {
            OnPropertyChanged("Caption");
            OnPropertyChanged("FirstPoint");
            OnPropertyChanged("SecondPoint");
            OnPropertyChanged("ThirdPoint");
        }

        private void InitializeList()
        {
            CarouselViewList = new List<CarouselViewListItem>()
            {
                new CarouselViewListItem()
                {
                    Caption = "Mental Math",
                    Description = "Math",
                    ImgSrc = "MathOperations_200.png",
                    FavoriteCommand = new Command (()=>{}),
                    OptionsCommand = new Command (async () =>
                    {
                        await navigation.PushAsync(new MathTasksOptionsPage());
                    }),
                    StatisticsCommand = new Command (async () =>
                    {
                        await navigation.PushAsync(new GeneralStatisticsPage());
                    })
                },
                new CarouselViewListItem()
                {
                    Caption = "Schulte Tables",
                    Description = "Schulte",
                    ImgSrc = "Schulte_Tables_200.png",
                    FavoriteCommand = new Command(()=>{}),
                    OptionsCommand = new Command(async () =>
                    {
                        await navigation.PushAsync(new SchulteTableTaskOptionsPage());
                    }),
                    StatisticsCommand = new Command(async () =>
                    {
                        await navigation.PushAsync(new SchulteTableTasksGeneralStatisticsPage());
                    })
                  },
                new CarouselViewListItem()
                  {
                    Caption = "Stroop Tasks",
                    Description = "Stroop",
                    ImgSrc = "Stroop_Ver2_200.png",
                    FavoriteCommand = new Command (()=>{}),
                    OptionsCommand = new Command(async () =>
                    {
                        await navigation.PushAsync(new StroopTaskOptionsPage());
                    }),
                    StatisticsCommand = new Command(async () =>
                    {
                       await navigation.PushAsync(new StroopTaskGeneralStatisticsPage());
                    })
                  }
            };
            SelectedCarouselListItem = CarouselViewList[0];
        }
        private void HighlightButton(int index)
        {
            if (NavigationButtonColors.Count != 0)
            {
                for (int i = 0; i < NavigationButtonColors.Count; i++)
                {
                    NavigationButtonColors[i] = DefaultColor;
                }
                NavigationButtonColors[index] = ActiveColor;
                UpdateView();
            }
        }
    }
}
