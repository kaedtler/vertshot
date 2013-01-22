using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VertShot.Menu
{
    public class MenuObject
    {
        public Vector2 position;
        protected Vector2 size;
        protected Rectangle rect { get { return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y); } }
        protected string text;
        protected bool replaceText = false;
        public string GetText { get { return replaceText ? TextBuilder.ReplacePlaceholder(text, value != null && value.GetType() == typeof(GameKeys) ? (GameKeys)value : GameKeys.None) : text; } }
        public object value;


        public virtual void RefreshPosition(Vector2 posDiff)
        {
            this.position += posDiff;
        }


        public virtual void Reset()
        {
        }


        public virtual void Update(GameTime gameTime)
        {
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
