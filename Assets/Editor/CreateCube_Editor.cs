using UnityEngine;
using UnityEditor;

public class CreateCube_Editor : EditorWindow
{
	//string myString = "Greetings my Lord";
	//bool groupEnabled;
	//bool myBool = true;
	//float myFloat = 1.23f;

	private GameObject _cubeParent = null;
	private GameObject _cPrefab = null;
	private int _cubeSize = 5;
	private int _cubeScale = 10;

	private int _gridSizeX = 10;
	private int _gridSizeY = 10;

	private GameObject[,] _cArray = null;
	private Serpents.GridCell[] _cellPositions = null;

	// Add menu named "My Window" to the Window menu
	[MenuItem("Window/CreateCube Window")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		CreateCube_Editor window = (CreateCube_Editor)EditorWindow.GetWindow(typeof(CreateCube_Editor));
		window.Show();
	}

	void OnGUI()
	{
		GUILayout.Label("Cube Settings", EditorStyles.boldLabel);
		//myString = EditorGUILayout.TextField("Text Field", myString);

		//groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
		//myBool = EditorGUILayout.Toggle("Toggle", myBool);
		//myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
		//EditorGUILayout.EndToggleGroup();

		_cubeParent = (GameObject)EditorGUILayout.ObjectField("Cube Parent", _cubeParent, typeof(GameObject), true);
		_cPrefab = (GameObject)EditorGUILayout.ObjectField("Cube Component Prefab", _cPrefab, typeof(GameObject), true);
		_cubeSize = EditorGUILayout.IntSlider("Cube Component Size", _cubeSize, 1, 40);
		_cubeScale = EditorGUILayout.IntSlider("Cube Scale", _cubeScale, 1, 10);

		_gridSizeX = EditorGUILayout.IntSlider("Grid Size X", _gridSizeX, 10, 1000);
		_gridSizeY = EditorGUILayout.IntSlider("Grid Size Y", _gridSizeY, 10, 1000);

		if (GUILayout.Button("Generate Cube"))
		{
			//_cArray = CubeGenerator.GenerateCube(_cubeParent.transform, _cPrefab, _cubeSize, _cubeScale);

			_cellPositions = Serpents.Grid.GenerateGrid(_gridSizeX, _gridSizeY);
			
			CreateTestGrid(_cubeParent.transform, _cellPositions, _cPrefab);
		}

		if (GUILayout.Button("Destroy Cube"))
		{
			CubeGenerator.DestroyCube(_cubeParent.transform);
		}
	}

	void CreateTestGrid(Transform parent, Serpents.GridCell[] gridCells, GameObject cellPrefab, float cellScale = 10f)
	{
		CubeGenerator.DestroyCube(parent);

		float cScale = cellScale / (_gridSizeX > _gridSizeY ? _gridSizeX : _gridSizeY);

		foreach (Serpents.GridCell cell in gridCells)
		{
			if (cell == null)
			{
				Debug.Log("Cell is null");
				continue;
			}

			GameObject comp = Instantiate(cellPrefab, parent);
			Vector3 newPos = new Vector3(cell.Position.x, 0f, cell.Position.y);

			comp.transform.localPosition = newPos;
			comp.transform.localScale = Vector3.one * (cScale);
		}
	}
}
