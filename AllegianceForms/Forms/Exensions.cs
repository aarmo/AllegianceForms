using System.Windows.Forms;

namespace AllegianceForms.Forms
{
    public static class Extensions
    {
        public static void ShowCentered(this Form f, Form owner)
        {
            f.Show(owner);

            f.Top = owner.Top + owner.Height / 2 - f.Height / 2;
            f.Left = owner.Left + owner.Width / 2 - f.Width / 2;
        }
    }
}
