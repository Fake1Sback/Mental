using Mental.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mental.Models
{
    public class CarouselViewListItem : BaseVM
    {
        private string _Caption;
        private string _Description;
        private string _ImgSrc;

        public string Caption
        {
            get
            {
                return _Caption;
            }
            set
            {
                _Caption = value;
                OnPropertyChanged("Caption");
            }
        }
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                OnPropertyChanged("Description");
            }
        }
        public string ImgSrc
        {
            get
            {
                return _ImgSrc;
            }
            set
            {
                _ImgSrc = value;
                OnPropertyChanged("ImgSrc");
            }
        }
        public Command FavoriteCommand { get; set; }
        public Command OptionsCommand { get; set; }
        public Command StatisticsCommand { get; set; }
    }
}
