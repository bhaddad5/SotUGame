using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;
using UnityEngine.UI;

public class SupportUiDisplay : MonoBehaviour
{
	public Image PartySupportPrefab;
	public TooltipProviderAdvanced SupportTooltip;

	private Location loc;
	public void Setup(Location loc, MainGameManager mgm)
	{
		this.loc = loc;
		SupportTooltip.Setup(GetSupportTooltip);

		RefreshUiDisplay(mgm);
	}

	public void RefreshUiDisplay(MainGameManager mgm)
	{
		for (int i = transform.childCount -1; i >= 0; i--)
		{
			GameObject.Destroy(transform.GetChild(i).gameObject);
		}

		float totalSupport = 0;
		foreach (var supp in loc.PartySupport)
		{
			var slice = GameObject.Instantiate(PartySupportPrefab, transform);
			slice.color = supp.Party.color;
			slice.fillAmount = supp.Support;
			slice.transform.localEulerAngles = new Vector3(0, 0, -(totalSupport * 360f));
			totalSupport += supp.Support;
		}
	}

	public string GetSupportTooltip()
	{
		string res = "";
		foreach (var support in loc.PartySupport)
		{
			res += $"{support.Party.Name}: {(support.Support * 100f).ToString("f0")}%\n";
		}

		//Remove trailing \n
		res = res.Substring(0, res.Length - 1);
		return res;
	}
}
