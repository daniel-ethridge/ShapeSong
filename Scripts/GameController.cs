using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviourPun
{

    private GameObject character;
    private GameObject head;
    public Transform[] spawnPoints;
    public static Transform[] spnPts;
    public static int roomOcc;
    public static int actorNumber;
    //public static List<int> charIDList = new List<int>();
    //private PhotonView pvGameControl;
    //public static List<string> aliasList = new List<string>();

    private void Awake()
    {
        spnPts = spawnPoints;
        PhotonNetwork.InstantiateRoomObject("AI", Vector3.zero, Quaternion.identity);
        actorNumber = PhotonNetwork.CurrentRoom.PlayerCount;
    }

    

    void Update()
    {
        roomOcc = PhotonNetwork.CurrentRoom.PlayerCount - 1;
    }
}