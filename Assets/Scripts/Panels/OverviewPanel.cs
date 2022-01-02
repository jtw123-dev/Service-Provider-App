using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OverviewPanel : MonoBehaviour
{
    public Text caseNumberTitle;
    public Text nameTitle;
    public Text dateTitle;
    public Text locationTitle;
    public Text locationNotes;
    public RawImage photoTaken;
    public Text photoNotes;

    public void OnEnable()
    {
        caseNumberTitle.text = "CASE NUMBER " + UIManager.Instance.activeCase.caseID;
        nameTitle.text = UIManager.Instance.activeCase.nameOfClient;
        dateTitle.text = DateTime.Today.ToString();
        locationNotes.text = "LOCATION NOTES: \n " + UIManager.Instance.activeCase.locationNotes;
        photoNotes.text = "PHOTO NOTES \n" + UIManager.Instance.activeCase.photoNotes;
        photoTaken.texture = UIManager.Instance.activeCase.photoTaken;
    }

    public void ProcessInfo ()
    {

    }
}
