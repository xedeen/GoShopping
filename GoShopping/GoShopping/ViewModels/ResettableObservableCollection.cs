using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoShopping.ViewModels
{
    public class ResettableObservableCollection<T> : ObservableCollection<T>
    {
        public void Reset()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(
              NotifyCollectionChangedAction.Reset));
        }
    }
}
