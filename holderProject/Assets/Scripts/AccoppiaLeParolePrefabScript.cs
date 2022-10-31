using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccoppiaLeParolePrefabScript : MonoBehaviour
{
	GameObject gameObjectReference;
    // Start is called before the first frame update
    void Start()
    {
        gameObjectReference = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void GetClick(){
		AccoppiaLeParoleManager.Instance.GetClick(gameObjectReference);
	}
}
