using System;
using System.Collections.Generic;
using UnityEngine;

namespace Serpents
{
	/// <summary>
	/// This class component is attached to the same object as the Serpent class, 
	/// it handles the user input for changing the direction of the connected Serpent
	/// </summary>
	public class PlayerInput : MonoBehaviour
	{
		[SerializeField]
		private InputType m_inputType;

		private Vector2Int m_inputDirection;

		public enum InputType
		{
			ArrowKeys = 0,
			WasdKeys,
			NumPad8426,
			COUNT = NumPad8426,
			//Click_Touch,
			//UI_Buttons,
			//COUNT = UI_Buttons,
		}

		/// <summary>
		/// Set the input type
		/// </summary>
		public void SetInputType(InputType inputType)
		{
			m_inputType = inputType;
		}

		/// <summary>
		/// Reset the input direction
		/// </summary>
		public void ResetInput()
		{
			m_inputDirection = Vector2Int.zero;
		}

		/// <summary>
		/// Get the input direction based on the input type
		/// </summary>
		public Vector2Int GetInputDirection()
		{
			switch (m_inputType)
			{
				case InputType.ArrowKeys:
					m_inputDirection = GetDirectionArrowKeys();
					break;
				case InputType.WasdKeys:
					m_inputDirection = GetDirectionWasd();
					break;
				case InputType.NumPad8426:
					m_inputDirection = GetDirection8426Keys();
					break;
				/*case InputType.Click_Touch:
					direction = GetDirectionClickTouch();
					break;
				case InputType.UI_Buttons:
					direction = GetDirectionUIButtons();
					break;*/
				default:
					m_inputDirection = GetDirectionArrowKeys();
					break;
			}

			return m_inputDirection;
		}

		/// <summary>
		/// Get input from Arrow Keys
		/// </summary>
		private Vector2Int GetDirectionArrowKeys()
		{
			if (Input.GetKey(KeyCode.DownArrow))
			{
				m_inputDirection = Vector2Int.down;
			}
			else if (Input.GetKey(KeyCode.UpArrow))
			{
				m_inputDirection = Vector2Int.up;
			}
			else if (Input.GetKey(KeyCode.RightArrow))
			{
				m_inputDirection = Vector2Int.right;
			}
			else if (Input.GetKey(KeyCode.LeftArrow))
			{
				m_inputDirection = Vector2Int.left;
			}

			return m_inputDirection;
		}

		/// <summary>
		/// Get input from WASD Keys
		/// </summary>
		private Vector2Int GetDirectionWasd()
		{
			if (Input.GetKey(KeyCode.S))
			{
				m_inputDirection = Vector2Int.down;
			}
			else if (Input.GetKey(KeyCode.W))
			{
				m_inputDirection = Vector2Int.up;
			}
			else if (Input.GetKey(KeyCode.D))
			{
				m_inputDirection = Vector2Int.right;
			}
			else if (Input.GetKey(KeyCode.A))
			{
				m_inputDirection = Vector2Int.left;
			}

			return m_inputDirection;
		}

		/// <summary>
		/// Get input from 8426 Keys (Arrows on key pad)
		/// </summary>
		private Vector2Int GetDirection8426Keys()
		{
			if (Input.GetKey(KeyCode.Keypad2))
			{
				m_inputDirection = Vector2Int.down;
			}
			else if (Input.GetKey(KeyCode.Keypad8))
			{
				m_inputDirection = Vector2Int.up;
			}
			else if (Input.GetKey(KeyCode.Keypad6))
			{
				m_inputDirection = Vector2Int.right;
			}
			else if (Input.GetKey(KeyCode.Keypad4))
			{
				m_inputDirection = Vector2Int.left;
			}

			return m_inputDirection;
		}

		/// <summary>
		/// Get input from mouse click or finger touch position, relative to serpent position on screen
		/// </summary>
		// Didn't get around to implementing an input system for this method
		private Vector2Int GetDirectionClickTouch()
		{
			throw new NotImplementedException("Input Method not impemented yet, oops");

			//return m_inputDirection;
		}

		/// <summary>
		/// Get input from arrow button in UI on screen
		/// </summary>
		// Didn't get around to implementing an input system for this method
		private Vector2Int GetDirectionUIButtons()
		{
			throw new NotImplementedException("Input Method not impemented yet, oops");

			//return m_inputDirection;
		}
	}
}