using System;
using System.Collections.Generic;
using UnityEngine;

namespace Serpents
{
	public class PlayerInput : MonoBehaviour
	{
		[SerializeField]
		private InputType m_inputType;

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

		public void SetInputType(InputType inputType)
		{
			m_inputType = inputType;
		}

		public Vector2Int GetInputDirection()
		{
			Vector2Int direction = Vector2Int.zero;

			switch (m_inputType)
			{
				case InputType.ArrowKeys:
					direction = GetDirectionArrowKeys();
					break;
				case InputType.WasdKeys:
					direction = GetDirectionWasd();
					break;
				case InputType.NumPad8426:
					direction = GetDirection8426Keys();
					break;
				/*case InputType.Click_Touch:
					direction = GetDirectionClickTouch();
					break;
				case InputType.UI_Buttons:
					direction = GetDirectionUIButtons();
					break;*/
				default:
					direction = GetDirectionArrowKeys();
					break;
			}

			return direction;
		}

		private Vector2Int GetDirectionArrowKeys()
		{
			Vector2Int direction = Vector2Int.zero;

			if (Input.GetKey(KeyCode.DownArrow))
			{
				direction = Vector2Int.down;
			}
			else if (Input.GetKey(KeyCode.UpArrow))
			{
				direction = Vector2Int.up;
			}
			else if (Input.GetKey(KeyCode.RightArrow))
			{
				direction = Vector2Int.right;
			}
			else if (Input.GetKey(KeyCode.LeftArrow))
			{
				direction = Vector2Int.left;
			}

			return direction;
		}

		private Vector2Int GetDirectionWasd()
		{
			Vector2Int direction = Vector2Int.zero;

			if (Input.GetKey(KeyCode.S))
			{
				direction = Vector2Int.down;
			}
			else if (Input.GetKey(KeyCode.W))
			{
				direction = Vector2Int.up;
			}
			else if (Input.GetKey(KeyCode.D))
			{
				direction = Vector2Int.right;
			}
			else if (Input.GetKey(KeyCode.A))
			{
				direction = Vector2Int.left;
			}

			return direction;
		}

		private Vector2Int GetDirection8426Keys()
		{
			Vector2Int direction = Vector2Int.zero;

			if (Input.GetKey(KeyCode.Keypad2))
			{
				direction = Vector2Int.down;
			}
			else if (Input.GetKey(KeyCode.Keypad8))
			{
				direction = Vector2Int.up;
			}
			else if (Input.GetKey(KeyCode.Keypad6))
			{
				direction = Vector2Int.right;
			}
			else if (Input.GetKey(KeyCode.Keypad4))
			{
				direction = Vector2Int.left;
			}

			return direction;
		}

		private Vector2Int GetDirectionClickTouch()
		{
			throw new NotImplementedException("Input Method not impemented yet, oops");

			//Vector2Int direction = Vector2Int.zero;

			/*if (Input.GetKey(KeyCode.DownArrow))
			{
				direction = Vector2Int.down;
			}
			else if (Input.GetKey(KeyCode.UpArrow))
			{
				direction = Vector2Int.up;
			}
			else if (Input.GetKey(KeyCode.RightArrow))
			{
				direction = Vector2Int.right;
			}
			else if (Input.GetKey(KeyCode.LeftArrow))
			{
				direction = Vector2Int.left;
			}*/

			//return direction;
		}

		private Vector2Int GetDirectionUIButtons()
		{
			throw new NotImplementedException("Input Method not impemented yet, oops");

			//Vector2Int direction = Vector2Int.zero;

			/*if (Input.GetKey(KeyCode.DownArrow))
			{
				direction = Vector2Int.down;
			}
			else if (Input.GetKey(KeyCode.UpArrow))
			{
				direction = Vector2Int.up;
			}
			else if (Input.GetKey(KeyCode.RightArrow))
			{
				direction = Vector2Int.right;
			}
			else if (Input.GetKey(KeyCode.LeftArrow))
			{
				direction = Vector2Int.left;
			}*/

			//return direction;
		}
	}
}