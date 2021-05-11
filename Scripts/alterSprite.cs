using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Photon.Pun;

public class alterSprite : MonoBehaviourPun
{
    private SpriteRenderer spriteRenderer;
    private Transform shapeSize;

    public bool selected = false;

    public float m_Hue = 0f;
    public float m_Sat;
    //public float m_Value;
    public float m_Size;

    
    private bool isBeingHeld = false;
    private float startPosX;
    private float startPosY;
    private Vector3 mousePos;

    public Slider m_SliderHue;

    //public static GameObject sliderSaturation;
    public Slider m_SliderSat;

    /*public static GameObject sliderValue;
    public Slider m_SliderValue;*/

/*    public static GameObject sliderSize;
    public Slider m_SliderSize;*/

    private static PhotonView shapePV;

    // Start is called before the first frame update
    void Start()
    {
        
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //ShapeSize = gameObject.GetComponent<Transform>();

        //sliderHue = GameObject.FindWithTag("sldrHue");
        m_SliderHue = GameObject.FindWithTag("sldrHue").GetComponent<Slider>();
        m_SliderHue.maxValue = 0.98F;
        m_SliderHue.minValue = 0;

        //sliderSaturation = GameObject.FindWithTag("sldrSaturation");
        m_SliderSat = GameObject.FindWithTag("sldrSize").GetComponent<Slider>();
        m_SliderSat.maxValue = 0.98F;
        m_SliderSat.minValue = 0.15F;

        /*sliderValue = GameObject.FindWithTag("sldrValue");
        m_SliderValue = sliderValue.GetComponent<Slider>();
        m_SliderValue.maxValue = 0.98F;
        m_SliderValue.minValue = 0;*/

        /*sliderSize = GameObject.FindWithTag("sldrSize");
        m_SliderSize = sliderSize.GetComponent<Slider>();
        m_SliderSize.maxValue = 0.98F;
        m_SliderSize.minValue = 0;*/
    }


    // Update is called once per frame


    void Update()
    {
        
        //m_Sat = m_SliderSat.value;
        /*m_Value = m_SliderValue.value;
        m_Size = m_SliderSize.value;*/

        /*spriteRenderer.color = Color.HSVToRGB(m_Hue, 1f, 1f);
        shapeSize.localScale = new Vector3(m_Size, m_Size, 1f);*/


        if (selected)
        {
            if ((m_Hue != m_SliderHue.value) || (m_Sat != m_SliderSat.value))
            {
                m_Hue = m_SliderHue.value;
                m_Sat = m_SliderSat.value;
                shapePV = PhotonView.Get(this);
                if (!shapePV.IsMine)
                {
                    shapePV.RequestOwnership();
                }

                shapePV.RPC("UpdateHue", RpcTarget.AllBufferedViaServer, m_Hue, m_Sat);
            }
            //shapeSize.localScale = new Vector3(m_Size, m_Size, 1f);
        }

        if (isBeingHeld)
        {
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            //shapePV = PhotonView.Get(this);
            //shapePV.RPC("MoveShape", RpcTarget.AllBufferedViaServer);
            gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, 0f);
        }
    }

    


    [PunRPC]
    private void UpdateHue(float m_Hue, float m_Sat)
    {
        spriteRenderer.color = Color.HSVToRGB(m_Hue, 1f, m_Sat);
    }
    /*
    public void changeShape(Sprite differentSprite)
    {
            spriteRenderer.sprite = differentSprite;
    }
    */


    private void OnMouseDown()
    {
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        startPosX = mousePos.x - gameObject.transform.localPosition.x;
        startPosY = mousePos.y - gameObject.transform.localPosition.y;
        isBeingHeld = true;
    }

    private void OnMouseUp()
    {
        isBeingHeld = false;
    }
}