using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VertShot
{
    static public class EnemyCollector
    {
        static List<Enemy> enemyList = new List<Enemy>();
        static Texture2D meteorTex;
        static public Vector2 MeteorSize { get { return new Vector2(meteorTex.Width, meteorTex.Height); } }

        static public List<Enemy> GetList { get { return enemyList; } }

        static public void Initialize(Texture2D meteorTex)
        {
            EnemyCollector.meteorTex = meteorTex;
        }

        static public void AddMeteor(Vector2 position)
        {
            Meteor meteor = new Meteor(meteorTex, position, 0.3f, new Vector2(0,1));
            enemyList.Add(meteor);
        }


        static public void Update(GameTime gameTime)
        {
            for (int i = enemyList.Count - 1; i >= 0; i--)
            {
                enemyList[i].Update(gameTime);
                if (!enemyList[i].IsAlive)
                    enemyList.RemoveAt(i);
            }
        }

        static public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in enemyList)
                enemy.Draw(spriteBatch);
        }
    }
}
