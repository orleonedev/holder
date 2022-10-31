using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoveVistoSolPrefabScript : MonoBehaviour
{
	public Sprite selectedImage;
	public Color selectedColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void GetClick(){
		DoveVistoManager.Instance.SelectImageToPlace(selectedImage, selectedColor);
	}
}
