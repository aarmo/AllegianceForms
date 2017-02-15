using IrrKlang;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllegianceForms.Engine
{
    public class SoundEffect
    {
        private static ISoundEngine _engine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.MultiThreaded);
        private static ISoundEngine _importantEngine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.MultiThreaded);

        private const int SoundCountDelayMS = 400;
        private const int MaxSoundCount = 2;
        private static int _soundCount = 0;
        private static DateTime NextSoundCount = DateTime.MinValue;
        private static Queue<ESounds> SoundEfectQueue = new Queue<ESounds>();

        public static void Init(float volume)
        {
            _engine.SoundVolume = volume;
            _importantEngine.SoundVolume = volume;
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

            Task.Run(() => PlayCatch(sound, important));
        }

        private static void PlayCatch(ESounds sound, bool important)
        {
            try
            {
                var path = ".//Art//Sounds//" + sound.ToString() + ".ogg";
                if (important)
                {
                    _importantEngine.Play2D(path);
                }
                else
                {
                    _engine.Play2D(path);
                }
            }
            catch {}
        }
    }
}
