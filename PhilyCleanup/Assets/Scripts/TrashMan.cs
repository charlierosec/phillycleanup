using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashMan : MonoBehaviour
{
	public int speed;
	public int cost;
	public int energy;
    private int score { get; set; }
    private int ds;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        ds = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (ds != score)
        {
            print($"New score {score}!");
        }
        
        ds = score;
    }

	public void Move(string direction)
	{
		int true_cost = cost - speed;
		if (energy < true_cost)
		{
			EventManager.OnGameOver();
		}
		else
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
			energy -= true_cost;
		}
	}

    public void AddScore(int points)
    {
        score += points;
    }
}
