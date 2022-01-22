using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.IO;
using System;
using Amazon.S3.Util;
using System.Collections.Generic;
using Amazon.CognitoIdentity;
using Amazon;
using UnityEngine.SceneManagement;
using System.Linq;

public class AWSManager : MonoBehaviour
{
    private static AWSManager _instance;
    public static AWSManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("AWSManager is null");
            }    
            
           return _instance;
        }
    }

    public string S3Region = RegionEndpoint.CACentral1.SystemName;
    private RegionEndpoint _S3Region
    {
        get { return RegionEndpoint.GetBySystemName(S3Region); }
    }

    private AmazonS3Client _s3Client;
    public AmazonS3Client S3Client
    {
        get
        {
            if (_s3Client ==null)
            {
                _s3Client = new AmazonS3Client(new CognitoAWSCredentials(
        "us-east-2:f796c670-0afe-480a-ad8d-d02c2f24a379", // Identity Pool ID
        RegionEndpoint.USEast2 // Region
         ),_S3Region);
        }

            return _s3Client;
        }
    }

    private void Awake()
    {
        _instance = this;
        UnityInitializer.AttachToGameObject(this.gameObject);
       
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;

        // ResultText is a label used for displaying status information
        
       /* S3Client.ListBucketsAsync(new ListBucketsRequest(), (responseObject) =>
        {
           
            if (responseObject.Exception == null)
            {            
                responseObject.Response.Buckets.ForEach((s3b) =>
                {
                    Debug.Log("Bucket Name " + s3b.BucketName);                
                });
            }
            else
            {
                Debug.Log("AWS Error" + responseObject.Exception);
            }
        });*/
    }      
    
    public void UploadToS3(string path, string caseID)
    {
        FileStream stream = new FileStream(path, FileMode.Open,FileAccess.ReadWrite,FileShare.ReadWrite);//get file 
        PostObjectRequest request = new PostObjectRequest()           //generate request to upload server
        {
            Bucket = "service-app-case-files-2",
            Key = "case#" + caseID,
            InputStream = stream,
            CannedACL = S3CannedACL.Private, // now need to make use of the request that was posted
            Region = _S3Region

          

        };
   
        S3Client.PostObjectAsync(request, (responseObj) =>
         {
             if (responseObj.Exception == null)
             {
                 Debug.Log("Successfully posted to bucket");
                 SceneManager.LoadScene(0);
             }
             else
             {
                 Debug.Log("Exception occured during uploading" + responseObj.Exception);
             }
         });
    }

    public void GetList(string caseNumber)
    {
        string target = "case#" + caseNumber;

        Debug.Log("AWSManager: get list");
        var request = new ListObjectsRequest()
        {
            BucketName = "service-app-case-files-2"
        };
        S3Client.ListObjectsAsync(request, (responseObject) =>
         {
             if (responseObject.Exception == null)
             {
                 responseObject.Response.S3Objects.ForEach((obj) =>
                 {
                     Debug.Log(obj.Key);

                     if (target==obj.Key)
                     {
                         Debug.Log("Found Case File");
                     }
                 });
             }
             else
             {
                 Debug.Log("Error getting List of Items from S3" + responseObject.Exception);
             }
         });
    
    }

}
