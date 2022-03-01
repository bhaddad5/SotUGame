using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.GameModel;
using Assets.GameModel.UiDisplayers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudBindings : MonoBehaviour
{
	[SerializeField] private TMP_Text PlayerName;
	[SerializeField] private TMP_Text PartyName;
	
	[SerializeField] private TMP_Text Actions;

	[SerializeField] private ResourceManagerUiDisplay Intrigue;
	[SerializeField] private ResourceManagerUiDisplay Wealth;
	[SerializeField] private ResourceManagerUiDisplay Influence;
	[SerializeField] private ResourceManagerUiDisplay Mandate;
	[SerializeField] private ResourceManagerUiDisplay Legacy;

	[SerializeField] private TMP_Text Month;
	[SerializeField] private TMP_Text Year;

	[SerializeField] private Button MainMenuButton;
	[SerializeField] private MainMenuBindings MainMenuPrefab;
	private MainMenuBindings mainMenu;

	private MainGameManager mgm;
	private MainMapScreenBindings mapDisplay;
	public void Setup(MainGameManager mgm, MainMapScreenBindings mapDisplay)
	{
		this.mapDisplay = mapDisplay;
		this.mgm = mgm;
		
		MainMenuButton.onClick.AddListener(OpenMainMenu);
	}

	public void EndTurn()
	{
		mgm.HandleTurnChange();
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if(mainMenu != null)
				CloseMainMenu();
			else if (mainMenu == null)
				OpenMainMenu();
		}
	}

	public void OpenMainMenu()
	{
		var popupParent = GameObject.Instantiate(UiPrefabReferences.Instance.PopupOverlayParent);
		mainMenu = GameObject.Instantiate(MainMenuPrefab, popupParent.transform);
		mainMenu.Setup(mgm);
	}

	public void CloseMainMenu()
	{
		GameObject.Destroy(mainMenu.transform.parent.gameObject);
		mainMenu = null;
	}

	public void RefreshUiDisplay(MainGameManager mgm)
	{
		PlayerName.text = $"{mgm.Data.FirstName} {mgm.Data.LastName}";
		PartyName.text = $"{mgm.Data.PartyName} Party";

		Actions.text = mgm.Data.Actions.ToString();
		Intrigue.RefreshResourceDisplay(mgm.Data.Intrigue);
		Wealth.RefreshResourceDisplay(mgm.Data.Wealth);
		Influence.RefreshResourceDisplay(mgm.Data.Influence);
		Mandate.RefreshResourceDisplay(mgm.Data.Mandate);
		Legacy.RefreshResourceDisplay(mgm.Data.Legacy);

		var DateTime = mgm.GetDateFromTurnNumber();
		Month.text = $"{DateTime:MMMM}";
		Year.text = $"{DateTime:yyyy}";
	}
}
