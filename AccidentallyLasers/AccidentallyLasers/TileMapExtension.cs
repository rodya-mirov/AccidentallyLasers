using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IsometricEngine;

namespace AccidentallyLasers
{
    public class TileMapExtension : TileMap<MapCellExtension>
    {
        private MapCellExtension[] grassTiles;
        private static int[] grassTileIndices = new int[] { 0, 1, 2, 3, 4, 5, 6, 7};
        private static int numGrassTiles = grassTileIndices.Length;

        public TileMapExtension()
            : base(false)
        {
            grassTiles = new MapCellExtension[numGrassTiles];

            for (int i = 0; i < numGrassTiles; i++)
                grassTiles[i] = new MapCellExtension(i);
        }

        private const bool viewMainAxes = false;

        public override MapCellExtension MakeMapCell(int x, int y)
        {
            if (x * y == 0 && viewMainAxes)
                return new MapCellExtension(40);

            Random ran = new Random(makeSeed(x, y));

            for (int warmups = 0; warmups < 0; warmups++)
                ran.Next();

            return grassTiles[ran.Next(numGrassTiles)];
        }

        /// <summary>
        /// Calculates the 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static int findEdgeIndex(int x, int y, int buildingXMin, int buildingXMax, int buildingYMin, int buildingYMax)
        {
            //the start of the wall edges
            int edgeIndex = 24;

            //the flags for grabbing the wall edges
            if (x == buildingXMin)
                edgeIndex += 4;
            if (x == buildingXMax)
                edgeIndex += 1;
            if (y == buildingYMin)
                edgeIndex += 2;
            if (y == buildingYMax)
                edgeIndex += 8;

            return edgeIndex;
        }

        #region Random seeding
        private const int RANDOM_SEED = 33476; //just a random collection of bits.  all ints are equally useful.

        /// <summary>
        /// Makes a random seed based on the known constant RANDOM_SEED as well as interpolating x and y.
        /// Has a period of *exactly* 65536 in both coordinates and otherwise has completely unique (but
        /// fairly continuous, bitwise) outputs.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int makeSeed(int x, int y)
        {
            return RANDOM_SEED ^ ((x << 16) | (y & 65535));
        }
        #endregion
    }
}
