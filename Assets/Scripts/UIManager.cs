using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance != null)
            {
                Debug.Log("_instance is null");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    public Case activeCase;
    public Case randomCaseID;
    public void CreateNewCase()
    {
        activeCase = new Case();
       
        randomCaseID.caseID = Random.Range(0, 1000).ToString();
        activeCase = randomCaseID;
    }
}
