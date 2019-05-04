using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
	public bool isMiniGame;
	public delegate void TestEvent();
	public static event TestEvent OnEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			if(OnEvent != null)
			{
				OnEvent();
			}
		}	
    }
}
