using UnityEngine;
using System.Collections;
using GD.Audio; 
using GD.Types;
using GD.Items; 

public class LockedDoor : MonoBehaviour
{
    public ItemData requiredKeyItem; 
    public float slideDistance; 
    public float slideSpeed; 
    
    public AudioClip openDoorSound;
    public AudioMixerGroupName audioGroup = AudioMixerGroupName.SFX;
    public AudioClip denySound;
    public AudioClip grantAccessSound;
    public AudioMixerGroupName audioGroup1 = AudioMixerGroupName.Voiceover;

    private Vector3 originalPosition; 
    private bool isOpen = false; 

    private void Start()
    {
        originalPosition = transform.position;
    }

    public void UnlockDoor()
    {
        if (isOpen) return;

        Debug.Log("Door is unlocked!");
        PlayOpenSound();
        StartCoroutine(OpenDoor());
    }

    private void PlayOpenSound()
    {
        if (openDoorSound != null)
        {
            AudioManager.Instance.PlaySound(openDoorSound, audioGroup, this.transform.position);
        }
        else
        {
            Debug.LogWarning("Open door sound is not assigned.");
        }
    }

    public void PlayDenySound()
    {
        AudioManager.Instance.PlaySound(denySound, audioGroup1, this.transform.position);
    }

    public void PlayGrantAccessSound()
    {
        AudioManager.Instance.PlaySound(grantAccessSound, audioGroup1, this.transform.position);
    }

    private IEnumerator OpenDoor()
    {
        isOpen = true;

        Vector3 targetPosition = originalPosition - new Vector3(0, slideDistance, 0);
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, slideSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        PlayOpenSound();
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        { 
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, slideSpeed * Time.deltaTime);
            yield return null;
        }

        isOpen = false; 
    }
}
