using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class clickObject : MonoBehaviour
{
    Ray ray;
    private alterSprite accessScript;
    private SpriteRenderer accessRenderer;
    //private Transform accessTransform;
    private GameObject otherObject;

    private bool firstSel = false;

    private float sliderHue;
    private float sliderSaturation;
    private float sliderValue;
    private float shapeScale;

    // Update is called once per frame
    void Update()
    {
        //Get Mouse Click
        if (Input.GetMouseButtonDown(0))
        {
            //Sends a ray
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {

                if (hit.transform.gameObject.tag == "btnInstance")
                {
                    print("btnInstance");
                }

                //Check to see if Game object hit is a shape created by the user.
                if (hit.transform.gameObject.tag == "InstantiatedShape")
                {
                    //Deselects any other object already selected. 
                    //firstSel keeps the variable 'access' from being referenced before it is assigned.
                    if ((otherObject != hit.transform.gameObject) && (firstSel == true))
                    {
                        accessScript.selected = false; 
                        if (firstSel == false) { firstSel = true; }
                    }
                    firstSel = true;

                    otherObject = hit.transform.gameObject;
                    accessScript = otherObject.GetComponent<alterSprite>();
                    accessRenderer = otherObject.GetComponent<SpriteRenderer>();
                    //accessTransform = otherObject.GetComponent<Transform>();

                    Color.RGBToHSV(accessRenderer.color, out sliderHue, out sliderSaturation, out sliderValue);
                    //shapeScale = accessTransform.localScale.x;
                    
                    accessScript.m_SliderHue.value = sliderHue;
                    accessScript.m_SliderSat.value = sliderValue;
                    //accessScript.m_SliderValue.value = sliderValue;
                    //accessScript.m_SliderSize.value = shapeScale;
                    accessScript.selected = true;
                }
            }
        }
    }
}
