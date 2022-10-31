using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageOrderScript : MonoBehaviour
{
	private int numberInSequence;
	public int NumberInSequence { get { return numberInSequence; } set { numberInSequence = value; } }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void GetUserTap(){
		print(numberInSequence);
		OrdinaSequenzaManager.Instance.AssignImageToAvailableSpot(numberInSequence, this.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite);
		this.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = Color.clear;
		this.GetComponent<UnityEngine.UI.Button>().enabled = false;
		//this.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = null;
	}
}
