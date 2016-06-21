
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
using System.IO;
using System.IO.Compression;
using System.Drawing;
using CodeImp.DoomBuilder.Data;
using CodeImp.DoomBuilder.Rendering;
using System.Drawing.Imaging;

#endregion

namespace CodeImp.DoomBuilder.IO
{
	internal unsafe class BGFReader : IImageReader
	{
		#region ================== Variables

		// Palette to use
		private readonly Playpal palette;
		private int shrinkFactor;

		public int ShrinkFactor { get { return shrinkFactor; } }

		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		public BGFReader(Playpal palette)
		{
			// Initialize
			this.palette = palette;

			// We have no destructor
			GC.SuppressFinalize(this);
		}

		#endregion

		#region ================== Methods
		
		// This validates the data as BGF file
		public bool Validate(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);

			// Initialize
			int datalength = (int)stream.Length - (int)stream.Position;

			// Need at least 4 bytes
			if (datalength < 4)
				return false;

			uint signature = reader.ReadUInt32();
			if (signature != 0x11464742)
				return false;

			uint version = reader.ReadUInt32();
			if (version < 9)
				return false;

			return true;
		}
		
		// This creates a Bitmap from the given data
		// Returns null on failure
		public Bitmap ReadAsBitmap(Stream stream)
		{
			int x, y;
			return ReadAsBitmap(stream, out x, out y);
		}
		
		// This creates a Bitmap from the given data
		// Returns null on failure
		public Bitmap ReadAsBitmap(Stream stream, out int offsetx, out int offsety)
		{
			BinaryReader reader = new BinaryReader(stream);

			stream.Seek(8, SeekOrigin.Begin);

			// Bitmap string name.
			string str = reader.ReadBytes(32).ToString();
			
			// Skip group counts and max indices.
			reader.ReadBytes(12);

			// Read shrink factor (image scale factor).
			shrinkFactor = (int)reader.ReadUInt32();

			// Read in just the first bitmap.
			uint width = reader.ReadUInt32();
			uint height = reader.ReadUInt32();
			offsetx = reader.ReadInt32();
			offsety = reader.ReadInt32();

			// Skip any hotspot data.
			byte hotspotCount = reader.ReadByte();
			for (int i = 0; i < hotspotCount; ++i)
				reader.ReadBytes(9);

			// BGF bitmaps can be compressed.
			bool isCompressed = reader.ReadBoolean();
			int datalength = reader.ReadInt32();
			byte[] rawdata;

			// If bitmap is compressed, uncompress first.
			if (isCompressed)
			{
				byte[] compressed = reader.ReadBytes(datalength);
				// init sourcestream
				rawdata = new byte[width * height];
				MemoryStream srcStream = new MemoryStream(compressed, false);

				// must skip two bytes not part of deflate but used by zlib
				srcStream.ReadByte();
				srcStream.ReadByte();

				// init .net decompressor
				DeflateStream destZ = new DeflateStream(srcStream, CompressionMode.Decompress);

				// decompress
				destZ.Read(rawdata, 0, (int)(width * height));

				// cleanup                
				destZ.Dispose();
				srcStream.Dispose();
			}
			else
			{
				rawdata = reader.ReadBytes((int)(width * height));
			}

			Bitmap bmp;
			// Convert the raw data to pixel format.
			PixelColorBlock pixeldata = ReadAsPixelData(rawdata, (int)width, (int)height);

			if(pixeldata != null)
			{
				// Create bitmap and lock pixels
				try
				{
					bmp = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);

					BitmapData bitmapdata = bmp.LockBits(new Rectangle(0, 0, (int)width, (int)height),
						ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
					PixelColor* targetdata = (PixelColor*)bitmapdata.Scan0.ToPointer();

					// Copy the pixels
					General.CopyMemory(targetdata, pixeldata.Pointer, (uint)(width * height * sizeof(PixelColor)));

					// Done
					bmp.UnlockBits(bitmapdata);

					// Set cyan as the transparent color.
					bmp.MakeTransparent(System.Drawing.Color.Cyan);
					// Add shrink factor from the bgf.
					if (false && shrinkFactor > 1)
						bmp = new Bitmap(bmp, new Size(bmp.Size.Width / (int)shrinkFactor, bmp.Size.Height / (int)shrinkFactor));
				}
				catch(Exception e)
				{
					// Unable to make bitmap
					General.ErrorLogger.Add(ErrorType.Error, "Unable to make Meridian 59 picture data. "
						+ e.GetType().Name + ": " + e.Message);
					return null;
				}
			}
			else
			{
				// Failed loading picture
				bmp = null;
			}

			// Return result
			return bmp;
		}

		// Not needed for BGFReader.
		public void DrawToPixelData(Stream stream, PixelColor* target, int targetwidth, int targetheight, int x, int y)
		{
			return;
		}

		// This creates pixel color data from the given data
		// Returns null on failure
		private PixelColorBlock ReadAsPixelData(byte[] data, int width, int height)
		{
			MemoryStream stream = new MemoryStream(data, false);

			// Valid width and height?
			if ((width <= 0) || (height <= 0))
				return null;

			// Allocate memory
			PixelColorBlock pixeldata = new PixelColorBlock(width, height);
			pixeldata.Clear();

			for (int i = 0; i < height * width; ++i)
				pixeldata.Pointer[i] = palette[stream.ReadByte()];

			// Return pointer
			return pixeldata;
		}

		#endregion
	}
}
