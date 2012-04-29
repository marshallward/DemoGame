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

using Tesserae;

namespace DemoGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class DemoGame : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public Mosaic mosaic;
        
        // Temporary
        public RenderTarget2D renderTarget;
        
        // Better place to put this?
        public int defaultWidth = 720;
        public int defaultHeight = 720;
        
        public DemoGame()
        {
            graphics = new GraphicsDeviceManager(this);
            
            // I don't want to use this, delete it?
            Content.RootDirectory = "assets";
        }
        
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here            
            base.Initialize();
            
            // Default window resolution
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = defaultWidth;
            graphics.PreferredBackBufferHeight = defaultHeight;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            
            // Temporary
            renderTarget = new RenderTarget2D(GraphicsDevice, defaultWidth, defaultHeight);
        }
        
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            mosaic = new Mosaic(this, "demo_map.tmx");
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            // TODO: Add your update logic here
            base.Update(gameTime);
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            // TODO: Add your drawing code here
            
            var testTexture = mosaic.spriteSheet[mosaic.idSheet[1]];
            
            // Draw RenderTarget
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetRenderTarget(renderTarget);
            spriteBatch.Draw(testTexture, Vector2.Zero, Color.White);
            GraphicsDevice.SetRenderTarget(null);
            
            // Draw on the back buffer
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                              SamplerState.PointClamp, null, null);
            //var rect = new Rectangle(0, 0, defaultWidth/2, defaultHeight/2);
            //spriteBatch.Draw(mosaic.renderTarget, rect, Color.White);
            spriteBatch.End();
        }
    }
}
