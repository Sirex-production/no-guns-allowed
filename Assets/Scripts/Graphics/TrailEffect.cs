using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEffect : MonoBehaviour
{
    [SerializeField] 

    private TrailRenderer _trComponent;

    // Start is called before the first frame update
    void Start()
    {
        _trComponent = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
