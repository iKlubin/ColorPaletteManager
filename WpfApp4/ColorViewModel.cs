using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApp4
{
    public class ColorViewModel : INotifyPropertyChanged
    {
        private bool _alphaChecked = true;
        private bool _redChecked = true;
        private bool _greenChecked = true;
        private bool _blueChecked = true;
        private double _alphaValue = 10;
        private double _redValue;
        private double _greenValue;
        private double _blueValue;
        private Color _currentColor;
        private ICommand _addCommand;
        private ICommand _deleteCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool AlphaChecked
        {
            get { return _alphaChecked; }
            set { _alphaChecked = value; OnPropertyChanged(); }
        }

        public bool RedChecked
        {
            get { return _redChecked; }
            set { _redChecked = value; OnPropertyChanged(); }
        }

        public bool GreenChecked
        {
            get { return _greenChecked; }
            set { _greenChecked = value; OnPropertyChanged(); }
        }

        public bool BlueChecked
        {
            get { return _blueChecked; }
            set { _blueChecked = value; OnPropertyChanged(); }
        }

        public double AlphaValue
        {
            get { return _alphaValue; }
            set { _alphaValue = value; OnPropertyChanged(); UpdateColor(); }
        }

        public double RedValue
        {
            get { return _redValue; }
            set { _redValue = value; OnPropertyChanged(); UpdateColor(); }
        }

        public double GreenValue
        {
            get { return _greenValue; }
            set { _greenValue = value; OnPropertyChanged(); UpdateColor(); }
        }

        public double BlueValue
        {
            get { return _blueValue; }
            set { _blueValue = value; OnPropertyChanged(); UpdateColor(); }
        }

        public Color CurrentColor
        {
            get { return _currentColor; }
            set { _currentColor = value; OnPropertyChanged(); }
        }

        // Логика добавления цвета в список
        private void AddColor()
        {
            var newColor = new SavedColorViewModel(CurrentColor);
            if (!_savedColors.Any(c => c.ColorString == newColor.ColorString))
            {
                SavedColors.Add(newColor);
            }
        }

        // Логика удаления цвета из списка
        private void DeleteColor(SavedColorViewModel color)
        {
            SavedColors.Remove(color);
        }

        // Логика обновления текущего цвета
        private void UpdateColor()
        {
            byte alpha = (byte)(AlphaChecked ? (AlphaValue / 10) * 255 : 255);
            byte red = (byte)(RedChecked ? (RedValue / 10) * 255 : 0);
            byte green = (byte)(GreenChecked ? (GreenValue / 10) * 255 : 0);
            byte blue = (byte)(BlueChecked ? (BlueValue / 10) * 255 : 0);

            CurrentColor = Color.FromArgb(alpha, red, green, blue);
        }

        // RelayCommand для вызова метода AddColor
        public ICommand AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new DelegateCommand(_ => AddColor(), _ => CanAddColor()));
            }
        }

        // RelayCommand для вызова метода DeleteColor
        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand = new DelegateCommand(obj => DeleteColor(obj as SavedColorViewModel)));
            }
        }

        // Проверка, можно ли добавить текущий цвет
        private bool CanAddColor()
        {
            return !_savedColors.Any(c => c.ColorString == CurrentColor.ToString());
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<SavedColorViewModel> _savedColors = new ObservableCollection<SavedColorViewModel>();

        public ObservableCollection<SavedColorViewModel> SavedColors
        {
            get { return _savedColors; }
            set { _savedColors = value; OnPropertyChanged(); }
        }

        public ColorViewModel()
        {
            UpdateColor();
        }
    }
}
