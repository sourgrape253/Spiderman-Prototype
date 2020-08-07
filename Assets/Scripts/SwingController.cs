using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingController : MonoBehaviour
{
    [SerializeField]

    public SwingPendulum pendulum;

    // Start is called before the first frame update
    void Start()
    {
        pendulum.Initialize();
    }

    // Update is called once per frame
    void Update()
    {

        //transform.localPosition = pendulum.MovePlayer(transform.localPosition, Time.deltaTime);
    }
}
