using UnityEngine;

/// <summary>
/// Currently just used to destroy the previewing test objects in the Unity editor scene view
/// </summary>
public class GridGenerator : MonoBehaviour
{
	// Not used for this project, but started as a prototype for generating the GridCell array for this project
	/*public static GameObject[,] GenerateCube(Transform parent, GameObject cubePrefab, int cubeSize, float cubeScale)
	{
		GameObject[,] cArray = new GameObject[cubeSize, cubeSize];
		if (parent != null && cubePrefab != null)
		{
			DestroyGrid(parent);

			float cScale = cubeScale / cubeSize;
			Vector3 pos = Vector3.zero;

			Vector3 scaleOffset = (Vector3.one + Vector3.down) * (cubeScale / 2);
			scaleOffset -= (Vector3.one + Vector3.down) * (cScale / 2);

			for (int x = 0; x < cubeSize; x++)
			{
				pos.x = x * cScale;

				for (int z = 0; z < cubeSize; z++)
				{
					pos.z = z * cScale;

					GameObject comp = Instantiate(cubePrefab, parent);
					Vector3 newPos = pos - scaleOffset;

					comp.transform.localPosition = newPos;
					comp.transform.localScale = Vector3.one * (cScale);

					cArray[x, z] = comp;
				}
			}
		}

		return cArray;
	}*/

	public static void DestroyGrid(Transform parent)
	{
		if (parent != null)
		{
			while (parent.childCount > 0)
			{
				DestroyImmediate(parent.GetChild(0).gameObject);
			}
		}
	}
}
