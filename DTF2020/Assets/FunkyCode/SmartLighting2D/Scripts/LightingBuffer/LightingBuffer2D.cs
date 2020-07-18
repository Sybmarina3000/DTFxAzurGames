using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class LightingBuffer2D : MonoBehaviour {
	public RenderTexture renderTexture;
	public int textureSize = 0;

	public LightingSource2D lightSource;
	public Camera bufferCamera;

	public List<PartiallyBatchedCollider> partiallyBatchedList_Collider = new List<PartiallyBatchedCollider>();
	public List<PartiallyBatchedTilemap> partiallyBatchedList_Tilemap = new List<PartiallyBatchedTilemap>();

	public bool free = true;

	public static List<LightingBuffer2D> list = new List<LightingBuffer2D>();

	public void OnEnable() {
		list.Add(this);
	}

	public void OnDisable() {
		list.Remove(this);
	}

	static public List<LightingBuffer2D> GetList() {
		return(list);
	}

	static public int GetCount() {
		return(list.Count);
	}

	public void Initiate (int textureSize) {
		SetUpRenderTexture (textureSize);
		SetUpCamera ();
	}

	void SetUpRenderTexture(int _textureSize) {
		textureSize = _textureSize;

		LightingDebug.NewRenderTextures ++;
		
		renderTexture = new RenderTexture(textureSize, textureSize, 16, LightingManager2D.Get().textureFormat);

		name = "Buffer " + GetCount() + " (size: " + textureSize + ")";
	}

	void SetUpCamera() {
		bufferCamera = gameObject.AddComponent<Camera>();
		bufferCamera.clearFlags = CameraClearFlags.Color;
		bufferCamera.backgroundColor = Color.white;
		bufferCamera.cameraType = CameraType.Game;
		bufferCamera.orthographic = true;
		bufferCamera.targetTexture = renderTexture;
		bufferCamera.farClipPlane = 0.5f;
		bufferCamera.nearClipPlane = 0f;
		bufferCamera.allowHDR = false;
		bufferCamera.allowMSAA = false;
		bufferCamera.enabled = false;
	}

	void LateUpdate() {
		float cameraZ = -1000f;

		Camera camera = LightingManager2D.Get().GetCamera();

		if (camera != null) {
			cameraZ = camera.transform.position.z - 10 - GetCount();
		}

		bufferCamera.transform.position = new Vector3(0, 0, cameraZ);

		transform.rotation = Quaternion.Euler(0, 0, 0);
	}
	
	public void OnRenderObject() {
		if(Camera.current != bufferCamera) {
			return;
		}

		LateUpdate ();

		LightingBufferShadow.Fill.Calculate();
		LightingBufferShadow.Penumbra.Calculate();
		LightingBufferShadow.Setup.Calculate(this);

		GL.PushMatrix();

		for (int layerID = 0; layerID < lightSource.layerCount; layerID++) {
			if (lightSource.layerSetting == null || lightSource.layerSetting.Length <= layerID) {
				continue;
			}

			LayerSetting layerSetting = lightSource.layerSetting[layerID];

			if (layerSetting == null) {
				continue;
			}

			if (lightSource.enableCollisions) {	
				if (layerSetting.renderingOrder == LightingLayerOrder.Default) {
					Default.Draw(this, layerSetting);
				} else {
					Sorted.Draw(this, layerSetting);
				}
			}
		}
	
		LightingSourceTexture.Draw(this);

		GL.PopMatrix();

		LightingDebug.LightBufferUpdates ++;
		LightingDebug.totalLightUpdates ++;

		bufferCamera.enabled = false;
	}

	class Default {
		static public void Draw(LightingBuffer2D buffer, LayerSetting layer) {
			LightingBufferDefault.Draw(buffer, layer);
		}
	}

	class Sorted {
		static public void Draw(LightingBuffer2D buffer, LayerSetting layer) {
			LightingBufferSorted.Draw(buffer, layer);
		}
	}	
}