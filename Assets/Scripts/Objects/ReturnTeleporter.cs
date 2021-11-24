using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Returns the player to the Middle Room
/// </summary>
public class ReturnTeleporter : MonoBehaviour
{
    public Transform targetTeleporter;
    public Transform player;

    /// <summary>
    /// When triggered by the player, it changes the player's 
    /// position to be the same as the returning portal's in the Middle Room
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponentInChildren<PlayerMovement>().enabled = false;
        player.transform.position = targetTeleporter.position;
        StartCoroutine(ActivateCharacterController());
    }

   private IEnumerator ActivateCharacterController()
    {
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponentInChildren<PlayerMovement>().enabled = true;
    }
}
