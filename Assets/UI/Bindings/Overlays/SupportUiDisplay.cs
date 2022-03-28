using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;
using UnityEngine.UI;

public class SupportUiDisplay : MonoBehaviour
{
	public Image PartySupportPrefab;

	private Location loc;
	public void Setup(Location loc, MainGameManager mgm)
	{
		this.loc = loc;

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
			slice.GetComponent<TooltipProviderBasic>().Tooltip = supp.Party.Name;
			slice.fillAmount = supp.Support;
			slice.transform.localEulerAngles = new Vector3(0, 0, -(totalSupport * 360f));
			totalSupport += supp.Support;
		}
	}
}
