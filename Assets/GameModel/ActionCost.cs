using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public struct ActionCost
	{
		public int IntrigueCost;
		public int WealthCost;
		public int InfluenceCost;
		public int PrestigeCost;

		public bool CanAffordCost(MainGameManager mgm)
		{
			if (mgm.Data.Actions <= 0)
				return false;

			return IntrigueCost <= mgm.Data.Intrigue &&
			       WealthCost <= mgm.Data.Wealth &&
			       InfluenceCost <= mgm.Data.Influence &&
			       PrestigeCost <= mgm.Data.Prestige;
		}

		public void SubtractCost(MainGameManager mgm)
		{
			mgm.Data.Actions -= 1;

			mgm.Data.Intrigue -= IntrigueCost;
			mgm.Data.Wealth -= WealthCost;
			mgm.Data.Influence -= InfluenceCost;
			mgm.Data.Prestige -= PrestigeCost;
		}
	}
}