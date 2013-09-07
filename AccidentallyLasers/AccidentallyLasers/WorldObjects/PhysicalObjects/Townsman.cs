using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using IsometricEngine.TileData;
using AccidentallyLasers.WorldObjects;

namespace AccidentallyLasers.WorldObjects.PhysicalObjects
{
    public class Townsman : PhysicalWorldObject
    {
        #region Constructors
        public Townsman(int squareX, int squareY)
            : base(SquareToPixelX(squareX), SquareToPixelY(squareY))
        {
            //all handled previously
        }
        #endregion

        #region Content Loading
        public static void LoadContent(Game game)
        {
            if (townsmanTexture == null)
                townsmanTexture = game.Content.Load<Texture2D>(textureLocation);
        }
        #endregion

        #region Texture and Visual Information
        private const String textureLocation = @"Images/NPCs/TownsMan1";
        private static Texture2D townsmanTexture;

        public override Rectangle SourceRectangle
        {
            get { return new Rectangle(0, 0, 64, 64); }
        }

        public override Texture2D Texture
        {
            get { return townsmanTexture; }
        }

        public override Color Tint
        {
            get { return Color.White; }
        }

        public override bool Visible
        {
            get { return true; }
        }

        public override int VisualOffsetX
        {
            get { return 32; }
        }

        public override int VisualOffsetY
        {
            get { return 48; }
        }
        #endregion

        #region Position and BoundingBox Tracking
        protected override int HalfWidth { get { return 10; } }
        protected override int HalfHeight { get { return 10; } }
        #endregion

        #region Updating
        public override void Update(GameTime time)
        {
            //do nothing
        }
        #endregion
    }
}
