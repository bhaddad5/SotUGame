using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public struct NpcEffect
	{
		public Npc NpcReference;
		public int OpinionEffect;
	}

	[Serializable]
	public struct SupportEffect
	{
		public Location Location;
		public Party Party;
		[Header("Other party's support levels will adjust automatically")]
		public float SupportChange;
	}

	[Serializable]
	public struct Effect
	{
		public List<NpcEffect> NpcEffects;

		public int IntrigueEffect;
		public int WealthEffect;
		public int InfluenceEffect;
		public int PrestigeEffect;
		public int MandateEffect;
		public int LegacyEffect;

		public List<SupportEffect> SupportEffects;
		public List<Npc> NpcsToControl;
		public List<Npc> NpcsToRemoveFromGame;
		
		public void ExecuteEffect(MainGameManager mgm)
		{
			foreach (var effect in NpcEffects)
			{
				var clamp = Mathf.Clamp(effect.NpcReference.Opinion + effect.OpinionEffect, 1, 5);
				effect.NpcReference.Opinion = Mathf.Max(clamp, 0);
				if (clamp == 5)
					effect.NpcReference.TakeControl(mgm);
			}

			mgm.Data.Intrigue = mgm.Data.Intrigue + IntrigueEffect;
			mgm.Data.Wealth = mgm.Data.Wealth + WealthEffect;
			mgm.Data.Influence = mgm.Data.Influence + InfluenceEffect;
			mgm.Data.Prestige = mgm.Data.Prestige + PrestigeEffect;
			mgm.Data.Mandate = mgm.Data.Mandate + MandateEffect;
			mgm.Data.Legacy = mgm.Data.Legacy + LegacyEffect;
			
			foreach (var controlledNpc in NpcsToControl)
			{
				controlledNpc.TakeControl(mgm);
			}

			foreach (var removedNpc in NpcsToRemoveFromGame)
			{
				removedNpc.Exists = false;
			}

			foreach (var effect in SupportEffects)
			{
				effect.Location.PartySupport = NormalizeSupport(HandleSupportChange(effect, effect.Location.PartySupport));
			}
		}

		private List<PartyLocationSupport> HandleSupportChange(SupportEffect effect, List<PartyLocationSupport> currLevels)
		{
			List<PartyLocationSupport> newSupportLevels = new List<PartyLocationSupport>();
			//Make sure the relevant party is in here.
			if(currLevels.All(ps => ps.Party != effect.Party))
				currLevels.Add(new PartyLocationSupport(){Party = effect.Party, Support = 0});

			foreach (var partyLocationSupport in currLevels)
			{
				var supp = partyLocationSupport;
				if (partyLocationSupport.Party == effect.Party)
				{
					supp.Support += effect.SupportChange;
				}
				else
				{
					supp.Support += (supp.Support) * -effect.SupportChange;
				}
				if(supp.Support >= .001f)
					newSupportLevels.Add(supp);
			}

			var hasCrushedParty = newSupportLevels.Any(sl => sl.Support < .05f);

			if (hasCrushedParty)
			{
				var crushedParty = newSupportLevels.First(sl => sl.Support < .05f);
				var crushEffect = new SupportEffect() { Location = effect.Location, Party = crushedParty.Party, SupportChange = -crushedParty.Support };
				newSupportLevels = HandleSupportChange(crushEffect, newSupportLevels);
			}

			return newSupportLevels;
		}

		private List<PartyLocationSupport> NormalizeSupport(List<PartyLocationSupport> currLevels)
		{
			float totalSupport = 0;
			List<PartyLocationSupport> newSupportLevels = new List<PartyLocationSupport>();
			foreach (var currLevel in currLevels)
			{
				totalSupport += ClampOutFraction(currLevel.Support);
				newSupportLevels.Add(new PartyLocationSupport(){Party = currLevel.Party, Support = ClampOutFraction(currLevel.Support)});
			}

			if (totalSupport < 1)
			{
				var remainderRecievingParty = newSupportLevels.FirstOrDefault();
				if (newSupportLevels.Any(sl => sl.Party.IsPlayerParty))
					remainderRecievingParty = newSupportLevels.FirstOrDefault(sl => sl.Party.IsPlayerParty);
				newSupportLevels.Remove(remainderRecievingParty);
				newSupportLevels.Add(new PartyLocationSupport(){Party = remainderRecievingParty.Party, Support = remainderRecievingParty.Support + 1-totalSupport});
			}

			newSupportLevels = newSupportLevels.OrderByDescending(s => s.Support).ToList();

			return newSupportLevels;
		}

		private float ClampOutFraction(float input)
		{
			return ((float)((int)(input * 100f))) / 100f;
		}
	}
}
