using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IsometricEngine;
using Microsoft.Xna.Framework;

namespace AccidentallyLasers
{
    public class TileMapComponent : DrawableGameComponent
    {
        private TileMapManagerExtension mapManager { get; set; }

        public TileMapComponent(LaserGame game, TileMapManagerExtension map)
            : base(game)
        {
            this.mapManager = map;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            mapManager.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            mapManager.Update(gameTime);
        }

        public override void Initialize()
        {
            base.Initialize();

            mapManager.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            mapManager.Draw(gameTime);
        }
    }
}
