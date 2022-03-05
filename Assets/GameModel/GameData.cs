using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public class GameData : ScriptableObject
	{
		public string FirstName = "";
		public string LastName = "";
		public string PartyName = "";

		public int StartingActions = 0;
		public int StartingIntrigue = 0;

		[HideInInspector] public int TurnNumber = 0;
		[HideInInspector] public int Actions = 0;
		[HideInInspector] public int Intrigue = 0;

		[HideInInspector] public int Wealth = 0;
		[HideInInspector] public int Influence = 0;

		[HideInInspector] public int Mandate = 0;

		[HideInInspector] public int Legacy = 0;

		public List<Party> Parties = new List<Party>();
		public List<Location> Locations = new List<Location>();
		public List<Interaction> StartOfTurnInteractions = new List<Interaction>();

		public void Setup(MainGameManager mgm)
		{
			TurnNumber = 0;
			Actions = StartingActions;
			Intrigue = StartingIntrigue;

			Wealth = 0;
			Intrigue = 0;
			Influence = 0;
			Mandate = 0;
			Legacy = 0;

			foreach (var ob in Locations)
				ob.Setup(mgm);

			foreach (var ob in StartOfTurnInteractions)
				ob.Setup();
		}
	}
}