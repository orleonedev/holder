using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongPressListener : MonoBehaviour
{
    [SerializeField]
    static public int triggerTime = 5;
    private int elapsedTime;
    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Stationary ) {
            elapsedTime += 1;

        }
        
    }
}
