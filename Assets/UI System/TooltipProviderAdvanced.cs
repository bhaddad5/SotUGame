using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipProviderAdvanced : MonoBehaviour, ITooltipProvider
{
	private Func<string> getTooltip;
	public void Setup(Func<string> getTooltip)
	{
		this.getTooltip = getTooltip;
	}

    public string GetTooltip()
    {
	    return getTooltip();
    }
}
