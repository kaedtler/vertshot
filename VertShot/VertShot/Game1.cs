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
        static public float gametimeCounter = 0;

        float meteorTime;
        float meteorElapsedTime;

        public static GameState gameState { get; private set; }
        static public Random rand = new Random((int)DateTime.Now.TimeOfDay.TotalMilliseconds);
        // Config Datei
        static public Files.Config Config;
        // Interne Auflösung
        static public readonly int Width = 1280;
        static public readonly int Height = 720;

        static public readonly Rectangle GameRect = new Rectangle(0, 0, Width, Height);

        int oldWidth;
        int oldHeight;
        bool oldFullscreen;

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

            Config = LoadSave.LoadConfig();

            SetResolution(Config.resWitdh, Config.resHeight, Config.fullscreen);
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

            // Tastaturbelegung aus Config setzen
            Input.AssignKeyboard = Config.assignKeyboard;


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

            Sound.Initialize();

            debugFont = Content.Load<SpriteFont>("DebugFont");
            buttonFont = Content.Load<SpriteFont>("OcraExtended");

            Background.AddBackPic(Content.Load<Texture2D>("Graphics/back1"), 0.05f);
            Background.AddBackPic(Content.Load<Texture2D>("Graphics/back2"), 0.075f);
            Background.AddBackPic(Content.Load<Texture2D>("Graphics/back3"), 0.1f);

            ShotCollector.Initialize(Content.Load<Texture2D>("Graphics/shot"));
            EnemyCollector.Initialize(Content.Load<Texture2D>("Graphics/meteor3"));
            EffectCollector.Initialize(Content.Load<Texture2D>("Graphics/explosion_34FR"));
            GameHud.Initialize(Content.Load<Texture2D>("Graphics/hud"), Content.Load<Texture2D>("Graphics/hudWithEnergy"), Content.Load<SpriteFont>("RepetitionScrolling"));

            Menu.Menu.Initialize(Content.Load<Texture2D>("Graphics/hudBack"), Content.Load<Texture2D>("Graphics/hudCornerTop"),
                Content.Load<Texture2D>("Graphics/hudCornerBottom"), Content.Load<Texture2D>("Graphics/hudBorderTop"), Content.Load<Texture2D>("Graphics/hudBorderLeft"));

            player = new Player(Content.Load<Texture2D>("Graphics/ship"), new Vector2(Width / 2, Height / 2));

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
            Input.UpdateBegin(gameTime);

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
                        Menu.Menu.Update(gameTime);
                    }
                    break;
                case GameState.Pause:
                    {
                        Menu.Menu.Update(gameTime);
                    }
                    break;
                case GameState.Game:
                case GameState.GameOver:
                    {
                        if (meteorElapsedTime >= meteorTime)
                        {
                            meteorElapsedTime -= meteorTime;
                            EnemyCollector.AddMeteor(new Vector2(rand.Next(0, Width - (int)EnemyCollector.MeteorSize.X), 0 - (int)EnemyCollector.MeteorSize.Y), rand.Next(300, 500) / 1000f);
                            meteorTime = rand.Next(100, 300);
                        }
                        meteorElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                        Background.Update(gameTime);
                        if (gameState != GameState.GameOver)
                        {
                            player.Update(gameTime);
                            gametimeCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        }
                        ShotCollector.Update(gameTime);
                        EnemyCollector.Update(gameTime);
                        EffectCollector.Update(gameTime);
                        GameHud.Update(gameTime);
                        Menu.Menu.Update(gameTime);
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
                enemyCounter = 990;
                gametimeCounter = 5990;
            }
            if (Input.IsGameKeyReleased(GameKeys.Debug4))
            {
                enemyCounter = 0;
                player.AddEnergy(9999f);
                player.AddShield(9999f);
            }
            if (Input.IsGameKeyReleased(GameKeys.Debug5))
            {
                Menu.Menu.ShowWindow(Menu.WindowTypes.MainMenu);
            }
            if (Input.IsGameKeyReleased(GameKeys.Debug6))
            {
                Menu.Menu.ShowWindow(Menu.WindowTypes.Pause);
            }
            if (Input.IsGameKeyReleased(GameKeys.Debug7))
            {
            }
            if (Input.IsGameKeyReleased(GameKeys.Debug8))
            {
                Menu.Menu.CloseAllWindows();
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
                Sound.PlaySound(Sound.Sounds.PlayerExplosion, player.rect.Center.X);
            }


            Input.UpdateEnd();
            base.Update(gameTime);
        }


        static public void SetGameState(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.MainMenu:
                    Menu.Menu.ShowWindow(Menu.WindowTypes.MainMenu);
                    Game1.gameState = GameState.MainMenu;
                    Sound.PlayMusic(Sound.Music.Menu);
                    break;
                case GameState.Pause:
                    Menu.Menu.ShowWindow(Menu.WindowTypes.Pause);
                    Game1.gameState = GameState.Pause;
                    Sound.PauseMusic();
                    break;
                case GameState.GameOver:
                    Menu.Menu.ShowWindow(Menu.WindowTypes.GameOver);
                    Game1.gameState = GameState.GameOver;
                    break;
                case GameState.NewGame:
                    EnemyCollector.Reset();
                    EffectCollector.Reset();
                    Game1.player.Reset();
                    GameHud.Reset();
                    enemyCounter = 0;
                    gametimeCounter = 0;
                    Menu.Menu.CloseAllWindows();
                    Game1.gameState = GameState.Game;
                    Sound.PlayMusic(Sound.Music.Game);
                    break;
                case GameState.Game:
                    Menu.Menu.CloseAllWindows();
                    Game1.gameState = GameState.Game;
                    Sound.ResumeMusic();
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

            Config.resWitdh = (short)newWidth;
            Config.resHeight = (short)newHeight;
            Config.fullscreen = fullscreen;


            graphics.PreferredBackBufferWidth = Config.resWitdh;
            graphics.PreferredBackBufferHeight = Config.resHeight;
            graphics.IsFullScreen = Config.fullscreen;

            graphics.ApplyChanges();

            scaleX = (float)Game1.Config.resWitdh / (float)Width;
            scaleY = (float)Game1.Config.resHeight / (float)Height;
            scaleMatrix = Matrix.CreateScale(scaleX < scaleY ? scaleX : scaleY);
            transMatrix = Matrix.CreateTranslation(new Vector3(
                scaleY < scaleX ? ((float)Game1.Config.resWitdh - (float)Width * scaleY) / 2f : 0,
                scaleX < scaleY ? ((float)Game1.Config.resHeight - (float)Height * scaleX) / 2f : 0, 0));
            mouseMatrix = Matrix.CreateScale(scaleX < scaleY ? 1 / scaleX : 1 / scaleY) *
                Matrix.CreateTranslation(new Vector3(
                    scaleY < scaleX ? ((float)Width - (float)Game1.Config.resWitdh / scaleY) * 0.5f : 0,
                    scaleX < scaleY ? ((float)Height - (float)Game1.Config.resHeight / scaleX) * 0.5f : 0, 0));
            rectBlackTop = new Rectangle(
                0,
                0,
                scaleX < scaleY ? Game1.Config.resWitdh : (int)(((float)Game1.Config.resWitdh - (float)Width * scaleY) / 2f),
                scaleY < scaleX ? Game1.Config.resHeight : (int)(((float)Game1.Config.resHeight - (float)Height * scaleX) / 2f)
                );
            rectBlackBottom = new Rectangle(
                scaleX < scaleY ? 0 : (int)(((float)Game1.Config.resWitdh + (float)Width * scaleY) / 2f),
                scaleY < scaleX ? 0 : (int)(((float)Game1.Config.resHeight + (float)Height * scaleX) / 2f),
                scaleX < scaleY ? Game1.Config.resWitdh : (int)(((float)Game1.Config.resWitdh - (float)Width * scaleY) / 2f),
                scaleY < scaleX ? Game1.Config.resHeight : (int)(((float)Game1.Config.resHeight - (float)Height * scaleX) / 2f)
                );
            Menu.Menu.RefreshWindowPositions();

            LoadSave.SaveConfig(Config);
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
                        Menu.Menu.Draw(spriteBatch);

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
                        Menu.Menu.Draw(spriteBatch);
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
