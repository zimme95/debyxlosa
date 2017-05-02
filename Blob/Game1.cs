﻿using System.Collections.Generic;
using System.Net;
using Blob.Managers;
using Blob.Models;
using Blob.Models.CollisionHandler;
using Blob.ResourcesProviders;
using GameEngine;
using GameEngine.Components;
using GameEngine.Managers;
using GameEngine.Systems;
using GameEngine.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Blob
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private SystemManager _SystemManager;
        private EntityManager _entityManager;
        private Texture2D hero;
        private Texture2D blob;
        private Texture2D circle;
        private Texture2D rectangle;

        //debugging attributes
        float frameCount = 0;
        float timeSinceLastUpdate = 0;
        float updateInterval = 1;
        float fps = 0;

        public Game1()
        {
            IsFixedTimeStep = false;
            graphics = new GraphicsDeviceManager(this);
            this.IsFixedTimeStep = false;
            this.graphics.SynchronizeWithVerticalRetrace = false;
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1200;
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
            // TODO: Add your initialization logic here
            hero = Content.Load<Texture2D>("player");
            blob = Content.Load<Texture2D>("dictator");
            
            GamePropertyManager.Instance.setGraphics(this.GraphicsDevice);
            //_moveSystem = new MoveSystem(this,graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            //Components.Add(_moveSystem);

            _SystemManager = new SystemManager(this, GameCollisionHandler.CollisionHandler, spriteBatch);
            //Borde ta bort Component.Add(). Systemet anropas manuellt i Update

            //Create a singleton holding the Game-instance instead of sending
            //it as a parameter to appropriate managers.
            GameProvider.getInstance().Game = this;

            //_InputSystem = new InputSystem(this, true);
            //Components.Add(_InputSystem);
            _entityManager = new EntityManager();
            createEntities();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //MediaPlayerManager.Instance.addSong(Content.Load<Song>(@"Music\disco"));
            //MediaPlayerManager.Instance.addSong(Content.Load<Song>(@"Music\game"));
            //MediaPlayerManager.Instance.addSong(Content.Load<Song>(@"Music\gameboy"));
            //MediaPlayerManager.Instance.addSong(Content.Load<Song>(@"Music\cloud"));
            //MediaPlayerManager.Instance.Start();
            circle = Content.Load<Texture2D>("circle");
            rectangle = Content.Load<Texture2D>("rectangle");

            // TODO: use this.Content to load your game content here

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            //Hjälp för debugging
            

            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //{
            //    Exit();
            //}

            // TODO: Add your update logic here
            _SystemManager.Update(gameTime);
            //base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            frameCount++;

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeSinceLastUpdate += elapsed;
            if (timeSinceLastUpdate > updateInterval)
            {
                fps = frameCount / timeSinceLastUpdate;
                this.Window.Title = "FPS: " + fps.ToString();
                frameCount = 0;
                timeSinceLastUpdate -= updateInterval;
            }
            // TODO: Add your drawing code here
            List<TextureComponent> textures = ComponentManager.Instance.getComponentsOfType<TextureComponent>();
            
            
            spriteBatch.Begin();
            foreach (TextureComponent texture in textures)
            {
                PositionComponent entityPosition =
                    ComponentManager.Instance.getComponentByID<PositionComponent>(texture.EntityId);
                RotationComponent entityRotation =
                    ComponentManager.Instance.getComponentByID<RotationComponent>(texture.EntityId);
                RectangleComponent rc = ComponentManager.Instance.getComponentByID<RectangleComponent>(texture.EntityId);
                spriteBatch.Draw(texture.Sprite, new Vector2(rc.BoundingRectangle.X + texture.Sprite.Width / 2, rc.BoundingRectangle.Y + texture.Sprite.Height / 2), null, Color.White, entityRotation.Rotation, entityRotation.Orgin, 1f, SpriteEffects.None, 0f);
                //spriteBatch.Draw(texture.Sprite, new Vector2(entityPosition.X, entityPosition.Y), null, Color.White, entityRotation.Rotation, entityRotation.Orgin, 1f, SpriteEffects.None, 0f);

                //spriteBatch.Draw(rectangle, rc.BoundingRectangle, null, Color.White);
                //spriteBatch.Draw(circle, rc.BoundingRectangle, null, Color.White);
            }
            _SystemManager.Draw(gameTime,spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        void createEntities()
        {
            EntityManager.createPlayer(new Vector2(200, 200), new Vector2(0, 0),
                new KeyMappings(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Space));

            EntityManager.createDictator(new Vector2(50, 50), new Vector2(50, 50));
            EntityManager.createDictator(new Vector2(750, 50), new Vector2(-50, 50));
            EntityManager.createDictator(new Vector2(50, 550), new Vector2(50, -50));
            EntityManager.createDictator(new Vector2(750, 550), new Vector2(-50, -50));

        }
    }
}
