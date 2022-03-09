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
		public int SupportChange;
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
		}
	}
}
