using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePositionChecker : MonoBehaviour
{
    [SerializeField] GameObject pointSetter;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform pos in GetComponentsInChildren<Transform>())
        {
            Debug.Log(pos.name + ": " + pos.position);
            Instantiate(pointSetter, pos.position, Quaternion.identity);
        }
        //GetComponent< Renderer>().bounds.center<<이걸로 mesh의 중간을 찾을 수 있음.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
