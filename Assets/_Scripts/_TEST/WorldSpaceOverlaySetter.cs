using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceOverlaySetter : MonoBehaviour
{
    private const string shaderTestMode = "unity_GUIZTestMode"; //The magic property we need to set
    [SerializeField] UnityEngine.Rendering.CompareFunction desiredUIComparison = UnityEngine.Rendering.CompareFunction.Always; //If you want to try out other effects
    
    [Tooltip("If set to 0, each material's default RenderQueue will be used")]
    [SerializeField] private int renderQueue = 0;

    [SerializeField] private Renderer _renderer;
    
    //Allows us to reuse materials
    private Dictionary<Material, Material> materialMappings = new Dictionary<Material, Material>();
    protected virtual void Start()
    {
        _renderer = GetComponent<Renderer>();
        
        Material material = _renderer.materials[0];

        if (material == null)
        {
            return;
        }
        
        Material materialCopy = new Material(material);
        
        materialCopy.SetInt(shaderTestMode, (int)desiredUIComparison);
        materialCopy.renderQueue = renderQueue > 0 ? renderQueue : materialCopy.renderQueue;
        _renderer.material = materialCopy;
    }
}
