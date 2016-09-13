
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
using System.Drawing;
using System.Windows.Forms;
using CodeImp.DoomBuilder.Map;
using CodeImp.DoomBuilder.Types;
using CodeImp.DoomBuilder.Geometry;

#endregion

namespace CodeImp.DoomBuilder.Windows
{
	internal partial class SectorEditFormMeridian : DelayedForm
	{
		#region ================== Events

		public event EventHandler OnValuesChanged; //mxd

		#endregion

		#region ================== Variables

		private ICollection<Sector> sectors;
		private List<SectorProperties> sectorprops; //mxd
		private bool preventchanges; //mxd
		private bool undocreated; //mxd

		private struct SectorProperties //mxd
		{
			public readonly int Brightness;
			public readonly int FloorHeight;
			public readonly int CeilHeight;
			public readonly string FloorTexture;
			public readonly string CeilTexture;
			public readonly int TextureXOffset;
			public readonly int TextureYOffset;
			public readonly int SectorTag;
			public readonly int AnimationSpeed;
			public readonly bool Flicker;
			public readonly int Depth;
			public readonly SDScrollFlags ScrollFlags;
			public readonly bool ScrollCeiling;
			public readonly bool ScrollFloor;
			public readonly int CeilTexRot;
			public readonly int FloorTexRot;

			public SectorProperties(Sector s) 
			{
				FloorHeight = s.FloorHeight;
				CeilHeight = s.CeilHeight;
				FloorTexture = s.FloorTexture;
				CeilTexture = s.CeilTexture;
				TextureXOffset = s.OffsetX;
				TextureYOffset = s.OffsetY;
				SectorTag = s.SectorTag;
				AnimationSpeed = s.AnimationSpeed;
				Flicker = s.Flicker;
				if (s.IsNoAmbient())
					Brightness = s.NoAmbientBrightness();
				else
					Brightness = s.AmbientBrightness();
				Depth = s.Depth;
				ScrollFlags = s.ScrollFlags;
				ScrollCeiling = s.ScrollCeiling;
				ScrollFloor = s.ScrollFloor;

				CeilTexRot = s.CeilTexRot;
				FloorTexRot = s.FloorTexRot;
			}
		}

		#endregion

		#region ================== Constructor

		// Constructor
		public SectorEditFormMeridian()
		{
			// Initialize
			InitializeComponent();

			// Initialize image selectors
			floortex.Initialize();
			ceilingtex.Initialize();

			// Set steps for brightness field
			brightness.StepValues = General.Map.Config.BrightnessLevels;
		}

		#endregion

		#region ================== Methods

		// This sets up the form to edit the given sectors
		public void Setup(ICollection<Sector> sectors)
		{
			preventchanges = true; //mxd

			// Keep this list
			this.sectors = sectors;
			if(sectors.Count > 1) this.Text = "Edit Sectors (" + sectors.Count + ")";
			sectorprops = new List<SectorProperties>(); //mxd

			////////////////////////////////////////////////////////////////////////
			// Set all options to the first sector properties
			////////////////////////////////////////////////////////////////////////

			// Get first sector
			Sector sc = General.GetByIndex(sectors, 0);

			// Floor/ceiling
			floorheight.Text = sc.FloorHeight.ToString();
			ceilingheight.Text = sc.CeilHeight.ToString();
			floortex.TextureName = sc.FloorTexture;
			ceilingtex.TextureName = sc.CeilTexture;
			texturexoffset.Text = sc.OffsetX.ToString();
			textureyoffset.Text = sc.OffsetY.ToString();
			sectortag.Text = sc.SectorTag.ToString();

			// Light/effects
			int lightlevel = 0;
			if (sc.IsNoAmbient())
			{
				noambientbox.Checked = true;
				lightlevel = sc.NoAmbientBrightness();
			}
			else
			{
				noambientbox.Checked = false;
				lightlevel = sc.AmbientBrightness();
			}
			brightness.Text = lightlevel.ToString();
			flickerbox.Checked = sc.Flicker;
			animationspeed.Text = sc.AnimationSpeed.ToString();

			// Depth
			switch(sc.Depth)
			{
				case 1: depthshallow.Checked = true; break;
				case 2: depthdeep.Checked = true; break;
				case 3: depthvery.Checked = true; break;
				default: depthnone.Checked = true; break;
			}

			// Scrolling
			scrollceiling.Checked = sc.ScrollCeiling;
			scrollfloor.Checked = sc.ScrollFloor;
			SetSpeedButton(scrollspeed, sc.ScrollFlags);
			SetDirectionButton(scrolldirection, sc.ScrollFlags);

			// Slopes
			ceiltexrot.Text = sc.CeilTexRot.ToString();
			floortexrot.Text = sc.FloorTexRot.ToString();

			// List of all vertexes in the sector.
			List<Vertex> vlist = sc.GetVertexes();
			// Vertex positions associated with floor slope.
			List<Vector3D> floorvt = sc.FloorSlopeVertexes;
			// Vertex position associated with ceiling slope.
			List<Vector3D> ceilvt = sc.CeilSlopeVertexes;

			if (vlist.Count > 0)
			{
				foreach (Vertex V in vlist)
				{
					// Add the items to each list (should maybe have one list?)
					floorvert1.Items.Add(V.Index);
					floorvert2.Items.Add(V.Index);
					floorvert3.Items.Add(V.Index);
					ceilvert1.Items.Add(V.Index);
					ceilvert2.Items.Add(V.Index);
					ceilvert3.Items.Add(V.Index);

					// Find which vertex is associated with our current slope.
					// Only valid if there are 3 items in floor/ceil slope vert (position) list.
					if (floorvt.Count == 3)
					{
						if (Math.Round(floorvt[0].x) == V.Position.x && Math.Round(floorvt[0].y) == V.Position.y)
						{
							// // Used to get height from plane.
							//floorvert1height.Text = Math.Round(Sector.GetFloorPlane(sc).GetZ(V.Position)).ToString();
							floorvert1height.Text = floorvt[0].z.ToString();
							// Select this vertex index.
							floorvert1.SelectedItem = V.Index;
						}
						if (Math.Round(floorvt[1].x) == V.Position.x && Math.Round(floorvt[1].y) == V.Position.y)
						{
							floorvert2height.Text = floorvt[1].z.ToString();
							floorvert2.SelectedItem = V.Index;
						}
						if (Math.Round(floorvt[2].x) == V.Position.x && Math.Round(floorvt[2].y) == V.Position.y)
						{
							floorvert3height.Text = floorvt[2].z.ToString();
							floorvert3.SelectedItem = V.Index;
						}
					}
					if (ceilvt.Count == 3)
					{
						if (Math.Round(ceilvt[0].x) == V.Position.x && Math.Round(ceilvt[0].y) == V.Position.y)
						{
							// Used to get height from plane.
							//ceilvert1height.Text = Math.Round(Sector.GetCeilingPlane(sc).GetZ(V.Position)).ToString();
							ceilvert1height.Text = ceilvt[0].z.ToString();
							ceilvert1.SelectedItem = V.Index;
						}
						if (Math.Round(ceilvt[1].x) == V.Position.x && Math.Round(ceilvt[1].y) == V.Position.y)
						{
							ceilvert2height.Text = ceilvt[1].z.ToString();
							ceilvert2.SelectedItem = V.Index;
						}
						if (Math.Round(ceilvt[2].x) == V.Position.x && Math.Round(ceilvt[2].y) == V.Position.y)
						{
							ceilvert3height.Text = ceilvt[2].z.ToString();
							ceilvert3.SelectedItem = V.Index;
						}
					}
				}
				// Some slopes don't have vertexes in the same sector... bug or intended?
				// Either way, add the height and find the vertex. Don't select it for now.
				if (floorvt.Count == 3)
				{
					if (floorvert1height.Text == "")
					{
						Vertex V = MapSet.NearestVertex(sc.Map.Vertices, floorvt[0]);
						if (V != null) floorvert1.Items.Add(V.Index);
						floorvert1height.Text = Math.Round(floorvt[0].z).ToString();
					}
					if (floorvert2height.Text == "")
					{
						Vertex V = MapSet.NearestVertex(sc.Map.Vertices, floorvt[1]);
						if (V != null) floorvert2.Items.Add(V.Index);
						floorvert2height.Text = Math.Round(floorvt[1].z).ToString();
					}
					if (floorvert3height.Text == "")
					{
						Vertex V = MapSet.NearestVertex(sc.Map.Vertices, floorvt[2]);
						if (V != null) floorvert3.Items.Add(V.Index);
						floorvert3height.Text = Math.Round(floorvt[2].z).ToString();
					}
				}
				if (ceilvt.Count == 3)
				{
					if (ceilvert1height.Text == "")
					{
						Vertex V = MapSet.NearestVertex(sc.Map.Vertices, ceilvt[0]);
						if (V != null) ceilvert1.Items.Add(V.Index);
						ceilvert1height.Text = Math.Round(ceilvt[0].z).ToString();
					}
					if (ceilvert2height.Text == "")
					{
						Vertex V = MapSet.NearestVertex(sc.Map.Vertices, ceilvt[1]);
						if (V != null) ceilvert2.Items.Add(V.Index);
						ceilvert2height.Text = Math.Round(ceilvt[1].z).ToString();
					}
					if (ceilvert3height.Text == "")
					{
						Vertex V = MapSet.NearestVertex(sc.Map.Vertices, ceilvt[2]);
						if (V != null) ceilvert3.Items.Add(V.Index);
						ceilvert3height.Text = Math.Round(ceilvt[2].z).ToString();
					}
				}
			}

			////////////////////////////////////////////////////////////////////////
			// Now go for all sectors and change the options when a setting is different
			////////////////////////////////////////////////////////////////////////

			// Go for all sectors
			foreach(Sector s in sectors)
			{
				// Floor/Ceiling
				if (s.FloorHeight.ToString() != floorheight.Text) floorheight.Text = "";
				if (s.CeilHeight.ToString() != ceilingheight.Text) ceilingheight.Text = "";
				if (s.FloorTexture != floortex.TextureName)
				{
					floortex.MultipleTextures = true; //mxd
					floortex.TextureName = "";
				}
				if (s.CeilTexture != ceilingtex.TextureName)
				{
					ceilingtex.MultipleTextures = true; //mxd
					ceilingtex.TextureName = "";
				}
				if (s.OffsetX.ToString() != texturexoffset.Text) texturexoffset.Text = "";
				if (s.OffsetY.ToString() != textureyoffset.Text) textureyoffset.Text = "";
				if (s.SectorTag.ToString() != sectortag.Text) sectortag.Text = "";

				// Light/effects
				if (s.Brightness != sc.Brightness) brightness.Text = "";
				if (noambientbox.CheckState != CheckState.Indeterminate)
				{
					if (s.IsNoAmbient() != noambientbox.Checked)
					{
						noambientbox.ThreeState = true;
						noambientbox.CheckState = CheckState.Indeterminate;
					}
				}

				if (flickerbox.CheckState != CheckState.Indeterminate)
				{
					if (s.Flicker != flickerbox.Checked)
					{
						flickerbox.ThreeState = true;
						flickerbox.CheckState = CheckState.Indeterminate;
					}
				}
				if (s.AnimationSpeed.ToString() != animationspeed.Text) animationspeed.Text = "";

				// Depth
				if (s.Depth != sc.Depth)
				{
					foreach (RadioButton r in depthbox.Controls)
					{
						r.Checked = false;
					}
				}

				// Scrolling
				if (s.ScrollFlags.Speed != sc.ScrollFlags.Speed)
				{
					foreach (RadioButton r in scrollspeed.Controls)
					{
						r.Checked = false;
					}
				}
				if (s.ScrollFlags.Direction != sc.ScrollFlags.Direction)
				{
					foreach (RadioButton r in scrolldirection.Controls)
					{
						r.Checked = false;
					}
				}

				if (scrollfloor.CheckState != CheckState.Indeterminate)
				{
					if (s.ScrollFloor != scrollfloor.Checked)
					{
						scrollfloor.ThreeState = true;
						scrollfloor.CheckState = CheckState.Indeterminate;
					}
				}
				if (scrollceiling.CheckState != CheckState.Indeterminate)
				{
					if (s.ScrollCeiling != scrollceiling.Checked)
					{
						scrollceiling.ThreeState = true;
						scrollceiling.CheckState = CheckState.Indeterminate;
					}
				}

				// Slopes
				if (s.CeilTexRot.ToString() != ceiltexrot.Text) ceiltexrot.Text = "";
				if (s.FloorTexRot.ToString() != floortexrot.Text) floortexrot.Text = "";
				if (FormHasFloorSlope())
				{
					// Cancel vertex selection.
					if (s.Index != sc.Index)
					{
						floorvert1.SelectedItem = null;
						floorvert2.SelectedItem = null;
						floorvert3.SelectedItem = null;
					}
					if (s.FloorSlopeVertexes.Count != 3)
						floorvert1height.Text = floorvert2height.Text = floorvert3height.Text = "";
					else
					{
						if (floorvert1height.Text != s.FloorSlopeVertexes[0].z.ToString())
							floorvert1height.Text = "";
						if (floorvert2height.Text != s.FloorSlopeVertexes[1].z.ToString())
							floorvert2height.Text = "";
						if (floorvert3height.Text != s.FloorSlopeVertexes[2].z.ToString())
							floorvert3height.Text = "";
					}
				}
				if (FormHasCeilSlope())
				{
					// Cancel vertex selection.
					if (s.Index != sc.Index)
					{
						ceilvert1.SelectedItem = null;
						ceilvert2.SelectedItem = null;
						ceilvert3.SelectedItem = null;
					}
					if (s.CeilSlopeVertexes.Count != 3)
						ceilvert1height.Text = ceilvert2height.Text = ceilvert3height.Text = "";
					else
					{
						if (ceilvert1height.Text != s.CeilSlopeVertexes[0].z.ToString())
							ceilvert1height.Text = "";
						if (ceilvert2height.Text != s.CeilSlopeVertexes[1].z.ToString())
							ceilvert2height.Text = "";
						if (ceilvert3height.Text != s.CeilSlopeVertexes[2].z.ToString())
							ceilvert3height.Text = "";
					}
				}

				//mxd. Store initial properties
				sectorprops.Add(new SectorProperties(s));
			}

			// Show sector height
			UpdateSectorHeight();

			preventchanges = false; //mxd
		}

		//mxd
		private void MakeUndo() 
		{
			if(undocreated) return;
			undocreated = true;

			//mxd. Make undo
			General.Map.UndoRedo.CreateUndo("Edit " + (sectors.Count > 1 ? sectors.Count + " sectors" : "sector"));
		}

		// This updates the sector height field
		private void UpdateSectorHeight()
		{
			int delta = 0;
			int index = -1; //mxd
			int i = 0; //mxd
			
			// Check all selected sectors
			foreach(Sector s in sectors) 
			{
				if(index == -1) 
				{
					// First sector in list
					delta = s.CeilHeight - s.FloorHeight;
					index = i; //mxd
				} 
				else if(delta != (s.CeilHeight - s.FloorHeight)) 
				{
					// We can't show heights because the delta
					// heights for the sectors is different
					index = -1;
					break;
				}

				i++;
			}

			if(index > -1) 
			{
				int fh = floorheight.GetResult(sectorprops[index].FloorHeight); //mxd
				int ch = ceilingheight.GetResult(sectorprops[index].CeilHeight); //mxd
				int height = ch - fh;
				sectorheight.Text = height.ToString();
				sectorheight.Visible = true;
				sectorheightlabel.Visible = true;
			} 
			else 
			{
				sectorheight.Visible = false;
				sectorheightlabel.Visible = false;
			}
		}

		private void UpdateCeilingHeight()
		{
			int i = 0;
			int diffheight = 0;

			if (ceilingheight.Text.StartsWith("++") || ceilingheight.Text.StartsWith("--"))
			{
				// Raise or lower by sector height
				foreach (Sector s in sectors)
				{
					if (i == 0)
						diffheight = (ceilingheight.GetResult(sectorprops[i].CeilHeight) - s.CeilHeight);
					s.CeilHeight += diffheight;

					// Fix up slopes too.
					if (s.IsCeilSloped())
					{
						for (int j = 0; j < 3; ++j)
						{
							s.CeilSlopeVertexes[j] = new Vector3D(s.CeilSlopeVertexes[j].x,
								s.CeilSlopeVertexes[j].y, s.CeilSlopeVertexes[j].z + diffheight);
						}
						s.CalculateMeridianSlope(false);
					}
					i++;
				}
			}
			else
			{
				//restore values
				if (string.IsNullOrEmpty(ceilingheight.Text))
				{
					foreach (Sector s in sectors)
						s.CeilHeight = sectorprops[i++].CeilHeight;
				}
				else //update values
				{
					foreach (Sector s in sectors)
						s.CeilHeight = ceilingheight.GetResult(sectorprops[i++].CeilHeight);
				}
			}
		}

		//mxd
		private void UpdateFloorHeight()
		{
			int i = 0;
			int diffheight = 0;

			if (floorheight.Text.StartsWith("++") || floorheight.Text.StartsWith("--"))
			{
				foreach (Sector s in sectors)
				{
					// Get the change in height from the first sector.
					if (i == 0)
						diffheight = (floorheight.GetResult(sectorprops[i].FloorHeight) - s.FloorHeight);
					s.FloorHeight += diffheight;

					// Fix up slopes too.
					if (s.IsFloorSloped())
					{
						for (int j = 0; j < 3; ++j)
						{
							s.FloorSlopeVertexes[j] = new Vector3D(s.FloorSlopeVertexes[j].x,
								s.FloorSlopeVertexes[j].y, s.FloorSlopeVertexes[j].z + diffheight);
						}
						s.CalculateMeridianSlope(true);
					}
					i++;
				}
			}
			else
			{
				//restore values
				if (string.IsNullOrEmpty(floorheight.Text))
				{
					foreach (Sector s in sectors)
						s.FloorHeight = sectorprops[i++].FloorHeight;
				}
				else //update values
				{
					foreach (Sector s in sectors)
						s.FloorHeight = floorheight.GetResult(sectorprops[i++].FloorHeight);
				}
			}
		}

		private void SetSpeedButton(GroupBox Box, SDScrollFlags Flags)
		{
			string str = SDScrollFlags.ScrollSpeed(Flags.Speed);
			RadioButton btn = Box.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Text == str);
			if (btn != null)
				btn.Checked = true;
		}

		private void SetDirectionButton(GroupBox Box, SDScrollFlags Flags)
		{
			string str = SDScrollFlags.ScrollDirection(Flags.Direction);
			RadioButton btn = Box.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Text == str);
			if (btn != null)
				btn.Checked = true;
		}

		private int GetDepthNum(string Name)
		{
			if (Name == "Shallow")
				return 1;
			if (Name == "Deep")
				return 2;
			if (Name == "Very deep")
				return 3;
			return 0;
		}

		private bool FormHasFloorSlope()
		{
			return (floorvert1height.Text != "" || floorvert2height.Text != "" || floorvert3height.Text != "");
		}

		private bool FormHasCeilSlope()
		{
			return (ceilvert1height.Text != "" || ceilvert2height.Text != "" || ceilvert3height.Text != "");
		}

		#endregion

		#region ================== Events

		// OK clicked
		private void apply_Click(object sender, EventArgs e)
		{
			MakeUndo(); //mxd

			// Go for all sectors
			foreach (Sector s in sectors)
			{
				// Floor/ceiling
				if (!String.IsNullOrEmpty(sectortag.Text))
					s.SectorTag = sectortag.GetResult(s.SectorTag);

				// Light/effects
				if (!String.IsNullOrEmpty(brightness.Text))
				{
					int lightLevel = brightness.GetResult(s.Brightness) / 2;

					if (noambientbox.Checked == false)
						lightLevel += 128;
					s.Brightness = lightLevel;
				}

				switch (flickerbox.CheckState)
				{
					case CheckState.Checked: s.Flicker = true; break;
					case CheckState.Unchecked: s.Flicker = false; break;
				}

				if (!String.IsNullOrEmpty(animationspeed.Text))
					s.AnimationSpeed = animationspeed.GetResult(s.AnimationSpeed);

				// Depth
				int depthNum = -1;
				foreach (RadioButton r in depthbox.Controls)
				{
					if (r.Checked)
					{
						depthNum = GetDepthNum(r.Text);
						break;
					}
				}
				if (depthNum >= 0) s.Depth = depthNum;

				// Scrolling
				switch (scrollceiling.CheckState)
				{
					case CheckState.Checked: s.ScrollCeiling = true; break;
					case CheckState.Unchecked: s.ScrollCeiling = false; break;
				}
				switch (scrollfloor.CheckState)
				{
					case CheckState.Checked: s.ScrollFloor = true; break;
					case CheckState.Unchecked: s.ScrollFloor = false; break;
				}
				int scrollSpeed = -1, scrollDir = -1;
				foreach (RadioButton r in scrollspeed.Controls)
				{
					if (r.Checked)
					{
						scrollSpeed = SDScrollFlags.ScrollSpeed(r.Text);
						break;
					}
				}
				foreach (RadioButton r in scrolldirection.Controls)
				{
					if (r.Checked)
					{
						scrollDir = SDScrollFlags.ScrollDirection(r.Text);
						break;
					}
				}
				if (scrollSpeed >= 0) s.ScrollFlags.Speed = scrollSpeed;
				if (scrollDir >= 0) s.ScrollFlags.Direction = scrollDir;
			}

			// Done
			General.Map.IsChanged = true;
			if(OnValuesChanged != null)	OnValuesChanged(this, EventArgs.Empty); //mxd
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		// Cancel clicked
		private void cancel_Click(object sender, EventArgs e)
		{
			//mxd. perform undo
			if(undocreated) General.Map.UndoRedo.WithdrawUndo();
			
			// And be gone
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		// Help
		private void SectorEditForm_HelpRequested(object sender, HelpEventArgs hlpevent) 
		{
			General.ShowHelp("w_sectoredit.html");
			hlpevent.Handled = true;
		}

		#endregion

		#region ================== mxd. Realtime Events

		private void texXOffset_OnValuesChanged(object sender, EventArgs e)
		{
			int i = 0;

			if (preventchanges) return;
			MakeUndo(); //mxd

			//restore values
			if (string.IsNullOrEmpty(texturexoffset.Text))
			{
				foreach (Sector s in sectors)
					s.OffsetX = sectorprops[i++].TextureXOffset;
			}
			else //update values
			{
				foreach (Sector s in sectors)
					s.OffsetX = texturexoffset.GetResult(sectorprops[i++].TextureXOffset);
			}

			General.Map.IsChanged = true;
			if (OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void texYOffset_OnValuesChanged(object sender, EventArgs e)
		{
			int i = 0;

			if (preventchanges) return;
			MakeUndo(); //mxd

			//restore values
			if (string.IsNullOrEmpty(textureyoffset.Text))
			{
				foreach (Sector s in sectors)
					s.OffsetY = sectorprops[i++].TextureYOffset;
			}
			else //update values
			{
				foreach (Sector s in sectors)
					s.OffsetY = textureyoffset.GetResult(sectorprops[i++].TextureYOffset);
			}

			General.Map.IsChanged = true;
			if (OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void ceilTexRot_OnValuesChanged(object sender, EventArgs e)
		{
			int i = 0;

			if (preventchanges) return;
			MakeUndo(); //mxd

			//restore values
			if (string.IsNullOrEmpty(ceiltexrot.Text))
			{
				foreach (Sector s in sectors)
					s.CeilTexRot = sectorprops[i++].CeilTexRot;
			}
			else //update values
			{
				foreach (Sector s in sectors)
					s.CeilTexRot = ceiltexrot.GetResult(sectorprops[i++].CeilTexRot);
			}

			General.Map.IsChanged = true;
			if (OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void floorTexRot_OnValuesChanged(object sender, EventArgs e)
		{
			int i = 0;

			if (preventchanges) return;
			MakeUndo(); //mxd

			//restore values
			if (string.IsNullOrEmpty(floortexrot.Text))
			{
				foreach (Sector s in sectors)
					s.FloorTexRot = sectorprops[i++].FloorTexRot;
			}
			else //update values
			{
				foreach (Sector s in sectors)
					s.FloorTexRot = floortexrot.GetResult(sectorprops[i++].FloorTexRot);
			}

			General.Map.IsChanged = true;
			if (OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		// Ceiling height changes
		private void ceilingheight_TextChanged(object sender, EventArgs e)
		{
			if(preventchanges) return;
			MakeUndo(); //mxd

			UpdateCeilingHeight();
			UpdateSectorHeight();

			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		// Floor height changes
		private void floorheight_TextChanged(object sender, EventArgs e)
		{
			if(preventchanges) return;
			MakeUndo(); //mxd

			UpdateFloorHeight();
			UpdateSectorHeight();

			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void floortex_OnValueChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd

			//restore values
			if(string.IsNullOrEmpty(floortex.TextureName)) 
			{
				int i = 0;
				foreach(Sector s in sectors) s.SetFloorTexture(sectorprops[i++].FloorTexture);
			
			} 
			else //update values
			{
				foreach(Sector s in sectors) s.SetFloorTexture(floortex.GetResult(s.FloorTexture));
			}

			// Update the used textures
			General.Map.Data.UpdateUsedTextures();

			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void ceilingtex_OnValueChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd

			//restore values
			if(string.IsNullOrEmpty(ceilingtex.TextureName)) 
			{
				int i = 0;
				foreach(Sector s in sectors) s.SetCeilTexture(sectorprops[i++].CeilTexture);
			
			} 
			else //update values
			{
				foreach(Sector s in sectors) s.SetCeilTexture(ceilingtex.GetResult(s.CeilTexture));
			}

			// Update the used textures
			General.Map.Data.UpdateUsedTextures();

			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void floorvertexheight_OnValuesChanged(object sender, EventArgs e)
		{
			if (preventchanges) return;
			MakeUndo(); //mxd

			if (floorvert1height.Text == "" && floorvert2height.Text == "" && floorvert3height.Text == "")
			{
				foreach (Sector s in sectors)
					s.RemoveMeridianSlope(true);
			}
			else
			{
				if (floorvert1height.Text == "" || floorvert2height.Text == "" || floorvert3height.Text == ""
					|| floorvert1.Text == "" || floorvert2.Text == "" || floorvert3.Text == "")
				{
					// Shouldn't clear everything, but can't update the map... return.
					return;
				}

				foreach (Sector s in sectors)
				{
					List<Vertex> vlist = s.GetVertexes();
					Vertex V;

					if (s.FloorSlopeVertexes.Count < 3)
					{
						s.FloorSlopeVertexes.Clear();
						V = vlist.FirstOrDefault(v => v.Index == Int32.Parse(floorvert1.Text));
						if (V == null)
							continue;
						s.FloorSlopeVertexes.Add(new Vector3D(V.Position.x, V.Position.y, Single.Parse(floorvert1height.Text)));

						V = vlist.FirstOrDefault(v => v.Index == Int32.Parse(floorvert2.Text));
						if (V == null)
							continue;
						s.FloorSlopeVertexes.Add(new Vector3D(V.Position.x, V.Position.y, Single.Parse(floorvert2height.Text)));

						V = vlist.FirstOrDefault(v => v.Index == Int32.Parse(floorvert3.Text));
						if (V == null)
							continue;
						s.FloorSlopeVertexes.Add(new Vector3D(V.Position.x, V.Position.y, Single.Parse(floorvert3height.Text)));
					}
					else
					{
						s.FloorSlopeVertexes[0] = new Vector3D(s.FloorSlopeVertexes[0].x, s.FloorSlopeVertexes[0].y, Single.Parse(floorvert1height.Text));
						s.FloorSlopeVertexes[1] = new Vector3D(s.FloorSlopeVertexes[1].x, s.FloorSlopeVertexes[1].y, Single.Parse(floorvert2height.Text));
						s.FloorSlopeVertexes[2] = new Vector3D(s.FloorSlopeVertexes[2].x, s.FloorSlopeVertexes[2].y, Single.Parse(floorvert3height.Text));
					}

					// Any ceilvert changes means recalculating the slope.
					s.CalculateMeridianSlope(true);
				}
			}

			General.Map.IsChanged = true;
			if (OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void ceilvertexheight_OnValuesChanged(object sender, EventArgs e)
		{
			if (preventchanges) return;
			MakeUndo(); //mxd

			if (ceilvert1height.Text == "" && ceilvert2height.Text == "" && ceilvert3height.Text == "")
			{
				foreach (Sector s in sectors)
					s.RemoveMeridianSlope(false);
			}
			else
			{
				if (ceilvert1height.Text == "" || ceilvert2height.Text == "" || ceilvert3height.Text == ""
					|| ceilvert1.Text == "" || ceilvert2.Text == "" || ceilvert3.Text == "")
				{
					// Shouldn't clear everything, but can't update the map... return.
					return;
				}

				foreach (Sector s in sectors)
				{
					List<Vertex> vlist = s.GetVertexes();
					Vertex V;

					if (s.CeilSlopeVertexes.Count < 3)
					{
						s.CeilSlopeVertexes.Clear();
						V = vlist.FirstOrDefault(v => v.Index == Int32.Parse(ceilvert1.Text));
						if (V == null)
							continue;
						s.CeilSlopeVertexes.Add(new Vector3D(V.Position.x, V.Position.y, Single.Parse(ceilvert1height.Text)));

						V = vlist.FirstOrDefault(v => v.Index == Int32.Parse(ceilvert2.Text));
						if (V == null)
							continue;
						s.CeilSlopeVertexes.Add(new Vector3D(V.Position.x, V.Position.y, Single.Parse(ceilvert2height.Text)));

						V = vlist.FirstOrDefault(v => v.Index == Int32.Parse(ceilvert3.Text));
						if (V == null)
							continue;
						s.CeilSlopeVertexes.Add(new Vector3D(V.Position.x, V.Position.y, Single.Parse(ceilvert3height.Text)));
					}
					else
					{
						s.CeilSlopeVertexes[0] = new Vector3D(s.CeilSlopeVertexes[0].x, s.CeilSlopeVertexes[0].y, Single.Parse(ceilvert1height.Text));
						s.CeilSlopeVertexes[1] = new Vector3D(s.CeilSlopeVertexes[1].x, s.CeilSlopeVertexes[1].y, Single.Parse(ceilvert2height.Text));
						s.CeilSlopeVertexes[2] = new Vector3D(s.CeilSlopeVertexes[2].x, s.CeilSlopeVertexes[2].y, Single.Parse(ceilvert3height.Text));
					}

					// Any ceilvert changes means recalculating the slope.
					s.CalculateMeridianSlope(false);
				}
			}

			General.Map.IsChanged = true;
			if (OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void floorvertexes_OnValuesChanged(object sender, EventArgs e)
		{
			if (preventchanges) return;
			MakeUndo(); //mxd

			if (floorvert1.Text == "" && floorvert2.Text == "" && floorvert3.Text == "")
			{
				foreach (Sector s in sectors)
					s.RemoveMeridianSlope(true);
			}
			else
			{
				if (floorvert1height.Text == "" || floorvert2height.Text == "" || floorvert3height.Text == ""
					|| floorvert1.Text == "" || floorvert2.Text == "" || floorvert3.Text == "")
				{
					// Shouldn't clear everything, but can't update the map... return.
					return;
				}
				if (floorvert1.Text == floorvert2.Text || floorvert1.Text == floorvert3.Text || floorvert2.Text == floorvert3.Text)
				{
					// Can't make a slope with two vertexes the same.
					return;
				}
				foreach (Sector s in sectors)
				{
					List<Vertex> vlist = s.GetVertexes();
					Vertex V;

					s.FloorSlopeVertexes.Clear();
					V = vlist.FirstOrDefault(v => v.Index == Int32.Parse(floorvert1.Text));
					if (V == null)
						continue;
					s.FloorSlopeVertexes.Add(new Vector3D(V.Position.x, V.Position.y, Single.Parse(floorvert1height.Text)));

					V = vlist.FirstOrDefault(v => v.Index == Int32.Parse(floorvert2.Text));
					if (V == null)
						continue;
					s.FloorSlopeVertexes.Add(new Vector3D(V.Position.x, V.Position.y, Single.Parse(floorvert2height.Text)));

					V = vlist.FirstOrDefault(v => v.Index == Int32.Parse(floorvert3.Text));
					if (V == null)
						continue;
					s.FloorSlopeVertexes.Add(new Vector3D(V.Position.x, V.Position.y, Single.Parse(floorvert3height.Text)));

					// Any floorvert changes means recalculating the slope.
					s.CalculateMeridianSlope(true);
				}
			}

			General.Map.IsChanged = true;
			if (OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void ceilvertexes_OnValuesChanged(object sender, EventArgs e)
		{
			if (preventchanges) return;
			MakeUndo(); //mxd

			if (ceilvert1.Text == "" && ceilvert2.Text == "" && ceilvert3.Text == "")
			{
				foreach (Sector s in sectors)
					s.RemoveMeridianSlope(false);
			}
			else
			{
				if (ceilvert1height.Text == "" || ceilvert2height.Text == "" || ceilvert3height.Text == ""
					|| ceilvert1.Text == "" || ceilvert2.Text == "" || ceilvert3.Text == "")
				{
					// Shouldn't clear everything, but can't update the map... return.
					return;
				}
				if (ceilvert1.Text == ceilvert2.Text || ceilvert1.Text == ceilvert3.Text || ceilvert2.Text == ceilvert3.Text)
				{
					// Can't make a slope with two vertexes the same.
					return;
				}
				foreach (Sector s in sectors)
				{
					List<Vertex> vlist = s.GetVertexes();
					Vertex V;

					s.CeilSlopeVertexes.Clear();
					V = vlist.FirstOrDefault(v => v.Index == Int32.Parse(ceilvert1.Text));
					if (V == null)
						continue;
					s.CeilSlopeVertexes.Add(new Vector3D(V.Position.x, V.Position.y, Single.Parse(ceilvert1height.Text)));

					V = vlist.FirstOrDefault(v => v.Index == Int32.Parse(ceilvert2.Text));
					if (V == null)
						continue;
					s.CeilSlopeVertexes.Add(new Vector3D(V.Position.x, V.Position.y, Single.Parse(ceilvert2height.Text)));

					V = vlist.FirstOrDefault(v => v.Index == Int32.Parse(ceilvert3.Text));
					if (V == null)
						continue;
					s.CeilSlopeVertexes.Add(new Vector3D(V.Position.x, V.Position.y, Single.Parse(ceilvert3height.Text)));

					// Any ceilvert changes means recalculating the slope.
					s.CalculateMeridianSlope(false);
				}
			}

			General.Map.IsChanged = true;
			if (OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void brightness_WhenTextChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd
			int i = 0;

			//restore values
			if(string.IsNullOrEmpty(brightness.Text)) 
			{
				foreach (Sector s in sectors)
				{
					int lightLevel = sectorprops[i++].Brightness / 2;
					if (noambientbox.Checked == false)
						lightLevel += 128;
					s.Brightness = lightLevel;
				}
			
			} 
			else //update values
			{
				foreach (Sector s in sectors)
				{
					int lightLevel = brightness.GetResult(sectorprops[i++].Brightness) / 2;

					if (noambientbox.Checked == false)
						lightLevel += 128;
					s.Brightness = lightLevel;
				}
			}

			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		#endregion
	}
}