//using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Serpents
{
	/// <summary>
	/// This class is a singleton to help manage the game settings (grid size, game speed), and initializing the serpents and the apple,
	/// as well as checking for the end game conditions
	/// </summary>
	public partial class SerpentsManager : MonoBehaviour
	{
		[Header("Mutable Parameters")]
		[SerializeField]
		[Range(5, 10000)]
		private int m_gridSizeX = 50;
		[SerializeField]
		[Range(5, 10000)]
		private int m_gridSizeY = 40;

		[SerializeField]
		[Range(1, 30)]
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
		private bool m_gameIsRunning = false;
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

		/// <summary>
		/// Subscribe to events
		/// </summary>
		private void OnEnable()
		{
			EventDelegate.appleConsumed += OnAppleConsumed;
			EventDelegate.serpentDied += OnSerpentDied;
		}

		/// <summary>
		/// Unsubscribe from events
		/// </summary>
		private void OnDisable()
		{
			EventDelegate.appleConsumed -= OnAppleConsumed;
			EventDelegate.serpentDied -= OnSerpentDied;
		}

		/// <summary>
		/// Setup singleton instance, create play grid and setup the bounds, set game speed 
		/// </summary>
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

			// Change the fixed update time scale
			Time.fixedDeltaTime = 1.0f / (float)m_gameSpeed;
		}

		/// <summary>
		/// Creates a Serpent instance with the given name, default input, and a colour.
		/// Called from the UI Manager.
		/// </summary>
		public Serpent CreatePlayer(string playerName, PlayerInput.InputType inputType, Color colour)
		{
			Serpent serp = Instantiate(m_serpentPrefab, m_gameRoot);

			serp.Initialize(playerName, GetRandomStartPosition(), GetRandomStartDirection(), inputType);

			// Set serpent colour
			serp.SetColour(colour);

			m_activeSerpents.Add(playerName);

			return serp;
		}

		/// <summary>
		/// Removes the serpent with the matching 'name'
		/// Called from UI Manager.
		/// </summary>
		public void RemoveSerpent(string name)
		{
			m_activeSerpents.Remove(name);
		}

		/// <summary>
		/// Creates the first apple and starts the game
		/// </summary>
		public void StartGame()
		{
			CreateApple();

			m_gameIsRunning = true;
		}

		/// <summary>
		/// Gets the GridCell at 'cellIndex' from the m_grid array
		/// </summary>
		public GridCell GetCell(int cellIndex)
		{
			return m_grid[cellIndex];
		}

		/// <summary>
		/// Gets the GridCell adjacent to the GridCell at 'cellIndex', in the direction of 'offset'.
		/// Returns null if the would be cell is a wall
		/// </summary>
		public GridCell GetAdjacentCell(int cellIndex, Vector2Int offset)
		{			
			if ((cellIndex % m_gridSizeY == 0 && offset == Vector2Int.down) 
				|| (cellIndex % m_gridSizeY == m_gridSizeY - 1 && offset == Vector2Int.up))
			{
				// Hit a wall, vertically
				return null;
			}

			int indexOffset = offset.x * m_gridSizeY + offset.y;
			int newIndex = cellIndex + indexOffset;
			if (newIndex < 0 || newIndex >= m_grid.Length)
			{
				// Hit a wall, horizontally
				return null;
			}

			return m_grid[newIndex];
		}

		/// <summary>
		/// Finds a GridCell that does not contain a serpent, creates an apple if none exists,
		/// then places the apple at that cell's position.
		/// </summary>
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

		/// <summary>
		/// If the apple is consumed, check if there any GridCells that don't contain a serpent,
		/// if so, place apple at a new position, if not, (serpents fill the grid) fire game over event
		/// </summary>
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

		/// <summary>
		/// Returns true if the given GridCell index is the same cell index that the apple was placed at.
		/// </summary>
		public bool DoesCellContainApple(int index)
		{
			return index == m_appleCellIndex;
		}

		/// <summary>
		/// Serpent has died (hit wall or serpent), remove it from the list of active serpents.
		/// Fire game over event if there are no more serpents
		/// </summary>
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

		/// <summary>
		/// Returns a random starting GridCell for placing a serpent, with padding so that the tail isn't placed in a wall.
		/// Currently assumes that all serpents start facing to the right (which is currently the case)
		/// This function should be refactored to add safety margins to prevent overlapping serpents and allow for different starting directions
		/// </summary>
		private int GetRandomStartPosition(int startSize = 2)
		{
			return Random.Range(m_gridSizeY * (startSize - 1), (m_gridSizeX * m_gridSizeY) / 2);
		}

		/// <summary>
		/// This function should be used to set the starting direction of the serpents at random, or potentially facing away from a nearby wall.
		/// </summary>
		private Vector2Int GetRandomStartDirection()
		{
			return Vector2Int.right;
		}
	}
}