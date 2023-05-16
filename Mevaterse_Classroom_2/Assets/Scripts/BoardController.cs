using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor;
using UnityEngine;

public class BoardController : MonoBehaviourPunCallbacks, IPunObservable
{
    private List<Material> slides = new List<Material>();
    private Object[] textures;
    private string imagesPath;
    private int current = 0;
    void Start()
    {
        //imagesPath = Application.persistentDataPath + "/slides";
        imagesPath = "Images";
        textures = Resources.LoadAll(imagesPath, typeof(Texture2D));

        Logger.Instance.LogInfo("Found " + textures.Length + "slides");
        foreach (Object tex in textures)
        {
            Material mat = new Material(Shader.Find("Standard"));
            mat.mainTexture = (Texture2D)tex;
            slides.Add(mat);
        }        

        /*if(Directory.Exists(imagesPath))
        {
            string[] slidesFound = Directory.GetFiles(imagesPath);
            if (slidesFound.Length > 0)
            {
                foreach (string slidePath in slidesFound)
                {
                    string fileName = slidePath.Substring(slidePath.LastIndexOf('\\') + 1);
                    string filePath = imagesPath + "/" + fileName;
                    Debug.Log(filePath);
                    
                    if(File.Exists(filePath) && !filePath.Contains(".meta"))
                    {
                        byte[] imageData = File.ReadAllBytes(filePath);
                        Texture2D tex = new Texture2D(1, 1);
                        tex.LoadImage(imageData);
                        Material material = new Material(Shader.Find("Standard"));
                        material.mainTexture = tex;
                        slides.Add(material);
                    }
                }
                Logger.Instance.LogInfo("Loaded " + slides.Count + " slides");
            }
            else
                Logger.Instance.LogError("No slides found in " + imagesPath);
        }*/

        GetComponent<Renderer>().material = slides[0];

    }
    void Update()
    {
        if (PhotonNetwork.LocalPlayer.UserId != Presenter.Instance.presenterID) return;

        if(Input.GetKeyUp(KeyCode.RightArrow))
        {
            photonView.RPC("ChangeSlideRpc", RpcTarget.All, +1);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            photonView.RPC("ChangeSlideRpc", RpcTarget.All, -1);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        /*photonView.RPC("ChangeSlideRpc", RpcTarget.All, +1);
        photonView.RPC("ChangeSlideRpc", RpcTarget.All, -1);*/
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // This is the owner of the object; send the variable value to other clients
            stream.SendNext(current);
        }
        else
        {
            // This is a non-owner client; receive the variable value from the owner
            current = (int)stream.ReceiveNext();
        }
    }

    /*private int ChangeCurrent(int value)
    {
        current += value;

        if (current >= slides.Count)
        {
            current = 0;
        }

        if (current < 0)
        {
            current = slides.Count - 1;
        }

        return current;

    }

    [PunRPC]
    public void ChangeSlideRpc(byte[] imageData)
    {
        Debug.Log("Here!!");
        Texture2D tex = new Texture2D(1, 1);
        tex.LoadImage(imageData);
        Debug.Log("Now here!!");
        GetComponent<Renderer>().material.mainTexture = tex;
    }*/

    [PunRPC]
    public void ChangeSlideRpc(int value)
    {
        current += value;

        if (current >= slides.Count)
        {
            current = 0;
        }

        if (current < 0)
        {
            current = slides.Count - 1;
        }
        GetComponent<Renderer>().material = slides[current];
    }
}
