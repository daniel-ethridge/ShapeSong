using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Photon.Pun;
using TMPro;


public class menu : MonoBehaviourPun
{
    //private bool GameIsPaused = false;
    private int numberOfPlayers = NetworkControllerScript.numPlayers;
    public GameObject waitCanvas;
    public Text wait;

    public GameObject UIMenu;

    //public GameObject btnCharMenu;
    public GameObject btnCreateCharacter;
    //private bool charCreated;
    private bool ready;

    public TMP_Text part1;
    public TMP_Text part2;
    public TMP_Text part3;

    private PhotonView pvMenu;
    private int adjustVal;

    //Intialize all menus as inactive
    void Start()
    {
        pvMenu = PhotonView.Get(this);
        numberOfPlayers = NetworkControllerScript.numPlayers;
        waitCanvas.SetActive(true);
        UIMenu.SetActive(true);
        btnCreateCharacter.SetActive(false);
        //charCreated = false;
        btnCreateCharacter.SetActive(true);
        wait.text = "Please wait. When you are instructed, click on the button below.";
        //wait.text = string.Format("Still waiting on {0} participants.", numberOfPlayers - 1 - GameController.roomOcc);
        adjustVal = 0;
    }

    public void CreateCharacter()
    {
        PhotonNetwork.Instantiate("Character", GameController.spnPts[GameController.roomOcc].position, GameController.spnPts[GameController.roomOcc].rotation);
        btnCreateCharacter.SetActive(false);
        waitCanvas.SetActive(false);
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 4)
        {
            adjustVal = 1;
        }
        pvMenu.RPC("SetNames", RpcTarget.AllBufferedViaServer, GameController.actorNumber-adjustVal, NetworkControllerScript.alias);
    }

    [PunRPC]
    private void SetNames(int actNmbr, string alias)
    {
        if (actNmbr == 1)
        {
            part1.text = alias;
        }
        else if (actNmbr == 2)
        {
            part2.text = alias;
        }
        else if (actNmbr == 3)
        {
            part3.text = alias;
        }
    }

    public void LeaveStudy()
    {
        Application.Quit();
        Debug.Log("Application.Quit() Called");
    }

}


    /*if (Input.GetKeyDown(KeyCode.Escape))
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }*/


   /* public void Resume()
    {
        shapesMenu.SetActive(false);
        characterMenu.SetActive(false);
        mainMenu.SetActive(false);
        //GameIsPaused = false;
    }

    void Pause()
    {
        mainMenu.SetActive(true);
        //GameIsPaused = true;
    }*/

    /*public void OpenCharacterMenu()
    {
        mainMenu.SetActive(false);
        characterMenu.SetActive(true);
        if (charCreated)
        {
            btnCreateCharacter.SetActive(false);
        }
    }*/

   /* public void OpenShapesMenu()
    {
        mainMenu.SetActive(false);
        shapesMenu.SetActive(true);
    }*/

    /*public void GoBack()
    {
        characterMenu.SetActive(false);
        shapesMenu.SetActive(false);
        mainMenu.SetActive(true);
    }*/

    /*public void GoToGenCharacter()
    {
        GameIsPaused = true;
        characterMenu.SetActive(true);
        btnCreateCharacter.SetActive(true);
        btnCharMenu.SetActive(false);
    }*/

    

   