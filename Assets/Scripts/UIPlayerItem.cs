using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Serpents
{
	/// <summary>
	/// Used to preview each player's name and serpent colour, as well as allowing which input method controls that players serpent
	/// </summary>
	public class UIPlayerItem : MonoBehaviour
	{
		[SerializeField]
		private TMP_Dropdown m_dropdown = null;

		[SerializeField]
		private TextMeshProUGUI m_playerNameText = null;
		private string m_playerName;

		[SerializeField]
		private Image m_colourChip = null;

		private const string NAME_TEXT = "Name: ";

		/// <summary>
		/// Display player/serpent name and a preview of the serpents colour
		/// </summary>
		public void SetParameters(string name, Color colour)
		{
			m_playerName = name;
			m_playerNameText.text = NAME_TEXT + m_playerName;

			m_colourChip.color = colour;
		}

		/// <summary>
		/// Setup dropdown to have the proper values to select user input method
		/// </summary>
		private void OnEnable()
		{
			m_dropdown.options.Clear();

			List<string> values = new List<string>();
			foreach (string name in Enum.GetNames(typeof(PlayerInput.InputType)))
			{
				if (!name.Equals("COUNT"))
				{
					values.Add(name);
				}
			}

			m_dropdown.AddOptions(values);
			m_dropdown.value = 0;

			m_dropdown.onValueChanged.AddListener(OnValueChanged);
		}

		private void OnDisable()
		{
			m_dropdown.onValueChanged.RemoveAllListeners();
		}

		/// <summary>
		/// If user input changed here, fire event to set the new input method to the proper serpent instance
		/// </summary>
		void OnValueChanged(int value)
		{
			PlayerInput.InputType type = (PlayerInput.InputType)value;
			EventDelegate.FirePlayerInputChanged(m_playerName, type);
		}
	}
}