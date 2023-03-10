using UnityEngine;

namespace Serpents
{
	/// <summary>
	/// Class to represent individual cells in the array of cells, contains the position, index within the array, and whether a serpent is occupying this cell
	/// </summary>
	public class GridCell
	{
		public Vector2 Position;
		public int CellIndex { get; private set; }
		public bool ContainsSerpent { get; private set; }

		public GridCell(Vector2 position, int index)
		{
			Position = position;
			CellIndex = index;
		}

		public void SetContainsSerpent(bool value)
		{
			ContainsSerpent = value;
		}
	}
}
