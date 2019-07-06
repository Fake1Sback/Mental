using Mental.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mental.Models
{
    public class BaseArrayProperty<T> : BaseVM
    {
        private IDictionary<int, T> arrayProperty;

        public BaseArrayProperty(IDictionary<int, T> _dictionary)
        {
            arrayProperty = _dictionary;
        }

        public T this[int index]
        {
            get
            {
                if (arrayProperty.ContainsKey(index))
                    return arrayProperty[index];
                else
                    return default(T);
            }
            set
            {
                arrayProperty[index] = value;
                OnPropertyChanged("Item[" + index + "]");
            }
        }       
    }
}
