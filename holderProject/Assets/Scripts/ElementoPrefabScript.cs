using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementoPrefabScript : MonoBehaviour
{
	public bool correctAnswer = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void GetClick()
	{
		AttenzioneElementoManager.Instance.GetClick(correctAnswer);
	}
}
