using Sirenix.OdinInspector;
using UnityEngine;

public class ScriptableGameObject : ScriptableObject
{
    #region Fields

    [SerializeField]
    [ContextMenuItem("Reset Name", "ResetName")]
    [FoldoutGroup("Info", expanded: true)]
    private new string name = string.Empty;

    [SerializeField]
    [ContextMenuItem("Reset Description", "ResetDescription")]
    [TextArea(2, 4)]
    [FoldoutGroup("Info")]
    private string description = string.Empty;

    #endregion Fields

    #region Properties

    public string Name { get => name; set => name = value; }

    public string Description { get => description; set => description = value; }

    #endregion Properties

    #region Core Methods

        
    public void ResetName()
    {
        Name = string.Empty;
    }

    public void ResetDescription()
    {
        Description = string.Empty;
    }

    public virtual void Reset()
    { 
        //noop - (no operation - not called)
    }

    #endregion Core Methods
}
