using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Serpents
{
	/// <summary>
	/// This class represents an instanced serpent object/player, it takes an input direction from the PlayerInput class and controls the movement of the Serpent
	/// </summary>
	[RequireComponent(typeof(PlayerInput))]
	public class Serpent : MonoBehaviour
	{
		[SerializeField]
		private PlayerInput m_playerInput;

		[SerializeField]
		private GameObject m_tailSegment = null;
		[SerializeField]
		private Transform m_tailBase = null;
		[SerializeField]
		private int m_serpentStartSize = 2;

		private string m_serpentName = "";
		private Color m_colour;

		private Vector2Int m_moveDirection;
		private Vector2Int m_moveInput = Vector2Int.zero;
		
		private int m_currentHeadCellIndex;
		private int m_indexOfLastCell;

		private Dictionary<int, SerpentSegment> m_serpentSegments = new Dictionary<int, SerpentSegment>();
		private List<Renderer> m_segmentRenderers = new List<Renderer>();

		/// <summary>
		/// A small class to help manage each body segment in a serpent
		/// </summary>
		public class SerpentSegment
		{
			public Transform _segment;
			public int _cellIndex { get; private set; }

			public SerpentSegment(Transform t, int i)
			{
				_segment = t;
				_cellIndex = i;
			}

			public void SetCellIndex(int i)
			{
				_cellIndex = i;
			}
		}

		/// <summary>
		/// Returns the name of the serpent
		/// </summary>
		public string GetName()
		{
			return m_serpentName;
		}

		/// <summary>
		/// Initializes the Serpent with the given parameters, and creates the initial tail segments
		/// </summary>
		public void Initialize(string name, int startIndex, Vector2Int startDirection, PlayerInput.InputType inputType)
		{
			m_serpentName = name;
			gameObject.name = m_serpentName;
			m_currentHeadCellIndex = startIndex;
			m_moveDirection = startDirection;
			m_playerInput.SetInputType(inputType);

			GridCell cell = SerpentsManager.Instance.GetCell(m_currentHeadCellIndex);
			
			Vector2 cellPos = cell.Position;
			transform.localPosition = new Vector3(cellPos.x, 0, cellPos.y);
			transform.localScale = Vector3.one * SerpentsManager.Instance.UnitScale;
			m_serpentSegments.Add(0, new SerpentSegment(transform, m_currentHeadCellIndex));
			cell.SetContainsSerpent(true);

			int index = startIndex;
			Vector2Int moveDir = -startDirection;
			for (int i = 1; i < m_serpentStartSize; i++)
			{
				Transform tailSeg = Instantiate(m_tailSegment, m_tailBase).transform;

				GridCell tailCell = SerpentsManager.Instance.GetAdjacentCell(index, moveDir);
				index = tailCell.CellIndex;
				Vector2 pos = tailCell.Position;
				tailSeg.position = new Vector3(pos.x, 0, pos.y);

				tailCell.SetContainsSerpent(true);

				m_serpentSegments.Add(i, new SerpentSegment(tailSeg, tailCell.CellIndex));
			}
		}

		/// <summary>
		/// Sets the colour of the serpent head and all tail segments
		/// </summary>
		public void SetColour(Color colour)
		{
			m_colour = colour;
			m_segmentRenderers.AddRange(GetComponentsInChildren<Renderer>());

			foreach (Renderer rend in m_segmentRenderers)
			{
				rend.material.color = m_colour;
			}
		}

		/// <summary>
		/// Sets the input type for this Serpent instance.
		/// Call from UI Manager when users change the input after a new serpent has been added
		/// </summary>
		public void SetInputType(PlayerInput.InputType inputType)
		{
			m_playerInput.SetInputType(inputType);
		}

		/// <summary>
		/// Retrieve the user input direction from the PlayerInput component, to be used in the next FixedUpdate call to change the Serpents direction
		/// </summary>
		void Update()
		{
			// Get user input from input component
			m_moveInput = m_playerInput.GetInputDirection();
		}

		/// <summary>
		/// Move the Serpent one cell postion, either in current direction or new direction.
		/// If Serpent hits a wall or serpent (other or self), it dies.
		/// If next cell contains the apple, it is consumed and this Serpent gains a new tail segment
		/// </summary>
		void FixedUpdate()
		{
			// If the game has not been started, do nothing/exit function
			if (!SerpentsManager.Instance.GameIsRunning)
			{ 
				return;
			}

			// If user changed to a valid direction, change Serpent direction
			if (m_moveInput != Vector2Int.zero && !IsOppositeOrSameDirection(m_moveInput, m_moveDirection))
			{
				m_moveDirection = m_moveInput;
			}

			// Move Serpent head 1 unit in given direction
			if (!MoveToNextCell())
			{
				// Hit a wall/serpent, dead
				// Fire a 'Dead Serpent' event
				EventDelegate.FireSerpentDied(GetName());
				KillSerpent();
				enabled = false;
				gameObject.SetActive(false);
			}

			// If apple is consumed, add a tail segment
			if (SerpentsManager.Instance.DoesCellContainApple(m_currentHeadCellIndex))
			{
				EventDelegate.FireAppleConsumed();
				AddTailSegment();
			}
		}
		
		/// <summary>
		/// Checks if the given directions are parallel to each other, 
		/// move direction is only changed if new input direction is perpendicular to current move direction
		/// </summary>
		bool IsOppositeOrSameDirection(Vector2Int v1, Vector2Int v2)
		{
			return Mathf.Abs(Vector2.Dot(v1, v2)) == 1;
		}

		/// <summary>
		/// If the next GridCell is valid (not wall or serpent), moves the Serpent head to new position and moves all tail segments to follow.
		/// Returns true if the Serpent moves successfully, false if it hits an obstacle.
		/// </summary>
		private bool MoveToNextCell()
		{			
			GridCell cell = SerpentsManager.Instance.GetAdjacentCell(m_currentHeadCellIndex, m_moveDirection);
			if (cell == null || cell.ContainsSerpent)
			{
				Debug.Log(cell == null ? "Next cell returned null, hit a wall" : "Next cell contains a serpent");
				return false;
			}

			m_currentHeadCellIndex = cell.CellIndex;
			Vector2 cellPos = cell.Position;
			transform.localPosition = new Vector3(cellPos.x, 0, cellPos.y);

			cell.SetContainsSerpent(true);

			List<int> newCellIndices = new List<int>();
			for (int i = 1; i < m_serpentSegments.Count; i++)
			{
				GridCell curCell = SerpentsManager.Instance.GetCell(m_serpentSegments[i]._cellIndex);
				curCell.SetContainsSerpent(false);

				Transform tailSeg = m_serpentSegments[i]._segment;
				GridCell newCell = SerpentsManager.Instance.GetCell(m_serpentSegments[i-1]._cellIndex);
				
				Vector2 pos = newCell.Position;
				tailSeg.position = new Vector3(pos.x, 0, pos.y);
				newCellIndices.Add(newCell.CellIndex);

				newCell.SetContainsSerpent(true);
				m_indexOfLastCell = curCell.CellIndex;
			}

			// Set new cell indices for the serpent segments
			int segmentKey = 0;
			m_serpentSegments[segmentKey].SetCellIndex(cell.CellIndex);

			foreach (int i in newCellIndices)
			{
				m_serpentSegments[++segmentKey].SetCellIndex(i);
			}

			// Reset the new direction vectors to zero
			m_moveInput = Vector2Int.zero;
			m_playerInput.ResetInput();

			return true;
		}

		/// <summary>
		/// Add a new tail segment to the end of the Serpent and set it's colour to match the Serpent
		/// </summary>
		private void AddTailSegment()
		{
			Transform tailSeg = Instantiate(m_tailSegment, m_tailBase).transform;
			
			GridCell tailCell = SerpentsManager.Instance.GetCell(m_indexOfLastCell);
			Vector2 pos = tailCell.Position;
			tailSeg.position = new Vector3(pos.x, 0, pos.y);

			tailCell.SetContainsSerpent(true);

			m_serpentSegments.Add(m_serpentSegments.Count, new SerpentSegment(tailSeg, tailCell.CellIndex));

			Renderer rend = tailSeg.GetComponent<Renderer>();
			rend.material.color = m_colour;
			m_segmentRenderers.Add(rend);
		}

		/// <summary>
		/// The Serpent has died, remove all references to this Serpent from the GridCell array (don't kill another Serpent for moving onto a dead Serpent)
		/// </summary>
		public void KillSerpent()
		{
			// Clear cells so they don't think the serpent is still occupying them
			foreach (var segment in m_serpentSegments.Values)
			{
				GridCell cell = SerpentsManager.Instance.GetCell(segment._cellIndex);
				cell.SetContainsSerpent(false);
			}
		}
	}
}