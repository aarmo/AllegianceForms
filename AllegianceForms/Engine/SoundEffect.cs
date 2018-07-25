using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Media;

namespace AllegianceForms.Engine
{
    public class SoundEffect
    {
        private const int MaxSoundCount = 1;

        private static List<SoundPlayer> _available;

        public static void Init()
        {
            //var volume = Convert.ToSingle(ConfigurationManager.AppSettings["SoundEffects.Volume"]);
            _available = new List<SoundPlayer>();

            for (var i = 0; i < MaxSoundCount; i++)
            {
                var m = new SoundPlayer();
                _available.Add(m);
            }
        }

        public static void Play(ESounds sound, bool important = false)
        {
            lock (_available)
            {
                if (_available.Count < 1) return;
            }

            var path = GetSoundFile(sound);
            Task.Run(() => PlayCatch(path));
        }

        public static string GetSoundFile(ESounds sound)
        {
            return StrategyGame.SoundsDir + sound + ".wav";
        }

        private static void PlayCatch(string path)
        {
            SoundPlayer s = null;
            try
            {
                lock (_available)
                {
                    if (_available.Count < 1) return;

                    s = _available[0];
                    _available.RemoveAt(0);
                }

                s.SoundLocation = path;
                s.PlaySync();
            }
            finally 
            {
                lock (_available)
                {
                    if (s != null) _available.Add(s);
                }
                
            }
        }
    }
}
