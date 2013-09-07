using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IsometricEngine;
using IsometricEngine.TileData;

namespace AccidentallyLasers.WorldObjects
{
    public abstract class InWorldObject : InGameObject
    {
        public static int SquareToPixelX(int squareX)
        {
            return squareX * Tile.TileInGameWidth + (Tile.TileInGameWidth / 2);
        }

        public static int SquareToPixelY(int squareY)
        {
            return squareY * Tile.TileInGameHeight + (Tile.TileInGameHeight / 2);
        }
    }
}
