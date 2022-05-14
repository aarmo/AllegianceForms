using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace AllegianceForms.Engine.Replay
{
    public class ReplayStructure
    {
        public GameSettings Settings { get; set; }
        public GameStats Stats { get; set; }
        public List<ReplayOrder> Orders { get; set; } = new List<ReplayOrder>();
        public bool GameComplete => Stats.GameComplete;

        private DateTime _lastOrderTime = DateTime.Now;
        public bool RecordingEnabled { get; set; }



        public ReplayStructure(GameSettings settings, GameStats stats)
        {
            Settings = settings;
            Stats = stats;
        }

        public void AddUnitOrder(EOrderType type, int team, int sector, int[] selectedUnitIds, Point mousePos, Keys key)
        {
            if (GameComplete || !RecordingEnabled) return;

            AddOrder(new ReplayOrder
            {
                OrderType = type,
                Team = team,
                SectorId = sector,
                MousePos = mousePos,
                Key = key,
                SelectedIds = selectedUnitIds
            });
        }

        public void AddAbilityOrder(int team, int sector, int[] selectedUnitIds, Keys key)
        {
            if (GameComplete || !RecordingEnabled) return;

            AddOrder(new ReplayOrder
            {
                OrderType = EOrderType.Ability,
                Team = team,
                SectorId = sector,
                Key = key,
                SelectedIds = selectedUnitIds
            });
        }

        public void AddQuickChatOrder(int team, int sector, string order)
        {
            if (GameComplete || !RecordingEnabled) return;

            AddOrder(new ReplayOrder
            {
                OrderType = EOrderType.QuickChat,
                Team = team,
                SectorId = sector,
                QuickChatOrder = order
            });
        }

        private void AddOrder(ReplayOrder order)
        {
            lock (Orders)
            { 
                order.DelayFromLastOrder = (DateTime.Now - _lastOrderTime).TotalMilliseconds;
                _lastOrderTime = DateTime.Now;
                Orders.Add(order);
            }
        }

        public static ReplayStructure LoadReplay(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var formatter = new BinaryFormatter();

                var s = (ReplayStructure)formatter.Deserialize(stream);
                stream.Close();

                return s;
            }
        }

        public static void SaveReplay(string filename, ReplayStructure s)
        {
            if (!s.GameComplete) return;

            using (var stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, s);
                stream.Close();
            }
        }
    }

    public class ReplayOrder
    {
        public double DelayFromLastOrder { get; set; }
        public int Team { get; set; }
        public int SectorId { get; set; }
        public Point MousePos { get; set; }
        public EOrderType OrderType { get; set; }
        public Keys Key { get; set; }
        public string QuickChatOrder { get; set; }
        public int[] SelectedIds { get; set; }
    }
}
