using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStat : MonoBehaviour
{
    public int visited = -1;
    public int x = 0;
    public int y = 0;
    public int dirX;
    public int dirY;

    private Renderer rend;
    private Color initialColor;

    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
        initialColor = rend.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(visited == -2)
        {
            rend.material.color = Color.red;
        }
        else
        {
            rend.material.color = initialColor;
        }
    }

    private void OnMouseEnter()
    {
        visited = -2;
    }

    private void OnMouseExit()
    {
        visited = -1;
    }
}
