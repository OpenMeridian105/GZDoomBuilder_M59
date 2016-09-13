
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
using System.IO;
using CodeImp.DoomBuilder.Config;
using CodeImp.DoomBuilder.Map;
using CodeImp.DoomBuilder.Geometry;
using CodeImp.DoomBuilder.Types;

#endregion

namespace CodeImp.DoomBuilder.IO
{
	public struct FileSidedef
	{
		public int id;
		public int flags;
		public int animateSpeed;
		public int texHigh;
		public int texMid;
		public int texLow;
	}

	internal class RooMapSetIO : MapSetIO
	{
		#region ================== Constants

		private const uint BF_POS_BACKWARDS   = 0x00000001;  // Draw + side bitmap right/left reversed
		private const uint BF_NEG_BACKWARDS   = 0x00000002;  // Draw - side bitmap right/left reversed
		private const uint BF_POS_TRANSPARENT = 0x00000004;  // + side bitmap has some transparency
		private const uint BF_NEG_TRANSPARENT = 0x00000008;  // - side bitmap has some transparency
		private const uint BF_POS_PASSABLE    = 0x00000010;  // + side bitmap can be walked through
		private const uint BF_NEG_PASSABLE    = 0x00000020;  // - side bitmap can be walked through
		private const uint BF_MAP_NEVER       = 0x00000040;  // Don't show wall on map
		private const uint BF_MAP_ALWAYS      = 0x00000080;  // Always show wall on map

		private const uint BF_POS_NOLOOKTHROUGH = 0x00000400; // + side bitmap can't be seen through even though it's transparent
		private const uint BF_NEG_NOLOOKTHROUGH = 0x00000800; // - side bitmap can't be seen through even though it's transparent

		private const uint BF_POS_ABOVE_BUP    = 0x00001000; // + side above texture bottom up
		private const uint BF_NEG_ABOVE_BUP    = 0x00002000; // - side above texture bottom up
		private const uint BF_POS_BELOW_TDOWN  = 0x00004000; // + side below texture top down
		private const uint BF_NEG_BELOW_TDOWN  = 0x00008000; // - side below texture top down
		private const uint BF_POS_NORMAL_TDOWN = 0x00010000; // + side normal texture top down
		private const uint BF_NEG_NORMAL_TDOWN = 0x00020000; // - side normal texture top down
		private const uint BF_POS_NO_VTILE     = 0x00040000; // + side no vertical tile
		private const uint BF_NEG_NO_VTILE     = 0x00080000; // - side no vertical tile
		
		private const uint WF_BACKWARDS      = 0x00000001;   // Draw bitmap right/left reversed
		private const uint WF_TRANSPARENT    = 0x00000002;   // normal wall has some transparency
		private const uint WF_PASSABLE       = 0x00000004;   // wall can be walked through
		private const uint WF_MAP_NEVER      = 0x00000008;   // Don't show wall on map
		private const uint WF_MAP_ALWAYS     = 0x00000010;   // Always show wall on map
		private const uint WF_NOLOOKTHROUGH  = 0x00000020;   // bitmap can't be seen through even though it's transparent
		private const uint WF_ABOVE_BOTTOMUP = 0x00000040;   // Draw upper texture bottom-up
		private const uint WF_BELOW_TOPDOWN  = 0x00000080;   // Draw lower texture top-down
		private const uint WF_NORMAL_TOPDOWN = 0x00000100;   // Draw normal texture top-down
		private const uint WF_NO_VTILE       = 0x00000200;   // Don't tile texture vertically (must be transparent)

		private const uint SF_DEPTH0 = 0x00000000; // No depth (default)
		private const uint SF_DEPTH1 = 0x00000001; // Shallow depth
		private const uint SF_DEPTH2 = 0x00000002; // Shallow depth
		private const uint SF_DEPTH3 = 0x00000003; // Shallow depth

		private const uint SF_SCROLL_FLOOR    = 0x00000080; // Scroll floor texture
		private const uint SF_SCROLL_CEILING  = 0x00000100; // Scroll ceiling texture
		private const uint SF_FLICKER         = 0x00000200; // Flicker light in sector
		private const uint SF_SLOPED_FLOOR    = 0x00000400; // Sloped floor
		private const uint SF_SLOPED_CEILING  = 0x00000800; // Sloped ceiling

		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		public RooMapSetIO(WAD wad, MapManager manager) : base(wad, manager) { }

		#endregion

		#region ================== Properties

		public override int MaxSidedefs { get { return ushort.MaxValue; } }
		public override int MaxVertices { get { return ushort.MaxValue; } }
		public override int MaxLinedefs { get { return ushort.MaxValue; } }
		public override int MaxSectors { get { return ushort.MaxValue; } }
		public override int MaxThings { get { return int.MaxValue; } }
		public override int MinTextureOffset { get { return short.MinValue; } }
		public override int MaxTextureOffset { get { return short.MaxValue; } }
		public override int VertexDecimals { get { return 0; } }
		public override string DecimalsFormat { get { return "0"; } }
		public override bool HasLinedefTag { get { return false; } }
		public override bool HasThingTag { get { return false; } }
		public override bool HasThingAction { get { return false; } }
		public override bool HasCustomFields { get { return false; } }
		public override bool HasThingHeight { get { return true; } }
		public override bool HasActionArgs { get { return false; } }
		public override bool HasMixedActivations { get { return false; } }
		public override bool HasPresetActivations { get { return false; } }
		public override bool HasBuiltInActivations { get { return false; } }
		public override bool HasNumericLinedefFlags { get { return true; } }
		public override bool HasNumericThingFlags { get { return true; } }
		public override bool HasNumericLinedefActivations { get { return true; } }
		public override int MaxTag { get { return ushort.MaxValue; } }
		public override int MinTag { get { return ushort.MinValue; } }
		public override int MaxAction { get { return byte.MaxValue; } }
		public override int MinAction { get { return byte.MinValue; } }
		public override int MaxArgument { get { return byte.MaxValue; } }
		public override int MinArgument { get { return byte.MinValue; } }
		public override int MaxEffect { get { return ushort.MaxValue; } }
		public override int MinEffect { get { return ushort.MinValue; } }
		public override int MaxBrightness { get { return 255; } }
		public override int MinBrightness { get { return 0; } }
		public override int MaxThingType { get { return short.MaxValue; } } //mxd. Editor numbers must be in [1 .. 32767] range
		public override int MinThingType { get { return 1; } } //mxd
		public override float MaxCoordinate { get { return short.MaxValue; } }
		public override float MinCoordinate { get { return short.MinValue; } }
		public override int MaxThingAngle { get { return short.MaxValue; } }
		public override int MinThingAngle { get { return short.MinValue; } }
		public override Dictionary<MapElementType, Dictionary<string, UniversalType>> UIFields { get { return uifields; } } //mxd
		
		#endregion

		#region ================== Reading

		// This reads a map from the file and returns a MapSet
		public override MapSet Read(MapSet map, string mapname)
		{
			// Find the index where first map lump begins
			int firstindex = 1;

			// Create vertexes list
			Dictionary<int, Vertex> vertexlink = new Dictionary<int, Vertex>();
			// Create sectors list
			Dictionary<int, Sector> sectorlink = new Dictionary<int, Sector>();

			// Read sectors.
			ReadSectors(map, firstindex, sectorlink);
			// Read linedefs, sidedefs and vertexes.
			ReadLinedefs(map, firstindex, vertexlink, sectorlink);
			// Read things.
			ReadThings(map, firstindex);

			// Remove unused vertices.
			map.RemoveUnusedVertices();
			
			// Check sector/vertex references - some Meridian maps have slopes
			// with vertexes outside the sector. Not sure if this was intended
			// or just incorrect references. Disabled for now.
			//CheckSectorSlopeVerts(sectorlink);
			foreach (KeyValuePair<int, Sector> entry in sectorlink)
			{
				if (FixSlopeVertRefs(entry.Value))
					map.ChangedDuringLoad = true;
			}

			// Return result;
			return map;
		}

		// This reads the THINGS from WAD file
		private void ReadThings(MapSet map, int firstindex)
		{
			int[] args = new int[Thing.NUM_ARGS];

			// Get the lump from wad file
			Lump lump = wad.FindLump("THINGS", firstindex);
			if (lump == null)
				throw new Exception("Could not find required lump THINGS!");
			
			// Prepare to read the items
			MemoryStream mem = new MemoryStream(lump.Stream.ReadAllBytes());
			BinaryReader reader = new BinaryReader(mem);

			// Number of things. Only load rooms with 2 things.
			short count = reader.ReadInt16();
			if (count != 2)
				throw new Exception(String.Format("Can only load .roo with 2 things, found {0}!", count));

			// Read items from the lump
			map.SetCapacity(0, 0, 0, 0, 2);
			for(int i = 0; i < 2; i++)
			{
				// Read properties from stream
				int x = reader.ReadInt32();
				int y = reader.ReadInt32();

				// Make string flags
				Dictionary<string, bool> stringflags = new Dictionary<string, bool>(StringComparer.Ordinal);

				// Create new item
				Thing t = map.CreateThing();
				t.Update(0, x, y, 0, 0, 0, 0, 1.0f, 1.0f, stringflags, 0, 0, args);
			}

			// Done
			mem.Dispose();
		}

		// This reads the SECTORS from WAD file
		// Returns a lookup table with indices
		private void ReadSectors(MapSet map, int firstindex, Dictionary<int, Sector> link)
		{
			// Get the lump from wad file
			Lump lump = wad.FindLump("SECTORS", firstindex);
			if(lump == null) throw new Exception("Could not find required lump SECTORS!");

			// Prepare to read the items
			MemoryStream mem = new MemoryStream(lump.Stream.ReadAllBytes());
			BinaryReader reader = new BinaryReader(mem);
			int numSectors = reader.ReadInt16();

			// Read items from the lump
			map.SetCapacity(0, 0, 0, numSectors, 0);
			for (int i = 0; i < numSectors; ++i)
			{
				// Read properties from stream
				ushort tag = reader.ReadUInt16();
				int texFloor = reader.ReadUInt16();
				int texCeil = reader.ReadUInt16();
				int xoffset = reader.ReadUInt16();
				int yoffset = reader.ReadUInt16();
				// Some maps had some extremely high values for offset due to a bug,
				// so set those offsets to 0.
				if (xoffset > 10000)
					xoffset = 0;
				if (yoffset > 10000)
					yoffset = 0;
				ushort hfloor = reader.ReadUInt16();
				ushort hceil = reader.ReadUInt16();
				
				byte bright = reader.ReadByte();
				int flags = reader.ReadInt32(); // Blakserv flags.
				byte speed = reader.ReadByte();
				
				// Create new item
				Sector s = map.CreateSector(i);

				// Read slope data.
				Vector3D vfloor = new Vector3D(0, 0, 0);
				Vector3D vceil = new Vector3D(0, 0, 0);
				float floorD = 0, ceilD = 0;
				int texRotFloor = 0, texRotCeil = 0;

				if ((flags & SF_SLOPED_FLOOR) == SF_SLOPED_FLOOR)
				{
					List<Vector3D> floorvert;
					CalculateSlope(reader, out vfloor, out floorD, out texRotFloor, out floorvert, true);
					s.FloorSlopeVertexes = floorvert;
				}
				if ((flags & SF_SLOPED_CEILING) == SF_SLOPED_CEILING)
				{
					List<Vector3D> ceilvert;// = new List<Vector2D>();
					CalculateSlope(reader, out vceil, out ceilD, out texRotCeil, out ceilvert, false);
					s.CeilSlopeVertexes = ceilvert;
				}

				int depth = (int)(flags & SF_DEPTH3);
				bool flicker = ((flags & SF_FLICKER) == SF_FLICKER);
				bool scrollFloor = ((flags & SF_SCROLL_FLOOR) == SF_SCROLL_FLOOR);
				bool scrollCeiling = ((flags & SF_SCROLL_CEILING) == SF_SCROLL_CEILING);
				s.ScrollFlags = new SDScrollFlags(SectorScrollSpeed(flags), SectorScrollDirection(flags));
				s.Update(hfloor, hceil, xoffset, yoffset, MakeGRDName(texFloor), MakeGRDName(texCeil),
					floorD, ceilD, texRotFloor, texRotCeil, vfloor.GetNormal(), vceil.GetNormal(),
					tag, bright, depth, speed, flicker, scrollFloor, scrollCeiling);

				// Add it to the lookup table
				link.Add(i, s);
			}

			// Done
			mem.Dispose();
		}

		private int RooAddVertex(MapSet map, Dictionary<int, Vertex> link, int x, int y)
		{
			// Return existing vertex num if it exists already.
			foreach(KeyValuePair<int, Vertex> entry in link)
			{
				if (entry.Value.Position.x == x
					&& entry.Value.Position.y == y)
					return entry.Key;
			}
			int vNum = map.Vertices.Count + 1;

			map.SetCapacity(vNum, 0, 0, 0, 0);
			Vertex v = map.CreateVertex(new Vector2D(x, y));
			link.Add(vNum, v);

			return vNum;
		}

		// This reads the LINEDEFS and SIDEDEFS from WAD file
		private void ReadLinedefs(MapSet map, int firstindex,
			Dictionary<int, Vertex> vertexlink,Dictionary<int, Sector> sectorlink)
		{
			// Get the linedefs lump from roo file
			Lump linedefslump = wad.FindLump("LINEDEFS", firstindex);
			if(linedefslump == null)
				throw new Exception("Could not find required lump LINEDEFS!");

			// Get the sidedefs lump from roo file
			Lump sidedefslump = wad.FindLump("SIDEDEFS", firstindex);
			if(sidedefslump == null)
				throw new Exception("Could not find required lump SIDEDEFS!");

			// Prepare to read the items
			MemoryStream linedefsmem = new MemoryStream(linedefslump.Stream.ReadAllBytes());
			MemoryStream sidedefsmem = new MemoryStream(sidedefslump.Stream.ReadAllBytes());
			BinaryReader readline = new BinaryReader(linedefsmem);
			BinaryReader readside = new BinaryReader(sidedefsmem);
			int numLineDefs = readline.ReadInt16();
			int numSideDefs = readside.ReadInt16();

			// Read in sidedefs.
			Dictionary<int, FileSidedef> FileSD = new Dictionary<int, FileSidedef>(numSideDefs);
			for (int i = 0; i < numSideDefs; ++i)
			{
				FileSidedef fsd = new FileSidedef();
				fsd.id = readside.ReadUInt16();
				fsd.texMid = readside.ReadUInt16();
				fsd.texHigh = readside.ReadUInt16();
				fsd.texLow = readside.ReadUInt16();
				fsd.flags = readside.ReadInt32();
				fsd.animateSpeed = readside.ReadByte();
				FileSD.Add(i, fsd);
			}

			// Read items from the lump
			map.SetCapacity(0, map.Linedefs.Count + numLineDefs, map.Sidedefs.Count + numSideDefs, 0, 0);
			for (int i = 0; i < numLineDefs; ++i)
			{
				// Read properties from stream
				int s1 = readline.ReadUInt16();
				int s2 = readline.ReadUInt16();
				int s1XOffset = readline.ReadInt16();
				int s2XOffset = readline.ReadInt16();
				int s1YOffset = readline.ReadInt16();
				int s2YOffset = readline.ReadInt16();

				int s1Sector = readline.ReadInt16();
				int s2Sector = readline.ReadInt16();

				int x0 = readline.ReadInt32();
				int y0 = readline.ReadInt32();
				int v1 = RooAddVertex(map, vertexlink, x0, y0);
				int x1 = readline.ReadInt32();
				int y1 = readline.ReadInt32();
				int v2 = RooAddVertex(map, vertexlink, x1, y1);

				// Check if not zero-length
				if (Vector2D.ManhattanDistance(vertexlink[v1].Position, vertexlink[v2].Position) > 0.0001f)
				{
					Linedef l = map.CreateLinedef(vertexlink[v1], vertexlink[v2]);
					l.FileSidedef1 = s1 - 1;
					l.FileSidedef2 = s2 - 1;
					Sidedef side1, side2;
					FileSidedef fsd1, fsd2;
					fsd1.flags = 0;
					fsd2.flags = 0;
					if (s1Sector >= 0)
					{
						side1 = map.CreateSidedef(l, true, sectorlink[s1Sector]);
						if (l.FileSidedef1 >= 0)
						{
							fsd1 = FileSD[l.FileSidedef1];
							side1.Update(s1XOffset, s1YOffset, MakeGRDName(fsd1.texHigh),
								MakeGRDName(fsd1.texMid), MakeGRDName(fsd1.texLow),
								fsd1.animateSpeed, fsd1.id);
						}
						else
						{
							side1.Update(s1XOffset, s1YOffset, "-", "-", "-", 0, 0);
						}
					}
					if (s2Sector >= 0)
					{
						side2 = map.CreateSidedef(l, false, sectorlink[s2Sector]);
						if (l.FileSidedef2 >= 0)
						{
							fsd2 = FileSD[l.FileSidedef2];
							side2.Update(s2XOffset, s2YOffset, MakeGRDName(fsd2.texHigh),
								MakeGRDName(fsd2.texMid), MakeGRDName(fsd2.texLow),
								fsd2.animateSpeed, fsd2.id);
						}
						else
						{
							side2.Update(s2XOffset, s2YOffset, "-", "-", "-", 0, 0);
						}
					}

					// Make string flags
					int linedefFlags = ParseLinedefFlags(fsd1.flags, fsd2.flags);
					l.FrontScrollFlags = new SDScrollFlags(WallScrollSpeed(fsd1.flags),WallScrollDirection(fsd1.flags));
					l.BackScrollFlags = new SDScrollFlags(WallScrollSpeed(fsd2.flags),WallScrollDirection(fsd2.flags));
					Dictionary<string, bool> stringflags = new Dictionary<string, bool>(StringComparer.Ordinal);
					foreach (string f in manager.Config.SortedLinedefFlags)
					{
						int fnum;
						if (int.TryParse(f, out fnum)) stringflags[f] = ((linedefFlags & fnum) == fnum);
					}
					l.Update(stringflags, 0, new List<int> { 0 }, 0, new int[Linedef.NUM_ARGS]);
					l.UpdateCache();
					l.ApplySidedFlags();
				}
			}

			// Done
			linedefsmem.Dispose();
			sidedefsmem.Dispose();
		}

		#endregion

		#region ================== Writing

		// This writes a MapSet to the file
		public override void Write(MapSet map, string mapname, int position)
		{
			// Create the file sidedef list.
			Dictionary<int, FileSidedef> fileSideDefs = new Dictionary<int, FileSidedef>();
			foreach (Linedef l in map.Linedefs)
			{
				if ((l.Start.Position.x == l.End.Position.x)
					&& (l.Start.Position.y == l.End.Position.y))
				{
					General.ErrorLogger.Add(ErrorType.Warning, "Linedef " + l.Index + " has zero length!.");
					continue;
				}
				if (l.Front != null)
					AddFileSidedef(l.Front, fileSideDefs);
				if (l.Back != null)
					AddFileSidedef(l.Back, fileSideDefs);
			}

			Vector2D mapBorder = new Vector2D();

			WriteSecurity(map, position, manager.Config.MapLumps);
			WriteMapBoundary(map, position, manager.Config.MapLumps, out mapBorder);
			WriteClientWalls(map, position, manager.Config.MapLumps);
			WriteNodes(map, position, manager.Config.MapLumps);
			WriteRoomeditWalls(map, position, manager.Config.MapLumps, fileSideDefs);
			WriteSidedefs(map, position, manager.Config.MapLumps, fileSideDefs);
			WriteSectors(map, position, manager.Config.MapLumps, mapBorder);
			WriteThings(map, position, manager.Config.MapLumps);
		}

		private void WriteClientWalls(MapSet map, int position, Dictionary<string, MapLumpInfo> maplumps)
		{
			// Create memory to write to
			MemoryStream mem = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(mem, WAD.ENCODING);

			writer.Write((Int32)0);

			// Find insert position and remove old lump
			int insertpos = MapManager.RemoveSpecificLump(wad, "CLIWALLS", position, MapManager.TEMP_MAP_HEADER, maplumps);
			if (insertpos == -1)
				insertpos = position + 1;
			if (insertpos > wad.Lumps.Count)
				insertpos = wad.Lumps.Count;

			// Create the lump from memory
			Lump lump = wad.Insert("CLIWALLS", insertpos, (int)mem.Length);
			lump.Stream.Seek(0, SeekOrigin.Begin);
			mem.WriteTo(lump.Stream);
		}

		private void WriteNodes(MapSet map, int position, Dictionary<string, MapLumpInfo> maplumps)
		{
			// Create memory to write to
			MemoryStream mem = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(mem, WAD.ENCODING);

			writer.Write((Int32)0);

			// Find insert position and remove old lump
			int insertpos = MapManager.RemoveSpecificLump(wad, "NODES", position, MapManager.TEMP_MAP_HEADER, maplumps);
			if (insertpos == -1)
				insertpos = position + 1;
			if (insertpos > wad.Lumps.Count)
				insertpos = wad.Lumps.Count;

			// Create the lump from memory
			Lump lump = wad.Insert("NODES", insertpos, (int)mem.Length);
			lump.Stream.Seek(0, SeekOrigin.Begin);
			mem.WriteTo(lump.Stream);
		}

		private void WriteSecurity(MapSet map, int position, Dictionary<string, MapLumpInfo> maplumps)
		{
			// Create memory to write to
			MemoryStream mem = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(mem, WAD.ENCODING);

			writer.Write((Int32)0);

			// Find insert position and remove old lump
			int insertpos = MapManager.RemoveSpecificLump(wad, "SECURITY", position, MapManager.TEMP_MAP_HEADER, maplumps);
			if (insertpos == -1)
				insertpos = position + 1;
			if (insertpos > wad.Lumps.Count)
				insertpos = wad.Lumps.Count;

			// Create the lump from memory
			Lump lump = wad.Insert("SECURITY", insertpos, (int)mem.Length);
			lump.Stream.Seek(0, SeekOrigin.Begin);
			mem.WriteTo(lump.Stream);
		}

		private void WriteMapBoundary(MapSet map, int position, Dictionary<string, MapLumpInfo> maplumps, out Vector2D mapBorder)
		{
			// Create memory to write to
			MemoryStream mem = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(mem, WAD.ENCODING);

			float mapMinX = MaxCoordinate;
 			float mapMinY = MaxCoordinate;
			float mapMaxX = MinCoordinate;
			float mapMaxY = MinCoordinate;

			if (map.Things.Count == 2)
			{
				float[] x = new float[2];
				float[] y = new float[2];
				int i = 0;

				foreach (Thing T in map.Things)
				{
					x[i] = T.Position.x;
					y[i] = T.Position.y;
					++i;
				}
				if (x[0] <= x[1])
				{
					mapMinX = x[0];
					mapMaxX = x[1];
				}
				else
				{
					mapMinX = x[1];
					mapMaxX = x[0];
				}
				if (y[0] <= y[1])
				{
					mapMinY = y[0];
					mapMaxY = y[1];
				}
				else
				{
					mapMinY = y[1];
					mapMaxY = y[0];
				}
			}
			else
			{
				foreach (Vertex V in map.Vertices)
				{
					if (V.Position.x < mapMinX) mapMinX = V.Position.x;
					if (V.Position.x > mapMaxX) mapMaxX = V.Position.x;
					if (V.Position.y < mapMinY) mapMinY = V.Position.y;
					if (V.Position.y > mapMaxY) mapMaxY = V.Position.y;
				}
			}

			mapBorder.x = mapMinX;
			mapBorder.y = mapMaxY;

			int width = (int)Math.Round((mapMaxX - mapMinX) * 16.0f);
			int height = (int)Math.Round((mapMaxY - mapMinY) * 16.0f);

			writer.Write((Int32)width);
			writer.Write((Int32)height);

			// Find insert position and remove old lump
			int insertpos = MapManager.RemoveSpecificLump(wad, "MAPBOUND", position, MapManager.TEMP_MAP_HEADER, maplumps);
			if (insertpos == -1)
				insertpos = position + 1;
			if (insertpos > wad.Lumps.Count)
				insertpos = wad.Lumps.Count;

			// Create the lump from memory
			Lump lump = wad.Insert("MAPBOUND", insertpos, (int)mem.Length);
			lump.Stream.Seek(0, SeekOrigin.Begin);
			mem.WriteTo(lump.Stream);
		}

		// This writes the THINGS to WAD file
		private void WriteThings(MapSet map, int position, Dictionary<string, MapLumpInfo> maplumps)
		{
			// Create memory to write to
			MemoryStream mem = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(mem, WAD.ENCODING);

			writer.Write((Int16)map.Things.Count);

			// Go for all things
			foreach(Thing t in map.Things)
			{
				writer.Write((Int32)t.Position.x);
				writer.Write((Int32)t.Position.y);
			}
			
			// Find insert position and remove old lump
			int insertpos = MapManager.RemoveSpecificLump(wad, "THINGS", position, MapManager.TEMP_MAP_HEADER, maplumps);
			if (insertpos == -1)
				insertpos = position + 1;
			if (insertpos > wad.Lumps.Count)
				insertpos = wad.Lumps.Count;
			
			// Create the lump from memory
			Lump lump = wad.Insert("THINGS", insertpos, (int)mem.Length);
			lump.Stream.Seek(0, SeekOrigin.Begin);
			mem.WriteTo(lump.Stream);
		}

		// This writes the LINEDEFS to WAD file
		private void WriteRoomeditWalls(MapSet map, int position, Dictionary<string, MapLumpInfo> maplumps,
										Dictionary<int, FileSidedef> fileSidedefs)
		{
			// Create memory to write to
			MemoryStream mem = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(mem, WAD.ENCODING);
			
			writer.Write((Int16)map.Linedefs.Count);

			// Go for all lines
			FileSidedef FSD1, FSD2;
			Sidedef SD1 = null, SD2 = null;
			bool retVal = false;

			foreach (Linedef l in map.Linedefs)
			{
				if (l.Front != null)
				{
					SD1 = l.Front;
					retVal = fileSidedefs.TryGetValue(l.FileSidedef1, out FSD1);
					if (!retVal)
						l.FileSidedef1 = 0;
				}
				else
					l.FileSidedef1 = 0;

				if (l.Back != null)
				{
					SD2 = l.Back;
					retVal = fileSidedefs.TryGetValue(l.FileSidedef2, out FSD2);
					if (!retVal)
						l.FileSidedef2 = 0;
				}
				else
					l.FileSidedef2 = 0;

				// Sidedef numbers
				writer.Write((UInt16)l.FileSidedef1);
				writer.Write((UInt16)l.FileSidedef2);

				short tmp = 0;
				if (l.FileSidedef1 > 0)
					tmp = (short)SD1.OffsetX;
				writer.Write((Int16)tmp);
				if (l.FileSidedef2 > 0)
					tmp = (short)SD2.OffsetX;
				writer.Write((Int16)tmp);
				if (l.FileSidedef1 > 0)
					tmp = (short)SD1.OffsetY;
				writer.Write((Int16)tmp);
				if (l.FileSidedef2 > 0)
					tmp = (short)SD2.OffsetY;
				writer.Write((Int16)tmp);

				tmp = -1;
				if (l.FileSidedef1 > 0)
					tmp = (short)SD1.Sector.Index;
				writer.Write((Int16)tmp);
				tmp = -1;
				if (l.FileSidedef2 > 0)
					tmp = (short)SD2.Sector.Index;
				writer.Write((Int16)tmp);

				Vector2D vs = l.Start.Position, ve = l.End.Position;
				writer.Write((Int32)Math.Round(vs.x));
				writer.Write((Int32)Math.Round(vs.y));
				writer.Write((Int32)Math.Round(ve.x));
				writer.Write((Int32)Math.Round(ve.y));
			}

			// Find insert position and remove old lump
			int insertpos = MapManager.RemoveSpecificLump(wad, "LINEDEFS", position, MapManager.TEMP_MAP_HEADER, maplumps);
			if(insertpos == -1) insertpos = position + 1;
			if(insertpos > wad.Lumps.Count) insertpos = wad.Lumps.Count;

			// Create the lump from memory
			Lump lump = wad.Insert("LINEDEFS", insertpos, (int)mem.Length);
			lump.Stream.Seek(0, SeekOrigin.Begin);
			mem.WriteTo(lump.Stream);
		}

		// This writes the SIDEDEFS to WAD file
		private void WriteSidedefs(MapSet map, int position, Dictionary<string, MapLumpInfo> maplumps,
			Dictionary<int, FileSidedef> fileSideDefs)
		{
			// Create memory to write to
			MemoryStream mem = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(mem, WAD.ENCODING);

			writer.Write((UInt16)fileSideDefs.Count);
			foreach (KeyValuePair<int, FileSidedef> fsd in fileSideDefs)
			{
				writer.Write((UInt16)fsd.Value.id);
				writer.Write((UInt16)fsd.Value.texMid);
				writer.Write((UInt16)fsd.Value.texHigh);
				writer.Write((UInt16)fsd.Value.texLow);
				writer.Write((Int32)fsd.Value.flags);
				writer.Write((Byte)fsd.Value.animateSpeed);
			}

			// Find insert position and remove old lump
			int insertpos = MapManager.RemoveSpecificLump(wad, "SIDEDEFS", position, MapManager.TEMP_MAP_HEADER, maplumps);
			if(insertpos == -1) insertpos = position + 1;
			if(insertpos > wad.Lumps.Count) insertpos = wad.Lumps.Count;

			// Create the lump from memory
			Lump lump = wad.Insert("SIDEDEFS", insertpos, (int)mem.Length);
			lump.Stream.Seek(0, SeekOrigin.Begin);
			mem.WriteTo(lump.Stream);
		}

		// This writes the SECTORS to WAD file
		private void WriteSectors(MapSet map, int position, Dictionary<string, MapLumpInfo> maplumps, Vector2D mapBorder)
		{
			// Create memory to write to
			MemoryStream mem = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(mem, WAD.ENCODING);

			writer.Write((Int16)map.Sectors.Count);

			// Go for all sectors
			foreach(Sector s in map.Sectors)
			{
				// Write properties to stream
				writer.Write((UInt16)s.SectorTag);
				writer.Write((UInt16)MakeGRDNumber(s.FloorTexture));
				writer.Write((UInt16)MakeGRDNumber(s.CeilTexture));
				writer.Write((UInt16)s.OffsetX);
				writer.Write((UInt16)s.OffsetY);
				writer.Write((UInt16)s.FloorHeight);
				writer.Write((UInt16)s.CeilHeight);
				writer.Write((Byte)s.Brightness);
				uint flags = MakeBlakservSectorFlags(s);
				writer.Write((Int32)flags);
				writer.Write((Byte)s.AnimationSpeed);
				if ((flags & SF_SLOPED_FLOOR) == SF_SLOPED_FLOOR)
					WriteSlopeInfo(writer, map, s, mapBorder, true);
				if ((flags & SF_SLOPED_CEILING) == SF_SLOPED_CEILING)
					WriteSlopeInfo(writer, map, s, mapBorder, false);
			}

			// Find insert position and remove old lump
			int insertpos = MapManager.RemoveSpecificLump(wad, "SECTORS", position, MapManager.TEMP_MAP_HEADER, maplumps);
			if(insertpos == -1) insertpos = position + 1;
			if(insertpos > wad.Lumps.Count) insertpos = wad.Lumps.Count;

			// Create the lump from memory
			Lump lump = wad.Insert("SECTORS", insertpos, (int)mem.Length);
			lump.Stream.Seek(0, SeekOrigin.Begin);
			mem.WriteTo(lump.Stream);
		}
		
		private void WriteSlopeInfo(BinaryWriter writer, MapSet map, Sector s, Vector2D mapBorder, bool floor)
		{
			List<Vector3D> svl;
			Plane pl;
			int texRot = 0;

			if (floor)
			{
				pl = Sector.GetFloorPlane(s);
				svl = s.FloorSlopeVertexes;
				texRot = s.FloorTexRot * 4096 / 360;
			}
			else
			{
				pl = Sector.GetCeilingPlane(s);
				svl = s.CeilSlopeVertexes;
				texRot = s.CeilTexRot * 4096 / 360;
			}

			// Create client slope.
			double[] u = new double[3];
			double[] v = new double[3];
			double[] uv = new double[3];
			Vector3D[] p = new Vector3D[3];
			double ucrossv;
			int i = 0;

			foreach (Vector3D V in svl)
			{
				p[i].x = (V.x - mapBorder.x) * 16.0f;
				p[i].y = (mapBorder.y - V.y) * 16.0f;
				p[i].z = V.z * 16.0f;
				++i;
			}

			// Compute new plane equation.
			u[0] = p[1].x - p[0].x;
			u[1] = p[1].y - p[0].y;
			u[2] = p[1].z - p[0].z;
			v[0] = p[2].x - p[0].x;
			v[1] = p[2].y - p[0].y;
			v[2] = p[2].z - p[0].z;
			uv[0] = u[2] * v[1] - u[1] * v[2];
			uv[1] = u[0] * v[2] - u[2] * v[0];
			uv[2] = u[1] * v[0] - u[0] * v[1];
			ucrossv = Math.Sqrt(uv[0] * uv[0] + uv[1] * uv[1] + uv[2] * uv[2]);

			Vector3D newABC = new Vector3D(
				(float)(uv[0] * 1024.0 / ucrossv),
				(float)(uv[1] * 1024.0 / ucrossv),
				(float)(uv[2] * 1024.0 / ucrossv));
			float offset = -(newABC.x * p[0].x + newABC.y * p[0].y + newABC.z * p[0].z);

			if (floor)
			{
				if (newABC.z < 0.0f)
				{
					// normals of floors must point up
					newABC.x = -newABC.x;
					newABC.y = -newABC.y;
					newABC.z = -newABC.z;
					offset = -offset;
				}
			}
			else
			{
				if (newABC.z > 0.0f)
				{
					// normals of ceilings must point down
					newABC.x = -newABC.x;
					newABC.y = -newABC.y;
					newABC.z = -newABC.z;
					offset = -offset;
				}
			}

			// Write out the new plane abcd.
			writer.Write((Single)newABC.x);
			writer.Write((Single)newABC.y);
			writer.Write((Single)newABC.z);
			writer.Write((Single)offset);

			// Write texture origin and rotation (in client units).
			writer.Write((Int32)Math.Round(p[0].x));
			writer.Write((Int32)Math.Round(p[0].y));
			writer.Write((Int32)texRot);

			// Write original vertex positions, in room editor units.
			foreach (Vector2D V in svl)
			{
				writer.Write((Int16)Math.Round(V.x));
				writer.Write((Int16)Math.Round(V.y));
				writer.Write((Int16)Math.Round(pl.GetZ(V.x, V.y)));
			}
		}

		#endregion

		#region Utility

		/// <summary>
		/// Read in and calculate a slope.
		/// </summary>
		/// <param name="Reader"></param>
		/// <param name="VSlope"></param>
		/// <param name="Offset"></param>
		/// <param name="TexRot"></param>
		/// <param name="verts"></param>
		/// <param name="IsFloor"></param>
		private void CalculateSlope(BinaryReader Reader, out Vector3D VSlope,
			out float Offset, out int TexRot, out List<Vector3D> verts, bool IsFloor)
		{
			double[] u = new double[3];
			double[] v = new double[3];
			double[] uv = new double[3];
			Vector3D[] p = new Vector3D[3];
			double ucrossv;

			// Discard existing slope calcs (used in client).
			Reader.ReadBytes(16);

			// Slope texture origin and rotation.
			// Texture origin is in client units - discard.
			Reader.ReadInt32();
			Reader.ReadInt32();
			TexRot = Reader.ReadInt32() * 360/4096; // in deg

			short temp;

			verts = new List<Vector3D>(3);
			for (int i = 0; i < 3; ++i)
			{
				temp = Reader.ReadInt16();
				p[i].x = (float)temp;
				temp = Reader.ReadInt16();
				p[i].y = (float)temp;
				temp = Reader.ReadInt16();
				p[i].z = (float)temp;
				verts.Add(new Vector3D(p[i].x, p[i].y, p[i].z));
			}

			u[0] = p[1].x - p[0].x;
			u[1] = p[1].y - p[0].y;
			u[2] = p[1].z - p[0].z;
			v[0] = p[2].x - p[0].x;
			v[1] = p[2].y - p[0].y;
			v[2] = p[2].z - p[0].z;
			uv[0] = u[2] * v[1] - u[1] * v[2];
			uv[1] = u[0] * v[2] - u[2] * v[0];
			uv[2] = u[1] * v[0] - u[0] * v[1];
			ucrossv = Math.Sqrt(uv[0] * uv[0] + uv[1] * uv[1] + uv[2] * uv[2]);
			VSlope.x = (float)(uv[0] / ucrossv);
			VSlope.y = (float)(uv[1] / ucrossv);
			VSlope.z = (float)(uv[2] / ucrossv);
			Offset = -(VSlope.x * p[0].x + VSlope.y * p[0].y + VSlope.z * p[0].z);

			if (IsFloor)
			{
				if (VSlope.z < 0)
				{
					// normals of floors must point up
					VSlope.x = -VSlope.x;
					VSlope.y = -VSlope.y;
					VSlope.z = -VSlope.z;
					Offset = -Offset;
				}
			}
			else
			{
				if (VSlope.z > 0)
				{
					// normals of ceilings must point down
					VSlope.x = -VSlope.x;
					VSlope.y = -VSlope.y;
					VSlope.z = -VSlope.z;
					Offset = -Offset;
				}
			}
		}

		/// <summary>
		/// Parses the blakserv linedef flags from the saved sidedef flags.
		/// </summary>
		/// <param name="PosFlags"></param>
		/// <param name="NegFlags"></param>
		/// <returns></returns>
		private int ParseLinedefFlags(int PosFlags, int NegFlags)
		{
			uint flags = 0;

			if ((PosFlags & WF_BACKWARDS) == WF_BACKWARDS)
				flags |= BF_POS_BACKWARDS;
			if ((PosFlags & WF_TRANSPARENT) == WF_TRANSPARENT)
				flags |= BF_POS_TRANSPARENT;
			if ((PosFlags & WF_PASSABLE) == WF_PASSABLE)
				flags |= BF_POS_PASSABLE;
			if ((PosFlags & WF_NOLOOKTHROUGH) == WF_NOLOOKTHROUGH)
				flags |= BF_POS_NOLOOKTHROUGH;
			if ((PosFlags & WF_ABOVE_BOTTOMUP) == WF_ABOVE_BOTTOMUP)
				flags |= BF_POS_ABOVE_BUP;
			if ((PosFlags & WF_BELOW_TOPDOWN) == WF_BELOW_TOPDOWN)
				flags |= BF_POS_BELOW_TDOWN;
			if ((PosFlags & WF_NORMAL_TOPDOWN) == WF_NORMAL_TOPDOWN)
				flags |= BF_POS_NORMAL_TDOWN;
			if ((PosFlags & WF_NO_VTILE) == WF_NO_VTILE)
				flags |= BF_POS_NO_VTILE;

			if ((NegFlags & WF_BACKWARDS) == WF_BACKWARDS)
				flags |= BF_NEG_BACKWARDS;
			if ((NegFlags & WF_TRANSPARENT) == WF_TRANSPARENT)
				flags |= BF_NEG_TRANSPARENT;
			if ((NegFlags & WF_PASSABLE) == WF_PASSABLE)
				flags |= BF_NEG_PASSABLE;
			if ((NegFlags & WF_NOLOOKTHROUGH) == WF_NOLOOKTHROUGH)
				flags |= BF_NEG_NOLOOKTHROUGH;
			if ((NegFlags & WF_ABOVE_BOTTOMUP) == WF_ABOVE_BOTTOMUP)
				flags |= BF_NEG_ABOVE_BUP;
			if ((NegFlags & WF_BELOW_TOPDOWN) == WF_BELOW_TOPDOWN)
				flags |= BF_NEG_BELOW_TDOWN;
			if ((NegFlags & WF_NORMAL_TOPDOWN) == WF_NORMAL_TOPDOWN)
				flags |= BF_NEG_NORMAL_TDOWN;
			if ((NegFlags & WF_NO_VTILE) == WF_NO_VTILE)
				flags |= BF_NEG_NO_VTILE;

			if (((PosFlags & WF_MAP_NEVER) == WF_MAP_NEVER)
				|| ((NegFlags & WF_MAP_NEVER) == WF_MAP_NEVER))
				flags |= BF_MAP_NEVER;
			if (((PosFlags & WF_MAP_ALWAYS) == WF_MAP_ALWAYS)
				|| ((NegFlags & WF_MAP_ALWAYS) == WF_MAP_ALWAYS))
				flags |= BF_MAP_ALWAYS;

			return (int)flags;
		}

		/// <summary>
		/// Converts a Linedef's flags into the blakserv/roo equivalent.
		/// </summary>
		/// <param name="l"></param>
		/// <param name="positive"></param>
		/// <returns></returns>
		private uint MakeBlakservSidedefFlags(Linedef l, bool positive)
		{
			uint blak_flags = 0, flags = 0;

			Dictionary<string, bool> stringflags = l.Flags;

			foreach (KeyValuePair<string, bool> f in l.Flags)
			{
				int fnum;
				if (f.Value && int.TryParse(f.Key, out fnum))
					blak_flags |= (uint)fnum;
			}

			if ((positive && ((blak_flags & BF_POS_BACKWARDS) == BF_POS_BACKWARDS))
				|| (!positive && ((blak_flags & BF_NEG_BACKWARDS) == BF_NEG_BACKWARDS)))
				flags |= WF_BACKWARDS;
			if ((positive && ((blak_flags & BF_POS_TRANSPARENT) == BF_POS_TRANSPARENT))
				|| (!positive && ((blak_flags & BF_NEG_TRANSPARENT) == BF_NEG_TRANSPARENT)))
				flags |= WF_TRANSPARENT;
			if ((positive && ((blak_flags & BF_POS_PASSABLE) == BF_POS_PASSABLE))
				|| (!positive && ((blak_flags & BF_NEG_PASSABLE) == BF_NEG_PASSABLE)))
				flags |= WF_PASSABLE;
			if ((positive && ((blak_flags & BF_POS_NOLOOKTHROUGH) == BF_POS_NOLOOKTHROUGH))
				|| (!positive && ((blak_flags & BF_NEG_NOLOOKTHROUGH) == BF_NEG_NOLOOKTHROUGH)))
				flags |= WF_NOLOOKTHROUGH;
			if ((positive && ((blak_flags & BF_POS_ABOVE_BUP) == BF_POS_ABOVE_BUP))
				|| (!positive && ((blak_flags & BF_NEG_ABOVE_BUP) == BF_NEG_ABOVE_BUP)))
				flags |= WF_ABOVE_BOTTOMUP;
			if ((positive && ((blak_flags & BF_POS_BELOW_TDOWN) == BF_POS_BELOW_TDOWN))
				|| (!positive && ((blak_flags & BF_NEG_BELOW_TDOWN) == BF_NEG_BELOW_TDOWN)))
				flags |= WF_BELOW_TOPDOWN;
			if ((positive && ((blak_flags & BF_POS_NORMAL_TDOWN) == BF_POS_NORMAL_TDOWN))
				|| (!positive && ((blak_flags & BF_NEG_NORMAL_TDOWN) == BF_NEG_NORMAL_TDOWN)))
				flags |= WF_NORMAL_TOPDOWN;
			if ((positive && ((blak_flags & BF_POS_NO_VTILE) == BF_POS_NO_VTILE))
				|| (!positive && ((blak_flags & BF_NEG_NO_VTILE) == BF_NEG_NO_VTILE)))
				flags |= WF_NO_VTILE;

			if ((blak_flags & BF_MAP_NEVER) == BF_MAP_NEVER)
				flags |= WF_MAP_NEVER;
			if ((blak_flags & BF_MAP_ALWAYS) == BF_MAP_ALWAYS)
				flags |= WF_MAP_ALWAYS;

			if (positive)
			{
				flags |= (uint)l.FrontScrollFlags.Speed << 10;
				flags |= (uint)l.FrontScrollFlags.Direction << 12;
			}
			else
			{
				flags |= (uint)l.BackScrollFlags.Speed << 10;
				flags |= (uint)l.BackScrollFlags.Direction << 12;
			}

			return flags;
		}

		private uint MakeBlakservSectorFlags(Sector S)
		{
			uint flags = 0;
			if (S.Flicker)
				flags |= SF_FLICKER;
			if (S.ScrollFloor)
				flags |= SF_SCROLL_FLOOR;
			if (S.ScrollCeiling)
				flags |= SF_SCROLL_CEILING;
			if (S.Depth > 0)
				flags |= (uint)S.Depth;

			// Floor slope flag (and check).
			if (S.FloorSlope.GetLengthSq() > 0 && !float.IsNaN(S.FloorSlopeOffset / S.FloorSlope.z))
				flags |= SF_SLOPED_FLOOR;
			if ((flags & SF_SLOPED_FLOOR) == SF_SLOPED_FLOOR
				&& (S.FloorSlopeVertexes == null || S.FloorSlopeVertexes.Count != 3))
			{
				General.ErrorLogger.Add(ErrorType.Error, "Sector " + S.Index
					+ " has an invalid floor slope! Missing at least one vertex. Not saving slope.");
				flags &= ~SF_SLOPED_FLOOR;
			}

			// Ceiling slope flag (and check).
			if (S.CeilSlope.GetLengthSq() > 0 && !float.IsNaN(S.CeilSlopeOffset / S.CeilSlope.z))
				flags |= SF_SLOPED_CEILING;
			if ((flags & SF_SLOPED_CEILING) == SF_SLOPED_CEILING
				&& (S.CeilSlopeVertexes == null || S.CeilSlopeVertexes.Count != 3))
			{
				General.ErrorLogger.Add(ErrorType.Error, "Sector " + S.Index
					+ " has an invalid ceiling slope! Missing at least one vertex. Not saving slope.");
				flags &= ~SF_SLOPED_CEILING;
			}

			flags |= (uint)S.ScrollFlags.Speed << 2;
			flags |= (uint)S.ScrollFlags.Direction << 4;

			return flags;
		}
	
		/// <summary>
		/// Gets the wall scroll speed from the saved blakserv sidedef flags.
		/// </summary>
		/// <param name="flags"></param>
		/// <returns></returns>
		private int WallScrollSpeed(int flags)
		{
			return ((flags & 0x00000C00) >> 10);
		}

		/// <summary>
		/// Gets the wall scroll direction from the saved blakserv sidedef flags.
		/// </summary>
		/// <param name="flags"></param>
		/// <returns></returns>
		private int WallScrollDirection(int flags)
		{
			return ((flags & 0x00007000) >> 12);
		}

		private int SectorScrollSpeed(int flags)
		{
			return ((flags & 0x0000000C) >> 2);
		}

		private int SectorScrollDirection(int flags)
		{
			return ((flags & 0x00000070) >> 4);
		}

		/// <summary>
		/// Takes a number and returns a Meridian grd format
		/// filename with no extension.
		/// </summary>
		/// <param name="Num"></param>
		/// <returns></returns>
		private string MakeGRDName(int Num)
		{
			// We have a 0-5 digit number.
			if (Num == 0)
				return "-";
			if (Num == 1)
				return "grd0001" + Num;
			if (Num < 10)
				return "grd0000" + Num;
			if (Num < 100)
				return "grd000" + Num;
			if (Num < 1000)
				return "grd00" + Num;
			if (Num < 10000)
				return "grd0" + Num;
			return "grd" + Num;
		}

		/// <summary>
		/// Takes a string filename and if it is in the Meridian
		/// grd format, returns the number.
		/// </summary>
		/// <param name="Name"></param>
		/// <returns></returns>
		private uint MakeGRDNumber(string Name)
		{
			if (Name == "-" || Name == "")
				return 0;
			if (!Name.StartsWith("grd", StringComparison.CurrentCultureIgnoreCase))
				return 0;
			return UInt16.Parse(Name.Substring(3, 5));
		}

		/// <summary>
		/// Takes a FileSidedef struct and a Sidedef, returns true if they match.
		/// </summary>
		/// <param name="s1"></param>
		/// <param name="s"></param>
		/// <param name="flags"></param>
		/// <returns></returns>
		private bool SidedefsEquivalent(FileSidedef s1, Sidedef s, uint flags)
		{
			return (s1.id == s.Tag &&
				s1.texHigh == MakeGRDNumber(s.HighTexture) &&
				s1.texLow == MakeGRDNumber(s.LowTexture) &&
				s1.texMid == MakeGRDNumber(s.MiddleTexture) &&
				s1.flags == (int)flags &&
				s1.animateSpeed == s.AnimateSpeed);
		}

		private void AddFileSidedef(Sidedef SD, Dictionary<int, FileSidedef> fileSideDefs)
		{
			bool positive = SD.IsFront;
			uint flags = MakeBlakservSidedefFlags(SD.Line, SD.IsFront);

			foreach (KeyValuePair<int, FileSidedef>f in fileSideDefs)
			{
				if (SidedefsEquivalent(f.Value, SD, flags))
				{
					if (positive)
						SD.Line.FileSidedef1 = f.Key;
					else
						SD.Line.FileSidedef2 = f.Key;
					return;
				}
			}

			FileSidedef fsd = new FileSidedef();
			fsd.id = SD.Tag;
			fsd.texMid = (int)MakeGRDNumber(SD.MiddleTexture);
			fsd.texHigh = (int)MakeGRDNumber(SD.HighTexture);
			fsd.texLow = (int)MakeGRDNumber(SD.LowTexture);
			fsd.flags = (int)flags;
			fsd.animateSpeed = SD.AnimateSpeed;
			fileSideDefs.Add(fileSideDefs.Count + 1, fsd);
			if (positive)
				SD.Line.FileSidedef1 = fileSideDefs.Count;
			else
				SD.Line.FileSidedef2 = fileSideDefs.Count;
		}

		private void CheckSectorSlopeVerts(Dictionary<int, Sector> sectors)
		{
			foreach (KeyValuePair<int, Sector> s in sectors)
			{
				List<Vertex> vList = s.Value.GetVertexes();

				if (s.Value.FloorSlopeVertexes.Count == 3)
				{
					foreach (Vector3D v3 in s.Value.FloorSlopeVertexes)
					{
						Vertex vert = MapSet.NearestVertexSquareRange(vList, v3, 1.0f);
						if (vert == null)
							General.ErrorLogger.Add(ErrorType.Error, "Sector " + s.Value.Index + " has an invalid vertex in floor slope!");
					}
				}
				if (s.Value.CeilSlopeVertexes.Count == 3)
				{
					foreach (Vector3D v3 in s.Value.CeilSlopeVertexes)
					{
						Vertex vert = MapSet.NearestVertexSquareRange(vList, v3, 1.0f);
						if (vert == null)
							General.ErrorLogger.Add(ErrorType.Error, "Sector " + s.Value.Index + " has an invalid vertex in ceiling slope!");
					}
				}
			}
		}

		/// <summary>
		/// Fixes broken sector slope vertex references, i.e. slopes that
		/// reference vertexes from outside the sector. Unfortunately we
		/// can't mark the map as 'changed' during loading, so user has to
		/// manually save. This is in RooMapSetIO instead of Sector class
		/// to avoid future merge conflicts with upstream.
		/// </summary>
		/// <param name="s"></param>
		private bool FixSlopeVertRefs(Sector s)
		{
			bool madeChanges = false;

			// Get this sector's vertexes.
			List<Vertex> vlist = s.GetVertexes();

			if (s.IsFloorSloped())
			{
				// Get a list of valid vertexes first to simplify later searching.
				List<Vertex> floorverts = new List<Vertex>();
				Vertex V;
				foreach (Vector3D v3d in s.FloorSlopeVertexes)
				{
					V = vlist.FirstOrDefault(v => v.Position.x == v3d.x && v.Position.y == v3d.y);
					if (V != null)
						floorverts.Add(V);
				}

				// Create a copy of the vertex position list since we may modify it.
				List<Vector3D> newfsv = new List<Vector3D>(s.FloorSlopeVertexes);

				int index = 0;
				foreach (Vector3D v3d in s.FloorSlopeVertexes)
				{
					// Check if vertex is valid.
					if (vlist.FirstOrDefault(v => v.Position.x == v3d.x && v.Position.y == v3d.y) == null)
					{
						// True if we fix this vertex.
						bool isFixed = false;

						// Floor vertex not inside sector, replace with one that is.
						foreach (Vertex vert in vlist)
						{
							// Exclude any vertex already in use.
							if (floorverts.Contains(vert))
								continue;
							// Check for matching z.
							if (Math.Round(Sector.GetFloorPlane(s).GetZ(vert.Position.x, vert.Position.y)) == Math.Round(v3d.z))
							{
								// Replace entry in new list.
								newfsv[index] = new Vector3D(vert.Position.x, vert.Position.y, v3d.z);

								// Add to list of vertexes in use.
								floorverts.Add(vert);
								General.ErrorLogger.Add(ErrorType.Warning, "Fixed floor slope vertex ref in sector " + s.Index);
								isFixed = true;
								break;
							}
						}

						// If we fixed something, set madeChanges to true so map
						// can show as changed.
						if (isFixed)
							madeChanges = true;
						else
						{
							// We have a problem. What likely happened is the incorrect slope reference
							// also came with an incorrect height to make the slope work. We can't fix
							// this programmatically as the result may not match what the slope is
							// supposed to look like. Log an error instead.
							General.ErrorLogger.Add(ErrorType.Error, "Sector " + s.Index + " has an invalid vertex in floor slope!");
						}
					}
					++index;
				}
				s.FloorSlopeVertexes = newfsv;
			}

			if (s.IsCeilSloped())
			{
				// Get a list of valid vertexes first to simplify later searching.
				List<Vertex> ceilverts = new List<Vertex>();
				Vertex V;
				foreach (Vector3D v3d in s.CeilSlopeVertexes)
				{
					V = vlist.FirstOrDefault(v => v.Position.x == v3d.x && v.Position.y == v3d.y);
					if (V != null)
						ceilverts.Add(V);
				}

				// Create a copy of the vertex position list since we may modify it.
				List<Vector3D> newcsv = new List<Vector3D>(s.CeilSlopeVertexes);

				int index = 0;
				foreach (Vector3D v3d in s.CeilSlopeVertexes)
				{
					// Check if vertex is valid.
					if (vlist.FirstOrDefault(v => v.Position.x == v3d.x && v.Position.y == v3d.y) == null)
					{
						// True if we fix this vertex.
						bool isFixed = false;

						// Ceiling vertex not inside sector, replace with one that is.
						foreach (Vertex vert in vlist)
						{
							// Exclude any vertex already in use.
							if (ceilverts.Contains(vert))
								continue;
							// Check for matching z.
							if (Math.Round(Sector.GetCeilingPlane(s).GetZ(vert.Position.x, vert.Position.y)) == Math.Round(v3d.z))
							{
								// Replace entry in new list.
								newcsv[index] = new Vector3D(vert.Position.x, vert.Position.y, v3d.z);

								// Add to list of vertexes in use.
								ceilverts.Add(vert);
								General.ErrorLogger.Add(ErrorType.Warning, "Fixed ceil slope vertex ref in sector " + s.Index);
								isFixed = true;
								break;
							}
						}

						// If we fixed something, set madeChanges to true so map
						// can show as changed.
						if (isFixed)
							madeChanges = true;
						else
						{
							// We have a problem. What likely happened is the incorrect slope reference
							// also came with an incorrect height to make the slope work. We can't fix
							// this programmatically as the result may not match what the slope is
							// supposed to look like. Log an error instead.
							General.ErrorLogger.Add(ErrorType.Error, "Sector " + s.Index + " has an invalid vertex in ceil slope!");
						}
					}
					++index;
				}
				s.CeilSlopeVertexes = newcsv;
			}

			return madeChanges;
		}

		#endregion
	}
}
