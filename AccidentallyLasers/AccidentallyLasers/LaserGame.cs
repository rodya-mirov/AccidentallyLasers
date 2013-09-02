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
using IsometricEngine.Utilities;
using IsometricEngine;

namespace AccidentallyLasers
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class LaserGame : Microsoft.Xna.Framework.Game
    {
        public static bool hyperDrive = false; //for debug purposes; this makes the framerate speed wildly out of control

        GraphicsDeviceManager graphics;
        TileMapManagerExtension mapManager;
        TileMapComponent mapComponent;
        FPSComponent fpsComponent;

        private int preferredWindowedWidth, preferredWindowedHeight;

        SpriteFont segoe;

        public LaserGame()
        {
            this.IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            mapManager = new TileMapManagerExtension(this);
            mapComponent = new TileMapComponent(this, mapManager);
            Components.Add(mapComponent);

            fpsComponent = new FPSComponent(this, "Fonts/Segoe");
            Components.Add(fpsComponent);

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);

            preferredWindowedWidth = graphics.PreferredBackBufferWidth;
            preferredWindowedHeight = graphics.PreferredBackBufferHeight;

            recognizeHyperDrive();

            base.Initialize();
        }

        private void recognizeHyperDrive()
        {
            this.IsFixedTimeStep = !hyperDrive;
            this.graphics.SynchronizeWithVerticalRetrace = !hyperDrive;
            this.graphics.ApplyChanges();
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            mapManager.SetViewDimensions(Window.ClientBounds.Width, Window.ClientBounds.Height);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            mapManager.SetViewDimensions(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            segoe = Content.Load<SpriteFont>("Fonts/Segoe");
            mapManager.Font = segoe;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            processKeyboardInput();

            base.Update(gameTime);
        }

        #region Keyboard Input
        private HashSet<Keys> keysHeld = new HashSet<Keys>();

        private void processKeyboardInput()
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Escape))
                this.Exit();

            processMovement(ks);

            if (ks.IsKeyDown(Keys.Enter))
            {
                if (!keysHeld.Contains(Keys.Enter))
                {
                    keysHeld.Add(Keys.Enter);
                    ToggleFullScreen();
                }
            }
            else
            {
                keysHeld.Remove(Keys.Enter);
            }

            if (ks.IsKeyDown(Keys.Space) != hyperDrive)
            {
                hyperDrive = !hyperDrive;
                recognizeHyperDrive();
            }
        }
        #endregion

        /// <summary>
        /// Toggle full screen on or off.  Also keeps the camera so that it's centered on the same point
        /// (assuming the window itself is centered)
        /// </summary>
        private void ToggleFullScreen()
        {
            int newWidth, newHeight;
            int oldWidth = graphics.PreferredBackBufferWidth;
            int oldHeight = graphics.PreferredBackBufferHeight;

            if (graphics.IsFullScreen)
            {
                newWidth = preferredWindowedWidth;
                newHeight = preferredWindowedHeight;
            }
            else
            {
                newWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                newHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }

            graphics.PreferredBackBufferWidth = newWidth;
            graphics.PreferredBackBufferHeight = newHeight;

            Point center = Camera.GetCenter();

            graphics.IsFullScreen = !graphics.IsFullScreen;

            this.mapManager.SetViewDimensions(newWidth, newHeight);

            Camera.CenterOnPoint(center);

            graphics.ApplyChanges();
        }

        private Point intendedWorldCenter = new Point(0, 0);

        /// <summary>
        /// Processes keyboard movement.  To keep the movement feeling
        /// "natural" (despite the isometric perspective) we have to slow
        /// the "horizontal" movement slightly (you move about 40% slower
        /// than you should) and speed the "vertical" movement by the same
        /// factor.
        /// 
        /// More precisely, moving "diagonally" is actually moving along
        /// an axis, and you move 2 units per tick (this feels pretty good).
        /// 
        /// When you move "up" (or down), it's really diagonally (with x and y
        /// having different signs) and due to isometric scaling, if we moved
        /// properly (roughly 1.4 units/tick in both directions) it feels slow.
        /// So we make it 2 units/tick in both directions (for a total speed of
        /// 2.8 units/tick, a significant increase over straight-line movement).
        /// 
        /// Similarly, when you move "right" (or left), it's also diagonal (with
        /// x and y changes having the same sign) and due to isometric scaling,
        /// if we moved properly (roughly 1.4 units/tick in both directions) it
        /// feels *really* fast.  So we slow it down, making it 1 unit/tick in
        /// each direction (for a total speed of 1.4 units/tick, a significant
        /// decrease from straight-line movement).
        /// 
        /// I don't think this is very abusable; jiggling back and forth along
        /// diagonals won't help you move right/left faster (it's exactly the 
        /// same speed, which is okay) and it actually slows you down moving
        /// up/down (which is what you want, though it slows you more than you'd
        /// like).  Similarly, moving along the cardinal directions won't help
        /// you move diagonally; moving right/left is so slow that the speed
        /// increase from moving up/down won't allow you to accelerate there
        /// either.
        /// </summary>
        /// <param name="ks"></param>
        private void processMovement(KeyboardState ks)
        {
            int horizontal = 0;
            int vertical = 0;

            if (ks.IsKeyDown(Keys.Left) || ks.IsKeyDown(Keys.A))
                horizontal -= 1;

            if (ks.IsKeyDown(Keys.Right) || ks.IsKeyDown(Keys.D))
                horizontal += 1;

            if (ks.IsKeyDown(Keys.Up) || ks.IsKeyDown(Keys.W))
                vertical += 1;

            if (ks.IsKeyDown(Keys.Down) || ks.IsKeyDown(Keys.S))
                vertical -= 1;

            int xChange, yChange;

            if (horizontal < 0)
            {
                if (vertical < 0)
                {
                    xChange = 0;
                    yChange = -2;
                }
                else if (vertical == 0)
                {
                    xChange = -1;
                    yChange = -1;
                }
                else //vertical > 0
                {
                    xChange = -2;
                    yChange = 0;
                }
            }
            else if (horizontal == 0)
            {
                if (vertical < 0)
                {
                    xChange = 2;
                    yChange = -2;
                }
                else if (vertical == 0)
                {
                    xChange = 0;
                    yChange = 0;
                }
                else //vertical > 0
                {
                    xChange = -2;
                    yChange = 2;
                }
            }
            else //horizontal > 0
            {
                if (vertical < 0)
                {
                    xChange = 2;
                    yChange = 0;
                }
                else if (vertical == 0)
                {
                    xChange = 1;
                    yChange = 1;
                }
                else //vertical > 0
                {
                    xChange = 0;
                    yChange = 2;
                }
            }

            intendedWorldCenter.X += xChange;
            intendedWorldCenter.Y += yChange;

            int x = intendedWorldCenter.X + intendedWorldCenter.Y;
            int y = (intendedWorldCenter.X - intendedWorldCenter.Y) / 2;
            Camera.CenterOnPoint(x, y);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
