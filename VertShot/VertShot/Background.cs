//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;



//namespace VertShot
//{
//    static public class Background
//    {
//        public struct BackPic
//        {
//            public Texture2D texture;
//            public Vector2 position;
//            public float speed;
//            public Rectangle rect { get { return new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), Game1.Width, Game1.Height); } }
//            public Rectangle rectOff { get { return new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y) - Game1.Height, Game1.Width, Game1.Height); } }
//        }
//        static List<BackPic> list = new List<BackPic>();

//        static public void AddBackPic(Texture2D texture, float speed)
//        {
//            BackPic backPic = new BackPic();
//            backPic.texture = texture;
//            backPic.speed = speed;
//            backPic.position = Vector2.Zero;
//            list.Add(backPic);
//        }

//        static public void ClearBackPics()
//        {
//            list.Clear();
//            list.TrimExcess();
//        }

//        static public void Update(GameTime gameTime)
//        {
//            for (int i = 0; i < list.Count;i++ )
//            {
//                BackPic backPic = list[i];
//                backPic.position += new Vector2(0, backPic.speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
//                if (backPic.position.Y >= Game1.Height)
//                    backPic.position.Y -= (float)backPic.rect.Height;
//                list[i] = backPic;
//            }
//        }

//        static public void Draw(SpriteBatch spriteBatch)
//        {
//            foreach (BackPic backPic in list)
//            {
//                spriteBatch.Draw(backPic.texture, backPic.rect, Color.White);
//                spriteBatch.Draw(backPic.texture, backPic.rectOff, Color.White);
//            }
//        }
//    }
//}
