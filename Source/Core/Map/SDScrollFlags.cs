
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

#endregion

namespace CodeImp.DoomBuilder
{
	public class SDScrollFlags
	{
		#region ================== Constants

		public const int SPEED_NONE = 0;
		public const int SPEED_SLOW = 1;
		public const int SPEED_MEDIUM = 2;
		public const int SPEED_FAST = 3;

		public const int SCROLL_N = 0;
		public const int SCROLL_NE = 1;
		public const int SCROLL_E = 2;
		public const int SCROLL_SE = 3;
		public const int SCROLL_S = 4;
		public const int SCROLL_SW = 5;
		public const int SCROLL_W = 6;
		public const int SCROLL_NW = 7;
		
		#endregion

		#region Properties

		private int speed;
		private int direction;

		public int Speed {get { return speed; } set { speed = value; } }
		public int Direction {get { return direction; } set { direction = value; } }

		#endregion

		#region Constructors

		public SDScrollFlags()
		{
			speed = SPEED_NONE;
			direction = SCROLL_N;
		}

		public SDScrollFlags(string speedStr, string directionStr)
		{
			this.SetSpeed(speedStr);
			this.SetDirection(directionStr);
		}

		public SDScrollFlags(int Speed, int Direction)
		{
			if (Speed >= SPEED_NONE && Speed <= SPEED_FAST)
				this.speed = Speed;
			else
				this.speed = SPEED_NONE;
			if (Direction >= SCROLL_N && Direction <= SCROLL_NW)
				this.direction = Direction;
			else
				this.direction = SCROLL_N;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Set the integer speed field from a string speed.
		/// </summary>
		/// <param name="SpeedStr"></param>
		public void SetSpeed(string SpeedStr)
		{
			if (SpeedStr == "Slow")
				speed = SPEED_SLOW;
			else if (SpeedStr == "Medium")
				speed = SPEED_MEDIUM;
			else if (SpeedStr == "Fast")
				speed = SPEED_FAST;
			else
				speed = SPEED_NONE;
		}

		/// <summary>
		///  Set the integer direction field from a string direction.
		/// </summary>
		/// <param name="DirectionStr"></param>
		public void SetDirection(string DirectionStr)
		{
			if (DirectionStr == "NE")
				direction = SCROLL_NE;
			else if (DirectionStr == "East")
				direction = SCROLL_E;
			else if (DirectionStr == "SE")
				direction = SCROLL_SE;
			else if (DirectionStr == "South")
				direction = SCROLL_S;
			else if (DirectionStr == "SW")
				direction = SCROLL_SW;
			else if (DirectionStr == "West")
				direction = SCROLL_W;
			else if (DirectionStr == "NW")
				direction = SCROLL_NW;
			else
				direction = SCROLL_N;
		}

		/// <summary>
		/// Takes an integer speed, returns the string name of the speed constant.
		/// </summary>
		/// <param name="Speed"></param>
		/// <returns></returns>
		public static string ScrollSpeed(int Speed)
		{
			if (Speed == SPEED_SLOW)
				return "Slow";
			if (Speed == SPEED_MEDIUM)
				return "Medium";
			if (Speed == SPEED_FAST)
				return "Fast";
			return "None";
		}

		/// <summary>
		/// Takes a string speed, returns the corresponding integer constant.
		/// </summary>
		/// <param name="Speed"></param>
		/// <returns></returns>
		public static int ScrollSpeed(string Speed)
		{
			if (Speed == "Slow")
				return SPEED_SLOW;
			if (Speed == "Medium")
				return SPEED_MEDIUM;
			if (Speed == "Fast")
				return SPEED_FAST;
			return SPEED_NONE;
		}

		/// <summary>
		/// Takes a string direction, returns the corresponding integer constant.
		/// </summary>
		/// <param name="Direction"></param>
		/// <returns></returns>
		public static int ScrollDirection(string Direction)
		{
			if (Direction == "NE")
				return SCROLL_NE;
			if (Direction == "East")
				return SCROLL_E;
			if (Direction == "SE")
				return SCROLL_SE;
			if (Direction == "South")
				return SCROLL_S;
			if (Direction == "SW")
				return SCROLL_SW;
			if (Direction == "West")
				return SCROLL_W;
			if (Direction == "NW")
				return SCROLL_NW;
			return SCROLL_N;
		}

		/// <summary>
		/// Takes an integer direction, returns the corresponding string direction.
		/// </summary>
		/// <param name="Direction"></param>
		/// <returns></returns>
		public static string ScrollDirection(int Direction)
		{
			if (Direction == SCROLL_NE)
				return "NE";
			if (Direction == SCROLL_E)
				return "East";
			if (Direction == SCROLL_SE)
				return "SE";
			if (Direction == SCROLL_S)
				return "South";
			if (Direction == SCROLL_SW)
				return "SW";
			if (Direction == SCROLL_W)
				return "West";
			if (Direction == SCROLL_NW)
				return "NW";
			return "North";
		}

		#endregion
	}
}
