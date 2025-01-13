using GD.Types;
using UnityEngine;
using UnityEngine.UIElements;
using GD.Audio;

public class TerminalInteraction : MonoBehaviour
{
    public Material highlightMaterial;
    public Renderer objectRenderer;
    public Material originalMaterial;

    public AudioClip startSound;
    public AudioMixerGroupName audioGroup1 = AudioMixerGroupName.Voiceover;

    public void Interact()
    {
        PlayStartSound();
    }

    private void PlayStartSound()
    {
        if (startSound != null)
        {
            AudioManager.Instance.PlaySound(startSound, audioGroup1, this.transform.position);
        }
        else
        {
            Debug.LogWarning("Start sound is not assigned.");
        }
    }

    public void Highlight(bool isHighlighted)
    {
        if (objectRenderer != null)
        {
            objectRenderer.material = isHighlighted ? highlightMaterial : originalMaterial;
        }
    }
}
