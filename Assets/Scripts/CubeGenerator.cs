using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
	public static GameObject[,] GenerateCube(Transform parent, GameObject cubePrefab, int cubeSize, float cubeScale)
	{
		//posOffset = Vector3.zero;
		GameObject[,] cArray = new GameObject[cubeSize, cubeSize];
		if (parent != null && cubePrefab != null)
		{
			DestroyCube(parent);

			float cScale = cubeScale / cubeSize;
			Vector3 pos = Vector3.zero;

			Vector3 scaleOffset = (Vector3.one + Vector3.down) * (cubeScale / 2);
			scaleOffset -= (Vector3.one + Vector3.down) * (cScale / 2);
			//posOffset = scaleOffset;

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
	}

	public static void DestroyCube(Transform parent)
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
