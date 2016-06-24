#region ================== Namespaces

using CodeImp.DoomBuilder.Map;
using System.Threading;

#endregion

namespace CodeImp.DoomBuilder.BuilderModes
{
	[ErrorChecker("Check sidedef tags", true, 60)]
	public class CheckTaggedSidedefs : ErrorChecker
	{
		#region ================== Constants

		private const int PROGRESS_STEP = 1000;

		#endregion

		#region ================== Constructor / Destructor

		// Only check in Meridian map format
		public override bool SkipCheck { get { return !General.Map.MERIDIAN; } }

		// Constructor
		public CheckTaggedSidedefs()
		{
			// Total progress is done when all lines are checked
			SetTotalProgress(General.Map.Map.Sidedefs.Count / PROGRESS_STEP);
		}

		#endregion

		#region ================== Methods

		// This runs the check
		public override void Run()
		{
			int progress = 0;
			int stepprogress = 0;

			// Go for all the sidedefs
			foreach(Sidedef sd in General.Map.Map.Sidedefs)
			{
				if (sd.Tag != 0
					&& sd.LongHighTexture == MapSet.EmptyLongName
					&& sd.LongMiddleTexture == MapSet.EmptyLongName
					&& sd.LongLowTexture == MapSet.EmptyLongName)
				{
					SubmitResult(new ResultBlankSidedefTag(sd));
				}
				
				// Handle thread interruption
				try { Thread.Sleep(0); }
				catch(ThreadInterruptedException) { return; }

				// We are making progress!
				if((++progress / PROGRESS_STEP) > stepprogress)
				{
					stepprogress = (progress / PROGRESS_STEP);
					AddProgress(1);
				}
			}
		}

		#endregion
	}
}
