using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int _xCoord;
    public int XCoord {
        get { return _xCoord; }
        set { _xCoord = value; }
    }

    private int _yCoord;
    public int YCoord {
        get { return _yCoord; }
        set { _yCoord = value; }
    }

    Renderer renderer;

    Color initColor;
    // Start is called before the first frame update
    void Start()
    {
        renderer = transform.GetChild(0).GetComponent<Renderer>();
        initColor = renderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHover(bool active)
    {
        Renderer renderer = GetComponent<Renderer>();

        //Call SetColor using the shader property name "_Color" and setting the color to red
        renderer.material.SetColor("_Color", active ? Color.red : initColor);
    }
}
