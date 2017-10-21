using System.Windows.Forms;

namespace AllegianceForms.Controls
{
    public partial class QuickChatItem : UserControl
    {
        public Engine.QuickChat.QuickChatItem Item { get; set; }

        public QuickChatItem(Engine.QuickChat.QuickChatItem item)
        {
            InitializeComponent();
            Item = item;

            if (Item == null) return;

            CommandText.Text = Item.QuickComms;
            Key.Text = Item.Key;
            OpenMenu.Visible = Item.OpenMenuId != string.Empty;
        }
    }
}
