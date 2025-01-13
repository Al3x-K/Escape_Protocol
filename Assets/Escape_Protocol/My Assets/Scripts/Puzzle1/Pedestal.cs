using UnityEngine;

public class Pedestal : MonoBehaviour
{
    public Color pedestalColor; // Assign the pedestal's color in the Inspector

    private Renderer pedestalRenderer;

    private void Start()
    {
        pedestalRenderer = GetComponent<Renderer>();
        if (pedestalRenderer != null)
        {
            pedestalRenderer.material.color = pedestalColor; // Set pedestal color
        }
        else
        {
            Debug.LogError("Renderer not found on Pedestal. Please add a Renderer component.");
        }
    }

    public bool IsCubeOnTop(PuzzleCube cube)
    {
        if (cube == null) return false;

        bool isMatch = cube.cubeColor == pedestalColor;
        Debug.Log($"{cube.name} on {name}: Match = {isMatch}");
        return isMatch;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f); // Visualize the detection range
    }
}
