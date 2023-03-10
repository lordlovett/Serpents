using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Serpents
{
	public class UIManager : MonoBehaviour
	{
		[SerializeField]
		private GameObject m_gameSetupScreen = null;
		[SerializeField]
		private GameObject m_gameOverScreen = null;

		[SerializeField]
		private TextMeshProUGUI m_playerCount = null;
		private const string PLAYER_COUNT_TEXT = "Player Count: ";

		[SerializeField]
		private GameObject m_playerPanel = null;
		[SerializeField]
		private UIPlayerItem m_playerUIPrefab = null;

		private Dictionary<string, Serpent> m_players = new Dictionary<string, Serpent>();
		private Dictionary<string, UIPlayerItem> m_playersUI = new Dictionary<string, UIPlayerItem>();

		private void OnEnable()
		{
			EventDelegate.gameOver += ShowGameOverScreen;
			EventDelegate.playerInputChanged += PlayerInputChanged;
		}

		private void OnDisable()
		{
			EventDelegate.gameOver -= ShowGameOverScreen;
			EventDelegate.playerInputChanged -= PlayerInputChanged;
		}

		private void Start()
		{
			for (int i = 0; i < SerpentsManager.Instance.StartingPlayerCount; i++)
			{
				string name = "Jormungandr_" + i.ToString();
				Color colour = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.8f, 1f);
				Serpent serp = SerpentsManager.Instance.CreatePlayer(name, PlayerInput.InputType.ArrowKeys, colour);

				UIPlayerItem playerItem = Instantiate(m_playerUIPrefab, m_playerPanel.transform);
				playerItem.SetParameters(name, colour);

				m_players.Add(name, serp);
				m_playersUI.Add(name, playerItem);
			}

			m_playerCount.text = PLAYER_COUNT_TEXT + m_players.Count;
		}

		private void ShowGameOverScreen()
		{
			m_gameOverScreen.SetActive(true);
		}

		public void StartGameButton()
		{
			if (m_players.Count > 0)
			{
				SerpentsManager.Instance.StartGame();
				m_gameSetupScreen.SetActive(false);
			}
		}

		public void AddPlayerButton()
		{
			string name = "Jormungandr_" + m_players.Count.ToString();
			Color colour = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.8f, 1f);
			Serpent serp = SerpentsManager.Instance.CreatePlayer(name, PlayerInput.InputType.ArrowKeys, colour);

			UIPlayerItem playerItem = Instantiate(m_playerUIPrefab, m_playerPanel.transform);
			playerItem.SetParameters(name, colour);

			m_players.Add(name, serp);
			m_playersUI.Add(name, playerItem);

			m_playerCount.text = PLAYER_COUNT_TEXT + m_players.Count;
		}

		public void RemovePlayerButton()
		{
			Serpent serp = m_players.Values.Last();
			m_players.Remove(serp.GetName());

			Destroy(m_playersUI[serp.GetName()].gameObject);
			m_playersUI.Remove(serp.GetName());

			SerpentsManager.Instance.RemoveSerpent(serp.GetName());
			serp.KillSerpent();
			Destroy(serp.gameObject);

			m_playerCount.text = PLAYER_COUNT_TEXT + m_players.Count;
		}

		private void PlayerInputChanged(string playerName, PlayerInput.InputType type)
		{
			// Set player input to 'type'
			m_players[playerName].SetInputType(type);
		}
	}
}