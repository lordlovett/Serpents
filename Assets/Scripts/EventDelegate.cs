using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Serpents
{
	public class EventDelegate : MonoBehaviour
	{
		#region Game Event Delegates
		public delegate void OnAppleConsumed();
		public static OnAppleConsumed appleConsumed;

		public static void FireAppleConsumed()
		{
			appleConsumed?.Invoke();
		}

		public delegate void OnSerpentDied(string serpentName);
		public static OnSerpentDied serpentDied;

		public static void FireSerpentDied(string serpentName)
		{
			serpentDied?.Invoke(serpentName);
		}

		#endregion

		#region UI Event Delegates
		public delegate void OnGameOver();
		public static OnGameOver gameOver;

		public static void FireGameOver()
		{
			gameOver?.Invoke();
		}

		public delegate void OnPlayerInputChanged(string name, PlayerInput.InputType type);
		public static OnPlayerInputChanged playerInputChanged;

		public static void FirePlayerInputChanged(string name, PlayerInput.InputType type)
		{
			playerInputChanged?.Invoke(name, type);
		}

		#endregion

	}
}