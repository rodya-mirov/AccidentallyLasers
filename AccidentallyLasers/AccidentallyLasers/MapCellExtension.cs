using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IsometricEngine;
using IsometricEngine.Utilities;

namespace AccidentallyLasers
{
    public class MapCellExtension : MapCell, Copyable<MapCellExtension>
    {
        public MapCellExtension(int baseTile)
            : base(baseTile)
        {
        }

        public void SetElevation(int elevation)
        {
            this.VisualElevation = elevation;
        }

        protected MapCellExtension()
            : base()
        {
        }

        public new MapCellExtension Copy()
        {
            MapCellExtension output = new MapCellExtension();
            output.Tiles = this.TilesCopy();
            return output;
        }
    }
}
