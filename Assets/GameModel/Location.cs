using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public struct PartyLocationSupport
	{
		public Party Party;
		public float Support;
	}

	[Serializable]
	public class Location : ScriptableObject
	{
		[HideInInspector]
		public string Id;

		public string Name;
		[TextArea(15, 20)]
		public string Description;

		public Sprite Icon;
		public Sprite BackgroundImage;

		public Vector2 UiPosition;

		public List<Interaction> VisibilityInteractions;
		public List<Interaction> VisibilityNotCompletedInteractions;
		public bool ClosedOnWeekends;

		public List<Policy> Policies = new List<Policy>();
		public List<Npc> Npcs = new List<Npc>();

		public List<PartyLocationSupport> StartingPartySupport = new List<PartyLocationSupport>();
		[HideInInspector]
		public List<PartyLocationSupport> PartySupport = new List<PartyLocationSupport>();

		[HideInInspector]
		public bool Controlled;

		public void Setup(MainGameManager mgm)
		{
			Controlled = false;

			foreach (var ob in Npcs)
				ob.Setup(mgm);
			foreach (var ob in Policies)
				ob.Setup();

			PartySupport = StartingPartySupport;

			float totalSupport = 0;
			HashSet<Party> supportedParties = new HashSet<Party>();
			foreach (var support in PartySupport)
			{
				if (supportedParties.Contains(support.Party))
					throw new Exception($"{support.Party.Name} has multiple entries in {Name}.  It can only have one.");
				supportedParties.Add(support.Party);

				totalSupport += support.Support;
			}

			if (totalSupport != 1)
			{
				throw new Exception($"Total Party Support in {Name} is not equal to 100!  Ensure that the total equals that.");
			}
		}
		
		public bool IsVisible(MainGameManager mgm)
		{
			foreach (var interaction in VisibilityInteractions)
			{
				if (interaction != null && interaction.Completed == 0)
					return false;
			}
			foreach (var interaction in VisibilityNotCompletedInteractions)
			{
				if (interaction != null && interaction.Completed > 0)
					return false;
			}
			return true;
		}

		public bool HasNewInteractions(MainGameManager mgm)
		{
			return Npcs.Any(n => n.IsVisible(mgm) && n.HasNewInteractions(mgm));
		}

		public bool IsAccessible(MainGameManager mgm)
		{
			var dayOfWeek = mgm.GetDateFromTurnNumber().DayOfWeek;
			return !ClosedOnWeekends || (dayOfWeek != DayOfWeek.Saturday && dayOfWeek != DayOfWeek.Sunday);
		}
	}
}
