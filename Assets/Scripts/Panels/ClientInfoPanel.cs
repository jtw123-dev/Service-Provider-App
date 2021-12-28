using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientInfoPanel : MonoBehaviour
{
    public Text caseNumberText;
    public InputField firstName, lastName;
    public GameObject locationPanel;

    public void ProcessInfo()
    {
        if (string.IsNullOrEmpty(firstName.text)||string.IsNullOrEmpty(lastName.text))
        {
            locationPanel.SetActive(false);
            Debug.Log("Is null");
        }
         UIManager.Instance.activeCase.nameOfClient = firstName.text + lastName.text;
    }
    public void NewCase()
    {
        caseNumberText.text = "CASE NUMBER: " + UIManager.Instance.activeCase.caseID.ToString();
       
    }
}
