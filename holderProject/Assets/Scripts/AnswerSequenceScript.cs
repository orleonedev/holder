using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerSequenceScript : MonoBehaviour
{
	private int imagePosition;
	public int ImagePosition { get { return imagePosition; } set { imagePosition = value; } }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void DeleteImageInSequence(){
		this.transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().color = Color.clear;
		OrdinaSequenzaManager.Instance.DeleteImageFromTappedSpot(imagePosition);
	}
}
