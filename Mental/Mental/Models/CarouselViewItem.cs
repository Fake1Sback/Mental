using Mental.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mental.Models
{
    public class CarouselViewItem<T> : BaseVM
    {
        public T _LeftItem;
        public T _CenterItem;
        public T _RightItem;

        public T LeftItem
        {
            get
            {
                return _LeftItem;
            }
        }

        public T CenterItem
        {
            get
            {
                return _CenterItem;
            }
        }

        public T RightItem
        {
            get
            {
                return _RightItem;
            }
        }

    }
}
