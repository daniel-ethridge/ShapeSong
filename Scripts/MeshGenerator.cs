using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviourPun
{
    private Mesh mesh;

    private float playerPosX = 0;
    private float playerPosY = 0;

    private float leftColorR = 1;
    private float leftColorG = 0;
    private float leftColorB = 0;
    private float leftColorT = 0.25f;

    private float centColorR = 0;
    private float centColorG = 0;
    private float centColorB = 0;
    private float centColorT = 0.50f;

    private float rightColorR = 0;
    private float rightColorG = 0;
    private float rightColorB = 1;
    private float rightColorT = 0.75f;

    private int xScale = 375;
    private int xSize;

    private int yScale = 200;
    private int ySize;

    private float xOffset = 6f;
    private float yOffset;
    private float scale = 0.1f;

    private float designFactor = 1;

    private Vector3[] vertices;
    private int[] triangles;
    private Color[] colors;

    private Gradient gradient;
    private GradientColorKey[] colorKey;
    private GradientAlphaKey[] alphaKey;

    public static GameObject goBkGndSatSldr;
    public Slider bkGndSatSldr;
    public static float bkGndSatSldrValue;
    public AK.Wwise.RTPC masterLFO;
    public AK.Wwise.RTPC charX;

    private static PhotonView MGPV;
    // Start is called before the first frame update
    void Start()
    {
        MGPV = PhotonView.Get(this);
        //goBkGndSatSldr = GameObject.FindWithTag("BckGndSldr");
        //bkGndSatSldr = goBkGndSatSldr.GetComponent<Slider>();

        bkGndSatSldrValue = 0f;
        bkGndSatSldr.value = 0f;

        //The background is procedurally generated.
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        gradient = new Gradient();

        colorKey = new GradientColorKey[3];
        alphaKey = new GradientAlphaKey[3];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;
        alphaKey[2].alpha = 0.0f;
        alphaKey[2].time = 1.0f;
    }

    private void Update()
    {
        //bkGndSatSldrValue = bkGndSatSldr.value;
        if (bkGndSatSldrValue != bkGndSatSldr.value)
        {
            MGPV.RPC("UpdateSaturation", RpcTarget.AllBufferedViaServer, bkGndSatSldr.value);
        }
        masterLFO.SetGlobalValue(bkGndSatSldrValue * 100);

        CreateShape();
        UpdateMesh();
    }

    [PunRPC]
    private void UpdateSaturation(float satVal)
    {
        bkGndSatSldrValue = satVal;
        bkGndSatSldr.value = satVal;
    }


    void CreateShape()
    {
        //The code in this section is adapted from a youtube video on the youtube channel Brackeys. 
        xSize = (int)Math.Ceiling(xScale / designFactor);
        ySize = (int)Math.Ceiling(yScale / designFactor);

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                vertices[i] = new Vector3((float) designFactor*scale*x, (float) designFactor*scale*y, 0);
                i++;
            }
        }

        int vert = 0;
        int tris = 0;
        triangles = new int[xSize * ySize * 6];

        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        colors = new Color[vertices.Length];

        //Control the saturation of the background
        colorKey[0].color.r = bkGndSatSldrValue * leftColorR;
        colorKey[0].color.g = bkGndSatSldrValue * leftColorG;
        colorKey[0].color.b = bkGndSatSldrValue * leftColorB;
        colorKey[0].time = leftColorT;

        colorKey[1].color.r = bkGndSatSldrValue * centColorR;
        colorKey[1].color.g = bkGndSatSldrValue * centColorG;
        colorKey[1].color.b = bkGndSatSldrValue * centColorB;
        colorKey[1].time = centColorT;

        colorKey[2].color.r = bkGndSatSldrValue * rightColorR;
        colorKey[2].color.g = bkGndSatSldrValue * rightColorG; 
        colorKey[2].color.b = bkGndSatSldrValue * rightColorB;
        colorKey[2].time = rightColorT;

        gradient.SetKeys(colorKey, alphaKey);

        //playerPosX = (float)(((Movement.playerPos.position.x - PostWwiseEvent.mapLeft) / PostWwiseEvent.mapHorizontalDist) + 1);
        //playerPosY = (float)(((Movement.playerPos.position.y - PostWwiseEvent.mapBottom) / PostWwiseEvent.mapVerticalDist) + 1);

        //charX.SetGlobalValue(100 * (playerPosX - 1));

        yOffset = (float) Math.Pow((double)playerPosX, 3*(double)playerPosY);

        //This for loop is where the main colors of the mesh are set.
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                //The colors of the mesh are set with a simple addition of sine functions.
                float perl = (float)((1/(2 * 1.981))*(Math.Sin(4f * 3.14f * x * y * xOffset + yOffset) + Math.Sin(0.5f * 3.14f * x * y * xOffset + yOffset)) + 0.5);
                colors[i] = gradient.Evaluate(perl);
                i++;
            }
        }
    }
    void UpdateMesh()
    {
        //Function adapted from Brackeys Youtube channel
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();
    }
}
