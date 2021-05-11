using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine.UI;

public class NetworkControllerScript : MonoBehaviourPunCallbacks
{
    public Text txtStatus;
    public GameObject btnStart;
    public byte MaxPlayers = 4;
    public static byte numPlayers;
    public InputField input;
    public static string alias;
    
    public static int actorNumber;
    private PhotonView pvNetControl;
    

    private void Start()
    {
        numPlayers = MaxPlayers;
        PhotonNetwork.ConnectUsingSettings();
        btnStart.SetActive(false);
        Status("Connecting to Server");
        pvNetControl = PhotonView.Get(this);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.AutomaticallySyncScene = true;
        btnStart.SetActive(true);
        Status("Wait until you are instructed to click the 'Start' button.");
    }

    public void btnStart_Click()
    {
        alias = input.text;
        
        string roomName = "Room1";
        Photon.Realtime.RoomOptions opts = new Photon.Realtime.RoomOptions();
        opts.IsOpen = true;
        opts.IsVisible = true;
        opts.MaxPlayers = MaxPlayers;

        PhotonNetwork.JoinOrCreateRoom(roomName, opts, Photon.Realtime.TypedLobby.Default);
        btnStart.SetActive(false);
        Status("Here we go!");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        
        
        //Debug.Log(actorNumber);
        SceneManager.LoadScene("Study");
    }

    private void Status(string msg)
    {
        //Debug.Log(msg);
        txtStatus.text = msg;
    }
}
