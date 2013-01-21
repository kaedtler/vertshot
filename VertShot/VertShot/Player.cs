using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VertShot
{
    public enum ShipWeapons
    {
        None,
        LaserLvl1,
        LaserLvl2
    }

    public class Player
    {
        const float MaxEnergy = 100f;
        const float MaxShield = 100f;
        public Texture2D texture;
        Vector2 startPosition;
        Vector2 position;
        Vector2 size;
        public Rectangle rect { get { return new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), Convert.ToInt32(size.X), Convert.ToInt32(size.Y)); } }
        public float energy { get; private set; }
        public float shield { get; private set; }
        public float collisionDamage { get; private set; }
        float speed;
        float shieldPerSecond = 3f;

        public ShipWeapons[] weaponSlot = new ShipWeapons[3];
        public GameKeys[] weaponSlotKey = new GameKeys[3];
        public float[] weaponDelay = new float[3];
        public float[] weaponDelayTime = new float[3];
        public Vector2[] weaponSlotPosition;

        public Player(Texture2D texture, Vector2 startPosition)
        {
            this.texture = texture;
            this.startPosition = startPosition;
            size = new Vector2(texture.Width, texture.Height);
            speed = 0.65f;
            collisionDamage = 20f;
            weaponSlotPosition = new Vector2[3] { new Vector2(size.X * 0.1f, size.Y / 2), new Vector2(size.X * 0.5f, size.Y / 2), new Vector2(size.X * 0.9f, size.Y / 2) };

            weaponSlot[0] = ShipWeapons.LaserLvl1;
            weaponSlotKey[0] = GameKeys.Fire2;
            weaponDelay[0] = 200;
             
            weaponSlot[1] = ShipWeapons.LaserLvl1;
            weaponSlotKey[1] = GameKeys.Fire1;
            weaponDelay[1] = 200;

            weaponSlot[2] = ShipWeapons.LaserLvl1;
            weaponSlotKey[2] = GameKeys.Fire2;
            weaponDelay[2] = 200;

            Reset();
        }

        public void Reset()
        {
            position = startPosition;
            energy = MaxEnergy;
            shield = MaxShield;
        }


        public void AddDamage(float damage, ShotType shotType)
        {
            if (shield > 0)
                damage = AddShieldDamage(damage, shotType);
            if (damage > 0)
            {
                float factor = 1;
                switch (shotType)
                {
                    case ShotType.Laser: factor = 1; break;
                    case ShotType.Explosive: factor = 1; break;
                    case ShotType.Collision: factor = 1.5f; break;
                }
                energy -= damage * factor;
            }
            Sound.PlaySound(Sound.Sounds.PlayerHit);
        }

        private float AddShieldDamage(float damage, ShotType shotType)
        {
            float shieldOld = shield;
            float factor = 1f;
            switch (shotType)
            {
                case ShotType.Laser: factor = 2f; break;
                case ShotType.Explosive: factor = 2f; break;
                case ShotType.Collision: factor = 2.5f; break;
            }
            shield = Math.Max(shield - damage * factor, 0);
            return ((shieldOld - (damage * factor)) - shield) / -2f;
        }

        public void AddEnergy(float addEnergy)
        {
            energy = Math.Min(energy + addEnergy, MaxEnergy);
        }

        public void AddShield(float addShield)
        {
            shield = Math.Min(shield + addShield, MaxShield);
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public void Update(GameTime gameTime)
        {

            position += Input.InputVector * new Vector2((float)gameTime.ElapsedGameTime.TotalMilliseconds * speed, (float)gameTime.ElapsedGameTime.TotalMilliseconds * speed);
            position.X = MathHelper.Clamp(position.X, 0, Game1.Width - size.X);
            position.Y = MathHelper.Clamp(position.Y, 0, Game1.Height - size.Y);

            for (int i = 0; i < weaponSlot.Length; i++)
            {
                weaponDelayTime[i] = Math.Max(weaponDelayTime[i] - (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0);
                if (weaponSlot[i] != ShipWeapons.None && Input.IsGameKeyDown(weaponSlotKey[i]) && weaponDelayTime[i] == 0)
                {
                    switch (weaponSlot[i])
                    {
                        case ShipWeapons.LaserLvl1:
                            ShotCollector.AddLaserShot(position + weaponSlotPosition[i] - new Vector2(2.5f,0), new Vector2(5, 20), new Vector2(0, -1), 0, 1);
                            weaponDelayTime[i] += weaponDelay[i];
                            break;
                    }
                }
            }


            if (shield < MaxShield && weaponDelayTime.Count(i => i == 0) == weaponDelayTime.Length)
                shield = Math.Min(shield + shieldPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds, MaxShield);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, new Color(Game1.Config.shipColorR, Game1.Config.shipColorG, Game1.Config.shipColorB));
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, position, new Color(Game1.Config.shipColorR, Game1.Config.shipColorG, Game1.Config.shipColorB));
        }
    }
}
