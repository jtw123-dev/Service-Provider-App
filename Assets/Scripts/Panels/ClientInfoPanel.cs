using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientInfoPanel : MonoBehaviour
{
    public Text caseNumberText;
    public InputField firstName, lastName;

    public void ProcessInfo()
    {
         
    }
    public void NewCase()
    {
        caseNumberText.text = "CASE NUMBER: " + UIManager.Instance.randomCaseID.caseID.ToString();
    }
}
