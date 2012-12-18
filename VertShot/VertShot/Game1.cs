using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace VertShot
{
    /// <summary>
    /// Dies ist der Haupttyp für Ihr Spiel
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        static public Player player;
        SpriteFont debugFont;

        Texture2D oneByOneTex;

        string debugText1;
        bool showDebug1 = true;

        static public int enemyCounter = 0;

        float meteorTime;
        float meteorElapsedTime;

        static public Random rand = new Random((int)DateTime.Now.TimeOfDay.TotalMilliseconds);
        static public readonly int Width = 1280;
        static public readonly int Height = 720;
        static public readonly Rectangle GameRect = new Rectangle(0, 0, Width, Height);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = Width;
            graphics.PreferredBackBufferHeight = Height;

            IsMouseVisible = true;

        }

        /// <summary>
        /// Ermöglicht dem Spiel, alle Initialisierungen durchzuführen, die es benötigt, bevor die Ausführung gestartet wird.
        /// Hier können erforderliche Dienste abgefragt und alle nicht mit Grafiken
        /// verbundenen Inhalte geladen werden.  Bei Aufruf von base.Initialize werden alle Komponenten aufgezählt
        /// sowie initialisiert.
        /// </summary>
        protected override void Initialize()
        {
            Input.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent wird einmal pro Spiel aufgerufen und ist der Platz, wo
        /// Ihr gesamter Content geladen wird.
        /// </summary>
        protected override void LoadContent()
        {
            // Erstellen Sie einen neuen SpriteBatch, der zum Zeichnen von Texturen verwendet werden kann.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            oneByOneTex = new Texture2D(GraphicsDevice, 1, 1);
            oneByOneTex.SetData<Color>(new Color[] { Color.White });

            // Tastaturbelegung setzen
            Input.AssignKeyboard[GameKeys.Menu] = Keys.Escape;
            Input.AssignKeyboard[GameKeys.Left] = Keys.Left;
            Input.AssignKeyboard[GameKeys.Right] = Keys.Right;
            Input.AssignKeyboard[GameKeys.Up] = Keys.Up;
            Input.AssignKeyboard[GameKeys.Down] = Keys.Down;
            Input.AssignKeyboard[GameKeys.Fire1] = Keys.Space;
            Input.AssignKeyboard[GameKeys.Fire2] = Keys.LeftControl;
            Input.AssignKeyboard[GameKeys.Debug1] = Keys.F1;
            Input.AssignKeyboard[GameKeys.Debug2] = Keys.F2;
            Input.AssignKeyboard[GameKeys.Debug3] = Keys.F3;
            Input.AssignKeyboard[GameKeys.Debug4] = Keys.F4;

            debugFont = Content.Load<SpriteFont>("DebugFont");
            
            Background.AddBackPic(Content.Load<Texture2D>("Graphics/back1"), 0.05f);
            Background.AddBackPic(Content.Load<Texture2D>("Graphics/back2"), 0.075f);
            Background.AddBackPic(Content.Load<Texture2D>("Graphics/back3"), 0.1f);

            ShotCollector.Initialize(Content.Load<Texture2D>("Graphics/shot"));
            EnemyCollector.Initialize(Content.Load<Texture2D>("Graphics/meteor2"));
            EffectCollector.Initialize(Content.Load<Texture2D>("Graphics/explosion_34FR"));
            GameHud.Initialize(Content.Load<Texture2D>("Graphics/hud"), Content.Load<Texture2D>("Graphics/hudWithEnergy"), oneByOneTex);

            Hud.Initialize(Content.Load<Texture2D>("Graphics/hudBack"), Content.Load<Texture2D>("Graphics/hudCornerTop"),
                Content.Load<Texture2D>("Graphics/hudCornerBottom"), Content.Load<Texture2D>("Graphics/hudBorderTop"), Content.Load<Texture2D>("Graphics/hudBorderLeft"));
            
            player = new Player(Content.Load<Texture2D>("Graphics/ship"), Color.Orange, new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2));
        }

        /// <summary>
        /// UnloadContent wird einmal pro Spiel aufgerufen und ist der Ort, wo
        /// Ihr gesamter Content entladen wird.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Entladen Sie jeglichen Nicht-ContentManager-Inhalt hier
        }

        /// <summary>
        /// Ermöglicht dem Spiel die Ausführung der Logik, wie zum Beispiel Aktualisierung der Welt,
        /// Überprüfung auf Kollisionen, Erfassung von Eingaben und Abspielen von Ton.
        /// </summary>
        /// <param name="gameTime">Bietet einen Schnappschuss der Timing-Werte.</param>
        protected override void Update(GameTime gameTime)
        {
            Input.UpdateBegin();

            // Ermöglicht ein Beenden des Spiels
            if (Input.IsGameKeyDown(GameKeys.Menu))
                this.Exit();


            if(meteorElapsedTime >= meteorTime)
            {
                meteorElapsedTime -= meteorTime;
                EnemyCollector.AddMeteor(new Vector2(rand.Next(0, Width - (int)EnemyCollector.MeteorSize.X), 0 - (int)EnemyCollector.MeteorSize.Y));
                meteorTime = rand.Next(100, 500);
            }
            meteorElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            Background.Update(gameTime);
            player.Update(gameTime);
            ShotCollector.Update(gameTime);
            EnemyCollector.Update(gameTime);
            EffectCollector.Update(gameTime);
            GameHud.Update(gameTime);


            // Debug Keys
            if (Input.IsGameKeyReleased(GameKeys.Debug1))
            {
                showDebug1 = !showDebug1;
            }
            if (Input.IsGameKeyReleased(GameKeys.Debug2))
            {
            }
            if (Input.IsGameKeyReleased(GameKeys.Debug3))
            {
            }
            if (Input.IsGameKeyReleased(GameKeys.Debug4))
            {
                enemyCounter = 0;
                player.AddEnergy(9999f);
                player.AddShield(9999f);
            }

            debugText1 = "Player Energy: " + player.energy;
            debugText1 += "\nPlayer Shield: " + player.shield;
            debugText1 += "\nEnemy killed: " + enemyCounter;
            debugText1 += "\nEnemys: " + EnemyCollector.GetList.Count;
            debugText1 += "\nShots: " + ShotCollector.GetList.Count;


            Input.UpdateEnd();
            base.Update(gameTime);
        }

        /// <summary>
        /// Dies wird aufgerufen, wenn das Spiel selbst zeichnen soll.
        /// </summary>
        /// <param name="gameTime">Bietet einen Schnappschuss der Timing-Werte.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            Background.Draw(spriteBatch);

            ShotCollector.Draw(spriteBatch);
            EnemyCollector.Draw(spriteBatch);

            player.Draw(spriteBatch);

            EffectCollector.Draw(spriteBatch);

            GameHud.Draw(spriteBatch);

            if (showDebug1) spriteBatch.DrawString(debugFont, debugText1, new Vector2(10, 110), Color.White);

            Hud.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
