using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Serpents
{
	public class Grid
	{
		public static GridCell[] GenerateGrid(int gridSizeX, int gridSizeY, float fieldSize = 10f)
		{
			GridCell[] posArray = new GridCell[gridSizeX * gridSizeY];

			float cellScale = fieldSize / (gridSizeX > gridSizeY ? gridSizeX : gridSizeY);
			
			float xDenominator = (gridSizeY > gridSizeX) ? (float)((float)gridSizeY / (float)gridSizeX) * 2f : 2f;
			float yDenominator = (gridSizeX > gridSizeY) ? (float)((float)gridSizeX / (float)gridSizeY) * 2f : 2f;
			Vector2 field = new Vector2(fieldSize / xDenominator, fieldSize / yDenominator);
			//Debug.LogFormat("field: {0}", field);
			Vector2 scaleOffset = Vector2.one * field;
			scaleOffset -= Vector2.one * (cellScale / 2);
			
			Vector2 pos = Vector2.zero;
			for (int x = 0; x < gridSizeX; x++)
			{
				pos.x = x * cellScale;

				for (int y = 0; y < gridSizeY; y++)
				{
					pos.y = y * cellScale;

					Vector2 newPos = pos - scaleOffset;

					posArray[x * gridSizeY + y] = new GridCell(newPos, x * gridSizeY + y);
				}
			}

			if (Application.isPlaying)
			{
				SerpentsManager.Instance.UnitScale = cellScale;
				Vector2 scale = new Vector2();
				scale.x = (field.x < field.y ? field.x / field.y : 1f);
				scale.y = (field.y < field.x ? field.y / field.x : 1f);
				SerpentsManager.Instance.BoundsScale = scale;
			}

			return posArray;
		}


	}
}