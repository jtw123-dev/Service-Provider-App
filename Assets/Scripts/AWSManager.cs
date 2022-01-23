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
using System.Runtime.Serialization.Formatters.Binary;

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
        
        Debug.Log("Made request file " + request.Region.ToString() );
        S3Client.PostObjectAsync(request, (responseObj) =>
         {
             Debug.Log("Inside post object async");
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

    public void GetList(string caseNumber, Action onComplete = null)
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
               bool caseFound =  responseObject.Response.S3Objects.Any(obj => obj.Key == target);
                 
                 if (caseFound == true)
                 {
                     Debug.Log("Case Found");
                     S3Client.GetObjectAsync("service-app-case-files-2", target, (responseObj) =>

                       {
                           //byte array to store data from file
                           if (responseObj.Response.ResponseStream !=null)
                           {
                               byte[] data = null;

                               //use stream reader to read response data
                               using (StreamReader reader = new StreamReader(responseObj.Response.ResponseStream))
                               {
                                   using (MemoryStream memory = new MemoryStream())
                                   {
                                       //populate data with memory stream  data 
                                       var buffer = new byte[512];
                                       var bytesRead = default(int);

                                       while ((bytesRead = reader.BaseStream.Read(buffer,0,buffer.Length)) >0)
                                       {
                                           memory.Write(buffer, 0, bytesRead);

                                       }
                                       data = memory.ToArray();
                                   }
                               }

                               using (MemoryStream memory = new MemoryStream(data))
                               {
                                   BinaryFormatter bf = new BinaryFormatter();
                                   Case downloadedCase = (Case)bf.Deserialize(memory);
                                   Debug.Log("Downloaded case name " + downloadedCase.nameOfClient);
                                   UIManager.Instance.activeCase = downloadedCase;
                                  
                                   if (onComplete!=null)
                                   {
                                       onComplete();
                                   }
                                   
                                   
                               }
                           }
                       });
                 }
                 else
                 {
                     Debug.Log("Case not found");
                 }
             }
             else
             {
                 Debug.Log("Error getting List of Items from S3" + responseObject.Exception);
             }
         });
    
    }

}
