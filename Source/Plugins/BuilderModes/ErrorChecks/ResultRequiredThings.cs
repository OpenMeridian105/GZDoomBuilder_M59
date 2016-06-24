#region ================== Namespaces

using System.Drawing;
using CodeImp.DoomBuilder.Geometry;

#endregion

namespace CodeImp.DoomBuilder.BuilderModes
{
	public class ResultRequiredThings : ErrorResult
	{
		#region ================== Variables

		private readonly int thingscount;

		#endregion
		
		#region ================== Properties

		public override int Buttons { get { return 0; } }

		#endregion

		#region ================== Constructor / Destructor

		public ResultRequiredThings(int thingscount)
		{
			// Initialize
			this.thingscount = thingscount;
			this.description = "Incorrect number of things.";
		}

		#endregion

		#region ================== Methods

		public override RectangleF GetZoomArea()
		{
			return new RectangleF(-500.0f, 500.0f, 1000.0f, 1000.0f);
		}

		// This sets if this result is displayed in ErrorCheckForm (mxd)
		internal override void Hide(bool hide)
		{
			hidden = hide;
		}

		// This must return the string that is displayed in the listbox
		public override string ToString()
		{
			if (thingscount > 2)
				return "Too many things - Meridian 59 rooms must contain two 'things' to mark the boundary of the room.";
			return "Not enough things - Meridian 59 rooms must contain two 'things' to mark the boundary of the room.";
		}

		#endregion
	}
}
