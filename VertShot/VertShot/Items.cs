using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VertShot
{
    static public class Items
    {
        public enum ItemTypes
        {
            Health25
        }


        struct Item
        {
            public ItemTypes type;
            public AnimatedSprite aSprite;
        }

        static Dictionary<ItemTypes, Texture2D> itemTextures = new Dictionary<ItemTypes,Texture2D>();

        static List<Item> itemList = new List<Item>();

        static public void Initialize()
        {
            itemTextures.Add(ItemTypes.Health25, Game1.game.Content.Load<Texture2D>("Graphics/health"));
        }

        static public void AddItem(ItemTypes itemType, Vector2 position, bool centerPos = false)
        {
            Item item = new Item();
            item.type = itemType;
            if (centerPos)
                position = position + new Vector2(itemTextures[itemType].Width / 2, itemTextures[itemType].Height / 2);

            switch (itemType)
            {
                case ItemTypes.Health25:
                    item.aSprite = new AnimatedSprite(itemTextures[itemType], position, new Point(48, 48), 70, 7, true, 0, 1, centerPos, 0.2f, new Vector2(0, 1)); break;
            }
            itemList.Add(item);
        }

        static public void Update(GameTime gameTime)
        {
            for (int i = itemList.Count - 1; i >= 0; i--)
            {
                if (Game1.player.rect.Intersects(itemList[i].aSprite.rect))
                {
                    Sound.PlaySound(Sound.Sounds.ItemCollect);
                    switch (itemList[i].type)
                    {
                        case ItemTypes.Health25: Game1.player.AddEnergy(25, true); itemList[i].aSprite.IsAlive = false; break;
                    }
                }
                itemList[i].aSprite.Update(gameTime);
                if (!Game1.GameRect.Intersects(itemList[i].aSprite.rect) || !itemList[i].aSprite.IsAlive)
                    itemList.RemoveAt(i);
            }
        }

        static public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Item item in itemList)
                item.aSprite.Draw(spriteBatch);
        }


    }
}
