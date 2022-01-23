using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectPanel : MonoBehaviour
{
    public Text informationText;

    public void OnEnable()
    {
        informationText.text = UIManager.Instance.activeCase.nameOfClient;
    }
    public void ProcessInfo()
    {

    }
}
