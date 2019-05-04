using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeInput : MonoBehaviour
{
	public Button button;
	public InputField inputField;

	public string ToEdit = "Hello World";
	public int x = 10;
	public int y = 10;

	void OnGUI()
	{
		
	}
	// Start is called before the first frame update
	void Start()
    {
		button.onClick.AddListener(HandleButtonClick);    
    }

	public void HandleButtonClick()
	{
		print(inputField.text);
	}
    // Update is called once per frame
    void Update()
    {
        
    }
}
