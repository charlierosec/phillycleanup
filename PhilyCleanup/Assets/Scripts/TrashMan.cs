using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashMan : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move("up");
        } else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move("down");    
        } else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move("left");
        } else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move("right");
        }
    }

    public void Move(string direction)
    {
        var move = this.transform.position;
        
        switch (direction)
        {
            case "up":
                move.y += 1;
                break;
            
            case "down":
                move.y -= 1;
                break;
            
            case "right":
                move.x += 1;
                break;
            
            case "left":
                move.x -= 1;
                break;
        }

        this.transform.position = move;
    }
}
