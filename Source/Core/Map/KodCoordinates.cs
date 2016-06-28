
#region ================== Copyright (c) 2007 Pascal vd Heiden

/*
 * Copyright (c) 2007 Pascal vd Heiden, www.codeimp.com
 * This program is released under GNU General Public License
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 */

#endregion

#region ================== Namespaces

using System;
using System.Linq;
using System.Collections.Generic;
using CodeImp.DoomBuilder;
using CodeImp.DoomBuilder.Map;
using CodeImp.DoomBuilder.Types;
using CodeImp.DoomBuilder.Geometry;

#endregion

namespace CodeImp.DoomBuilder.Map
{
	public class KodCoordinates
	{
		#region ================== Constants

		public const int FINENESS = 64;

		#endregion

		#region Properties

		private int row;
		private int col;
		private int finerow;
		private int finecol;

		public int Row { get { return row; } }
		public int Col { get { return col; } }
		public int FineRow { get { return finerow; } }
		public int FineCol { get { return finecol; } }

		#endregion

		#region Constructors

		public KodCoordinates()
		{
			this.row = 0;
			this.col = 0;
			this.finerow = 0;
			this.finecol = 0;
		}

		public KodCoordinates(Vector2D coords)
		{
			this.row = 0;
			this.col = 0;
			this.finerow = 0;
			this.finecol = 0;
			
			if (General.Map.Map == null || General.Map.Map.Things == null)
				return;

			// Need a list of things and their coordinates.
			ICollection<Thing> things = General.Map.Map.Things;

			if (things.Count != 2)
				return;

			Thing t1 = (Thing)things.ElementAt(0);
			Thing t2 = (Thing)things.ElementAt(1);

			int top = 0, left = 0;
			if (t1.Position.x <= t2.Position.x)
			{
				top = (int)Math.Round(t1.Position.y);
				left = (int)Math.Round(t1.Position.x);
			}
			else
			{
				top = (int)Math.Round(t2.Position.y);
				left = (int)Math.Round(t2.Position.x);
			}

			int coordX = (int)Math.Round(coords.x);
			int coordY = (int)Math.Round(coords.y);

			// Server coordinates start at (1, 1)
			this.row = 1 + (top - coordY) / FINENESS;
			this.col = 1 + (coordX - left) / FINENESS;
			this.finerow = (top - coordY) % FINENESS;
			this.finecol = (coordX - left) % FINENESS;
		}

		#endregion

		#region Methods

		#endregion
	}
}
