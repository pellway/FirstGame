using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FirstGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Texture2D background;
        private Texture2D shuttle;
        private Texture2D arrow;
        private float angle = 0;

        private int shuttleXPos = 450;
        private int shuttleYPos = 240;
        private int speedOffset = 2;

        private SpriteFont font;
        private int score = 0;

        private AnimatedSprite animatedSprite;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);

            // Define Window Size
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
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
            background = Content.Load<Texture2D>("background/stars");
            shuttle = Content.Load<Texture2D>("sprite/shuttle");
            arrow = Content.Load<Texture2D>("sprite/arrow");

            font = Content.Load<SpriteFont>("Arial");

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            score++;

            animatedSprite.Update();

            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Left) && shuttleXPos >= 0)
            {
                shuttleXPos = shuttleXPos - speedOffset;
            }
            if (state.IsKeyDown(Keys.Right) && shuttleXPos <= 1280 - 142)
            {
                shuttleXPos = shuttleXPos + speedOffset;
            }
            if (state.IsKeyDown(Keys.Up) && shuttleYPos >= 0)
            {
                shuttleYPos = shuttleYPos - speedOffset;
            }
            if (state.IsKeyDown(Keys.Down) && shuttleYPos <= 720 - 220)
            {
                shuttleYPos = shuttleYPos + speedOffset;
            }

            angle += 0.01f;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.MonoGameOrange);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, 1280, 720), Color.White);
            spriteBatch.Draw(shuttle, new Vector2(shuttleXPos, shuttleYPos), Color.White);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Score is: " + score, new Vector2(50, 50), Color.White);
            spriteBatch.End();

            spriteBatch.Begin();
            Vector2 location = new Vector2(400, 200);
            Rectangle sourceRectangle = new Rectangle(0, 0, arrow.Width, arrow.Height);
            Vector2 origin = new Vector2(arrow.Width / 2, arrow.Height/2);
            spriteBatch.Draw(arrow, location, sourceRectangle, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);
            spriteBatch.End();

            animatedSprite.Draw(spriteBatch, new Vector2(400, 200));

            base.Draw(gameTime);
        }
    }
}
