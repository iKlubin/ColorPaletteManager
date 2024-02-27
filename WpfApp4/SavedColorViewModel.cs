using System.Windows.Media;

namespace WpfApp4
{
    public class SavedColorViewModel
    {
        public Color Color { get; set; }

        public string ColorString => Color.ToString();

        public SavedColorViewModel(Color color)
        {
            Color = color;
        }
    }
}
