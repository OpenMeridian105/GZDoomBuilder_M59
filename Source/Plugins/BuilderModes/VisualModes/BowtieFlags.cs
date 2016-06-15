#region === Copyright (c) 2010 Pascal van der Heiden ===

using System.Collections.Generic;
using CodeImp.DoomBuilder.Geometry;

#endregion

namespace CodeImp.DoomBuilder.BuilderModes
{
	public class BowtieFlags
	{
		#region Bitmasks

		private const uint BT_BELOW_POS     = 1U;       // below wall is bowtie & positive sector is on top at endpoint 0
		private const uint BT_BELOW_NEG     = 2U;       // below wall is bowtie & negative sector is on top at endpoint 0
		private const uint BT_ABOVE_POS     = 4U;       // above wall is bowtie & positive sector is on top at endpoint 0
		private const uint BT_ABOVE_NEG     = 8U;       // above wall is bowtie & negative sector is on top at endpoint 0

		private const uint BT_MASK_BELOW_BOWTIE = 3U;   // mask to test for bowtie on below wall
		private const uint BT_MASK_ABOVE_BOWTIE = 12U;  // mask to test for bowtie on above wall

		#endregion
		
		protected uint flags;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="Value"></param>
		public BowtieFlags(uint Value = 0)
		{
			flags = Value;
		}

		public uint Value
		{
			get { return flags; }
			set
			{
				if (flags != value)
				{
					flags = value;
				}
			}
		}

		/// <summary>
		/// Below wall is bowtie and positive sector is on top at endpoint 0
		/// </summary>
		public bool IsBelowPos
		{
			get { return (flags & BT_BELOW_POS) == BT_BELOW_POS; }
			set
			{
				if (value)
					flags |= BT_BELOW_POS;
				else
					flags &= ~BT_BELOW_POS;
			}
		}

		/// <summary>
		/// Below wall is bowtie and negative sector is on top at endpoint 0
		/// </summary>
		public bool IsBelowNeg
		{
			get { return (flags & BT_BELOW_NEG) == BT_BELOW_NEG; }
			set
			{
				if (value)
					flags |= BT_BELOW_NEG;
				else
					flags &= ~BT_BELOW_NEG;
			}
		}

		/// <summary>
		/// Above wall is bowtie and positive sector is on top at endpoint 0
		/// </summary>
		public bool IsAbovePos
		{
			get { return (flags & BT_ABOVE_POS) == BT_ABOVE_POS; }
			set
			{
				if (value)
					flags |= BT_ABOVE_POS;
				else
					flags &= ~BT_ABOVE_POS;
			}
		}

		/// <summary>
		/// Above wall is bowtie and negative sector is on top at endpoint 0
		/// </summary>
		public bool IsAboveNeg
		{
			get { return (flags & BT_ABOVE_NEG) == BT_ABOVE_NEG; }
			set
			{
				if (value)
					flags |= BT_ABOVE_NEG;
				else
					flags &= ~BT_ABOVE_NEG;
			}
		}

		/// <summary>
		/// Mask for bowtie variants on below wall.
		/// Contains bits of IsBelowPos and IsBelowNeg.
		/// </summary>
		public bool IsBelowBowtie
		{
			get { return (flags & BT_MASK_BELOW_BOWTIE) == BT_MASK_BELOW_BOWTIE; }
			set
			{
				if (value)
					flags |= BT_MASK_BELOW_BOWTIE;
				else
					flags &= ~BT_MASK_BELOW_BOWTIE;
			}
		}

		/// <summary>
		/// Mask for bowtie variants on above wall.
		/// Contains bits of IsAbovePos and IsAboveNeg
		/// </summary>
		public bool IsAboveBowtie
		{
			get { return (flags & BT_MASK_ABOVE_BOWTIE) == BT_MASK_ABOVE_BOWTIE; }
			set
			{
				if (value)
					flags |= BT_MASK_ABOVE_BOWTIE;
				else
					flags &= ~BT_MASK_ABOVE_BOWTIE;
			}
		}
	}
}
