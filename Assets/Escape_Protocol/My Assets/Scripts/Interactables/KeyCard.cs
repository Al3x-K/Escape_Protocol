using GD.Items;
using UnityEngine;

public class Keycard : MonoBehaviour
{
    public ItemData itemData; 
    public Material highlightMaterial; 
    private Material originalMaterial;

    private Renderer objectRenderer;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalMaterial = objectRenderer.material;
        }
    }

    public void Highlight(bool enable)
    {
        if (objectRenderer != null)
        {
            objectRenderer.material = enable ? highlightMaterial : originalMaterial;
        }
    }
}
