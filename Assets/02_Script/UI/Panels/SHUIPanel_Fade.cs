using UnityEngine;
using System;
using System.Collections;

public class SHUIPanel_Fade : SHUIBasePanel
{
    #region Virtual Functions
    public override void OnAfterShow(params object[] pArgs)
    {
        if ((null == pArgs) || (1 > pArgs.Length))
            return;

        ((Action)pArgs[0])();
        Close();
    }
    #endregion
}
