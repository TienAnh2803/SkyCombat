using UnityEngine;
using Fusion;

public class PlayerMovementHandler : NetworkBehaviour
{
    NetworkPlayerController networkPlayerController;

    private void Awake()
    {
        networkPlayerController = GetComponent<NetworkPlayerController>();
    }

    public override void FixedUpdateNetwork()
    {
        networkPlayerController.Rb.velocity = transform.forward * networkPlayerController.speed;
        if (GetInput(out NetworkInputData networkInputData))
        {
            float verticalInput = networkInputData.movementInput.y;
            float horizontalInput = networkInputData.movementInput.x;

            if (verticalInput != 0 || horizontalInput != 0)
            {
                networkPlayerController.Rotate(networkInputData.movementInput.y, networkInputData.movementInput.x);
            }
            else
            {
                networkPlayerController.RotateBack();
            }

            if (networkInputData.isShooting)
            {
                networkPlayerController.Fire();
            }
        }
    }
}
