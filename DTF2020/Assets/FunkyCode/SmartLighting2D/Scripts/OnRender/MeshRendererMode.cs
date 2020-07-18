using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MeshRendererMode : MonoBehaviour {
    public static MeshRendererMode instance;
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    public static MeshRendererMode Get() {
        if (instance != null) {
			return(instance);
		}

		foreach(MeshRendererMode meshModeObject in Object.FindObjectsOfType(typeof(MeshRendererMode))) {
			instance = meshModeObject;
			return(instance);
		}

        if (instance == null) {
            GameObject meshRendererMode = new GameObject("On Render");
            instance = meshRendererMode.AddComponent<MeshRendererMode>();

            instance.Initialize();
        }

        return(instance);
    }

    public void Initialize() {         
        LightingManager2D manager = LightingManager2D.Get();
        transform.parent = manager.transform;
        
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = manager.mainBuffer.GetMaterial();
           
        meshRenderer.sortingLayerName = manager.sortingLayerName;
        meshRenderer.sortingLayerID = manager.sortingLayerID;
        meshRenderer.sortingOrder = manager.sortingLayerOrder;

        // Disable Mesh Renderer Settings
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        meshRenderer.receiveShadows = false;
        meshRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        meshRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        meshRenderer.allowOcclusionWhenDynamic = false;

        UpdatePosition();

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = LightingManager2D.GetRenderMesh();
    }

    void LateUpdate() {
        UpdatePosition();
    }

    public void UpdatePosition() {
        Camera camera = LightingManager2D.Get().GetCamera();
        if (camera == null) {
            return;
        }
        
        Vector3 position = camera.transform.position;
        position.z += camera.nearClipPlane + 0.1f;

        transform.position = position;
        transform.rotation = camera.transform.rotation;
        transform.localScale = LightingManager2D.Render_Size();
    }
}