using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.GameModel.UiDisplayers;
using Assets.GameModel.Save;
using UnityEngine;

namespace Assets.GameModel
{
	public class MainGameManager : MonoBehaviour
	{
		public int MajorVersion;
		public int MinorVersion;
		public int Patch;
		public string VersionName;

		[SerializeField]
		private GameData DefaultGameData;
		[HideInInspector]
		public GameData Data;

		public string DefaultFirstName => DefaultGameData.FirstName;
		public string DefaultLastName => DefaultGameData.LastName;
		public string DefaultPartyName => DefaultGameData.PartyName;

		[SerializeField] private HudBindings HudUiDisplayPrefab;
		[SerializeField] private MainMapScreenBindings MainMapUiDisplayPrefab;

		private HudBindings hudUiDisplay;
		private MainMapScreenBindings mainMapUiDisplay;
		
		public void InitializeGame(string saveDataPath, string firstName, string lastName, string partyName)
		{
			if (hudUiDisplay != null)
			{
				GameObject.Destroy(hudUiDisplay.gameObject);
			}
			if (mainMapUiDisplay != null)
			{
				mainMapUiDisplay.CloseCurrentDepartment(false);
				GameObject.Destroy(mainMapUiDisplay.gameObject);
			}

			hudUiDisplay = Instantiate(HudUiDisplayPrefab);
			mainMapUiDisplay = Instantiate(MainMapUiDisplayPrefab);

			NullCleanupLogic.CleanUpAnnoyingNulls(DefaultGameData);
			DefaultDataLogic.ImposeDefaultsOnNullFields(DefaultGameData);
			DefaultGameData.Setup(this);

			Data = DefaultGameData;

			if (saveDataPath != null)
			{
				SaveLoadHandler.LoadAndApplyToGameData(saveDataPath, Data);
			}
			else
			{
				Data.FirstName = firstName;
				Data.LastName = lastName;
				Data.PartyName = partyName;
			}

			hudUiDisplay.Setup(this, mainMapUiDisplay);
			mainMapUiDisplay.Setup(this, Data.Locations);
			RefreshAllUi();

			TryRunStartOfTurnInteractions();
		}

		void OnApplicationQuit()
		{
			DefaultGameData.Setup(this);
			Debug.Log("Data reset on quit");
		}

		public void RefreshAllUi()
		{
			hudUiDisplay.RefreshUiDisplay(this);
			mainMapUiDisplay.RefreshUiDisplay(this);
		}

		public void HandleTurnChange()
		{
			Data.TurnNumber++;
			Data.Actions = Data.StartingActions;
			
			var dateTime = GetDateFromTurnNumber();
			if (Data.TurnNumber % 2 == 0 && 
			    (dateTime.Day == 15 || dateTime.Day == DateTime.DaysInMonth(dateTime.Year, dateTime.Month)))
			{
				//TODO: Elections!
			}

			string path = LoadSaveHelpers.FileToValidPath("Autosave");
			if (path == null)
				return;
			File.WriteAllText(path, SaveLoadHandler.SaveToJson(Data));

			mainMapUiDisplay.CloseCurrentDepartment(true);

			RefreshAllUi();

			TryRunStartOfTurnInteractions();
		}

		private void TryRunStartOfTurnInteractions()
		{
			foreach (var startOfTurnInteraction in Data.StartOfTurnInteractions)
			{
				if (startOfTurnInteraction.InteractionValid(this))
				{
					/*bool succeeded = startOfTurnInteraction.GetInteractionSucceeded();
					var res = startOfTurnInteraction.GetInteractionResult(succeeded);
					var displayHandler = new InteractionResultDisplayManager();
					displayHandler.DisplayInteractionResult(startOfTurnInteraction.Completed, res, !succeeded, this, () =>
					{
						res.Execute(this);
						if(succeeded)
							startOfTurnInteraction.Completed++;
						RefreshAllUi();
					});*/
				}
			}
		}
		
		public DateTime GetDateFromTurnNumber()
		{
			int elapsedYears = Data.TurnNumber / 12;
			int elapsedMonths = Data.TurnNumber % 12;

			var currentDate = new DateTime(2030 + elapsedYears, elapsedMonths+1, 1);

			return currentDate;
		}
	}
}