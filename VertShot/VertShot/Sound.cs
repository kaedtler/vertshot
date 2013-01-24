using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace VertShot
{
    static public class Sound
    {
        public enum Sounds
        {
            Laser,
            SmallExplosion,
            BigExplosion,
            PlayerExplosion,
            ItemCollect,
            Alarm
        }

        public enum LoopSounds
        {
            Alarm
        }

        public enum Music
        {
            Menu,
            Game
        }

        public struct SoundProp
        {
            public float volume;
            public float pitch;
            public SoundProp(float volume, float pitch) { this.volume = volume; this.pitch = pitch; }
        }

        static Dictionary<Sounds, SoundEffect> SoundDic = new Dictionary<Sounds, SoundEffect>();
        static Dictionary<Sounds, SoundProp> SoundPropDic = new Dictionary<Sounds, SoundProp>();
        static Dictionary<LoopSounds, SoundEffectInstance> LoopDic = new Dictionary<LoopSounds, SoundEffectInstance>();

        static Dictionary<Music, Song> MusicDic = new Dictionary<Music, Song>();

        static public bool audioEnabled { get; private set; }


        // Audio objects
        static AudioEngine engine;
        static SoundBank effectSoundBank;
        static SoundBank musicSoundBank;
        static WaveBank effectWaveBank;
        static WaveBank musicWaveBank;

        static AudioListener audioListener;

        static public void Initialize()
        {
            audioEnabled = true;
            try
            {
                engine = new AudioEngine("Content/Xact/Xact.xgs");
            }
            catch (InvalidOperationException)
            {
                audioEnabled = false;
            }

            if (audioEnabled)
            {

                effectWaveBank = new WaveBank(engine, "Content/Xact/EffectWavebank.xwb");
                musicWaveBank = new WaveBank(engine, "Content/Xact/MusicWavebank.xwb", 0, 16);
                engine.Update();
                effectSoundBank = new SoundBank(engine, "Content/Xact/EffectSoundbank.xsb");
                musicSoundBank = new SoundBank(engine, "Content/Xact/MusicSoundbank.xsb");

                audioListener = new AudioListener();

                SetSoundVolume();
                SetMusicVolume();
                engine.Update();
            }
        }

        static public void Update()
        {
            if (audioEnabled)
            {
                if (Game1.Config.sound3d) audioListener.Position = new Vector3(Game1.player.rect.X, 0, Game1.player.rect.Y);
                engine.Update();
            }
        }

        static public void PlaySound(Sounds sound, Vector2 position)
        {
            if (audioEnabled)
                if (Game1.Config.sound3d)
                {
                    audioListener.Position = new Vector3(Game1.player.rect.X, 0, Game1.player.rect.Y);
                    AudioEmitter audioEmitter = new AudioEmitter();
                    audioEmitter.Position = new Vector3(position.X, position.Y, 0);
                    effectSoundBank.PlayCue(sound.ToString(), audioListener, audioEmitter);
                }
                else
                    effectSoundBank.PlayCue(sound.ToString());
        }

        static public void PlaySound(Sounds sound)
        {
            if (audioEnabled)
                effectSoundBank.PlayCue(sound.ToString());
        }

        static public void PlayMusic(Music music)
        {
            if (audioEnabled)
                musicSoundBank.PlayCue(music.ToString());
        }

        static public void PauseMusic()
        {
            if (audioEnabled)
                engine.GetCategory("Music").Pause();
        }

        static public void ResumeMusic()
        {
            if (audioEnabled)
                engine.GetCategory("Music").Resume();
        }

        static public void SetMusicVolume()
        {
            if (audioEnabled)
                engine.GetCategory("Music").SetVolume((float)Game1.Config.musicVol / 10f);
        }

        static public void SetSoundVolume()
        {
            if (audioEnabled)
                engine.GetCategory("Default").SetVolume((float)Game1.Config.soundVol / 10f);
        }
    }
}
