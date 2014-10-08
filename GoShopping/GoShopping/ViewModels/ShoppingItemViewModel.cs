using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GoShopping.ViewModels
{
    public class ShoppingItemViewModel : INotifyPropertyChanged
    {
        private string _text;

        private bool _completed;

        private Color _color = Colors.Red;
        private Color _prevColor = Colors.Green;

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }

        public bool Completed
        {
            get { return _completed; }
            set
            {
                _completed = value;
                OnPropertyChanged("Completed");
            }
        }

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged("Color");
            }
        }

        public Color PrevColor
        {
            get { return _prevColor; }
            set
            {
                _prevColor = value;
                OnPropertyChanged("PrevColor");
            }
        }

        public ShoppingItemViewModel(string text)
        {
            Text = text;
        }

        public ShoppingItemViewModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
