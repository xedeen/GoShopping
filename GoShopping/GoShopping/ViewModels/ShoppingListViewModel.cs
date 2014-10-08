using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization;
using SystemColor = System.Windows.Media.Color;

namespace GoShopping.ViewModels
{
    public class ShoppingListViewModel
    {
        private ResettableObservableCollection<ShoppingItemViewModel> _todoItems =
            new ResettableObservableCollection<ShoppingItemViewModel>();

        public ShoppingListViewModel()
        {
            _todoItems.CollectionChanged += (s, e) => UpdateToDoColors();
        }

        public ResettableObservableCollection<ShoppingItemViewModel> Items
        {
            get { return _todoItems; }
        }

        private void UpdateToDoColors()
        {
            double itemCount = _todoItems.Count;
            double index = 0;
            foreach (var todoItem in _todoItems)
            {
                double val = (index/itemCount)*155.0;
                index++;

                if (!todoItem.Completed)
                {
                    todoItem.Color = SystemColor.FromArgb(255, 255, (byte) val, 0);
                }
            }
        }

        public static ShoppingListViewModel CreateOrLoadFromStorage()
        {
            ShoppingListViewModel model=new ShoppingListViewModel();
            try
            {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.FileExists("current_list.lst"))
                    model = model.Load<ShoppingListViewModel>(store, "current_list.lst");
            }
            }
            catch 
            {
            }
            return model;
        }

        public static void SaveAll(ShoppingListViewModel model)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                Save<ShoppingListViewModel>(store, "current_list.lst", model);
            }
        }

        public static void Save<T>(IsolatedStorageFile store, string fileName, T item)
        {
            using (var fileStream = new IsolatedStorageFileStream(fileName, FileMode.Create, FileAccess.Write,
                FileShare.None, store))
            {
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(fileStream, item);
            }
        }


        public T Load<T>(IsolatedStorageFile store, string fileName)
        {
            using (var fileStream = new IsolatedStorageFileStream(fileName, FileMode.Open, FileAccess.Read,
                FileShare.None, store))
            {
                var serializer = new DataContractSerializer(typeof(T));
                return (T)serializer.ReadObject(fileStream);
            }
        }

    }
}
