using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public struct NpcEffect
	{
		[Header("Defaults to the interaction's NPC, if present")]
		public Npc OptionalNpcReference;
		public int OpinionEffect;
	}

	[Serializable]
	public struct Effect
	{
		public List<NpcEffect> NpcEffects;
		
		public int IntrigueEffect;
		public int WealthEffect;
		public int InfluenceEffect;
		public int MandateEffect;
		public int LegacyEffect;

		public List<PartyLocationSupport> SupportEffects;
		public List<Npc> NpcsToControl;
		public List<Npc> NpcsToRemoveFromGame;
		
		public void ExecuteEffect(MainGameManager mgm)
		{
			foreach (var effect in NpcEffects)
			{
				var clamp = Mathf.Clamp(effect.OptionalNpcReference.Opinion + effect.OpinionEffect, 1, 5);
				effect.OptionalNpcReference.Opinion = Mathf.Max(clamp, 0);
				if (clamp == 5)
					effect.OptionalNpcReference.Controlled = true;
			}

			mgm.Data.Intrigue = mgm.Data.Intrigue + IntrigueEffect;
			mgm.Data.Wealth = mgm.Data.Wealth + WealthEffect;
			mgm.Data.Influence = mgm.Data.Influence + InfluenceEffect;
			mgm.Data.Mandate = mgm.Data.Mandate + MandateEffect;
			mgm.Data.Legacy = mgm.Data.Legacy + LegacyEffect;
			
			foreach (var controlledNpc in NpcsToControl)
			{
				controlledNpc.Controlled = true;
			}

			foreach (var removedNpc in NpcsToRemoveFromGame)
			{
				removedNpc.Exists = false;
			}
		}
	}
}
