using IrrKlang;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace AllegianceForms.Engine
{
    public class SoundEffect
    {
        private static ISoundEngine _engine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.MultiThreaded);
        private static ISoundEngine _importantEngine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.MultiThreaded);

        private const int SoundCountDelayMS = 400;
        private const int MaxSoundCount = 2;

        private static float _volume = 0.5f;
        private static int _soundCount = 0;
        private static DateTime NextSoundCount = DateTime.MinValue;

        public static void Init()
        {
            _volume = Convert.ToSingle(ConfigurationManager.AppSettings["SoundEffects.Volume"]);

            _engine.SoundVolume = _volume;
            _importantEngine.SoundVolume = _volume;
        }

        public static void Play(ESounds sound, bool important = false)
        {
            if (NextSoundCount > DateTime.Now)
            {
                if (_soundCount > MaxSoundCount && !important) return;
                _soundCount++;
            }
            else
            {
                _soundCount = 1;
                NextSoundCount = DateTime.Now.AddMilliseconds(SoundCountDelayMS);
            }

            var path = GetSoundFile(sound);
            Task.Run(() => PlayCatch(path, important));
        }

        public static string GetSoundFile(ESounds sound)
        {
            return StrategyGame.SoundsDir + sound.ToString() + ".ogg";
        }

        private static void PlayCatch(string path, bool important)
        {
            if (important)
            {
                try
                {
                    _importantEngine.Play2D(path);
                }
                catch
                {
                    _importantEngine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.MultiThreaded);
                    _importantEngine.SoundVolume = _volume;
                }
            }
            else
            {
                try
                {
                    _engine.Play2D(path);
                }
                catch
                {
                    _engine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.MultiThreaded);
                    _engine.SoundVolume = _volume;
                }
            }

        }
    }
}
