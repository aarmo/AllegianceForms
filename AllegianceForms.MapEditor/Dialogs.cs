using System.Windows.Forms;

namespace AllegianceForms.MapEditor
{
    public class Dialogs
    {
        public static string OpenFile(string title, string filter)
        {
            var dialog = new OpenFileDialog { Title = title, Filter = filter};

            if (dialog.ShowDialog() != DialogResult.OK) return string.Empty;

            return dialog.FileName;
        }

        public static string SaveFile(string title, string filter)
        {
            var dialog = new SaveFileDialog { Title = title, Filter = filter};

            if (dialog.ShowDialog() != DialogResult.OK) return string.Empty;

            return dialog.FileName;
        }
    }
}