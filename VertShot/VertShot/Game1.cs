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
    public enum GameState
    {
        MainMenu,
        Pause,
        Game,
        GameOver,
        Quit,
        NewGame
    }


    /// <summary>
    /// Dies ist der Haupttyp für Ihr Spiel
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        static public Game1 game;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        static public Player player;
        SpriteFont debugFont;
        static public SpriteFont buttonFont;

        static public Texture2D oneTexture;

        string debugText1;
        bool showDebug1 = false;

        static bool shutdown = false;

        static public int enemyCounter = 0;

        float meteorTime;
        float meteorElapsedTime;

        public static GameState gameState { get; private set; }
        static public Random rand = new Random((int)DateTime.Now.TimeOfDay.TotalMilliseconds);
        // Interne Auflösung
        static public readonly int Width = 1280;
        static public readonly int Height = 720;
        // Tatsächliche Auflösung
        static public int GraphicWidth = 800;
        static public int GraphicHeight = 600;
        static public readonly Rectangle GameRect = new Rectangle(0, 0, Width, Height);

        int oldWidth = 1280;
        int oldHeight = 720;
        bool oldFullscreen = false;

        public bool IsFullScreen { get { return graphics.IsFullScreen; } }

        static public Matrix scaleMatrix { get; private set; }
        static public Matrix transMatrix { get; private set; }
        static public Matrix mouseMatrix { get; private set; }
        public float GetScale { get { return scaleX < scaleY ? scaleX : scaleY; } }
        float scaleX;
        float scaleY;
        Rectangle rectBlackTop;
        Rectangle rectBlackBottom;

        public Game1()
        {
            game = this;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            SetResolution(GraphicWidth, GraphicHeight, false);

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

            oneTexture = new Texture2D(GraphicsDevice, 1, 1);
            oneTexture.SetData<Color>(new Color[] { Color.White });

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
            Input.AssignKeyboard[GameKeys.Debug5] = Keys.F5;
            Input.AssignKeyboard[GameKeys.Debug6] = Keys.F6;
            Input.AssignKeyboard[GameKeys.Debug7] = Keys.F7;
            Input.AssignKeyboard[GameKeys.Debug8] = Keys.F8;
            Input.AssignKeyboard[GameKeys.Debug9] = Keys.F9;
            Input.AssignKeyboard[GameKeys.Debug10] = Keys.F10;
            Input.AssignKeyboard[GameKeys.Debug11] = Keys.F11;
            Input.AssignKeyboard[GameKeys.Debug12] = Keys.F12;

            debugFont = Content.Load<SpriteFont>("DebugFont");
            buttonFont = Content.Load<SpriteFont>("OcraExtended");

            Background.AddBackPic(Content.Load<Texture2D>("Graphics/back1"), 0.05f);
            Background.AddBackPic(Content.Load<Texture2D>("Graphics/back2"), 0.075f);
            Background.AddBackPic(Content.Load<Texture2D>("Graphics/back3"), 0.1f);

            ShotCollector.Initialize(Content.Load<Texture2D>("Graphics/shot"));
            EnemyCollector.Initialize(Content.Load<Texture2D>("Graphics/meteor2"));
            EffectCollector.Initialize(Content.Load<Texture2D>("Graphics/explosion_34FR"));
            GameHud.Initialize(Content.Load<Texture2D>("Graphics/hud"), Content.Load<Texture2D>("Graphics/hudWithEnergy"));

            Hud.Initialize(Content.Load<Texture2D>("Graphics/hudBack"), Content.Load<Texture2D>("Graphics/hudCornerTop"),
                Content.Load<Texture2D>("Graphics/hudCornerBottom"), Content.Load<Texture2D>("Graphics/hudBorderTop"), Content.Load<Texture2D>("Graphics/hudBorderLeft"));

            player = new Player(Content.Load<Texture2D>("Graphics/ship"), Color.Orange, new Vector2(Width / 2, Height / 2));

            SetGameState(GameState.MainMenu);
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
            if (shutdown)
                this.Exit();

            if (Input.IsGameKeyDown(GameKeys.Menu) && gameState == GameState.Game)
                SetGameState(GameState.Pause);

            switch (gameState)
            {
                case GameState.MainMenu:
                    {
                        Background.Update(gameTime);
                        Hud.Update(gameTime);
                    }
                    break;
                case GameState.Pause:
                    {
                        Hud.Update(gameTime);
                    }
                    break;
                case GameState.Game:
                case GameState.GameOver:
                    {
                        if (meteorElapsedTime >= meteorTime)
                        {
                            meteorElapsedTime -= meteorTime;
                            EnemyCollector.AddMeteor(new Vector2(rand.Next(0, Width - (int)EnemyCollector.MeteorSize.X), 0 - (int)EnemyCollector.MeteorSize.Y));
                            meteorTime = rand.Next(100, 500);
                        }
                        meteorElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                        Background.Update(gameTime);
                        if (gameState != GameState.GameOver) player.Update(gameTime);
                        ShotCollector.Update(gameTime);
                        EnemyCollector.Update(gameTime);
                        EffectCollector.Update(gameTime);
                        GameHud.Update(gameTime);
                        Hud.Update(gameTime);
                    }
                    break;
            }


            #region DEBUG KEYS
            if (Input.IsGameKeyReleased(GameKeys.Debug1))
            {
                showDebug1 = !showDebug1;
            }
            if (Input.IsGameKeyReleased(GameKeys.Debug2))
            {
            }
            if (Input.IsGameKeyReleased(GameKeys.Debug3))
            {
                gameState++;
                if (gameState > GameState.Game)
                    gameState = 0;
            }
            if (Input.IsGameKeyReleased(GameKeys.Debug4))
            {
                enemyCounter = 0;
                player.AddEnergy(9999f);
                player.AddShield(9999f);
            }
            if (Input.IsGameKeyReleased(GameKeys.Debug5))
            {
                Hud.ShowWindow(HudWindowTypes.MainMenu);
            }
            if (Input.IsGameKeyReleased(GameKeys.Debug6))
            {
                Game1.game.SetResolution(1600, 720, false);
            }
            if (Input.IsGameKeyReleased(GameKeys.Debug7))
            {
            }
            if (Input.IsGameKeyReleased(GameKeys.Debug8))
            {
                Hud.CloseAllWindows();
            }
            if (Input.IsGameKeyReleased(GameKeys.Debug9))
            {
            }
            if (Input.IsGameKeyReleased(GameKeys.Debug10))
            {
            }
            if (Input.IsGameKeyReleased(GameKeys.Debug11))
            {
            }
            if (Input.IsGameKeyReleased(GameKeys.Debug12))
            {
            }
            #endregion

            if (showDebug1)
            {
                debugText1 = "Player Energy: " + player.energy;
                debugText1 += "\nPlayer Shield: " + player.shield;
                debugText1 += "\nEnemy killed: " + enemyCounter;
                debugText1 += "\nEnemys: " + EnemyCollector.GetList.Count;
            }


            // TEMP
            if (player.energy <= 0 && gameState == GameState.Game)
            {
                SetGameState(GameState.GameOver);
                player.SetPosition(new Vector2(-100, -100));
            }


            Input.UpdateEnd();
            base.Update(gameTime);
        }


        static public void SetGameState(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.MainMenu:
                    Hud.ShowWindow(HudWindowTypes.MainMenu);
                    Game1.gameState = GameState.MainMenu;
                    break;
                case GameState.Pause:
                    Hud.ShowWindow(HudWindowTypes.Pause);
                    Game1.gameState = GameState.Pause;
                    break;
                case GameState.GameOver:
                    Hud.ShowWindow(HudWindowTypes.GameOver);
                    Game1.gameState = GameState.GameOver;
                    break;
                case GameState.NewGame:
                    EnemyCollector.Reset();
                    EffectCollector.Reset();
                    Game1.player.Reset();
                    GameHud.Reset();
                    Hud.CloseAllWindows();
                    Game1.gameState = GameState.Game;
                    break;
                case GameState.Game:
                    Hud.CloseAllWindows();
                    Game1.gameState = GameState.Game;
                    break;
                case GameState.Quit:
                    Game1.shutdown = true;
                    break;
            }
        }

        internal void SetLastResolution()
        {
            SetResolution(oldWidth, oldHeight, oldFullscreen);
        }

        public void SetResolution(int newWidth, int newHeight, bool fullscreen)
        {
            oldWidth = graphics.PreferredBackBufferWidth;
            oldHeight = graphics.PreferredBackBufferHeight;
            oldFullscreen = graphics.IsFullScreen;

            Game1.GraphicWidth = newWidth;
            Game1.GraphicHeight = newHeight;

            graphics.PreferredBackBufferWidth = Game1.GraphicWidth;
            graphics.PreferredBackBufferHeight = Game1.GraphicHeight;

            graphics.IsFullScreen = fullscreen;

            graphics.ApplyChanges();

            scaleX = (float)Game1.GraphicWidth / (float)Width;
            scaleY = (float)Game1.GraphicHeight / (float)Height;
            scaleMatrix = Matrix.CreateScale(scaleX < scaleY ? scaleX : scaleY);
            transMatrix = Matrix.CreateTranslation(new Vector3(
                scaleY < scaleX ? ((float)Game1.GraphicWidth - (float)Width * scaleY) / 2f : 0,
                scaleX < scaleY ? ((float)Game1.GraphicHeight - (float)Height * scaleX) / 2f : 0, 0));
            mouseMatrix = Matrix.CreateScale(scaleX < scaleY ? 1 / scaleX : 1 / scaleY) *
                Matrix.CreateTranslation(new Vector3(
                    scaleY < scaleX ? ((float)Width - (float)Game1.GraphicHeight / scaleY) * 0.5f : 0,
                    scaleX < scaleY ? ((float)Height - (float)Game1.GraphicHeight / scaleX) * 0.5f : 0, 0));
            rectBlackTop = new Rectangle(
                0,
                0,
                scaleX < scaleY ? Game1.GraphicWidth : (int)(((float)Game1.GraphicWidth - (float)Width * scaleY) / 2f),
                scaleY < scaleX ? Game1.GraphicHeight : (int)(((float)Game1.GraphicHeight - (float)Height * scaleX) / 2f)
                );
            rectBlackBottom = new Rectangle(
                scaleX < scaleY ? 0 : (int)(((float)Game1.GraphicWidth + (float)Width * scaleY) / 2f),
                scaleY < scaleX ? 0 : (int)(((float)Game1.GraphicHeight + (float)Height * scaleX) / 2f),
                scaleX < scaleY ? Game1.GraphicWidth : (int)(((float)Game1.GraphicWidth - (float)Width * scaleY) / 2f),
                scaleY < scaleX ? Game1.GraphicHeight : (int)(((float)Game1.GraphicHeight - (float)Height * scaleX) / 2f)
                );
            Hud.RefreshWindowPositions();
        }


        /// <summary>
        /// Dies wird aufgerufen, wenn das Spiel selbst zeichnen soll.
        /// </summary>
        /// <param name="gameTime">Bietet einen Schnappschuss der Timing-Werte.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, null, scaleMatrix * transMatrix);


            switch (gameState)
            {
                case GameState.MainMenu:
                    {
                        Background.Draw(spriteBatch);
                        Hud.Draw(spriteBatch);

                        spriteBatch.End();
                        spriteBatch.Begin();
                    }
                    break;
                case GameState.Game:
                case GameState.Pause:
                case GameState.GameOver:
                    {
                        Background.Draw(spriteBatch);
                        ShotCollector.Draw(spriteBatch);
                        EnemyCollector.Draw(spriteBatch);
                        player.Draw(spriteBatch);
                        EffectCollector.Draw(spriteBatch);
                        GameHud.Draw(spriteBatch);
                        Hud.Draw(spriteBatch);
                        spriteBatch.End();
                        spriteBatch.Begin();
                    }
                    break;
            }

            spriteBatch.End();
            spriteBatch.Begin();
            if (scaleX != scaleY)
            {
                spriteBatch.Draw(oneTexture, rectBlackTop, Color.Black);
                spriteBatch.Draw(oneTexture, rectBlackBottom, Color.Black);
            }


            if (showDebug1) spriteBatch.DrawString(debugFont, debugText1, new Vector2(10, 110), Color.White);



            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
