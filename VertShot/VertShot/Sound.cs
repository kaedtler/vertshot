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
            PlayerHit,
            PlayerExplosion,
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

        static public void Initialize()
        {
            audioEnabled = true;
            try
            {
                SetSoundVolume();
                SetMusicVolume();
            }
            catch (NoAudioHardwareException)
            {
                audioEnabled = false;
            }

            if (audioEnabled)
            {
                //SoundDic[Sounds.Alarm] = Game1.game.Content.Load<SoundEffect>("");
                SoundDic[Sounds.BigExplosion] = Game1.game.Content.Load<SoundEffect>("Sounds/104439__dkmedic__bomb");
                SoundDic[Sounds.Laser] = Game1.game.Content.Load<SoundEffect>("Sounds/77087__supraliminal__laser-short");
                SoundDic[Sounds.PlayerExplosion] = Game1.game.Content.Load<SoundEffect>("Sounds/80499__ggctuk__exp-obj-large02");
                SoundDic[Sounds.PlayerHit] = Game1.game.Content.Load<SoundEffect>("Sounds/78457__sancho82__bum");
                SoundDic[Sounds.SmallExplosion] = Game1.game.Content.Load<SoundEffect>("Sounds/78457__sancho82__bum");

                SoundPropDic[Sounds.Laser] = new SoundProp(0.8f, 0.4f);

                //LoopDic[LoopSounds.Alarm] = SoundDic[Sounds.Alarm].CreateInstance();
                //LoopDic[LoopSounds.Alarm].IsLooped = true;

                MusicDic[Music.Game] = Game1.game.Content.Load<Song>("Music/on-the-run-2_loop");
                MusicDic[Music.Menu] = Game1.game.Content.Load<Song>("Music/glow-in-the-dark");

                MediaPlayer.IsRepeating = true;
            }
        }

        static public void PlaySound(Sounds sound)
        {
            if (audioEnabled)
                Play(sound, 0f);
        }

        static void Play(Sounds sound, float pan)
        {
            if (audioEnabled)
                if (SoundPropDic.ContainsKey(sound))
                    SoundDic[sound].Play(SoundPropDic[sound].volume, SoundPropDic[sound].pitch, pan);
                else
                    SoundDic[sound].Play(1f, 0f, pan);
        }

        static public void PlaySound(Sounds sound, float xPosition)
        {
            if (audioEnabled)
                Play(sound, (xPosition - (float)Game1.Width / 2f) / (float)Game1.Width / 2f);
        }

        static public void PlayLoopSound(LoopSounds sound)
        {
            if (audioEnabled)
                LoopDic[sound].Play();
        }

        static public void StopLoopSound(LoopSounds sound)
        {
            if (audioEnabled)
                LoopDic[sound].Stop();
        }

        static public void PlayMusic(Music music)
        {
            if (audioEnabled)
                MediaPlayer.Play(MusicDic[music]);
        }

        static public void PauseMusic()
        {
            if (audioEnabled)
                MediaPlayer.Pause();
        }

        static public void ResumeMusic()
        {
            if (audioEnabled)
                MediaPlayer.Resume();
        }

        static public void SetMusicVolume()
        {
            if (audioEnabled)
                MediaPlayer.Volume = (float)Game1.Config.musicVol / 10f;
        }

        static public void SetSoundVolume()
        {
            if (audioEnabled)
                SoundEffect.MasterVolume = (float)Game1.Config.soundVol / 10f;
        }
    }
}
