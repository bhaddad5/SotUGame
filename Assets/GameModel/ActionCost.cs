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

		public bool CanAffordCost(MainGameManager mgm)
		{
			return IntrigueCost <= mgm.Data.Intrigue &&
			       WealthCost <= mgm.Data.Wealth &&
			       InfluenceCost <= mgm.Data.Influence;
		}

		public void SubtractCost(MainGameManager mgm)
		{
			mgm.Data.Intrigue -= IntrigueCost;
			mgm.Data.Wealth -= WealthCost;
			mgm.Data.Influence -= InfluenceCost;
		}
	}
}