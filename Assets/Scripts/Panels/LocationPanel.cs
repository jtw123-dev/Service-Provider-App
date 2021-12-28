using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationPanel : MonoBehaviour
{
    public RawImage map;
    public InputField mapNotes;
    public Text caseNumberTitle;

    public void OnEnable()
    {
        caseNumberTitle.text = "CASE NUMBER " + UIManager.Instance.activeCase.caseID;
    }

    public void ProcessInfo()
    {
        if (string.IsNullOrEmpty(mapNotes.text)==false)
        {
            UIManager.Instance.activeCase.locationNotes = mapNotes.text;
        }
    }
}
