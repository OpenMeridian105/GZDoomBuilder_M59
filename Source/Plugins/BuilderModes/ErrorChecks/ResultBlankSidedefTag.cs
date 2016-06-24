#region ================== Namespaces

using System;
using CodeImp.DoomBuilder.Map;
using CodeImp.DoomBuilder.Rendering;

#endregion

namespace CodeImp.DoomBuilder.BuilderModes
{
	public class ResultBlankSidedefTag : ErrorResult
	{
		#region ================== Variables
		
		private readonly Sidedef side;
		
		#endregion
		
		#region ================== Properties

		public override int Buttons { get { return 1; } }
		public override string Button1Text { get { return "Remove Tag"; } }
		
		#endregion
		
		#region ================== Constructor / Destructor
		
		// Constructor
		public ResultBlankSidedefTag(Sidedef sd)
		{
			// Initialize
			this.side = sd;
			this.viewobjects.Add(sd);
			this.hidden = sd.IgnoredErrorChecks.Contains(this.GetType()); //mxd
			this.description = "This sidedef has a tag (used in client for changing textures) but has no starting texture.";
		}
		
		#endregion
		
		#region ================== Methods

		// This sets if this result is displayed in ErrorCheckForm (mxd)
		internal override void Hide(bool hide) 
		{
			hidden = hide;
			Type t = this.GetType();
			if (hide) side.IgnoredErrorChecks.Add(t);
			else if (side.IgnoredErrorChecks.Contains(t)) side.IgnoredErrorChecks.Remove(t);
		}
		
		// This must return the string that is displayed in the listbox
		public override string ToString()
		{
			string frontorback = side.IsFront ? "front" : "back";

			return "Sidedef " + side.Index + " (" + frontorback + ") has a tag but no textures";
		}
		
		// Rendering
		public override void PlotSelection(IRenderer2D renderer)
		{
			renderer.PlotLinedef(side.Line, General.Colors.Selection);
			renderer.PlotVertex(side.Line.Start, ColorCollection.VERTICES);
			renderer.PlotVertex(side.Line.End, ColorCollection.VERTICES);
		}
		
		// Fix by removing tag
		public override bool Button1Click(bool batchMode)
		{
			if (!batchMode)
				General.Map.UndoRedo.CreateUndo("Remove unneeded tag");

			side.Tag = 0;

			General.Map.Map.Update();
			return true;
		}

		#endregion
	}
}
