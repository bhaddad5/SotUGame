using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.GameModel.Save
{
	[Serializable]
	public struct SaveGameState
	{
		public string FirstName;
		public string LastName;
		public string PartyName;

		public int TurnNumber;
		public int Actions;
		public int Intrigue;
		public int Wealth;
		public int Influence;
		public int Prestige;
		public int Mandate;
		public int Legacy;
		
		public List<SavedLocationState> Locations;
		public List<SavedInteractionState> StartTurnInteractions;

		public static SaveGameState FromData(GameData data)
		{
			var res = new SaveGameState();

			res.FirstName = data.FirstName ?? "Hunter";
			res.LastName = data.LastName ?? "Downe";
			res.PartyName = data.PartyName ?? "Family";
			res.TurnNumber = data.TurnNumber;
			res.Actions = data.Actions;
			res.Intrigue = data.Intrigue;
			res.Wealth = data.Wealth;
			res.Influence = data.Influence;
			res.Prestige = data.Prestige;
			res.Mandate = data.Mandate;
			res.Legacy = data.Legacy;

			res.Locations = new List<SavedLocationState>();
			foreach (var dataLocation in data.Locations)
			{
				if (dataLocation != null)
					res.Locations.Add(SavedLocationState.FromData(dataLocation));
			}

			res.StartTurnInteractions = new List<SavedInteractionState>();
			foreach (var startOfTurnInteraction in data.StartOfTurnInteractions)
			{
				if(startOfTurnInteraction != null)
					res.StartTurnInteractions.Add(SavedInteractionState.FromData(startOfTurnInteraction));
			}

			return res;
		}

		public void ApplyToData(GameData data)
		{
			data.FirstName = FirstName ?? "Hunter";
			data.LastName = LastName ?? "Downe";
			data.TurnNumber = TurnNumber;
			data.Actions = Actions;
			data.Intrigue = Intrigue;
			data.Wealth = Wealth;
			data.Influence = Influence;
			data.Prestige = Prestige;
			data.Mandate = Mandate;
			data.Legacy = Legacy;

			foreach (var location in Locations)
			{
				location.ApplyToData(data.Locations.FirstOrDefault(d => d?.Id == location.Id));
			}

			foreach (var startTurnInteraction in StartTurnInteractions)
			{
				startTurnInteraction.ApplyToData(data.StartOfTurnInteractions.FirstOrDefault(i => i?.Id == startTurnInteraction.Id));
			}
		}
	}

	[Serializable]
	public struct SavedLocationState
	{
		public string Id;

		public bool Controlled;

		public List<SavedNpcState> Npcs;
		public List<SavedPolicyState> Policies;

		public static SavedLocationState FromData(Location data)
		{
			var res = new SavedLocationState();

			res.Id = data.Id;
			res.Controlled = data.Controlled;

			res.Npcs = new List<SavedNpcState>();
			foreach (var dataNpc in data.Npcs)
			{
				if(dataNpc != null)
					res.Npcs.Add(SavedNpcState.FromData(dataNpc));
			}

			res.Policies = new List<SavedPolicyState>();
			foreach (var dataPolicy in data.Policies)
			{
				if (dataPolicy != null)
					res.Policies.Add(SavedPolicyState.FromData(dataPolicy));
			}

			return res;
		}

		public void ApplyToData(Location data)
		{
			if (data == null)
			{
				Debug.Log($"Could not find location with id {Id}");
				return;
			}

			data.Controlled = Controlled;

			foreach (var npc in Npcs)
			{
				npc.ApplyToData(data.Npcs.FirstOrDefault(d => d?.Id == npc.Id));
			}
			foreach (var policy in Policies)
			{
				policy.ApplyToData(data.Policies.FirstOrDefault(d => d?.Id == policy.Id));
			}
		}
	}

	[Serializable]
	public struct SavedPolicyState
	{
		public string Id;

		public bool Active;

		public static SavedPolicyState FromData(Policy data)
		{
			var res = new SavedPolicyState();
			res.Id = data.Id;
			res.Active = data.Active;

			return res;
		}

		public void ApplyToData(Policy data)
		{
			if (data == null)
			{
				Debug.Log($"Could not find Policy with id {Id}");
				return;
			}
			data.Active = Active;
		}
	}
	
	[Serializable]
	public struct SavedNpcState
	{
		public string Id;

		public bool Controlled;
		public bool Exists;
		public int Opinion;

		public List<SavedInteractionState> Interactions;

		public static SavedNpcState FromData(Npc data)
		{
			var res = new SavedNpcState();
			res.Id = data.Id;
			res.Controlled = data.Controlled;
			res.Exists = data.Exists;
			res.Opinion = data.Opinion;

			res.Interactions = new List<SavedInteractionState>();
			foreach (var dataInteraction in data.Interactions)
			{
				if (dataInteraction != null)
					res.Interactions.Add(SavedInteractionState.FromData(dataInteraction));
			}
			
			return res;
		}

		public void ApplyToData(Npc data)
		{
			if (data == null)
			{
				Debug.Log($"Could not find npc with id {Id}");
				return;
			}

			data.Controlled = Controlled;
			data.Exists = Exists;
			data.Opinion = Opinion;

			foreach (var interaction in Interactions)
			{
				interaction.ApplyToData(data.Interactions.FirstOrDefault(d => d?.Id == interaction.Id));
			}
		}
	}
	
	[Serializable]
	public struct SavedInteractionState
	{
		public string Id;

		public int Completed;
		public bool New;

		public static SavedInteractionState FromData(Interaction data)
		{
			var res = new SavedInteractionState();
			res.Id = data.Id;
			res.Completed = data.Completed;
			res.New = data.New;

			return res;
		}

		public void ApplyToData(Interaction data)
		{
			if (data == null)
			{
				Debug.Log($"Could not find interaction with id {Id}");
				return;
			}
			data.Completed = Completed;
			data.New = New;
		}
	}
}
