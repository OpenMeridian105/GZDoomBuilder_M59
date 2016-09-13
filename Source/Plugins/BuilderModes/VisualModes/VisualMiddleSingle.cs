
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
using System.Drawing;
using CodeImp.DoomBuilder.Map;
using CodeImp.DoomBuilder.Geometry;
using CodeImp.DoomBuilder.Rendering;
using CodeImp.DoomBuilder.Types;
using CodeImp.DoomBuilder.VisualModes;
using CodeImp.DoomBuilder.Data;

#endregion

namespace CodeImp.DoomBuilder.BuilderModes
{
	internal sealed class VisualMiddleSingle : BaseVisualGeometrySidedef
	{
		#region ================== Constants

		#endregion
		
		#region ================== Variables

		private bool repeatmidtex;
		private Plane topclipplane;
		private Plane bottomclipplane;

		#endregion
		
		#region ================== Properties

		#endregion
		
		#region ================== Constructor / Setup
		
		// Constructor
		public VisualMiddleSingle(BaseVisualMode mode, VisualSector vs, Sidedef s) : base(mode, vs, s)
		{
			//mxd
			geometrytype = VisualGeometryType.WALL_MIDDLE;
			partname = "mid";
			
			// We have no destructor
			GC.SuppressFinalize(this);
		}

		private bool SetupMeridian()
		{
			Vector2D vl, vr;
			float h0, h1, h2, h3;
			bool drawTopDown;
			skyhack = false;
			//mxd. Apply sky hack?
			UpdateSkyRenderFlag();

			//mxd. lightfog flag support
			int lightvalue;
			bool lightabsolute;
			GetLightValue(out lightvalue, out lightabsolute);

			// Load sector data
			SectorData sd = mode.GetSectorData(Sidedef.Sector);

			CalculateWallSideHeights();

			// Left and right vertices for this sidedef
			if (Sidedef.IsFront)
			{
				vl = new Vector2D(Sidedef.Line.Start.Position.x, Sidedef.Line.Start.Position.y);
				vr = new Vector2D(Sidedef.Line.End.Position.x, Sidedef.Line.End.Position.y);
				h0 = z2;
				h1 = z1;
				h2 = zz1;
				h3 = zz2;
				drawTopDown = (Sidedef.Line.IsFlagSet("65536"));
			}
			else
			{
				vl = new Vector2D(Sidedef.Line.End.Position.x, Sidedef.Line.End.Position.y);
				vr = new Vector2D(Sidedef.Line.Start.Position.x, Sidedef.Line.Start.Position.y);
				h0 = zz2;
				h1 = zz1;
				h2 = z1;
				h3 = z2;
				drawTopDown = (Sidedef.Line.IsFlagSet("131072"));
			}

			// Texture given?
			if (Sidedef.LongMiddleTexture != MapSet.EmptyLongName)
			{
				// Load texture
				base.Texture = General.Map.Data.GetTextureImage(Sidedef.LongMiddleTexture);
				if (base.Texture == null || base.Texture is UnknownImage)
				{
					base.Texture = General.Map.Data.UnknownTexture3D;
					setuponloadedtexture = Sidedef.LongMiddleTexture;
				}
				else
				{
					if (!base.Texture.IsImageLoaded)
						setuponloadedtexture = Sidedef.LongMiddleTexture;
				}
			}
			else
			{
				// Use missing texture
				base.Texture = General.Map.Data.MissingTexture3D;
				setuponloadedtexture = 0;
			}

			// Determine texture coordinates plane as they would be in normal circumstances.
			// We can then use this plane to find any texture coordinate we need.
			TexturePlane tp = CalculateTexturePlane(h0, h1, h2, h3, drawTopDown);

			float geotop = Sidedef.Sector.CeilHeight;
			float geobottom = Sidedef.Sector.FloorHeight;

			// Left top and right bottom of the geometry that
			tp.vlt = new Vector3D(vl.x, vl.y, h0);
			tp.vrb = new Vector3D(vr.x, vr.y, h2);
			tp.vlb = new Vector3D(vl.x, vl.y, h1);
			tp.vrt = new Vector3D(vr.x, vr.y, h3);

			// Get ceiling and floor heights
			float fl = sd.Floor.plane.GetZ(vl);
			float fr = sd.Floor.plane.GetZ(vr);
			float cl = sd.Ceiling.plane.GetZ(vl);
			float cr = sd.Ceiling.plane.GetZ(vr);

			// Anything to see?
			if (((cl - fl) > 0.01f) || ((cr - fr) > 0.01f))
			{
				// Keep top and bottom planes for intersection testing
				top = sd.Ceiling.plane;
				bottom = sd.Floor.plane;

				// Create initial polygon, which is just a quad between floor and ceiling
				WallPolygon poly = new WallPolygon();
				poly.Add(new Vector3D(vl.x, vl.y, fl));
				poly.Add(new Vector3D(vl.x, vl.y, cl));
				poly.Add(new Vector3D(vr.x, vr.y, cr));
				poly.Add(new Vector3D(vr.x, vr.y, fr));

				// Determine initial color
				int lightlevel = lightabsolute ? lightvalue : sd.Ceiling.brightnessbelow + lightvalue;

				//mxd. This calculates light with doom-style wall shading
				PixelColor wallbrightness = PixelColor.FromInt(mode.CalculateBrightness(lightlevel, Sidedef));
				PixelColor wallcolor = PixelColor.Modulate(sd.Ceiling.colorbelow, wallbrightness);
				fogfactor = CalculateFogFactor(lightlevel);
				poly.color = wallcolor.WithAlpha(255).ToInt();

				// Determine if we should repeat the middle texture
				repeatmidtex = !Sidedef.IsNoVTile();

				if (!repeatmidtex)
				{
					skyhack = true;
					if (tp.tlt.y < 0.0f)
					{
						float tex, wall, ratio, temp;

						tex = tp.tlb.y - tp.tlt.y;
						if (tex == 0)
							tex = 1.0f;
						temp = -tp.tlt.y;
						ratio = temp / tex;

						wall = tp.vlt.z - tp.vlb.z;
						temp = wall * ratio;
						tp.vlt.z -= temp;
						tp.tlt.y = 0.0f;
					}

					if (tp.trt.y < 0.0f)
					{
						float tex, wall, ratio, temp;

						tex = tp.trb.y - tp.trt.y;
						if (tex == 0)
							tex = 1.0f;
						temp = -tp.trt.y;
						ratio = temp / tex;

						wall = tp.vrt.z - tp.vrb.z;
						temp = wall * ratio;
						tp.vrt.z -= temp;
						tp.trt.y = 0.0f;
					}

					// Create crop planes (we also need these for intersection testing)
					topclipplane = new Plane(new Vector3D(0, 0, -1), Math.Max(tp.vlt.z, tp.vrt.z));
					bottomclipplane = new Plane(new Vector3D(0, 0, 1), -Math.Min(tp.vlb.z, tp.vrb.z));

					// Crop polygon by these heights
					CropPoly(ref poly, topclipplane, true);
					CropPoly(ref poly, bottomclipplane, true);
				}

				// Cut out pieces that overlap 3D floors in this sector
				List<WallPolygon> polygons = new List<WallPolygon> { poly };
				ClipExtraFloors(polygons, sd.ExtraFloors, false); //mxd

				if (polygons.Count > 0)
				{
					// Process the polygon and create vertices
					List<WorldVertex> verts = CreatePolygonVertices(polygons, tp, sd, lightvalue, lightabsolute);
					if (verts.Count > 2)
					{
						base.SetVertices(verts);
						return true;
					}
				}
			}

			base.SetVertices(null); //mxd
			return false;
		}

		public bool SkySideSetup()
		{
			Vector2D vl, vr;
			float h0, h1, h2, h3;
			bool drawTopDown;

			renderassky = true;

			//mxd. lightfog flag support
			int lightvalue;
			bool lightabsolute;
			GetLightValue(out lightvalue, out lightabsolute);

			// Load sector data
			SectorData sd = mode.GetSectorData(Sidedef.Sector);

			CalculateWallSideHeights();

			// Left and right vertices for this sidedef
			if (Sidedef.IsFront)
			{
				vl = new Vector2D(Sidedef.Line.Start.Position.x, Sidedef.Line.Start.Position.y);
				vr = new Vector2D(Sidedef.Line.End.Position.x, Sidedef.Line.End.Position.y);
				h0 = z2;
				h1 = z1;
				h2 = zz1;
				h3 = zz2;
				drawTopDown = (Sidedef.Line.IsFlagSet("65536"));
			}
			else
			{
				vl = new Vector2D(Sidedef.Line.End.Position.x, Sidedef.Line.End.Position.y);
				vr = new Vector2D(Sidedef.Line.Start.Position.x, Sidedef.Line.Start.Position.y);
				h0 = zz2;
				h1 = zz1;
				h2 = z1;
				h3 = z2;
				drawTopDown = (Sidedef.Line.IsFlagSet("131072"));
			}

			// Texture given?
			if (Sidedef.LongMiddleTexture != MapSet.EmptyLongName)
			{
				// Load texture
				base.Texture = General.Map.Data.GetTextureImage(Sidedef.LongMiddleTexture);
				if (base.Texture == null || base.Texture is UnknownImage)
				{
					base.Texture = General.Map.Data.UnknownTexture3D;
					setuponloadedtexture = Sidedef.LongMiddleTexture;
				}
				else
				{
					if (!base.Texture.IsImageLoaded)
						setuponloadedtexture = Sidedef.LongMiddleTexture;
				}
			}
			else
			{
				// Use missing texture
				base.Texture = General.Map.Data.MissingTexture3D;
				setuponloadedtexture = 0;
			}

			// Determine texture coordinates plane as they would be in normal circumstances.
			// We can then use this plane to find any texture coordinate we need.
			TexturePlane tp = CalculateTexturePlane(h0, h1, h2, h3, drawTopDown);

			float geotop = Sidedef.Sector.CeilHeight;
			float geobottom = Sidedef.Sector.FloorHeight;

			// Left top and right bottom of the geometry that
			tp.vlt = new Vector3D(vl.x, vl.y, geotop);
			tp.vrb = new Vector3D(vr.x, vr.y, geobottom);
			tp.vlb = new Vector3D(vl.x, vl.y, geobottom);
			tp.vrt = new Vector3D(vr.x, vr.y, geotop);

			// Get ceiling and floor heights
			float fl = sd.Floor.plane.GetZ(vl);
			float fr = sd.Floor.plane.GetZ(vr);
			float cl = sd.Ceiling.plane.GetZ(vl);
			float cr = sd.Ceiling.plane.GetZ(vr);

			// Anything to see?
			if (((cl - fl) > 0.01f) || ((cr - fr) > 0.01f))
			{
				// Keep top and bottom planes for intersection testing
				top = sd.Ceiling.plane;
				bottom = sd.Floor.plane;

				// Create initial polygon, which is just a quad between floor and ceiling
				WallPolygon poly = new WallPolygon();
				poly.Add(new Vector3D(vl.x, vl.y, fl));
				poly.Add(new Vector3D(vl.x, vl.y, cl));
				poly.Add(new Vector3D(vr.x, vr.y, cr));
				poly.Add(new Vector3D(vr.x, vr.y, fr));

				// Determine initial color
				int lightlevel = lightabsolute ? lightvalue : sd.Ceiling.brightnessbelow + lightvalue;

				//mxd. This calculates light with doom-style wall shading
				PixelColor wallbrightness = PixelColor.FromInt(mode.CalculateBrightness(lightlevel, Sidedef));
				PixelColor wallcolor = PixelColor.Modulate(sd.Ceiling.colorbelow, wallbrightness);
				fogfactor = CalculateFogFactor(lightlevel);
				poly.color = wallcolor.WithAlpha(255).ToInt();

				// Cut out pieces that overlap 3D floors in this sector
				List<WallPolygon> polygons = new List<WallPolygon> { poly };
				ClipExtraFloors(polygons, sd.ExtraFloors, false); //mxd

				if (polygons.Count > 0)
				{
					// Process the polygon and create vertices
					List<WorldVertex> verts = CreatePolygonVertices(polygons, tp, sd, lightvalue, lightabsolute);
					if (verts.Count > 2)
					{
						base.SetVertices(verts);
						return true;
					}
				}
			}

			base.SetVertices(null); //mxd
			return false;
		}

		// This builds the geometry. Returns false when no geometry created.
		public override bool Setup()
		{
			if (General.Map.MERIDIAN)
				return SetupMeridian();

			Vector2D vl, vr;

			//mxd. Apply sky hack?
			UpdateSkyRenderFlag();

			//mxd. lightfog flag support
			int lightvalue;
			bool lightabsolute;
			GetLightValue(out lightvalue, out lightabsolute);

			Vector2D tscale = new Vector2D(Sidedef.Fields.GetValue("scalex_mid", 1.0f),
										   Sidedef.Fields.GetValue("scaley_mid", 1.0f));
			Vector2D toffset = new Vector2D(Sidedef.Fields.GetValue("offsetx_mid", 0.0f),
											Sidedef.Fields.GetValue("offsety_mid", 0.0f));
			
			// Left and right vertices for this sidedef
			if(Sidedef.IsFront)
			{
				vl = new Vector2D(Sidedef.Line.Start.Position.x, Sidedef.Line.Start.Position.y);
				vr = new Vector2D(Sidedef.Line.End.Position.x, Sidedef.Line.End.Position.y);
			}
			else
			{
				vl = new Vector2D(Sidedef.Line.End.Position.x, Sidedef.Line.End.Position.y);
				vr = new Vector2D(Sidedef.Line.Start.Position.x, Sidedef.Line.Start.Position.y);
			}

			// Load sector data
			SectorData sd = mode.GetSectorData(Sidedef.Sector);
			
			// Texture given?
			if(Sidedef.LongMiddleTexture != MapSet.EmptyLongName)
			{
				// Load texture
				base.Texture = General.Map.Data.GetTextureImage(Sidedef.LongMiddleTexture);
				if(base.Texture == null || base.Texture is UnknownImage)
				{
					base.Texture = General.Map.Data.UnknownTexture3D;
					setuponloadedtexture = Sidedef.LongMiddleTexture;
				}
				else
				{
					if(!base.Texture.IsImageLoaded)
						setuponloadedtexture = Sidedef.LongMiddleTexture;
				}
			}
			else
			{
				// Use missing texture
				base.Texture = General.Map.Data.MissingTexture3D;
				setuponloadedtexture = 0;
			}
			
			// Get texture scaled size
			Vector2D tsz = new Vector2D(base.Texture.ScaledWidth, base.Texture.ScaledHeight);
			tsz = tsz / tscale;
			
			// Get texture offsets
			Vector2D tof = new Vector2D(Sidedef.OffsetX, Sidedef.OffsetY);
			tof = tof + toffset;
			tof = tof / tscale;
			if(General.Map.Config.ScaledTextureOffsets && !base.Texture.WorldPanning)
				tof = tof * base.Texture.Scale;
			
			// Determine texture coordinates plane as they would be in normal circumstances.
			// We can then use this plane to find any texture coordinate we need.
			// The logic here is the same as in the original VisualMiddleSingle (except that
			// the values are stored in a TexturePlane)
			// NOTE: I use a small bias for the floor height, because if the difference in
			// height is 0 then the TexturePlane doesn't work!
			TexturePlane tp = new TexturePlane();
			float floorbias = (Sidedef.Sector.CeilHeight == Sidedef.Sector.FloorHeight) ? 1.0f : 0.0f;
			if(Sidedef.Line.IsFlagSet(General.Map.Config.LowerUnpeggedFlag))
			{
				// When lower unpegged is set, the middle texture is bound to the bottom
				tp.tlt.y = tsz.y - (Sidedef.Sector.CeilHeight - Sidedef.Sector.FloorHeight);
			}
			tp.trb.x = tp.tlt.x + (float)Math.Round(Sidedef.Line.Length); //mxd. (G)ZDoom snaps texture coordinates to integral linedef length
			tp.trb.y = tp.tlt.y + (Sidedef.Sector.CeilHeight - (Sidedef.Sector.FloorHeight + floorbias));
			
			// Apply texture offset
			tp.tlt += tof;
			tp.trb += tof;
			
			// Transform pixel coordinates to texture coordinates
			tp.tlt /= tsz;
			tp.trb /= tsz;
			
			// Left top and right bottom of the geometry that
			tp.vlt = new Vector3D(vl.x, vl.y, Sidedef.Sector.CeilHeight);
			tp.vrb = new Vector3D(vr.x, vr.y, Sidedef.Sector.FloorHeight + floorbias);
			tp.vlb = new Vector3D(vl.x, vl.y, Sidedef.Sector.FloorHeight + floorbias);
			// Make the right-top coordinates
			tp.trt = new Vector2D(tp.trb.x, tp.tlt.y);
			tp.vrt = new Vector3D(tp.vrb.x, tp.vrb.y, tp.vlt.z);
			

			// Get ceiling and floor heights
			float fl = sd.Floor.plane.GetZ(vl);
			float fr = sd.Floor.plane.GetZ(vr);
			float cl = sd.Ceiling.plane.GetZ(vl);
			float cr = sd.Ceiling.plane.GetZ(vr);

			// Anything to see?
			if(((cl - fl) > 0.01f) || ((cr - fr) > 0.01f))
			{
				// Keep top and bottom planes for intersection testing
				top = sd.Ceiling.plane;
				bottom = sd.Floor.plane;
				
				// Create initial polygon, which is just a quad between floor and ceiling
				WallPolygon poly = new WallPolygon();
				poly.Add(new Vector3D(vl.x, vl.y, fl));
				poly.Add(new Vector3D(vl.x, vl.y, cl));
				poly.Add(new Vector3D(vr.x, vr.y, cr));
				poly.Add(new Vector3D(vr.x, vr.y, fr));
				
				// Determine initial color
				int lightlevel = lightabsolute ? lightvalue : sd.Ceiling.brightnessbelow + lightvalue;

				//mxd. This calculates light with doom-style wall shading
				PixelColor wallbrightness = PixelColor.FromInt(mode.CalculateBrightness(lightlevel, Sidedef));
				PixelColor wallcolor = PixelColor.Modulate(sd.Ceiling.colorbelow, wallbrightness);
				fogfactor = CalculateFogFactor(lightlevel);
				poly.color = wallcolor.WithAlpha(255).ToInt();

				// Cut out pieces that overlap 3D floors in this sector
				List<WallPolygon> polygons = new List<WallPolygon> { poly };
				ClipExtraFloors(polygons, sd.ExtraFloors, false); //mxd

				if(polygons.Count > 0)
				{
					// Process the polygon and create vertices
					List<WorldVertex> verts = CreatePolygonVertices(polygons, tp, sd, lightvalue, lightabsolute);
					if(verts.Count > 2)
					{
						base.SetVertices(verts);
						return true;
					}
				}
			}
			
			base.SetVertices(null); //mxd
			return false;
		}

		//mxd
		internal void UpdateSkyRenderFlag()
		{
			renderassky = (Sidedef.LongMiddleTexture == MapSet.EmptyLongName && Sidedef.Sector != null && Sidedef.Sector.CeilTexture == General.Map.Config.SkyFlatName);
		}

		#endregion
		
		#region ================== Methods

		// This performs a fast test in object picking
		public override bool PickFastReject(Vector3D from, Vector3D to, Vector3D dir)
		{
			if (!repeatmidtex)
			{
				// When the texture is not repeated, leave when outside crop planes
				if ((pickintersect.z < bottomclipplane.GetZ(pickintersect)) ||
				   (pickintersect.z > topclipplane.GetZ(pickintersect)))
					return false;
			}

			return base.PickFastReject(from, to, dir);
		}

		// Return texture name
		public override string GetTextureName()
		{
			return this.Sidedef.MiddleTexture;
		}

		// This changes the texture
		protected override void SetTexture(string texturename)
		{
			this.Sidedef.SetTextureMid(texturename);
			General.Map.Data.UpdateUsedTextures();
			this.Setup();
		}

		protected override void SetTextureOffsetX(int x)
		{
			Sidedef.Fields.BeforeFieldsChange();
			Sidedef.Fields["offsetx_mid"] = new UniValue(UniversalType.Float, (float)x);
		}

		protected override void SetTextureOffsetY(int y)
		{
			Sidedef.Fields.BeforeFieldsChange();
			Sidedef.Fields["offsety_mid"] = new UniValue(UniversalType.Float, (float)y);
		}

		protected override void MoveTextureOffset(int offsetx, int offsety)
		{
			Sidedef.Fields.BeforeFieldsChange();
			float oldx = Sidedef.Fields.GetValue("offsetx_mid", 0.0f);
			float oldy = Sidedef.Fields.GetValue("offsety_mid", 0.0f);
			float scalex = Sidedef.Fields.GetValue("scalex_mid", 1.0f);
			float scaley = Sidedef.Fields.GetValue("scaley_mid", 1.0f);
			Sidedef.Fields["offsetx_mid"] = new UniValue(UniversalType.Float, GetRoundedTextureOffset(oldx, offsetx, scalex, Texture.Width)); //mxd
			Sidedef.Fields["offsety_mid"] = new UniValue(UniversalType.Float, GetRoundedTextureOffset(oldy, offsety, scaley, Texture.Height)); //mxd
		}

		protected override Point GetTextureOffset()
		{
			float oldx = Sidedef.Fields.GetValue("offsetx_mid", 0.0f);
			float oldy = Sidedef.Fields.GetValue("offsety_mid", 0.0f);
			return new Point((int)oldx, (int)oldy);
		}

		//mxd
		protected override void ResetTextureScale() 
		{
			Sidedef.Fields.BeforeFieldsChange();
			if(Sidedef.Fields.ContainsKey("scalex_mid")) Sidedef.Fields.Remove("scalex_mid");
			if(Sidedef.Fields.ContainsKey("scaley_mid")) Sidedef.Fields.Remove("scaley_mid");
		}

		//mxd
		public override void OnTextureFit(FitTextureOptions options) 
		{
			if(!General.Map.UDMF) return;
			if(string.IsNullOrEmpty(Sidedef.MiddleTexture) || Sidedef.MiddleTexture == "-" || !Texture.IsImageLoaded) return;
			FitTexture(options);
			Setup();

			// Update linked effects
			SectorData sd = mode.GetSectorDataEx(Sector.Sector);
			if(sd != null) sd.Reset(true);
		}
		
		#endregion
	}
}
