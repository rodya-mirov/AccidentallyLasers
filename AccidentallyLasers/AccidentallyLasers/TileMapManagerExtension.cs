using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IsometricEngine;
using Microsoft.Xna.Framework;
using AccidentallyLasers.WorldObjects;
using AccidentallyLasers.WorldObjects.PhysicalObjects;

namespace AccidentallyLasers
{
    public class TileMapManagerExtension : TileMapManager<InWorldObject, MapCellExtension, TileMapExtension>
    {
        private const string TileSheetPath = @"Images\Tilesets\IsometricTileSheet";

        protected List<InWorldObject> worldObjects;

        public TileMapManagerExtension(LaserGame game)
            : base(game, TileSheetPath)
        {
            worldObjects = new List<InWorldObject>();

            worldObjects.Add(new Townsman(0, 0));
            worldObjects.Add(new Townsman(1, 0));
            worldObjects.Add(new Townsman(1, 1));
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Townsman.LoadContent(this.game);
        }

        protected override IEnumerable<InWorldObject> InGameObjects()
        {
            foreach (InWorldObject obj in worldObjects)
                yield return obj;
        }

        #region Pathing
        public override bool CanMoveFromSquareToSquare(int startX, int startY, int endX, int endY)
        {
            return Math.Abs(startX - endX) + Math.Abs(startY - endY) == 1;
        }

        public override IEnumerable<Point> GetAdjacentPoints(int X, int Y)
        {
            if (CanMoveFromSquareToSquare(X, Y, X - 1, Y))
                yield return new Point(X - 1, Y);

            if (CanMoveFromSquareToSquare(X, Y, X + 1, Y))
                yield return new Point(X + 1, Y);

            if (CanMoveFromSquareToSquare(X, Y, X, Y - 1))
                yield return new Point(X, Y - 1);

            if (CanMoveFromSquareToSquare(X, Y, X, Y + 1))
                yield return new Point(X, Y + 1);
        }
        #endregion
    }
}
