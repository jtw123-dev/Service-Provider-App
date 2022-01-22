using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
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

    public void CreateNewCase()
    {
        activeCase = new Case();
        activeCase.caseID = Random.Range(0, 1000).ToString();
    }

    public void SubmitButton()
    {
        Case awsCase = new Case();
        awsCase.caseID = activeCase.caseID;
        awsCase.nameOfClient = activeCase.nameOfClient;
        awsCase.date = activeCase.date;
        awsCase.locationNotes = activeCase.locationNotes;
        awsCase.photoTaken = activeCase.photoTaken;
        awsCase.photoNotes = activeCase.photoNotes;

        BinaryFormatter bf = new BinaryFormatter();
        string filePath = Application.persistentDataPath + "/case # " + awsCase.caseID + ".dat";

        FileStream file = File.Create(filePath);
        bf.Serialize(file, awsCase);
        file.Close();

        Debug.Log("Application Data Path: " + Application.persistentDataPath);

        AWSManager.Instance.UploadToS3(filePath, awsCase.caseID );
    }
}
