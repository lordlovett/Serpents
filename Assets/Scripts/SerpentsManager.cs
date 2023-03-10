//using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Serpents
{
	public partial class SerpentsManager : MonoBehaviour
	{
		[SerializeField]
		private bool m_gameIsRunning = false;
		
		[Header("Mutable Parameters")]
		[SerializeField]
		[Range(5, 1000000)]
		private int m_gridSizeX = 50;
		[SerializeField]
		[Range(5, 1000000)]
		private int m_gridSizeY = 40;

		[SerializeField]
		[Range(1, 60)]
		private int m_gameSpeed = 30;

		[SerializeField]
		private int m_startingPlayers = 1;

		[Header("Immutable Parameters")]
		[SerializeField]
		private Transform m_bounds = null;

		[SerializeField]
		private Transform m_gameRoot = null;

		[SerializeField]
		private Serpent m_serpentPrefab = null;

		[SerializeField]
		private GameObject m_applePrefab = null;

		// Internal variables
		private GridCell[] m_grid;
		
		private float m_unitScale;
		private Vector2 m_boundsScale;

		private GameObject m_appleObject = null;
		private int m_appleCellIndex = -1;

		private List<string> m_activeSerpents = new List<string>();

		public static SerpentsManager Instance { get; private set; }
		public int GridSizeX { get => m_gridSizeX; }
		public int GridSizeY { get => m_gridSizeY; }

		public bool GameIsRunning { get => m_gameIsRunning; }
		public int StartingPlayerCount { get => m_startingPlayers; }

		public float UnitScale 
		{
			get => m_unitScale;
			set => m_unitScale = value;
		}

		public Vector2 BoundsScale
		{
			get => m_boundsScale;
			set => m_boundsScale = value;
		}

		// Subscribe to events
		private void OnEnable()
		{
			EventDelegate.appleConsumed += OnAppleConsumed;
			EventDelegate.serpentDied += OnSerpentDied;
		}

		// Unsubscribe from events
		private void OnDisable()
		{
			EventDelegate.appleConsumed -= OnAppleConsumed;
			EventDelegate.serpentDied -= OnSerpentDied;
		}

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(this);
			}
			else
			{
				Instance = this;
			}

			// Initialize grid parameters
			m_grid = Grid.GenerateGrid(m_gridSizeX, m_gridSizeY);

			// Resize play bounds to fit grid
			m_bounds.localScale = new Vector3(BoundsScale.x, 1f, BoundsScale.y);

			Time.fixedDeltaTime = 1.0f / (float)m_gameSpeed;
		}

		void Start()
		{
			// TODO: consider moving to a game start function, called from UI 
			/*for (int i = 0; i < m_startingPlayers; i++)
			{
				string name = "Jormungandr_" + i.ToString("N2");
				Serpent serp = Instantiate(m_serpentPrefab, m_gameRoot);

				PlayerInput.InputType input = (PlayerInput.InputType)Random.Range(0, (int)PlayerInput.InputType.COUNT);
				input = PlayerInput.InputType.ArrowKeys;
				
				serp.Initialize(name, GetRandomStartPosition(), GetRandomStartDirection(), input);

				// Set serpent colour
				serp.SetColour(Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.8f, 1f));

				m_activeSerpents.Add(name);
			}*/

			CreateApple();
		}

		public Serpent CreatePlayer(string playerName, PlayerInput.InputType inputType, Color colour)
		{
			Serpent serp = Instantiate(m_serpentPrefab, m_gameRoot);

			serp.Initialize(playerName, GetRandomStartPosition(), GetRandomStartDirection(), inputType);

			// Set serpent colour
			serp.SetColour(colour);

			m_activeSerpents.Add(playerName);

			return serp;
		}

		public void RemoveSerpent(string name)
		{
			m_activeSerpents.Remove(name);
		}

		public void StartGame()
		{
			m_gameIsRunning = true;
		}

		public GridCell GetCell(int cellIndex)
		{
			return m_grid[cellIndex];
		}

		public GridCell GetAdjacentCell(int cellIndex, Vector2Int offset)
		{
			int newIndex = cellIndex;
			if ((cellIndex % m_gridSizeY == 0 && offset == Vector2Int.down) 
				|| (cellIndex % m_gridSizeY == m_gridSizeY - 1 && offset == Vector2Int.up))
			{
				Debug.Log("Hitting wall...");
				return null;
			}

			int indexOffset = offset.x * m_gridSizeY + offset.y;
			newIndex = cellIndex + indexOffset;
			if (newIndex < 0 || newIndex >= m_grid.Length)
			{
				Debug.Log("Hitting wall...");
				return null;
			}

			return m_grid[newIndex];
		}

		private void CreateApple()
		{
			GridCell appleSpawn = GetCell(0);
			do
			{
				appleSpawn = GetCell(Random.Range(0, m_gridSizeX * m_gridSizeY));
			}
			while (appleSpawn.ContainsSerpent);

			if (m_appleObject == null)
			{
				m_appleObject = Instantiate(m_applePrefab, m_gameRoot);
				m_appleObject.transform.localScale = Vector3.one * UnitScale;
			}

			m_appleObject.transform.localPosition = new Vector3(appleSpawn.Position.x, 0, appleSpawn.Position.y);

			m_appleObject.SetActive(true);
			m_appleCellIndex = appleSpawn.CellIndex;
		}

		private void OnAppleConsumed()
		{
			if (m_grid.Any(c => !c.ContainsSerpent))
			{
				CreateApple();
			}
			else
			{
				// Serpents have filled the grid, game over
				EventDelegate.FireGameOver();
			}
		}

		public bool DoesCellContainApple(int index)
		{
			return index == m_appleCellIndex;
		}

		private void OnSerpentDied(string serpentName)
		{
			if (m_activeSerpents.Contains(serpentName))
			{
				m_activeSerpents.Remove(serpentName);
			}

			// Check for any remaining serpents, Game Over if 0 remain
			if (m_gameIsRunning && m_activeSerpents.Count == 0)
			{
				EventDelegate.FireGameOver();
			}			
		}

		private int GetRandomStartPosition(int startSize = 2)
		{
			return Random.Range(m_gridSizeY * (startSize - 1), (m_gridSizeX * m_gridSizeY) / 2);
		}

		private Vector2Int GetRandomStartDirection()
		{
			Vector2 dir = Random.insideUnitCircle.normalized;

			return Vector2Int.right;
		}
	}
}