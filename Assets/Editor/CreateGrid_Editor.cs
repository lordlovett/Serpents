using UnityEngine;
using UnityEditor;

/// <summary>
/// This Editor script was taken (and modified) from a small project I started awhile ago,
/// I used it to prototype the generation of the grid cells for this game
/// </summary>
public class CreateGrid_Editor : EditorWindow
{
	private GameObject _gridParent = null;
	private GameObject _cPrefab = null;

	private int _gridSizeX = 10;
	private int _gridSizeY = 10;

	private Serpents.GridCell[] _cellPositions = null;

	// Add menu named "My Window" to the Window menu
	[MenuItem("Window/CreateGrid Window")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		CreateGrid_Editor window = (CreateGrid_Editor)EditorWindow.GetWindow(typeof(CreateGrid_Editor));
		window.Show();
	}

	void OnGUI()
	{
		GUILayout.Label("Grid Settings", EditorStyles.boldLabel);

		_gridParent = (GameObject)EditorGUILayout.ObjectField("Grid Parent", _gridParent, typeof(GameObject), true);
		_cPrefab = (GameObject)EditorGUILayout.ObjectField("Grid Cell Prefab", _cPrefab, typeof(GameObject), true);

		_gridSizeX = EditorGUILayout.IntSlider("Grid Size X", _gridSizeX, 10, 1000);
		_gridSizeY = EditorGUILayout.IntSlider("Grid Size Y", _gridSizeY, 10, 1000);

		if (GUILayout.Button("Generate Grid"))
		{
			_cellPositions = Serpents.Grid.GenerateGrid(_gridSizeX, _gridSizeY);
			
			CreateTestGrid(_gridParent.transform, _cellPositions, _cPrefab);
		}

		if (GUILayout.Button("Destroy Grid"))
		{
			GridGenerator.DestroyGrid(_gridParent.transform);
		}
	}

	void CreateTestGrid(Transform parent, Serpents.GridCell[] gridCells, GameObject cellPrefab, float cellScale = 10f)
	{
		GridGenerator.DestroyGrid(parent);

		float cScale = cellScale / (_gridSizeX > _gridSizeY ? _gridSizeX : _gridSizeY);

		foreach (Serpents.GridCell cell in gridCells)
		{
			if (cell == null)
			{
				continue;
			}

			GameObject comp = Instantiate(cellPrefab, parent);
			Vector3 newPos = new Vector3(cell.Position.x, 0f, cell.Position.y);

			comp.transform.localPosition = newPos;
			comp.transform.localScale = Vector3.one * (cScale);
		}
	}
}
