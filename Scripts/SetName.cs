using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetName : MonoBehaviour
{
    public InputField nickname;
    public TextMesh captionText;

    void Start()
    {
        captionText.text = nickname.text;
    }
}
