using AllegianceForms.Controls;
using AllegianceForms.Engine;
using AllegianceForms.Engine.Bases;
using AllegianceForms.Engine.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AllegianceForms.Forms
{
    public partial class CommandBar : Form
    {
        private const int AttackButton=0;
        private const int StopButton=1;
        private const int DockButton=2;
        private const int PatrolButton=3;
        private const int MineButton=4;
        private const int BuildButton=5;
        private const int CaptureButton=6;

        private const int ScoutButton=7;
        private const int FigButton=8;
        private const int BbrButton=9;
        private const int GsButton=10;
        private const int IntButton=11;
        private const int SfButton=12;
        private const int SbButton = 13;
        private const int FbButton=14;
        private const int TtButton=15;

        private const int NumButtons = 16;

        private StrategyGame _game;
        private CommandBarButton[] _buttons = new CommandBarButton[NumButtons];
        private bool[] _shownButtons = new bool[NumButtons];

        public CommandBar(StrategyGame game, Sector form)
        {
            InitializeComponent();

            _game = game;

            _buttons[AttackButton] = new CommandBarButton(form, this, "Attack\n[A]", Keys.A);
            _buttons[StopButton] = new CommandBarButton(form, this, "Stop\n[S]", Keys.S);
            _buttons[DockButton] = new CommandBarButton(form, this, "Dock\n[D]", Keys.D);
            _buttons[PatrolButton] = new CommandBarButton(form, this, "Patrol\n[R]", Keys.R);
            _buttons[MineButton] = new CommandBarButton(form, this, "Mine\n[E]", Keys.E);
            _buttons[BuildButton] = new CommandBarButton(form, this, "Build\n[B]", Keys.B);
            _buttons[CaptureButton] = new CommandBarButton(form, this, "Capture\n[C]", Keys.C);

            _buttons[ScoutButton] = new CommandBarButton(form, this, "Scout\n[S]", Keys.S);
            _buttons[FigButton] = new CommandBarButton(form, this, "Fighter\n[F]", Keys.F);
            _buttons[BbrButton] = new CommandBarButton(form, this, "Bomber\n[B]", Keys.B);
            _buttons[GsButton] = new CommandBarButton(form, this, "Gunship\n[G]", Keys.G);
            _buttons[IntButton] = new CommandBarButton(form, this, "Interceptor\n[I]", Keys.I);
            _buttons[SfButton] = new CommandBarButton(form, this, "Stealth Fighter\n[T]", Keys.T);
            _buttons[SbButton] = new CommandBarButton(form, this, "Stealth Bomber\n[O]", Keys.O);
            _buttons[FbButton] = new CommandBarButton(form, this, "Fighter Bomber\n[X]", Keys.X);
            _buttons[TtButton] = new CommandBarButton(form, this, "Troop Transport\n[P]", Keys.P);

            flowLayoutPanelButtons.Controls.Add(new CommandBarButton(form, this, "Research\n[F5]", Keys.F5));
            flowLayoutPanelButtons.Controls.Add(new CommandBarButton(form, this, "Pilots\n[F6]", Keys.F6));
        }

        public void RefreshCommandText(List<Ship> selectedShips, List<Base> selectedBases)
        {
            try { 
                lock (_game)
                {
                    if (selectedShips.Count > 0)
                    {
                        ClearBaseButtons();

                        AddButtonIfNotShown(AttackButton);
                        AddButtonIfNotShown(StopButton);
                        AddButtonIfNotShown(DockButton);
                        AddButtonIfNotShown(PatrolButton);
                
                        if (selectedShips.Any(_ => _.Type == EShipType.Miner))
                        {
                            AddButtonIfNotShown(MineButton);
                        }

                        if (selectedShips.Any(_ => _.Type == EShipType.Constructor))
                        {
                            AddButtonIfNotShown(BuildButton);
                        }

                        if (selectedShips.Any(_ => _.Type == EShipType.TroopTransport))
                        {
                            AddButtonIfNotShown(CaptureButton);
                        }
                    }
                    else if (selectedBases.Any(_ => _.CanLaunchShips()))
                    {
                        ClearShipButtons();

                        AddButtonIfNotShown(ScoutButton);
                        AddButtonIfNotShown(FigButton);

                        if (_game.TechTree[0].HasResearchedShipType(EShipType.Bomber)) AddButtonIfNotShown(BbrButton);
                        if (_game.TechTree[0].HasResearchedShipType(EShipType.Gunship)) AddButtonIfNotShown(GsButton);
                        if (_game.TechTree[0].HasResearchedShipType(EShipType.Interceptor)) AddButtonIfNotShown(IntButton);
                        if (_game.TechTree[0].HasResearchedShipType(EShipType.StealthFighter)) AddButtonIfNotShown(SfButton);
                        if (_game.TechTree[0].HasResearchedShipType(EShipType.StealthBomber)) AddButtonIfNotShown(SbButton);
                        if (_game.TechTree[0].HasResearchedShipType(EShipType.FighterBomber)) AddButtonIfNotShown(FbButton);
                        if (_game.TechTree[0].HasResearchedShipType(EShipType.TroopTransport)) AddButtonIfNotShown(TtButton);
                    }
                    else 
                    {
                        ClearAllButtons();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.Log.Error(ex);
            }
        }

        private void RemoveButtonIfShown(int buttonId)
        {
            if (!_shownButtons[buttonId]) return;
            
            flowLayoutPanelOrders.Controls.Remove(_buttons[buttonId]);
            _shownButtons[buttonId] = false;
        }
        private void AddButtonIfNotShown(int buttonId)
        {
            if (_shownButtons[buttonId]) return;

            flowLayoutPanelOrders.Controls.Add(_buttons[buttonId]);            
            _shownButtons[buttonId] = true;
        }

        private void ClearShipButtons()
        {
            RemoveButtonIfShown(AttackButton);
            RemoveButtonIfShown(StopButton);
            RemoveButtonIfShown(DockButton);
            RemoveButtonIfShown(PatrolButton);
            RemoveButtonIfShown(MineButton);
            RemoveButtonIfShown(BuildButton);
            RemoveButtonIfShown(CaptureButton);
        }

        private void ClearBaseButtons()
        {
            RemoveButtonIfShown(ScoutButton);
            RemoveButtonIfShown(FigButton);
            RemoveButtonIfShown(BbrButton);
            RemoveButtonIfShown(GsButton);
            RemoveButtonIfShown(IntButton);
            RemoveButtonIfShown(SfButton);
            RemoveButtonIfShown(SbButton);
            RemoveButtonIfShown(FbButton);
            RemoveButtonIfShown(TtButton);
        }

        private void ClearAllButtons()
        {
            ClearBaseButtons();
            ClearShipButtons();
        }

        internal void SetCreditsLabel(int v)
        {
            ResourcesLabel.Text = v.ToString();
        }

        internal void SetPilotsLabel(int v)
        {
            PilotsLabel.Text = v.ToString();
        }
    }
}
