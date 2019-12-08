using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace FirstGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Window size
        private int windowHeight = 700;
        private int windowWidth = 500;

        // Manages game states
        private int GameStatus = 0;
        private readonly int StartScreen = 1;
        private readonly int ExitScreen = 2;
        private readonly int PlayScreen = 3;
        private readonly int PauseScreen = 4;
        
        private Texture2D startButton;
        private Texture2D exitButton;
        private Vector2 startButtonPos;
        private Vector2 exitButtonPos;

        // Assets for entities
        private Texture2D background;
        private Texture2D shuttle;
        private Texture2D arrow;
        private float angle = 0;

        // Position of player
        private int shuttleXPos = 250;
        private int shuttleYPos = 240;
        private int speedOffset = 4;

        // Position of enemy
        private int enemyXPos = 250;
        private int enemyYPos = 200;
        private bool enemyGoRight = true;
        private bool enemyGoLeft = false;

        // Fonts attributes for text
        private SpriteFont font;
        private int time = 0;
        private int timeDisplay = 0;

        // BGM for game
        private SoundEffect bgm;

        private AnimatedSprite animatedSprite;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);

            // Define Window Size
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here

            IsMouseVisible = true;

            // Set position of buttons
            startButtonPos = new Vector2((windowWidth / 2) - 50, 200);
            exitButtonPos = new Vector2((windowWidth / 2) - 50, 250);

            // Set game state to start menu
            GameStatus = StartScreen;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {


        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load resources
            startButton = Content.Load<Texture2D>("button/start");
            exitButton = Content.Load<Texture2D>("button/exit");

            background = Content.Load<Texture2D>("background/stars");
            shuttle = Content.Load<Texture2D>("sprite/shuttle");
            arrow = Content.Load<Texture2D>("sprite/arrow");

            font = Content.Load<SpriteFont>("Arial");

            // Audio settings for gameplay
            bgm = Content.Load<SoundEffect>("audio/test_bgm");
            bgm.Play();

            Texture2D texture = Content.Load<Texture2D>("sprite/smiley");
            // Numbers determine rows and columns of Sprite Map
            animatedSprite = new AnimatedSprite(texture, 4, 4);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            if (GameStatus == StartScreen)
            {
                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.Enter)) {
                    GameStatus = PlayScreen;
                }
                if (state.IsKeyDown(Keys.Escape)) {
                    Exit();
                }
            }

            if (GameStatus == PauseScreen)
            {
                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.Enter))
                {
                    GameStatus = PlayScreen;
                }
                if (state.IsKeyDown(Keys.Escape))
                {
                    Exit();
                }
            }

            if (GameStatus == PlayScreen) 
            {
                time++;
                if (time % 60 == 0) {
                    timeDisplay++;
                }

                animatedSprite.Update();

                if (enemyXPos == 0) {
                    enemyGoRight = true;
                    enemyGoLeft = false;
                }
                if (enemyXPos == windowWidth - 64) {
                    enemyGoRight = false;
                    enemyGoLeft = true;
                }
                if (enemyGoRight == true) {
                    enemyXPos++;
                }
                if (enemyGoLeft == true) {
                    enemyXPos--;
                }
                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.Left) && shuttleXPos >= 0) {
                    shuttleXPos -= speedOffset;
                }
                if (state.IsKeyDown(Keys.Right) && shuttleXPos <= windowWidth - 42) {
                    shuttleXPos += speedOffset;
                }
                if (state.IsKeyDown(Keys.Up) && shuttleYPos > 0) {
                    shuttleYPos -= speedOffset;
                }
                if (state.IsKeyDown(Keys.Down) && shuttleYPos <= windowHeight - 52) {
                    shuttleYPos += speedOffset;
                }

                angle += 0.01f;

                if (state.IsKeyDown(Keys.E)) GameStatus = PauseScreen;
            }

            if (GameStatus == PlayScreen) base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.MonoGameOrange);

            // Draw start menu
            if (GameStatus == StartScreen)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(startButton, startButtonPos, Color.White);
                spriteBatch.Draw(exitButton, exitButtonPos, Color.White);
                spriteBatch.End();
            }

            // Draw play screen
            if (GameStatus == PlayScreen)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(background, new Rectangle(0, 0, 1280, 720), Color.White);
                spriteBatch.Draw(shuttle, new Vector2(shuttleXPos, shuttleYPos), Color.White);

                spriteBatch.DrawString(font, "Time Elapsed: " + timeDisplay + " Seconds", new Vector2(50, 50), Color.White);

                Vector2 location = new Vector2(400, 200);
                Rectangle sourceRectangle = new Rectangle(0, 0, arrow.Width, arrow.Height);
                Vector2 origin = new Vector2(arrow.Width / 2, arrow.Height / 2);
                spriteBatch.Draw(arrow, location, sourceRectangle, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);

                animatedSprite.Draw(spriteBatch, new Vector2(enemyXPos, enemyYPos));
                spriteBatch.End();
            }

            // Draw pause menu
            if (GameStatus == PauseScreen)
            {
                spriteBatch.Begin();

                spriteBatch.Draw(background, new Rectangle(0, 0, 1280, 720), Color.White);
                spriteBatch.Draw(shuttle, new Vector2(shuttleXPos, shuttleYPos), Color.White);

                spriteBatch.DrawString(font, "Time Elapsed: " + timeDisplay + " Seconds", new Vector2(50, 50), Color.White);

                Vector2 location = new Vector2(400, 200);
                Rectangle sourceRectangle = new Rectangle(0, 0, arrow.Width, arrow.Height);
                Vector2 origin = new Vector2(arrow.Width / 2, arrow.Height / 2);
                spriteBatch.Draw(arrow, location, sourceRectangle, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);

                animatedSprite.Draw(spriteBatch, new Vector2(enemyXPos, enemyYPos));

                // Above is same as PlayScreen, below is pause
                spriteBatch.Draw(background, new Rectangle(0, 0, 1280, 720), Color.White * 0.35f);
                spriteBatch.Draw(startButton, startButtonPos, Color.White);
                spriteBatch.Draw(exitButton, exitButtonPos, Color.White);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
