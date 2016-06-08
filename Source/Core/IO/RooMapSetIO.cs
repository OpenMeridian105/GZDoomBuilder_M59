
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
		public override bool HasThingTag { get { return true; } }
		public override bool HasThingAction { get { return true; } }
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
		public override int MaxBrightness { get { return short.MaxValue; } }
		public override int MinBrightness { get { return short.MinValue; } }
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

			// Read sectors
			ReadSectors(map, firstindex, sectorlink);

			// Read linedefs, sidedefs and vertexes
			ReadLinedefs(map, firstindex, vertexlink, sectorlink);

			// Read things
			ReadThings(map, firstindex);
			
			// Remove unused vertices
			map.RemoveUnusedVertices();
			
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

			// Skip count
			reader.ReadInt16();
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
			for (int i = 0; i < numSectors; i++)
			{
				// Read properties from stream
				ushort tag = reader.ReadUInt16();
				ushort floorBitmap = reader.ReadUInt16();
				ushort ceilingBitmap = reader.ReadUInt16(); //string tceil = Lump.MakeNormalName(reader.ReadBytes(8), WAD.ENCODING);
				ushort xoffset = reader.ReadUInt16();
				ushort yoffset = reader.ReadUInt16();
				ushort hfloor = reader.ReadUInt16();
				ushort hceil = reader.ReadUInt16();
				
				byte bright = reader.ReadByte();
				int flags = reader.ReadInt32(); // Blakserv flags.
				byte speed = reader.ReadByte(); // Need to fix how this gets handled in doombuilder.
				
				// Create new item
				Sector s = map.CreateSector(i);
				s.Update(hfloor, hceil, "", "", 0, tag, bright);

				// Should read slope data here?
				if ((flags & 0x400) == 0x400)
					reader.ReadBytes(46);
				if ((flags & 0x800) == 0x800)
					reader.ReadBytes(46);

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
				int textureNormal = readside.ReadUInt16();
				int textureAbove = readside.ReadUInt16();
				int textureBelow = readside.ReadUInt16();
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
							side1.Update(s1XOffset, s1YOffset, "", "", "", fsd1.animateSpeed, fsd1.id);
						}
						else
						{
							side1.Update(s1XOffset, s1YOffset, "", "", "", 0, 0);
						}
					}
					if (s2Sector >= 0)
					{
						side2 = map.CreateSidedef(l, false, sectorlink[s2Sector]);
						if (l.FileSidedef2 >= 0)
						{
							fsd2 = FileSD[l.FileSidedef2];
							side2.Update(s2XOffset, s2YOffset, "", "", "", fsd2.animateSpeed, fsd2.id);
						}
						else
						{
							side2.Update(s2XOffset, s2YOffset, "", "", "", 0, 0);
						}
					}

					// Make string flags
					int linedefFlags = ParseLDFlags(fsd1.flags, fsd2.flags);
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
				}
			}

			// Done
			linedefsmem.Dispose();
			sidedefsmem.Dispose();
		}
		
		private int ParseLDFlags(int pos_flags, int neg_flags)
		{
			uint flags = 0;
			if ((pos_flags & WF_BACKWARDS) == WF_BACKWARDS)
				flags |= BF_POS_BACKWARDS;
			if ((pos_flags & WF_TRANSPARENT) == WF_TRANSPARENT)
				flags |= BF_POS_TRANSPARENT;
			if ((pos_flags & WF_PASSABLE) == WF_PASSABLE)
				flags |= BF_POS_PASSABLE;
			if ((pos_flags & WF_NOLOOKTHROUGH) == WF_NOLOOKTHROUGH)
				flags |= BF_POS_NOLOOKTHROUGH;
			if ((pos_flags & WF_ABOVE_BOTTOMUP) == WF_ABOVE_BOTTOMUP)
				flags |= BF_POS_ABOVE_BUP;
			if ((pos_flags & WF_BELOW_TOPDOWN) == WF_BELOW_TOPDOWN)
				flags |= BF_POS_BELOW_TDOWN;
			if ((pos_flags & WF_NORMAL_TOPDOWN) == WF_NORMAL_TOPDOWN)
				flags |= BF_POS_NORMAL_TDOWN;
			if ((pos_flags & WF_NO_VTILE) == WF_NO_VTILE)
				flags |= BF_POS_NO_VTILE;

			if ((neg_flags & WF_BACKWARDS) == WF_BACKWARDS)
				flags |= BF_NEG_BACKWARDS;
			if ((neg_flags & WF_TRANSPARENT) == WF_TRANSPARENT)
				flags |= BF_NEG_TRANSPARENT;
			if ((neg_flags & WF_PASSABLE) == WF_PASSABLE)
				flags |= BF_NEG_PASSABLE;
			if ((neg_flags & WF_NOLOOKTHROUGH) == WF_NOLOOKTHROUGH)
				flags |= BF_NEG_NOLOOKTHROUGH;
			if ((neg_flags & WF_ABOVE_BOTTOMUP) == WF_ABOVE_BOTTOMUP)
				flags |= BF_NEG_ABOVE_BUP;
			if ((neg_flags & WF_BELOW_TOPDOWN) == WF_BELOW_TOPDOWN)
				flags |= BF_NEG_BELOW_TDOWN;
			if ((neg_flags & WF_NORMAL_TOPDOWN) == WF_NORMAL_TOPDOWN)
				flags |= BF_NEG_NORMAL_TDOWN;
			if ((neg_flags & WF_NO_VTILE) == WF_NO_VTILE)
				flags |= BF_NEG_NO_VTILE;

			if (((pos_flags & WF_MAP_NEVER) == WF_MAP_NEVER)
				|| ((neg_flags & WF_MAP_NEVER) == WF_MAP_NEVER))
				flags |= BF_MAP_NEVER;
			if (((pos_flags & WF_MAP_ALWAYS) == WF_MAP_ALWAYS)
				|| ((neg_flags & WF_MAP_ALWAYS) == WF_MAP_ALWAYS))
				flags |= BF_MAP_ALWAYS;

			return (int)flags;
		}

		private int WallScrollSpeed(int flags)
		{
			return ((flags & 0x00000C00) >> 10);
		}
		private int WallScrollDirection(int flags)
		{
			return ((flags & 0x00007000) >> 12);
		}

		#endregion

		#region ================== Writing

		// This writes a MapSet to the file
		public override void Write(MapSet map, string mapname, int position)
		{
			Dictionary<Vertex, int> vertexids = new Dictionary<Vertex,int>();
			Dictionary<Sidedef, int> sidedefids = new Dictionary<Sidedef,int>();
			Dictionary<Sector, int> sectorids = new Dictionary<Sector,int>();
			
			// First index everything
			foreach(Vertex v in map.Vertices) vertexids.Add(v, vertexids.Count);
			foreach(Sidedef sd in map.Sidedefs) sidedefids.Add(sd, sidedefids.Count);
			foreach(Sector s in map.Sectors) sectorids.Add(s, sectorids.Count);
			
			// Write lumps to wad (note the backwards order because they
			// are all inserted at position+1 when not found)
			WriteSectors(map, position, manager.Config.MapLumps);
			WriteVertices(map, position, manager.Config.MapLumps);
			WriteSidedefs(map, position, manager.Config.MapLumps, sectorids);
			WriteLinedefs(map, position, manager.Config.MapLumps, sidedefids, vertexids);
			WriteThings(map, position, manager.Config.MapLumps);
		}

		// This writes the THINGS to WAD file
		private void WriteThings(MapSet map, int position, Dictionary<string, MapLumpInfo> maplumps)
		{
			// Create memory to write to
			MemoryStream mem = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(mem, WAD.ENCODING);
			
			// Go for all things
			foreach(Thing t in map.Things)
			{
				// Convert flags
				int flags = 0;
				foreach(KeyValuePair<string, bool> f in t.Flags)
				{
					int fnum;
					if(f.Value && int.TryParse(f.Key, out fnum)) flags |= fnum;
				}

				// Write properties to stream
				// Write properties to stream
				writer.Write((UInt16)t.Tag);
				writer.Write((Int16)t.Position.x);
				writer.Write((Int16)t.Position.y);
				writer.Write((Int16)t.Position.z);
				writer.Write((Int16)t.AngleDoom);
				writer.Write((UInt16)t.Type);
				writer.Write((UInt16)flags);
				writer.Write((Byte)t.Action);
				writer.Write((Byte)t.Args[0]);
				writer.Write((Byte)t.Args[1]);
				writer.Write((Byte)t.Args[2]);
				writer.Write((Byte)t.Args[3]);
				writer.Write((Byte)t.Args[4]);
			}
			
			// Find insert position and remove old lump
			int insertpos = MapManager.RemoveSpecificLump(wad, "THINGS", position, MapManager.TEMP_MAP_HEADER, maplumps);
			if(insertpos == -1) insertpos = position + 1;
			if(insertpos > wad.Lumps.Count) insertpos = wad.Lumps.Count;
			
			// Create the lump from memory
			Lump lump = wad.Insert("THINGS", insertpos, (int)mem.Length);
			lump.Stream.Seek(0, SeekOrigin.Begin);
			mem.WriteTo(lump.Stream);
		}

		// This writes the VERTEXES to WAD file
		private void WriteVertices(MapSet map, int position, Dictionary<string, MapLumpInfo> maplumps)
		{
			// Create memory to write to
			MemoryStream mem = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(mem, WAD.ENCODING);

			// Go for all vertices
			foreach(Vertex v in map.Vertices)
			{
				// Write properties to stream
				writer.Write((Int16)(int)Math.Round(v.Position.x));
				writer.Write((Int16)(int)Math.Round(v.Position.y));
			}

			// Find insert position and remove old lump
			int insertpos = MapManager.RemoveSpecificLump(wad, "VERTEXES", position, MapManager.TEMP_MAP_HEADER, maplumps);
			if(insertpos == -1) insertpos = position + 1;
			if(insertpos > wad.Lumps.Count) insertpos = wad.Lumps.Count;

			// Create the lump from memory
			Lump lump = wad.Insert("VERTEXES", insertpos, (int)mem.Length);
			lump.Stream.Seek(0, SeekOrigin.Begin);
			mem.WriteTo(lump.Stream);
		}

		// This writes the LINEDEFS to WAD file
		private void WriteLinedefs(MapSet map, int position, Dictionary<string, MapLumpInfo> maplumps, IDictionary<Sidedef, int> sidedefids, IDictionary<Vertex, int> vertexids)
		{
			// Create memory to write to
			MemoryStream mem = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(mem, WAD.ENCODING);

			// Go for all lines
			foreach(Linedef l in map.Linedefs)
			{
				// Convert flags
				int flags = 0;
				foreach(KeyValuePair<string, bool> f in l.Flags)
				{
					int fnum;
					if(f.Value && int.TryParse(f.Key, out fnum)) flags |= fnum;
				}

				// Add activates to flags
				flags |= (l.Activate & manager.Config.LinedefActivationsFilter);
				
				// Write properties to stream
				writer.Write((UInt16)vertexids[l.Start]);
				writer.Write((UInt16)vertexids[l.End]);
				writer.Write((UInt16)flags);
				writer.Write((Byte)l.Action);
				writer.Write((Byte)l.Args[0]);
				writer.Write((Byte)l.Args[1]);
				writer.Write((Byte)l.Args[2]);
				writer.Write((Byte)l.Args[3]);
				writer.Write((Byte)l.Args[4]);

				// Front sidedef
				ushort sid = (l.Front == null ? ushort.MaxValue : (UInt16)sidedefids[l.Front]);
				writer.Write(sid);

				// Back sidedef
				sid = (l.Back == null ? ushort.MaxValue : (UInt16)sidedefids[l.Back]);
				writer.Write(sid);
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
		private void WriteSidedefs(MapSet map, int position, Dictionary<string, MapLumpInfo> maplumps, IDictionary<Sector, int> sectorids)
		{
			// Create memory to write to
			MemoryStream mem = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(mem, WAD.ENCODING);

			// Go for all sidedefs
			foreach(Sidedef sd in map.Sidedefs)
			{
				// Write properties to stream
				writer.Write((Int16)sd.OffsetX);
				writer.Write((Int16)sd.OffsetY);
				writer.Write(Lump.MakeFixedName(sd.HighTexture, WAD.ENCODING));
				writer.Write(Lump.MakeFixedName(sd.LowTexture, WAD.ENCODING));
				writer.Write(Lump.MakeFixedName(sd.MiddleTexture, WAD.ENCODING));
				writer.Write((UInt16)sectorids[sd.Sector]);
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
		private void WriteSectors(MapSet map, int position, Dictionary<string, MapLumpInfo> maplumps)
		{
			// Create memory to write to
			MemoryStream mem = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(mem, WAD.ENCODING);

			// Go for all sectors
			foreach(Sector s in map.Sectors)
			{
				// Write properties to stream
				writer.Write((Int16)s.FloorHeight);
				writer.Write((Int16)s.CeilHeight);
				writer.Write(Lump.MakeFixedName(s.FloorTexture, WAD.ENCODING));
				writer.Write(Lump.MakeFixedName(s.CeilTexture, WAD.ENCODING));
				writer.Write((Int16)s.Brightness);
				writer.Write((UInt16)s.Effect);
				writer.Write((UInt16)s.Tag);
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
		
		#endregion
	}
}
