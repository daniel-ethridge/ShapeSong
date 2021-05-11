using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PostWwiseEvent : MonoBehaviourPun
{
    

    //public static bool wwiseSnapDone;

    //importing a clock into Unity from Wwise
    public AK.Wwise.Event clockEvent;
    public AK.Wwise.Event stopClock;
    private bool clockStart = false;
    private bool clockBeginning = false;

    private bool deleteDone = true;

    //Variables for character to play notes
    private int playerSection = -1;
    private int pastPlayerSection = -2;
    private bool notePlaying = false;
    private bool noteWasPlaying = false;
    public AK.Wwise.Event stopNotes;
    public AK.Wwise.Event A3;
    public AK.Wwise.Event C4;
    public AK.Wwise.Event D4;
    public AK.Wwise.Event F4;
    public AK.Wwise.Event G4;
    public AK.Wwise.Event A4;
    public AK.Wwise.Event C5;
    public AK.Wwise.Event D5;

    //Defining the map boundaries
    public static float mapBottom = -9.4f;
    public static float mapTop = 4f;
    public static float mapVerticalDist;
    private float mapVerticalDivis;

    public static float mapRight = 1.2f;
    public static float mapLeft = -30f;
    public static float mapHorizontalDist;

    //Variables affecting shape animation according to play speed;
    public float scootDistance = 0.6f;
    public float posFactor = 1.0f;
    public AK.Wwise.RTPC playSpeed;

    private SpriteRenderer spriteRenderer;
    private float hue;
    private float saturation;
    private float value;

    private List<GameObject> circleList;
    //private List<GameObject> triangleList;

    //List of lists of callback functions
    private AkCallbackManager.EventCallback[][] callbacks;

    //AnimationVariables
    private static int[] shapeIDs;
    private static int[] moveConstants;
    private static int kdMove = 0;
    private static int snrMove = 0;
    private static int hhMove = 0;
    private static int ltMove = 0;
    private static int mtMove = 0;
    private static int htMove = 0;
    private static int cbMove = 0;
    private static int clpMove = 0;
    private static int lowBgMove = 0;
    private static int midBgMove = 0;
    private static int highBgMove = 0;

    //Percussion variables
    private List<GameObject> squareList;
    private int sqListCount;
    private AK.Wwise.Event[] percEvents;
    private AK.Wwise.Event[] percStops;
    private AK.Wwise.RTPC[] percRTPCs;
    private AK.Wwise.RTPC[] percVolumes;
    private AK.Wwise.RTPC[] percSwitches;
    public static bool[] percPlaying;

    public AK.Wwise.Event kdEvent;
    public AK.Wwise.Event kdStop;
    public AK.Wwise.RTPC kdPosition;
    public AK.Wwise.RTPC kdVolume;
    public AK.Wwise.RTPC kdSwitch;
    private bool kdPlaying = false;
    private Animator kdAnim;
    private float kdHue;

    public AK.Wwise.Event snrEvent;
    public AK.Wwise.Event snrStop;
    public AK.Wwise.RTPC snrPosition;
    public AK.Wwise.RTPC snrVolume;
    public AK.Wwise.RTPC snrSwitch;
    private bool snrPlaying = false;
    private Animator snrAnim;
    private float snrHue;

    public AK.Wwise.Event hhEvent;
    public AK.Wwise.Event hhStop;
    public AK.Wwise.RTPC hhPosition;
    public AK.Wwise.RTPC hhVolume;
    public AK.Wwise.RTPC hhSwitch;
    private bool hhPlaying = false;
    private Animator hhAnim;
    private float hhHue;

    public AK.Wwise.Event ltEvent;
    public AK.Wwise.Event ltStop;
    public AK.Wwise.RTPC ltPosition;
    public AK.Wwise.RTPC ltVolume;
    public AK.Wwise.RTPC ltSwitch;
    private bool ltPlaying = false;
    private Animator ltAnim;
    private float ltHue;

    public AK.Wwise.Event mtEvent;
    public AK.Wwise.Event mtStop;
    public AK.Wwise.RTPC mtPosition;
    public AK.Wwise.RTPC mtVolume;
    public AK.Wwise.RTPC mtSwitch;
    private bool mtPlaying = false;
    private Animator mtAnim;
    private float mtHue;

    public AK.Wwise.Event htEvent;
    public AK.Wwise.Event htStop;
    public AK.Wwise.RTPC htPosition;
    public AK.Wwise.RTPC htVolume;
    public AK.Wwise.RTPC htSwitch;
    private bool htPlaying = false;
    private Animator htAnim;
    private float htHue;

    public AK.Wwise.Event cbEvent;
    public AK.Wwise.Event cbStop;
    public AK.Wwise.RTPC cbPosition;
    public AK.Wwise.RTPC cbVolume;
    public AK.Wwise.RTPC cbSwitch;
    private bool cbPlaying = false;
    private Animator cbAnim;
    private float cbHue;

    public AK.Wwise.Event clpEvent;
    public AK.Wwise.Event clpStop;
    public AK.Wwise.RTPC clpPosition;
    public AK.Wwise.RTPC clpVolume;
    public AK.Wwise.RTPC clpSwitch;
    private bool clpPlaying = false;
    private Animator clpAnim;
    private float clpHue;

    //background music variables
    private AK.Wwise.Event[] bkGndEvents;
    public static AK.Wwise.Event[] bkGndStops;
    private AK.Wwise.RTPC[] bkGndSwitches;
    private AK.Wwise.RTPC[] bkGndBright;
    private AK.Wwise.RTPC[] bkGndDelay;
    public static bool[] bkGndPlaying;

    public AK.Wwise.Event lowBkGndEvent;
    public AK.Wwise.Event lowBkGndStop;
    public AK.Wwise.RTPC lowSwitch;
    public AK.Wwise.RTPC lowBright;
    public AK.Wwise.RTPC lowDelay;
    private bool lowPlaying = false;
    private Animator lowBgAnim;
    private float lowBgHue;

    public AK.Wwise.Event midBkGndEvent;
    public AK.Wwise.Event midBkGndStop;
    public AK.Wwise.RTPC midSwitch;
    public AK.Wwise.RTPC midBright;
    public AK.Wwise.RTPC midDelay;
    private bool midPlaying = false;
    private Animator midBgAnim;
    private float midBgHue;

    public AK.Wwise.Event highBkGndEvent;
    public AK.Wwise.Event highBkGndStop;
    public AK.Wwise.RTPC highSwitch;
    public AK.Wwise.RTPC highBright;
    public AK.Wwise.RTPC highDelay;
    private bool highPlaying = false;
    private Animator highBgAnim;
    private float highBgHue;

    private PhotonView aipv;
    //bool characterFound = false;

    void Start()
    {

        aipv = PhotonView.Get(this);

        mapHorizontalDist = Math.Abs(mapRight - mapLeft);
        mapVerticalDist = Math.Abs(mapBottom - mapTop);
        mapVerticalDivis = (float)mapVerticalDist / 8;

        shapeIDs = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        moveConstants = new int[] { kdMove, snrMove, hhMove, ltMove, mtMove, htMove, cbMove, clpMove, lowBgMove, midBgMove, highBgMove };


        callbacks = new AkCallbackManager.EventCallback[][]
        {
            new AkCallbackManager.EventCallback[] { KdCallback, SnrCallback, HhCallback, LtCallback, MtCallback, HtCallback, CbCallback, ClpCallback },
            new AkCallbackManager.EventCallback[] { LowBgCallback, MidBgCallback, HighBgCallback }
        };

        //Create lists for Wwise events and Wwise RTPCs
        percEvents = new AK.Wwise.Event[] { kdEvent, snrEvent, hhEvent, ltEvent, mtEvent, htEvent, cbEvent, clpEvent };
        percStops = new AK.Wwise.Event[] { kdStop, snrStop, hhStop, ltStop, mtStop, htStop, cbStop, clpStop };
        percRTPCs = new AK.Wwise.RTPC[] { kdPosition, snrPosition, hhPosition, ltPosition, mtPosition, htPosition, cbPosition, clpPosition };
        percVolumes = new AK.Wwise.RTPC[] { kdVolume, snrVolume, hhVolume, ltVolume, mtVolume, htVolume, cbVolume, clpVolume };
        percSwitches = new AK.Wwise.RTPC[] { kdSwitch, snrSwitch, hhSwitch, ltSwitch, mtSwitch, htSwitch, cbSwitch, clpSwitch };
        percPlaying = new bool[] { kdPlaying, snrPlaying, hhPlaying, ltPlaying, mtPlaying, htPlaying, cbPlaying, clpPlaying };

        bkGndEvents = new AK.Wwise.Event[] { lowBkGndEvent, midBkGndEvent, highBkGndEvent };
        bkGndStops = new AK.Wwise.Event[] { lowBkGndStop, midBkGndStop, highBkGndStop };
        bkGndSwitches = new AK.Wwise.RTPC[] { lowSwitch, midSwitch, highSwitch };
        bkGndBright = new AK.Wwise.RTPC[] { lowBright, midBright, highBright };
        bkGndDelay = new AK.Wwise.RTPC[] { lowDelay, midDelay, highDelay };
        bkGndPlaying = new bool[] { lowPlaying, midPlaying, highPlaying };
    }


    // Update is called once per frame
    void Update()
    {
        /*if (!characterFound)
        {

        }*/
        if (!ShapeManager.deleteSqDone)
        {
            deleteDone = false;
        }
        else if (!ShapeManager.deleteCirDone)
        {
            deleteDone = false;
        }
        else
        {
            deleteDone = true;
        }

        if (deleteDone)
        {
            //Press J to play a note
            PlayNote();

            squareList = ShapeManager.squareList;
            circleList = ShapeManager.circleList;
            //triangleList = ShapeManager.triangleList;

            //Shape distance from player affects different audio filters
            for (int i = 0; i <= ShapeManager.sqIdx; i++)
            {
                percRTPCs[i].SetGlobalValue(posFactor * Vector3.Distance(Movement.playerPos.position, squareList[i].transform.position));
                spriteRenderer = squareList[i].GetComponent<SpriteRenderer>();
                Color.RGBToHSV(spriteRenderer.color, out hue, out saturation, out value);
                percVolumes[i].SetGlobalValue(value * 100);
            }

            for (int i = 0; i <= ShapeManager.cirIdx; i++)
            {
                bkGndDelay[i].SetGlobalValue(posFactor * Vector3.Distance(Movement.playerPos.position, circleList[i].transform.position));
                spriteRenderer = circleList[i].GetComponent<SpriteRenderer>();
                Color.RGBToHSV(spriteRenderer.color, out hue, out saturation, out value);
                bkGndBright[i].SetGlobalValue(value * 100);
            }


            playSpeed.SetGlobalValue(scootDistance * 50f);

            if (!clockStart)
            {
                if (squareList.Count + circleList.Count > 0)
                {
                    clockEvent.Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, ClockCallback);
                    clockStart = true;
                    clockBeginning = true;
                }
            }

            if (clockStart)
            {
                if (squareList.Count + circleList.Count <= 0)
                {
                    stopClock.Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, ClockCallback);
                    clockStart = false;
                    clockBeginning = false;
                    Debug.Log(clockStart);
                }
            }

            //Only add in a new track if the loop is at the beginning
            if (clockBeginning)
            {
                PlayPercInstrument(squareList, percSwitches, percPlaying);
                PlayBackgrounds(circleList, bkGndSwitches, bkGndPlaying);
                clockBeginning = false;
            }

                //Talks to shapemanager script to know if 
            if ((ShapeManager.deleteSqCalled) && (squareList.Count > 0))
            {
                ShapeManager.deleteSqDone = false;
                ShapeManager.deleteSqCalled = false;

                //percStopped = false;
                int temp = ShapeManager.sqIdx;
                aipv.RPC("StopPerc", RpcTarget.AllBufferedViaServer, temp);
                //percCall = false;
                ShapeManager.deleteSquare();
                
            }

            if ((ShapeManager.deleteCirCalled) && (circleList.Count > 0))
            {
                ShapeManager.deleteCirDone = false;
                ShapeManager.deleteCirCalled = false;
                //bkGndStopped = false;
                int temp = ShapeManager.cirIdx;
                aipv.RPC("StopBkGnd", RpcTarget.AllBufferedViaServer, temp);
                
                ShapeManager.deleteCircle();
                
            }
        }
    }

    [PunRPC]
    private void StopPerc(int idx)
    {
        percStops[idx].Post(gameObject);
        //percStopped = true;
    }

    [PunRPC]
    private void StopBkGnd(int idx)
    {
        bkGndStops[idx].Post(gameObject);
        //bkGndStopped = true;
    }


    //Screen evenly divided vertically. Note played upon pressing J is determined by character vertical position.
    int GetPlayerSection()
    {
        if (Movement.playerPos.position.y <= mapBottom + mapVerticalDivis)
        {
            return 0;
        }

        else if (Movement.playerPos.position.y <= mapBottom + 2 * mapVerticalDivis)
        {
            return 1;
        }

        else if (Movement.playerPos.position.y <= mapBottom + 3 * mapVerticalDivis)
        {
            return 2;
        }

        else if (Movement.playerPos.position.y <= mapBottom + 4 * mapVerticalDivis)
        {
            return 3;
        }

        else if (Movement.playerPos.position.y <= mapBottom + 5 * mapVerticalDivis)
        {
            return 4;
        }

        else if (Movement.playerPos.position.y <= mapBottom + 6 * mapVerticalDivis)
        {
            return 5;
        }

        else if (Movement.playerPos.position.y <= mapBottom + 7 * mapVerticalDivis)
        {
            return 6;
        }

        else 
        {
            return 7;
        }
    }

    void PlayNote()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            notePlaying = true;
            Debug.Log("Note Playing");
        }

        else if (Input.GetKeyUp(KeyCode.J))
        {
            notePlaying = false;
            noteWasPlaying = true;
            Debug.Log("Note Not Playing");
        }

        if (notePlaying)
        {
            Movement.snap = true;
            playerSection = GetPlayerSection();
            if (playerSection != pastPlayerSection)
            {
                pastPlayerSection = playerSection;
                aipv.RPC("BroadcastNote", RpcTarget.AllBufferedViaServer, playerSection);
            }
        }

        else if (noteWasPlaying)
        {
            noteWasPlaying = false;
            aipv.RPC("StopBroadcastNote", RpcTarget.AllBufferedViaServer);
        }
    }

    [PunRPC]
    private void BroadcastNote(int plrSect)
    {
        switch (plrSect)
        {
            case 0:
                stopNotes.Post(gameObject);
                A3.Post(gameObject);
                break;

            case 1:
                stopNotes.Post(gameObject);
                C4.Post(gameObject);
                break;

            case 2:
                stopNotes.Post(gameObject);
                D4.Post(gameObject);
                break;

            case 3:
                stopNotes.Post(gameObject);
                F4.Post(gameObject);
                break;

            case 4:
                stopNotes.Post(gameObject);
                G4.Post(gameObject);
                break;

            case 5:
                stopNotes.Post(gameObject);
                A4.Post(gameObject);
                break;

            case 6:
                stopNotes.Post(gameObject);
                C5.Post(gameObject);
                break;

            case 7:
                stopNotes.Post(gameObject);
                D5.Post(gameObject);
                break;
        }
    }

    [PunRPC]
    private void StopBroadcastNote()
    {
        stopNotes.Post(gameObject);
        pastPlayerSection = -1;
    }

    //Play percussion instrument
    void PlayPercInstrument(List<GameObject> shapes, AK.Wwise.RTPC[] switches, bool[] playing)
    {
        for (int i = 0; i <= ShapeManager.sqIdx; i++)
        {
            spriteRenderer = shapes[i].GetComponent<SpriteRenderer>();
            Color.RGBToHSV(spriteRenderer.color, out hue, out saturation, out value);
            switches[i].SetGlobalValue(hue * 100);
            switch (i)
            {
                case 0:
                    kdHue = hue;
                    kdAnim = shapes[0].GetComponent<Animator>();
                    break;
                case 1:
                    snrHue = hue;
                    snrAnim = shapes[1].GetComponent<Animator>();
                    break;
                case 2:
                    hhHue = hue;
                    hhAnim = shapes[2].GetComponent<Animator>();
                    break;
                case 3:
                    ltHue = hue;
                    ltAnim = shapes[3].GetComponent<Animator>();
                    break;
                case 4:
                    mtHue = hue;
                    mtAnim = shapes[4].GetComponent<Animator>();
                    break;
                case 5:
                    htHue = hue;
                    htAnim = shapes[5].GetComponent<Animator>();
                    break;
                case 6:
                    cbHue = hue;
                    cbAnim = shapes[6].GetComponent<Animator>();
                    break;
                case 7:
                    clpHue = hue;
                    clpAnim = shapes[7].GetComponent<Animator>();
                    break;
            }

            if (!playing[i])
            {
                playing[i] = true;
                Sync();
            }
        }
    }

    // Play background instrument
    void PlayBackgrounds(List<GameObject> shapes, AK.Wwise.RTPC[] switches, bool[] playing)
    {
        for (int i = 0; i <= ShapeManager.cirIdx; i++)
        {
            spriteRenderer = shapes[i].GetComponent<SpriteRenderer>();
            Color.RGBToHSV(spriteRenderer.color, out hue, out saturation, out value);
            switches[i].SetGlobalValue(hue * 100);

            switch (i)
            {
                case 0:
                    lowBgHue = hue;
                    lowBgAnim = shapes[0].GetComponent<Animator>();
                    break;
                case 1:
                    midBgHue = hue;
                    midBgAnim = shapes[1].GetComponent<Animator>();
                    break;
                case 2:
                    highBgHue = hue;
                    highBgAnim = shapes[2].GetComponent<Animator>();
                    break;
            }

            if (!playing[i])
            {
                playing[i] = true;
                Sync();
            }
        }
    }

    //public static void SetPlaying(bool[] shapePlay, int idx, bool playing) => shapePlay[idx] = playing;

/*    public void CleanBackgrounds(int idx, int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (i <= idx)
            {
                bkGndPlaying[i] = false;
            }
            else
            {
                bkGndPlaying[i] = true;
                bkGndStops[i].Post(this.gameObject);
            }
        }
    }*/


    //Series of callback functions. Clock callback makes sure everything lines up. The rest make sure the animations are triggered at the right times.
    void ClockCallback(object in_cookie, AkCallbackType in_type, object in_info)
    {
        AkMusicSyncCallbackInfo musicSyncInfo = in_info as AkMusicSyncCallbackInfo;
        string cueName = musicSyncInfo.userCueName;
        if (cueName == "Start")
        {
            clockBeginning = true;
        }
    }

    void KdCallback(object in_cookie, AkCallbackType in_type, object in_info)
    {
        AkMusicSyncCallbackInfo musicSyncInfo = in_info as AkMusicSyncCallbackInfo;
        string cueName = musicSyncInfo.userCueName;
        PlayAnim(cueName, squareList[0], kdHue, kdAnim, shapeIDs[0]);
    }

    void SnrCallback(object in_cookie, AkCallbackType in_type, object in_info)
    {
        AkMusicSyncCallbackInfo musicSyncInfo = in_info as AkMusicSyncCallbackInfo;
        string cueName = musicSyncInfo.userCueName;
        PlayAnim(cueName, squareList[1], snrHue, snrAnim, shapeIDs[1]);
    }

    void HhCallback(object in_cookie, AkCallbackType in_type, object in_info)
    {
        AkMusicSyncCallbackInfo musicSyncInfo = in_info as AkMusicSyncCallbackInfo;
        string cueName = musicSyncInfo.userCueName;
        PlayAnim(cueName, squareList[2], hhHue, hhAnim, shapeIDs[2]);
    }

    void LtCallback(object in_cookie, AkCallbackType in_type, object in_info)
    {
        AkMusicSyncCallbackInfo musicSyncInfo = in_info as AkMusicSyncCallbackInfo;
        string cueName = musicSyncInfo.userCueName;
        PlayAnim(cueName, squareList[3], ltHue, ltAnim, shapeIDs[3]);
    }

    void MtCallback(object in_cookie, AkCallbackType in_type, object in_info)
    {
        AkMusicSyncCallbackInfo musicSyncInfo = in_info as AkMusicSyncCallbackInfo;
        string cueName = musicSyncInfo.userCueName;
        PlayAnim(cueName, squareList[4], mtHue, mtAnim, shapeIDs[4]);
    }

    void HtCallback(object in_cookie, AkCallbackType in_type, object in_info)
    {
        AkMusicSyncCallbackInfo musicSyncInfo = in_info as AkMusicSyncCallbackInfo;
        string cueName = musicSyncInfo.userCueName;
        PlayAnim(cueName, squareList[5], htHue, htAnim, shapeIDs[5]);
    }

    void CbCallback(object in_cookie, AkCallbackType in_type, object in_info)
    {
        AkMusicSyncCallbackInfo musicSyncInfo = in_info as AkMusicSyncCallbackInfo;
        string cueName = musicSyncInfo.userCueName;
        PlayAnim(cueName, squareList[6], cbHue, cbAnim, shapeIDs[6]);
    }

    void ClpCallback(object in_cookie, AkCallbackType in_type, object in_info)
    {
        AkMusicSyncCallbackInfo musicSyncInfo = in_info as AkMusicSyncCallbackInfo;
        string cueName = musicSyncInfo.userCueName;
        PlayAnim(cueName, squareList[7], clpHue, clpAnim, shapeIDs[7]);
    }

    void LowBgCallback(object in_cookie, AkCallbackType in_type, object in_info)
    {
        AkMusicSyncCallbackInfo musicSyncInfo = in_info as AkMusicSyncCallbackInfo;
        string cueName = musicSyncInfo.userCueName;
        PlayAnim(cueName, circleList[0], lowBgHue, lowBgAnim, shapeIDs[8]);
    }

    void MidBgCallback(object in_cookie, AkCallbackType in_type, object in_info)
    {
        AkMusicSyncCallbackInfo musicSyncInfo = in_info as AkMusicSyncCallbackInfo;
        string cueName = musicSyncInfo.userCueName;
        PlayAnim(cueName, circleList[1], midBgHue, midBgAnim, shapeIDs[9]);
    }

    void HighBgCallback(object in_cookie, AkCallbackType in_type, object in_info)
    {
        AkMusicSyncCallbackInfo musicSyncInfo = in_info as AkMusicSyncCallbackInfo;
        string cueName = musicSyncInfo.userCueName;
        PlayAnim(cueName, circleList[2], highBgHue, highBgAnim, shapeIDs[10]);
    }

    //If a new track comes in, all other tracks are immediately restarted. This works because this would only ever happen when all the tracks have looped back to the beginning
    void Sync()
    {
        for (int i = 0; i <= ShapeManager.sqIdx; i++)
        {
            if (percPlaying[i])
            {
                percEvents[i].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, callbacks[0][i]);
            }
        }

        for (int i = 0; i <= ShapeManager.cirIdx; i++)
        {
            if (bkGndPlaying[i])
            {
                bkGndEvents[i].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, callbacks[1][i]);
            }
        }
    }

    //Animation function
    void PlayAnim(string cueName, GameObject shape, float bgHue, Animator animator, int ID)
    {
        if (bgHue < 0.2)
        {
            if (cueName == "trackOne")
            {
                animator.Play("circleReact");
                moveConstants[ID] = MoveHorizontal(shape, moveConstants[ID]);
            }
        }
        else if (bgHue < 0.4)
        {
            if (cueName == "trackTwo")
            {
                animator.Play("circleReact");
                moveConstants[ID] = MoveVertical(shape, moveConstants[ID]);
            }
        }
        else if (bgHue < 0.6)
        {
            if (cueName == "trackThree")
            {
                animator.Play("circleReact");
                moveConstants[ID] = MoveTriangle(shape, moveConstants[ID]);
            }
        }
        else if (bgHue < 0.8)
        {
            if (cueName == "trackFour")
            {
                animator.Play("circleReact");
                moveConstants[ID] = MoveSquare(shape, moveConstants[ID]);
            }
        }
        else if (bgHue <= 1.0)
        {
            if (cueName == "trackFive")
            {
                animator.Play("circleReact");
                moveConstants[ID] = MovePentagon(shape, moveConstants[ID]);
            }
        }
    }

    int MoveHorizontal(GameObject shape, int moveConst)
    {
        if (moveConst == 0)
        {
            ObjectMoveLeft(shape);
            moveConst = 1;
            return moveConst;
        }
        else
        {
            ObjectMoveRight(shape);
            moveConst = 0;
            return moveConst;
        }
    }

    int MoveVertical(GameObject shape, int moveConst)
    {
        if (moveConst == 0)
        {
            ObjectMoveUp(shape);
            moveConst = 1;
            return moveConst;
        }
        else
        {
            ObjectMoveDown(shape);
            moveConst = 0;
            return moveConst;
        }
    }

    int MoveTriangle(GameObject shape, int moveConst)
    {
        if (moveConst == 0)
        {
            ObjectMoveRight(shape);
            moveConst = 1;
            return moveConst;
        }

        else if (moveConst == 1)
        {
            ObjectMoveRight(shape);
            moveConst = 2;
            return moveConst;
        }

        else if (moveConst == 2)
        {
            ObjectMoveDiagUL(shape);
            moveConst = 3;
            return moveConst;
        }

        else
        {
            ObjectMoveDiagDL(shape);
            moveConst = 0;
            return moveConst;
        }
    }

    int MoveSquare(GameObject shape, int moveConst)
    {
        if (moveConst == 0)
        {
            ObjectMoveRight(shape);
            moveConst = 1;
            return moveConst;
        }

        else if (moveConst == 1)
        {
            ObjectMoveUp(shape);
            moveConst = 2;
            return moveConst;
        }

        else if (moveConst == 2)
        {
            ObjectMoveLeft(shape);
            moveConst = 3;
            return moveConst;
        }

        else
        {
            ObjectMoveDown(shape);
            moveConst = 0;
            return moveConst;
        }
    }

    int MovePentagon(GameObject shape, int moveConst)
    {
        if (moveConst == 0)
        {
            ObjectMoveRight(shape);
            moveConst = 1;
            return moveConst;
        }

        else if (moveConst == 1)
        {
            ObjectMoveRight(shape);
            moveConst = 2;
            return moveConst;
        }

        else if (moveConst == 2)
        {
            ObjectMoveUp(shape);
            moveConst = 3;
            return moveConst;
        }

        else if (moveConst == 3)
        {
            ObjectMoveDiagUL(shape);
            moveConst = 4;
            return moveConst;
        }

        else if (moveConst == 4)
        {
            ObjectMoveDiagDL(shape);
            moveConst = 5;
            return moveConst;
        }

        else
        {
            ObjectMoveDown(shape);
            moveConst = 0;
            return moveConst;
        }
    }

    void ObjectMoveRight(GameObject shape)
    {
        if (shape.transform.position.x >= mapRight)
        {
            shape.transform.position = shape.transform.position - new Vector3(mapHorizontalDist, 0f, 0f);
        }
        else
        {
            shape.transform.position = shape.transform.position + new Vector3(scootDistance * 0.6f, 0f, 0f);
        }
    }

    void ObjectMoveLeft(GameObject shape)
    {
        if (shape.transform.position.x <= mapLeft)
        {
            shape.transform.position = shape.transform.position + new Vector3(mapHorizontalDist, 0f, 0f);
        }
        else
        {
            shape.transform.position = shape.transform.position - new Vector3(scootDistance * 0.6f, 0f, 0f);
        }
    }

    void ObjectMoveDown(GameObject shape)
    {
        if (shape.transform.position.y <= mapBottom)
        {
            shape.transform.position = shape.transform.position + new Vector3(0f, mapVerticalDist, 0f);
        }
        else
        {
            shape.transform.position = shape.transform.position - new Vector3(0f, scootDistance * 0.6f, 0f);
        }
    }

    void ObjectMoveUp(GameObject shape)
    {
        if (shape.transform.position.y >= mapTop)
        {
            shape.transform.position = shape.transform.position - new Vector3(0f, mapVerticalDist, 0f);
        }
        else
        {
            shape.transform.position = shape.transform.position + new Vector3(0f, scootDistance * 0.6f, 0f);
        }
    }

    void ObjectMoveDiagDR(GameObject shape)
    {
        ObjectMoveDown(shape);
        ObjectMoveRight(shape);
    }

    void ObjectMoveDiagUR(GameObject shape)
    {
        ObjectMoveUp(shape);
        ObjectMoveRight(shape);
    }

    void ObjectMoveDiagDL(GameObject shape)
    {

        ObjectMoveDown(shape);
        ObjectMoveLeft(shape);
    }

    void ObjectMoveDiagUL(GameObject shape)
    {
        ObjectMoveUp(shape);
        ObjectMoveLeft(shape);
    }
}

// All code below this point was previously used then scrapped. Not deleted in case I needed it in the future.


/*switch (i)
                    {
                        case 0:
                            kdAnim = shapes[0].GetComponent<Animator>();
                            
                            events[0].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, callbacks[0]);
                            break;
                        case 1:
                            snrAnim = shapes[1].GetComponent<Animator>();
                            events[1].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, SnrCallback);
                            events[0].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, KdCallback);
                            break;
                        case 2:
                            hhAnim = shapes[2].GetComponent<Animator>();
                            events[2].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, HhCallback);
                            events[1].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, SnrCallback);
                            events[0].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, KdCallback);
                            break;
                        case 3:
                            ltAnim = shapes[3].GetComponent<Animator>();
                            events[3].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, LtCallback);
                            events[2].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, HhCallback);
                            events[1].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, SnrCallback);
                            events[0].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, KdCallback);
                            break;
                    }*/


/*    private float tempo = 150f;
    private float subdivision = 0f;
    private float beat = 0f;
    private bool beatChange = false;*/
/*if (subChange)
{
    for (int i = 0; i < shapes.Count; i++)
    {
        if (beat >= instrumentReset[i])
        {
            beat = (beat + instrumentReset[i]) - 4;
        }

        play = (beat - offsets[i]) % frqs[i]; 

        if (play == 0)
        {
            events[i].Post(gameObject);
            ObjectGrow(shapes[i]);
            ObjectMove(shapes[i]);
        }

        if (play == 0.125)
        {
            ObjectShrink(shapes);
        }
    }
    //subChange = false;
}*/

/* void ObjectGrow(GameObject shape)
{
    shapeTransform = shape.GetComponent<Transform>();
    shapeTransform.localScale += growthAmt;
}

void ObjectShrink(List<GameObject> shapes)
{
    for (int i = 0; i < shapes.Count; i++)
    {
        shapes[i].transform.localScale = new Vector3(1, 1, 1);
    }
}*/

/*void SetDrumParams(List<GameObject> shapes, float[][,] complexities, float[][] resetPoints)
{
    int count = shapes.Count;
    for (int i = 0; i < count; i++)
    {
        spriteRenderer = shapes[i].GetComponent<SpriteRenderer>();
        Color.RGBToHSV(spriteRenderer.color, out hue, out saturation, out value);
        if (hue < 0.2)
        {
            percFrqs[i] = complexities[i][0, 0];
            percOffsets[i] = complexities[i][0, 1];
            instrumentReset[i] = resetPoints[i][0];
        }

        else if ((0.2 <= hue) && (hue < 0.4))
        {
            percFrqs[i] = complexities[i][1, 0];
            percOffsets[i] = complexities[i][1, 1];
            instrumentReset[i] = resetPoints[i][1];
        }

        else if ((0.4 <= hue) && (hue < 0.6))
        {
            percFrqs[i] = complexities[i][2, 0];
            percOffsets[i] = complexities[i][2, 1];
            instrumentReset[i] = resetPoints[i][2];
        }

        else if ((0.6 <= hue) && (hue < 0.8))
        {
            percFrqs[i] = complexities[i][3, 0];
            percOffsets[i] = complexities[i][3, 1];
            instrumentReset[i] = resetPoints[i][3];
        }

        else
        {
            percFrqs[i] = complexities[i][4, 0];
            percOffsets[i] = complexities[i][4, 1];
            instrumentReset[i] = resetPoints[i][4];
        }
    }
}*/

/*switch (i)
{
    case 0:
        lowBgAnim = shapes[0].GetComponent<Animator>();
        events[0].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, LowBgCallback);
        break;
    case 1:
        midBgAnim = shapes[1].GetComponent<Animator>();
        break;
    case 2:
        highBgAnim = shapes[2].GetComponent<Animator>();
        events[2].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, HighBgCallback);
        events[1].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, MidBgCallback);
        events[0].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, LowBgCallback);
        break;
}*/


//percFrqs = new float[] { kickFrq, snareFrq, hiHatFrq, lowTomFrq, midTomFrq, highTomFrq, highCowbellFrq, clapFrq };
//percOffsets = new float[] { kickOffset, snareOffset, hiHatOffset, lowTomOffset, midTomOffset, highTomOffset, highCowbellOffset, clapOffset };
//instrumentReset = new float[8];
//SetDrumParams(squareList, complexities, resetPoints);

//private static bool subChange = false;
//public float tempo = 150;
//private Transform posReset;
//private Vector3[] squarePos;
//private Vector3 growthAmt = new Vector3(0.5f, 0.5f, 0.5f);

/* private float[,] kdComplexity;
 private float kickFrq = 0f;
 private float kickOffset = 0f;*/


/*private float[,] snrComplexity;
private float snareFrq = 0f;
private float snareOffset = 0f;*/


/*private float[,] hhComplexity;
private float hiHatFrq = 0f;
private float hiHatOffset = 0f;*/


/*private float[,] ltComplexity;
private float lowTomFrq = 0f;
private float lowTomOffset = 0f;*/


/*private float[,] mtComplexity;
private float midTomFrq = 0f;
private float midTomOffset = 0f;*/


/*private float[,] htComplexity;
private float highTomFrq = 0f;
private float highTomOffset = 0f;*/


/*private float[,] cbComplexity;
private float highCowbellFrq = 0f;
private float highCowbellOffset = 0f;*/


/*private float[,] clpComplexity;
private float clapFrq = 0f;
private float clapOffset = 0f;*/


/*private float[] percFrqs;
private float[] percOffsets;
private float[][,] complexities;
private float[] instrumentReset;
private float[][] resetPoints;*/

/*subdivision += Time.deltaTime;
   UpdateBeat();*/

/*private Animator kdAnim;
private Animator snrAnim;
private Animator hhAnim;
private Animator ltAnim;
private Animator mtAnim;
private Animator htAnim;
private Animator cbAnim;
private Animator clpAnim;*/

//Initialize ordered pairs {Event Post Frquency, Beat Offset}. The ordered pair used depends on color of a given square
/*kdComplexity = new float[,] { { 4f, 0f }, { 2f, 0f }, { 1f, 0f }, { 1.5f, 0f }, { 1.5f, 0f } };
snrComplexity = new float[,] { { 2f, 1f }, { 1.5f, 1f }, { 1.5f, 0f }, { 1.5f, 0f }, { 1.5f, 0.5f } };
hhComplexity = new float[,] { { 2f, 0f }, { 2f, 0.5f }, { 1.5f, 1f }, { 1.5f, 0.5f }, { 0.5f, 0f } };
ltComplexity = new float[,] { { 4f, 1f }, { 2f, 1f }, { 1f, 0f }, { 1.5f, 0f }, { 1.5f, 0f } };
mtComplexity = new float[,] { { 4f, 2f }, { 4f, 1.5f }, { 2f, 1f }, { 2f, 1.5f }, { 1.5f, 1f } };
htComplexity = new float[,] { { 4f, 2f }, { 4f, 1.5f }, { 2f, 1.5f }, { 2f, 1f }, { 1.5f, 1f } };
cbComplexity = new float[,] { { 4f, 2f }, { 4f, 1.5f }, { 2f, 1.5f }, { 2f, 1f }, { 1.5f, 1f } };
clpComplexity = new float[,] { { 4f, 3f }, { 4f, 1f }, { 3f, 0f }, { 1.5f, 0f }, { 1.5f, 1f } };*/


/*complexities = new float[][,] //Combines all complexity arrays into one multidimensional array.
{
    kdComplexity,
    snrComplexity,
    hhComplexity,
    ltComplexity,
    mtComplexity,
    htComplexity,
    cbComplexity,
    clpComplexity
};

resetPoints = new float[][]
{
    new float[] { 4f, 4f, 4f, 4f, 2f }, //Kick Drum
    new float[] { 4f, 4f, 4f, 2f, 4f }, //Snare
    new float[] { 4f, 4f, 4f, 4f, 4f }, //Hi Hat
    new float[] { 4f, 4f, 4f, 4f, 2f }, //Low Tom
    new float[] { 4f, 4f, 4f, 4f, 4f }, //Mid Tom
    new float[] { 4f, 4f, 4f, 4f, 4f }, //High Tom
    new float[] { 4f, 4f, 4f, 4f, 4f }, //Cowbell
    new float[] { 4f, 4f, 4f, 4f, 4f }  //Clap
};*/

/*squarePos = new[] { new Vector3(-42f, -7f, 0f), new Vector3(-42f, -5f, 0f), new Vector3(-42f, -3f, 0f),
                              new Vector3(-42f, -1f, 0f), new Vector3(-42f, 1f, 0f), new Vector3(-42f, 3f, 0f),
                              new Vector3(-42f, 5f, 0f), new Vector3(-42f, 7f, 0f) };*/



/*shapeAnim = new Animator[][]
{
    //new Animator[] { kdAnim, snrAnim, hhAnim, ltAnim, mtAnim, htAnim, cbAnim, clpAnim},
    new Animator[] { lowBgAnim, midBgAnim, highBgAnim }
};*/


/*int index;
    if ((bgHue == lowBgHue) || (bgHue == kdHue))
    {
        index = 0;
    }
    else if ((bgHue == midBgHue) || (bgHue == snrHue))
    {
        index = 1;
    }
    else if ((bgHue == highBgHue) || (bgHue == ltHue))
    { 
        index = 2;
    }
    else 
    {
        index = 3;
    }*/

//private float play;

/*switch (i)
            {
                case 0:
                    kdAnim = shapes[0].GetComponent<Animator>();

                    events[0].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, callbacks[0]);
                    break;
                case 1:
                    snrAnim = shapes[1].GetComponent<Animator>();
                    events[1].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, SnrCallback);
                    events[0].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, KdCallback);
                    break;
                case 2:
                    hhAnim = shapes[2].GetComponent<Animator>();
                    events[2].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, HhCallback);
                    events[1].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, SnrCallback);
                    events[0].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, KdCallback);
                    break;
                case 3:
                    ltAnim = shapes[3].GetComponent<Animator>();
                    events[3].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, LtCallback);
                    events[2].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, HhCallback);
                    events[1].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, SnrCallback);
                    events[0].Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, KdCallback);
                    break;
            }*/

/*if (subChange)
{
    for (int i = 0; i < shapes.Count; i++)
    {
        if (beat >= instrumentReset[i])
        {
            beat = (beat + instrumentReset[i]) - 4;
        }

        play = (beat - offsets[i]) % frqs[i]; 

        if (play == 0)
        {
            events[i].Post(gameObject);
            ObjectGrow(shapes[i]);
            ObjectMove(shapes[i]);
        }

        if (play == 0.125)
        {
            ObjectShrink(shapes);
        }
    }
    //subChange = false;
}*/


/* void UpdateBeat()
{
    if (subdivision >= (0.125f / (tempo / 60)))
    {
        beat += 0.125f;
        //bkGndBeat += 0.125f;
        subdivision = 0f;
        //subChange = true;
    }

    if (beat >= 8)
    {
        beat = 0f;
        beatChange = true;
    }

    *//*if (bkGndBeat >= 8)
    {
        bkGndBeat = 0f;
        bkGndChange = true;
    }*//*
}*/
