using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected float cooldownDuration;

    public void Init(float timer)
    {
        cooldownDuration = timer;
    }

    protected void RevertPlayer(GameObject newPlayer)
    {
        PlayerController oldPlayerController = GetComponent<PlayerController>();
        PlayerController newPlayerController = newPlayer.GetComponent<PlayerController>();

        if (oldPlayerController.stuckToSurface)
        {
            newPlayerController.rb.gravityScale = oldPlayerController.rb.gravityScale;
        }
        
        newPlayerController.stuckToSurface = oldPlayerController.stuckToSurface;
        newPlayerController.StuckSurface = oldPlayerController.StuckSurface;
        newPlayerController.SurfaceContactInstance = oldPlayerController.SurfaceContactInstance;

        // Reparent the held item so it doesn't get destroyed
        if (oldPlayerController.PulledObject && oldPlayerController.PulledObject.transform.parent == transform)
        {
            oldPlayerController.PulledObject.transform.parent = newPlayer.transform;
        }

        newPlayerController.PulledObject = oldPlayerController.PulledObject;
        newPlayerController.PullContactInstance = oldPlayerController.PullContactInstance;

        if (oldPlayerController.gumInstance)
        {
            GumMovement gum = oldPlayerController.gumInstance.GetComponent<GumMovement>();
            newPlayerController.gumInstance = oldPlayerController.gumInstance;
            gum.owner = newPlayerController;
            // Reparent the new gum under the new player controller
            if (newPlayerController.gumInstance.transform.parent == transform)
            {
                newPlayerController.gumInstance.transform.parent = newPlayer.transform;
            }
        }

        GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow = newPlayer.transform;
    }
}
