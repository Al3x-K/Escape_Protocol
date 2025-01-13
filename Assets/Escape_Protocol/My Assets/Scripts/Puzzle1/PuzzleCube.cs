using UnityEngine;
using GD.Audio;
using GD.Types;

public class PuzzleCube : MonoBehaviour
{
    public Color cubeColor; // Assign the cube's color in the Inspector
    public AudioClip goodPlacementSound; // Sound for correct placement
    public AudioClip badPlacementSound; // Sound for incorrect placement
    public AudioMixerGroupName audioGroup = AudioMixerGroupName.SFX;
    public void CheckPlacement(Transform pedestal)
    {
        var pedestalScript = pedestal.GetComponent<Pedestal>();
        if (pedestalScript != null)
        {
            if (pedestalScript.pedestalColor == cubeColor)
            {
                Debug.Log($"{name} placed on {pedestal.name}: Correct!");
                PlaySound(goodPlacementSound);
            }
            else
            {
                Debug.Log($"{name} placed on {pedestal.name}: Incorrect!");
                PlaySound(badPlacementSound);
            }
        }
        else
        {
            Debug.LogWarning($"{pedestal.name} does not have a Pedestal script.");
        }
    }

    private void PlaySound(AudioClip sound)
    {
        if (sound != null)
        {
            Debug.Log($"Playing sound: {sound.name}");
            AudioManager.Instance.PlaySound(sound, audioGroup, transform.position);
        }
        else
        {
            Debug.LogWarning("Sound is not assigned.");
        }
    }
}
