#region ================== Namespaces

using System.Threading;
using CodeImp.DoomBuilder.Map;

#endregion

namespace CodeImp.DoomBuilder.BuilderModes 
{
	[ErrorChecker("Check required things", true, 50)]
	public class CheckRequiredThings : ErrorChecker
	{
		#region ================== Constants

		private const int PROGRESS_STEP = 1000;

		#endregion

		// Only check in Meridian map format
		public override bool SkipCheck { get { return !General.Map.MERIDIAN; } }

		// Constructor
		public CheckRequiredThings()
		{
			// Total progress is done when all things are checked
			SetTotalProgress(General.Map.Map.Things.Count / PROGRESS_STEP);
		}

		// This runs the check
		public override void Run() 
		{
			int progress = 0;
			int stepprogress = 0;

			// Go for all things
			if (General.Map.Map.Things.Count != 2)
				SubmitResult(new ResultRequiredThings(General.Map.Map.Things.Count));
			// Handle thread interruption
			try { Thread.Sleep(0); } catch(ThreadInterruptedException) { return; }

			// We are making progress!
			if((++progress / PROGRESS_STEP) > stepprogress) 
			{
				stepprogress = (progress / PROGRESS_STEP);
				AddProgress(1);
			}
		}
	}
}
