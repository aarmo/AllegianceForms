using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using System.IO;
using System.Configuration;

namespace AllegianceForms.Engine
{
    public class SoundEffect
    {
        // We use multiple devices so we can play the same voice at once a few times
        private const int NumAudioDevices = 4;
        private const int MinMsDelayBetweenAudio = 240;

        private static float _baseVolume;

        private static XAudio2[] _audioDevices;
        private static MasteringVoice[] _masteringVoices;

        private static Dictionary<string, SoundEffectVoice[]> _soundEffectPool;
        private static Dictionary<string, int> _lastSoundEffectIndex;
        private static Dictionary<string, DateTime> _lastSoundEffectTime;

        public static void Init()
        {
            _audioDevices = new XAudio2[NumAudioDevices];
            _masteringVoices = new MasteringVoice[NumAudioDevices];

            for (var i = 0; i < NumAudioDevices; i++)
            {
                _audioDevices[i] = new XAudio2();
                _masteringVoices[i] = new MasteringVoice(_audioDevices[i]);
            }

            _soundEffectPool = new Dictionary<string, SoundEffectVoice[]>();
            _lastSoundEffectIndex = new Dictionary<string, int>();
            _lastSoundEffectTime = new Dictionary<string, DateTime>();

            _baseVolume = float.Parse(ConfigurationManager.AppSettings["SoundEffects.Volume"]);
        }

        public static void Dispose()
        {
            if (_soundEffectPool != null)
            {
                foreach (var k in _soundEffectPool.Keys)
                {
                    for (var i = 0; i < NumAudioDevices; i++)
                    { 
                        _soundEffectPool[k][i].Dispose();
                    }
                }
            }

            for (var i = 0; i < NumAudioDevices; i++)
            {
                if (_masteringVoices != null) _masteringVoices[i].Dispose();
                if (_audioDevices != null) _audioDevices[i].Dispose();
            }
        }

        public static string GetSoundFile(ESounds sound)
        {
            return StrategyGame.SoundsDir + sound + ".wav";
        }

        public static void Play(ESounds sound, bool important = false)
        {
            var path = GetSoundFile(sound);
            Task.Run(() => PlayInBackground(path, important));
        }

        private static void PlayInBackground(string path, bool important)
        {
            try
            {
                PlaySoundEffectVoice(path, important);
            }
            catch (Exception ex)
            {
                Program.Log.Error("Sound Effect Error", ex);
            }
        }

        /// <summary>
        /// Start playing a sound in a pool of SharpDX voices.
        /// Supported format are Wav(pcm+adpcm) and XWMA
        /// </summary>
        private static void PlaySoundEffectVoice(string fileName, bool louder)
        {
            lock (_soundEffectPool)
            { 
                if (!_soundEffectPool.ContainsKey(fileName))
                {
                    var voices = new SoundEffectVoice[NumAudioDevices];

                    for (var i = 0; i < NumAudioDevices; i++)
                    {
                        voices[i] = new SoundEffectVoice(_audioDevices[i], fileName, _baseVolume);
                    };

                    _soundEffectPool.Add(fileName, voices);
                    _lastSoundEffectIndex.Add(fileName, 0);
                    _lastSoundEffectTime.Add(fileName, DateTime.MinValue);
                }

                // Prevent spamming of the same voice too often
                if (_lastSoundEffectTime[fileName].AddMilliseconds(MinMsDelayBetweenAudio) > DateTime.Now)
                    return;

                // Walk through the pool of voices for this effect
                _lastSoundEffectIndex[fileName]++;
                if (_lastSoundEffectIndex[fileName] > NumAudioDevices - 1)
                {
                    _lastSoundEffectIndex[fileName] = 0;
                }

                _lastSoundEffectTime[fileName] = DateTime.Now;
                _soundEffectPool[fileName][_lastSoundEffectIndex[fileName]].Start();
            }
        }
    }

    /// <summary>
    /// Maintain a SharpDX Voice and it's Audio Buffer for use in our pool
    /// </summary>
    public class SoundEffectVoice : IDisposable
    {
        private XAudio2 _audioDevice;                
        private AudioBuffer _audioBuffer;
        private SourceVoice _voice;
        private SoundStream _stream;

        private float _baseVolume;
        private bool _resetAfterEnd = false;

        public SoundEffectVoice(XAudio2 device, string soundFileName, float volume)
        {
            _audioDevice = device;
            _baseVolume = volume;

            // TODO: This could be optimised: cache & copy the audio file bytes?
            _stream = new SoundStream(File.OpenRead(soundFileName));
            var format = _stream.Format;
            _audioBuffer = new AudioBuffer
            {
                Stream = _stream.ToDataStream(),
                AudioBytes = (int)_stream.Length,
                Flags = BufferFlags.EndOfStream,                
            };
            _stream.Close();

            _voice = new SourceVoice(_audioDevice, format, true);
            _voice.BufferEnd += VoiceBufferEnd;
        }

        public void Dispose()
        {
            if (_voice != null && !_voice.IsDisposed)
            {
                _voice.DestroyVoice();
                _voice.Dispose();
            }

            if (_stream != null)
            {
                _stream.Dispose();
            }
        }

        private void VoiceBufferEnd(IntPtr obj)
        {
            if (_resetAfterEnd)
            {
                _resetAfterEnd = false;
                _voice.SubmitSourceBuffer(_audioBuffer, _stream.DecodedPacketsInfo);
                _voice.Start();
            }
        }

        public bool Queued => _voice.State.BuffersQueued > 0;

        public void Start(bool louder = true)
        {
            if (_voice == null || _voice.IsDisposed) return;

            if (Queued)
            {
                // Gracefully restart this voice
                _voice.FlushSourceBuffers();
                _resetAfterEnd = true;
                return;
            }

            // Emphasis the important voices to rise above the chaos!
            _voice.SetVolume(_baseVolume * (louder ? 2f : 1f));
            _voice.SubmitSourceBuffer(_audioBuffer, _stream.DecodedPacketsInfo);
            _voice.Start();
        }

        public void Stop()
        {
            if (_voice == null || _voice.IsDisposed) return;

            _voice.FlushSourceBuffers();
        }
    }
}
