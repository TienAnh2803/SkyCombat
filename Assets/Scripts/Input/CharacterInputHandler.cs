using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    Vector2 moveInputVector = Vector2.zero;
    bool isShooting = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space))
        {
            isShooting = true;
        }else {
            isShooting = false;
        }

    }

    public NetworkInputData GetNetworkInput() {
        NetworkInputData networkInputData = new NetworkInputData();
        networkInputData.movementInput = moveInputVector;
        networkInputData.isShooting = isShooting;
        return networkInputData;
    }
}
