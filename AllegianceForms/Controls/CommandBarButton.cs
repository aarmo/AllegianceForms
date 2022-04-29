using System.Windows.Forms;
using AllegianceForms.Forms;
using AllegianceForms.Engine;
using System.Drawing;
using System;

namespace AllegianceForms.Controls
{
    public partial class CommandBarButton : UserControl
    {
        private static readonly Color NormColour = Color.Black;
        private static readonly Color HoverColour = Color.DarkGreen;
        private static readonly Color ClickColour = Color.DarkGoldenrod;

        private CommandBar _bar;
        private Sector _game;
        private Keys _key;

        public CommandBarButton(Sector form, CommandBar bar, string text, Keys key)
        {
            InitializeComponent();

            _bar = bar;
            _game = form;
            _key = key;
            ButtonText.Text = text;
        }

        private void Name_MouseLeave(object sender, EventArgs e)
        {
            BackColor = NormColour;
        }

        private void Name_MouseEnter(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mouseover);
            BackColor = HoverColour;
        }

        private void ButtonText_MouseDown(object sender, MouseEventArgs e)
        {
            BackColor = ClickColour;
            _game.Sector_KeyDown(_bar, new KeyEventArgs(_key));
            _game.Focus();
        }

        private void ButtonText_MouseUp(object sender, MouseEventArgs e)
        {
            BackColor = HoverColour;
        }
    }
}
