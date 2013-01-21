using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

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
            public float pan;
            public SoundProp(float volume, float pitch, float pan) { this.volume = volume; this.pitch = pitch; this.pan = pan; }
        }

        static Dictionary<Sounds, SoundEffect> SoundDic = new Dictionary<Sounds, SoundEffect>();
        static Dictionary<Sounds, SoundProp> SoundPropDic = new Dictionary<Sounds, SoundProp>();
        static Dictionary<LoopSounds, SoundEffectInstance> LoopDic = new Dictionary<LoopSounds, SoundEffectInstance>();

        static public void Initialize()
        {
            //SoundDic[Sounds.Alarm] = Game1.game.Content.Load<SoundEffect>("");
            SoundDic[Sounds.BigExplosion] = Game1.game.Content.Load<SoundEffect>("Sounds/104439__dkmedic__bomb");
            SoundDic[Sounds.Laser] = Game1.game.Content.Load<SoundEffect>("Sounds/77087__supraliminal__laser-short");
            SoundDic[Sounds.PlayerExplosion] = Game1.game.Content.Load<SoundEffect>("Sounds/80499__ggctuk__exp-obj-large02");
            SoundDic[Sounds.PlayerHit] = Game1.game.Content.Load<SoundEffect>("Sounds/78457__sancho82__bum");
            SoundDic[Sounds.SmallExplosion] = Game1.game.Content.Load<SoundEffect>("Sounds/78457__sancho82__bum");

            SoundPropDic[Sounds.Laser] = new SoundProp(0.8f, 0.4f, 0f);

            //LoopDic[LoopSounds.Alarm] = SoundDic[Sounds.Alarm].CreateInstance();
            //LoopDic[LoopSounds.Alarm].IsLooped = true;
        }

        static public void PlaySound(Sounds sound)
        {
            if (SoundPropDic.ContainsKey(sound))
                SoundDic[sound].Play(SoundPropDic[sound].volume * ((float)Game1.Config.soundVol / 10f), SoundPropDic[sound].pitch, SoundPropDic[sound].pan);
            else
                SoundDic[sound].Play((float)Game1.Config.soundVol / 10f, 0f, 0f);
        }

        static public void PlayLoopSound(LoopSounds sound)
        {
            LoopDic[sound].Play();
        }

        static public void StopLoopSound(LoopSounds sound)
        {
            LoopDic[sound].Stop();
        }

        static public void PlayMusic(Music music)
        {
            
        }
    }
}
