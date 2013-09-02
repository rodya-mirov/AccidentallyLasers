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
        private const int numGrassTiles = 4;

        public TileMapExtension()
            : base(false)
        {
            grassTiles = new MapCellExtension[numGrassTiles];

            for (int i = 0; i < numGrassTiles; i++)
                grassTiles[i] = new MapCellExtension(i);
        }

        public override MapCellExtension MakeMapCell(int x, int y)
        {
            Random ran = new Random(makeSeed(x, y));

            for (int warmups = 0; warmups < 3; warmups++)
                ran.Next();

            for (int realXMin = 0; realXMin <= 100; realXMin += 6)
            {
                for (int realYMin = 0; realYMin <= 100; realYMin += 7)
                {
                    int realXMax = realXMin + 3;
                    int realYMax = realYMin + 4;

                    if (x < realXMin || x > realXMax || y < realYMin || y > realYMax)
                        continue;

                    MapCellExtension output = new MapCellExtension(24);

                    for (int level = 0; level <= 1; level++)
                    {
                        int xmin = realXMin;
                        int xmax = xmin + 3 - 2 * level;

                        int ymin = realYMin;
                        int ymax = realYMin + 4;

                        if (xmin <= x && x <= xmax && ymin <= y && y <= ymax)
                        {
                            int edgeIndex = findEdgeIndex(x, y, xmin, xmax, ymin, ymax);

                            //if it's a wall, add that tile (randomized for flavor)
                            if (x == xmax)
                            {
                                output.AddTile(ran.Next(4) + 8, level);
                                if (y == ymin)
                                    output.AddTile(17, level);
                                else
                                    output.AddTile(16, level);
                            }
                            if (y == ymin)
                            {
                                output.AddTile(ran.Next(4) + 12, level);
                                if (level == 0)
                                {
                                    output.AddTile(20, 0);
                                }
                            }

                            //add a random roof tile...
                            output.AddTile(40 + ran.Next(4), level + 1);

                            //and the edge of the roof, if appropriate
                            output.AddTile(edgeIndex, level + 1);
                        }
                    }

                    return output;
                }
            }

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
