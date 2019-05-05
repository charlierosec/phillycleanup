using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CodeInput : MonoBehaviour
{
	public Button button;
	public InputField inputField;
	public InputField outputField;

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
		outputField.text = "";
    }

	public void HandleButtonClick()
	{
		print(inputField.text);
        var lx = new Lexer(inputField.text);
        var pr = new Parser(lx.Scan());
        var it = new Interpreter();
        var parsed = pr.Parse();
        
        if (pr.Errors.Count == 0)
        {
	        it.Interpret(parsed);
        }
        else
        {
	        outputField.text = "Error: " + pr.Errors.First().Message;
	        
	        Text text = outputField.transform.Find("Text").GetComponent<Text>();
	        text.color = Color.red;
        }
	}
    // Update is called once per frame
    void Update()
    {
        
    }
}
