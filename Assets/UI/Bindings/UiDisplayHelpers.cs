using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.GameModel;
using UnityEngine;

namespace Assets.GameModel.UiDisplayers
{
	public static class UiDisplayHelpers
	{
		public static string ApplyDynamicValuesToString(string str, MainGameManager mgm)
		{
			if (str == null)
				return null;

			var res = str;
			res = res.Replace("{FirstName}", mgm.Data.FirstName);
			res = res.Replace("{LastName}", mgm.Data.LastName);

			return res;
		}

		public static string GetCostString(this ActionCost cost)
		{
			string str = "";
			if (cost.IntrigueCost > 0)
				str += $"-{cost.IntrigueCost} Intrigue, ";
			if (cost.WealthCost > 0)
				str += $"-${cost.WealthCost}M, ";
			if (cost.InfluenceCost > 0)
				str += $"-{cost.InfluenceCost} Influence, ";

			if (str.EndsWith(", "))
				str = str.Substring(0, str.Length - 2);
			return str;
		}

		public static string GetEffectsString(this Effect effect)
		{
			string str = "";
			if (effect.IntrigueEffect > 0)
				str += $"+{effect.IntrigueEffect} Intrigue, ";
			if (effect.WealthEffect > 0)
				str += $"+${effect.WealthEffect}M, ";
			if (effect.InfluenceEffect > 0)
				str += $"+{effect.InfluenceEffect} Influence, ";
			if (effect.MandateEffect > 0)
				str += $"+{effect.MandateEffect} Mandate, ";
			if (effect.LegacyEffect > 0)
				str += $"+{effect.LegacyEffect} Legacy, ";

			if (!String.IsNullOrEmpty(str))
				str = $"Player: {str}";

			if (str.EndsWith(", "))
				str = str.Substring(0, str.Length - 2);

			foreach (var npcEffect in effect.NpcEffects)
			{
				if (!String.IsNullOrEmpty(str))
					str += "\n";

				str += $"{npcEffect.NpcReference.FirstName} {npcEffect.NpcReference.LastName}: ";

				if (npcEffect.OpinionEffect != 0)
					str += $"+{npcEffect.OpinionEffect} Opinion, ";

				if (str.EndsWith(", "))
					str = str.Substring(0, str.Length - 2);
			}

			return str;
		}

		public static string GetInvalidTooltip(this ActionCost cost, MainGameManager mgm)
		{
			List<string> tooltips = new List<string>();
			if (cost.IntrigueCost > mgm.Data.Intrigue)
				tooltips.Add($"{cost.IntrigueCost} Intrigue");
			if (cost.WealthCost > mgm.Data.Wealth)
				tooltips.Add($"${cost.WealthCost}M");
			if (cost.InfluenceCost > mgm.Data.Influence)
				tooltips.Add($"{cost.InfluenceCost} Influence");
			if (cost.PrestigeCost > mgm.Data.Prestige)
				tooltips.Add($"{cost.PrestigeCost} Prestige");

			return TooltipsToString(tooltips);
		}

		public static string GetInvalidTooltip(this ActionRequirements req, MainGameManager mgm)
		{
			List<string> tooltips = new List<string>();

			foreach (var department in req.RequiredDepartmentsControled)
			{
				if(!department.Controlled)
					tooltips.Add($"Control of {department.Name}");
			}

			foreach (var npc in req.RequiredNpcsControled)
			{
				if (!npc.Controlled)
					tooltips.Add($"Control of {npc.FirstName}");
			}
			
			foreach (var requiredPolicy in req.RequiredPolicies)
			{
				if (!requiredPolicy.Active)
					tooltips.Add($"{requiredPolicy.Name}");
			}

			foreach (var interaction in req.RequiredInteractions)
			{
				if(interaction.Completed == 0)
					tooltips.Add($"{interaction.Name}");
			}
			
			if (req.RequiredMandate > mgm.Data.Mandate)
				tooltips.Add($"{req.RequiredMandate} Mandate");

			return TooltipsToString(tooltips);
		}

		private static string TooltipsToString(List<string> tooltips)
		{
			string finalTooltip = "";
			foreach (var tt in tooltips)
			{
				finalTooltip += $", {tt}";
			}

			if (finalTooltip.Length > 0)
			{
				finalTooltip = finalTooltip.Substring(2);
				finalTooltip = $"Requires: {finalTooltip}";
			}

			return finalTooltip;
		}
	}
}