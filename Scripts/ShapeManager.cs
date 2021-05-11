using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

public class ShapeManager: MonoBehaviourPun
{

    private Vector3 offset = new Vector3(0.5f, 0f, 0f);

    public GameObject square;
    //public GameObject circle;
    private PostWwiseEvent postWwiseEvent;
    public static List<GameObject> squareList = new List<GameObject>();
    public static List<GameObject> circleList = new List<GameObject>();
    private static List<GameObject> triList = new List<GameObject>();
    public static int sqIdx;
    public static int cirIdx;
    public static int triIdx;
    public static bool deleteSqCalled;
    public static bool deleteCirCalled;
    public static bool deleteSqDone = true;
    public static bool deleteCirDone = true;
    private Transform playerPos;
    //private static GameObject deletedShape;
    /*private static int squareCountLocal = 0;
    private static int squareCountRemote = 0;*/


    /*private static int circleCountLocal = 0;
    private static int circleCountRemote = 0;*/

    private static PhotonView AIPV;
    private int tempID;
    //private Vector3 inst;

    //public static List<GameObject> triangleList = new List<GameObject>();

    //These functions are called by the buttons in the shape menu to instantiate objects.

    void Start()
    { 
        AIPV = PhotonView.Get(this);
        deleteSqCalled = false;
        deleteCirCalled = false;
        sqIdx = -1;
        cirIdx = -1;
        triIdx = -1;
        postWwiseEvent = gameObject.GetComponent<PostWwiseEvent>();
        //inst = new Vector3(-50f, -8f, 0f);
    }

    //Square creation functions
    public void createSquare()
    {
        if (sqIdx < 7)
        {
            /*if (photonView.IsMine)
            {
                playerPos = Movement.playerPos;
            }*/
            if (squareList.Count - 1 == sqIdx)
            {
                tempID = PhotonNetwork.Instantiate("Square", new Vector3(-20f, -2f, 0f), Quaternion.identity).GetComponent<PhotonView>().ViewID;
                AIPV.RPC("addSquareToList", RpcTarget.AllBufferedViaServer, tempID);
            }
            else if (squareList.Count - 1 > sqIdx)
            {
                AIPV.RPC("setSquareVisible", RpcTarget.AllBufferedViaServer);
            }
            else
            {}
        }
    }

    [PunRPC]
    private void addSquareToList(int tempID)
    { 
        //squareList.Add(Instantiate(square, playerPos.position + offset, Quaternion.identity));
        squareList.Add(PhotonView.Find(tempID).gameObject);
        sqIdx += 1;
    }

    [PunRPC]
    private void setSquareVisible()
    {
        sqIdx += 1;
        PostWwiseEvent.percPlaying[sqIdx] = false;
        squareList[sqIdx].SetActive(true);
    }

    //Square deletion functions
    public void deleteSquareReady()
    {
        if (deleteSqDone)
        {
            if (sqIdx >= 0)
            {
                Debug.Log("Square Index is " + sqIdx.ToString());
                deleteSqCalled = true;
            }
        }
    }

    public static void deleteSquare()
    {
        //This function is called directly from the PostWwiseEvent script when deleteSqCalled is true;
        if (squareList.Count > 0)
        {
            //PhotonNetwork.Destroy(squareList[sqIdx]);
            AIPV.RPC("setSquareInvisible", RpcTarget.AllBufferedViaServer);
        }
        else
        {
            deleteSqDone = true;
        }
    }

    [PunRPC]
    private void setSquareInvisible()
    {
        squareList[sqIdx].SetActive(false);
        sqIdx -= 1;
        deleteSqDone = true;
    }

    //Circle creation
    public void createCircle()
    {
        if (cirIdx < 2)
        {
            if (circleList.Count - 1 == cirIdx)
            {
                tempID = PhotonNetwork.Instantiate("Circle", new Vector3(-20f, -2f, 0f), Quaternion.identity).GetComponent<PhotonView>().ViewID;
                AIPV.RPC("addCircleToList", RpcTarget.AllBufferedViaServer, tempID);
            }
            else if (circleList.Count - 1 > cirIdx)
            {
                AIPV.RPC("setCircleVisible", RpcTarget.AllBufferedViaServer);
            }
            else
            { }
        }
    }

    [PunRPC]
    private void addCircleToList(int tempID)
    {
        circleList.Add(PhotonView.Find(tempID).gameObject);
        cirIdx += 1;
    }

    [PunRPC]
    private void setCircleVisible()
    {
        cirIdx += 1;
        PostWwiseEvent.bkGndPlaying[cirIdx] = false;
        circleList[cirIdx].SetActive(true);
        //postWwiseEvent.CleanBackgrounds(cirIdx, circleList.Count);
    }

    //Circle deletion functions
    public void deleteCircleReady()
    {
        if (deleteCirDone)
        {
            if (cirIdx >= 0)
            {
                //Debug.Log("Square Index is " + sqIdx.ToString());
                deleteCirCalled = true;
            }
        }
    }

    public static void deleteCircle()
    {
        //This function is called directly from the PostWwiseEvent script when deleteSqCalled is true;
        if (circleList.Count > 0)
        {
            //PhotonNetwork.Destroy(squareList[sqIdx]);
            AIPV.RPC("setCircleInvisible", RpcTarget.AllBufferedViaServer);
        }
        else
        {
            deleteCirDone = true;
        }
    }

    [PunRPC]
    private void setCircleInvisible()
    {
        circleList[cirIdx].SetActive(false);
        cirIdx -= 1;
        deleteCirDone = true;
    }
}