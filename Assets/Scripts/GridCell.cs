using UnityEngine;

namespace Serpents
{
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
