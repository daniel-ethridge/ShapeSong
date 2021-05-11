using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Movement : MonoBehaviourPun
{
    //public static bool wwiseSnap = false;
    public static Transform playerPos;
    public float speed = 10f;
    public Rigidbody2D rb;

    private menu Menu;
    private float blOn = 1f;
    private float blOff = 0.8f;
    private float blAmt;
    public static bool snap = false;
    private float value;
    private float ignore;

    public SpriteRenderer head;
    public SpriteRenderer torso;
    public SpriteRenderer leftArm;
    public SpriteRenderer rightArm;
    public SpriteRenderer leftLeg;
    public SpriteRenderer rightLeg;

    public Slider headColor;
    public Slider torsoColor;
    public Slider armColor;
    public Slider legColor;

    private float headVal;
    private float torsoVal;
    private float armVal;
    private float legVal;
    private static bool changeVal;
    private static bool pastColor;

    public AK.Wwise.RTPC melodyHead;
    public AK.Wwise.RTPC melodyTorso;
    public AK.Wwise.RTPC melodyArms;
    public AK.Wwise.RTPC melodyLegs;
    public AK.Wwise.RTPC charXPos;
    public int selected;


    //private string nameOne = "Ralph";
    //private string nameTwo = "Bill";
    private GameObject character;
    private Player localPlayer;
    private PhotonView pvChar;
    private static int charID;
    private static int control;
    private static int myControl;

    public TextMesh Caption;
    //private Text CaptionText;

    Vector2 movement;

    private static int[] charIDList = new int[] {0, 0, 0, 0};

    public void Start()
    {
        //parts = new TMP_Text[] { part1, part2, part2 };
        //selected = GameController.actorNumber - 1;
        changeVal = false;
        pastColor = true;
        pvChar = PhotonView.Get(this);
        if (photonView.IsMine)
        {
            photonView.RPC("addCharIDtoList", RpcTarget.AllBufferedViaServer, GameController.actorNumber-1, photonView.ViewID);
            control = photonView.ViewID;
            myControl = photonView.ViewID;
            playerPos = PhotonView.Find(control).gameObject.GetComponent<Transform>();
        }

        headColor = GameObject.FindWithTag("headColor").GetComponent<Slider>();
        torsoColor = GameObject.FindWithTag("torsoColor").GetComponent<Slider>();
        armColor = GameObject.FindWithTag("armColor").GetComponent<Slider>();
        legColor = GameObject.FindWithTag("legColor").GetComponent<Slider>();

        //Instantiate character color sliders

        //Player position as a static variable that all other scripts use
        

        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).name == "Alias")
            {
                //PhotonNetwork.CurrentRoom.AddPlayer();
                localPlayer = PhotonNetwork.CurrentRoom.GetPlayer(GameController.actorNumber);
                //Debug.Log(PhotonNetwork.CurrentRoom == null);
                localPlayer.NickName = NetworkControllerScript.alias; //.aliasList[NetworkControllerScript.actorNumber-1];
                Caption.text = pvChar.Owner.NickName;
                //parts[GameController.actorNumber].text = pvChar.Owner.NickName;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (photonView.ViewID == myControl)
            {
                Color.RGBToHSV(head.color, out value, out ignore, out ignore);
                headVal = value;

                Color.RGBToHSV(torso.color, out value, out ignore, out ignore);
                torsoVal = value;

                Color.RGBToHSV(leftArm.color, out value, out ignore, out ignore);
                armVal = value;

                Color.RGBToHSV(leftLeg.color, out value, out ignore, out ignore);
                legVal = value;
                

                snap = true;
            }
        }

        if (photonView.CreatorActorNr == GameController.actorNumber)
        {
            if ((Input.GetKeyUp("a")) || (Input.GetKeyUp("d")))
            {
                this.movement.x = 0;
            }
            if ((Input.GetKeyUp("w")) || (Input.GetKeyUp("s")))
            {
                this.movement.y = 0;
            }

            if (Input.GetKeyDown("a"))
            {
                if (rb.position.x >= PostWwiseEvent.mapLeft)
                {
                    this.movement.x = -1;
                }
                else { this.movement.x = 0; }
            }

            else if (Input.GetKeyDown("d"))
            {
                if (rb.position.x <= PostWwiseEvent.mapRight)
                {
                    this.movement.x = 1;
                }
            }
                    
            if (Input.GetKeyDown("w"))
            {
                if (rb.position.y <= PostWwiseEvent.mapTop)
                {
                    this.movement.y = 1;
                }
            }

            else if (Input.GetKeyDown("s"))
            {
                if (rb.position.y >= PostWwiseEvent.mapBottom)
                {
                    this.movement.y = -1;
                }
            }

            Color.RGBToHSV(head.color, out value, out ignore, out ignore);
            melodyHead.SetGlobalValue(value * 90);

            Color.RGBToHSV(torso.color, out value, out ignore, out ignore);
            melodyTorso.SetGlobalValue(value * 90);

            Color.RGBToHSV(leftArm.color, out value, out ignore, out ignore);
            melodyArms.SetGlobalValue(value * 90);

            Color.RGBToHSV(leftLeg.color, out value, out ignore, out ignore);
            melodyLegs.SetGlobalValue(value * 90);
        }

        if (photonView.ViewID == control)
        {
            Controls();
        }
    }

    

    public void SelectFirstParticipantCharacter()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 4)
        {
            control = charIDList[1];
        } 
        else
        {
            control = charIDList[0];
        }

        snap = true;
        Debug.Log("First: " + control.ToString());
    }

    public void SelectSecondParticipantCharacter()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 4)
        {
            control = charIDList[2];
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            control = charIDList[1];
        }

        Debug.Log("Second: " + control.ToString());
        snap = true;
    }

    public void SelectThirdParticipantCharacter()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 4)
        {
            control = charIDList[3];
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount >= 3)
        {
            control = charIDList[2];
        }

        snap = true;
        

        Debug.Log("Third: " + control.ToString());
    }

    void FixedUpdate()
    {
        //Move the character
        if ((rb.position.x >= PostWwiseEvent.mapRight) && (this.movement.x == 1))
        {
            this.movement.x = 0;
        }
        if ((rb.position.x <= PostWwiseEvent.mapLeft) && (this.movement.x == -1))
        {
            this.movement.x = 0;
        }
        if ((rb.position.y >= PostWwiseEvent.mapTop) && (this.movement.y == 1))
        {
            this.movement.y = 0;
        }
        if ((rb.position.y <= PostWwiseEvent.mapBottom) && (this.movement.y == -1))
        {
            this.movement.y = 0;
        }
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        charXPos.SetGlobalValue(100 * (rb.position.x - PostWwiseEvent.mapLeft) / PostWwiseEvent.mapHorizontalDist);
        
    }



            
        /*Color.RGBToHSV(head.color, out value, out ignore, out ignore);
        headColor.value = value;
        Color.RGBToHSV(torso.color, out value, out ignore, out ignore);
        torsoColor.value = value;
        Color.RGBToHSV(leftArm.color, out value, out ignore, out ignore);
        armColor.value = value;
        Color.RGBToHSV(leftLeg.color, out value, out ignore, out ignore);
        legColor.value = value;*/
    

    private void Controls()
    {
        //Get value of character color sliders. 
        if (snap)
        {
            /*Color.RGBToHSV(head.color, out value, out ignore, out ignore);
            headVal = value;
            headColor.value = headVal;*/

            /*Color.RGBToHSV(torso.color, out value, out ignore, out ignore);
            melodyTorso.SetGlobalValue(value * 100);

            Color.RGBToHSV(leftArm.color, out value, out ignore, out ignore);
            melodyArms.SetGlobalValue(value * 100);

            Color.RGBToHSV(leftLeg.color, out value, out ignore, out ignore);
            melodyLegs.SetGlobalValue(value * 100);*/

            headColor.value = headVal;
            torsoColor.value = torsoVal;
            armColor.value = armVal;
            legColor.value = legVal;
            snap = false;
        }

        if (headVal != headColor.value)
        {
            headVal = headColor.value;
            changeVal = true;
        }

        if (torsoVal != torsoColor.value)
        {
            torsoVal = torsoColor.value;
            changeVal = true;
        }

        if (armVal != armColor.value)
        {
            armVal = armColor.value;
            changeVal = true;
        }

        if (legVal != legColor.value)
        {
            legVal = legColor.value;
            changeVal = true;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            blAmt = blOn;
            changeVal = true;
        }

        if (Input.GetKeyUp(KeyCode.J))
        {
            blAmt = blOff;
            changeVal = true;
        }

        if ((changeVal) && (pastColor))
        {
            snap = true;
            pvChar.RPC("UpdateBodyColors", RpcTarget.AllBufferedViaServer, headVal, torsoVal, armVal, legVal, blAmt);
            pastColor = false;
        }    
    }

    [PunRPC]
    private void UpdateBodyColors(float headVal, float torsoVal, float armVal, float legVal, float blAmt)
    {
        head.color = Color.HSVToRGB(headVal, 1f, 1f);
        torso.color = Color.HSVToRGB(torsoVal, 1f, 1f);
        leftArm.color = Color.HSVToRGB(armVal, 1f, 1f);
        rightArm.color = Color.HSVToRGB(armVal, 1f, 1f);
        leftLeg.color = Color.HSVToRGB(legVal, 1f, 1f);
        rightLeg.color = Color.HSVToRGB(legVal, 1f, 1f);
        pastColor = true;
        changeVal = false;
        //snap = true;
    }

    [PunRPC]
    private void addCharIDtoList(int idx, int ID)
    {
        charIDList[idx] = ID;
        Debug.Log("CharIDList index " + (idx).ToString() + " is now " + ID.ToString());
    }
}