using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNamePickerBindings : MonoBehaviour
{
	private Action<string, string, string> startGame;
	[SerializeField] private TMP_InputField FirstName;
	[SerializeField] private TMP_InputField LastName;
	[SerializeField] private TMP_InputField PartyName;

	public void Setup(string defaultFirstName, string defaultLastName, string defaultPartyName, Action<string, string, string> startGame)
	{
		this.startGame = startGame;
		FirstName.text = defaultFirstName;
		LastName.text = defaultLastName;
		PartyName.text = defaultPartyName;
	}

	public void StartGame()
	{
		string fn = FirstName.text;
		string ln = LastName.text;
		string pn = PartyName.text;

		if (!String.IsNullOrEmpty(fn) && !String.IsNullOrEmpty(ln) && !string.IsNullOrEmpty(pn))
		{
			ClosePopup();
			startGame?.Invoke(fn, ln, pn);
		}
	}

	public void ClosePopup()
	{
		GameObject.Destroy(transform.parent.gameObject);
	}
}
