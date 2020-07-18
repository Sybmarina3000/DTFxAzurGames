using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] 
public class LightingMainBuffer2D : MonoBehaviour {
	static private LightingMainBuffer2D instance;

	public RenderTexture renderTexture;

	// Should Be Static
	private LightingMaterial material = null; 
	public Camera bufferCamera;

	public int screenWidth = 640;
	public int screenHeight = 480;

	// This Should Be Changed To Setting
	const float cameraOffset = 40f;

	public float cameraSize = 5f;

	static Vector2D pos2D = Vector2D.Zero();
	static Vector2D size2D = Vector2D.Zero();
	static Vector2D offset = Vector2D.Zero();

	static public LightingMainBuffer2D Get() {
		if (instance != null) {
			return(instance);
		}

		foreach(LightingMainBuffer2D mainBuffer in Object.FindObjectsOfType(typeof(LightingMainBuffer2D))) {
			instance = mainBuffer;
			return(instance);
		}

		GameObject setMainBuffer = new GameObject ();
		setMainBuffer.transform.parent = LightingManager2D.Get().transform;
		setMainBuffer.name = "Main Buffer";
		setMainBuffer.layer = LightingManager2D.lightingLayer;

		instance = setMainBuffer.AddComponent<LightingMainBuffer2D> ();
		instance.Initialize();
		
		return(instance);
	}

	 public static void ForceUpdate() {
		LightingMainBuffer2D buffer =  Get();

		if (buffer) {
			buffer.gameObject.SetActive(false);
			buffer.gameObject.SetActive(true);
		}
	}

	public void Initialize() {
		SetUpRenderTexture ();

		SetUpCamera ();
	}

	void SetUpRenderTexture() {
		LightingManager2D manager = LightingManager2D.Get();

		screenWidth = (int)(Screen.width * manager.lightingResolution);
		screenHeight = (int)(Screen.height * manager.lightingResolution);

		LightingDebug.NewRenderTextures ++;
		
		renderTexture = new RenderTexture (screenWidth, screenHeight, 16, LightingManager2D.Get().textureFormat);
		renderTexture.Create ();
	}

	public Material GetMaterial() {
		if (material == null || material.Get() == null) {
			//Debug.Log("Smart Lighting 2D: Main Buffer 2D Material is Null");
			material = LightingMaterial.Load(Max2D.shaderPath + "Particles/Multiply");
		}

		material.SetTexture(renderTexture);

		return(material.Get());
	}

	public void OnPreCull() {
		LightingManager2D manager = LightingManager2D.Get();

		if (manager.disableEngine) {
			return;
		}
		
		if (manager.meshRendererMode != null) {
			manager.meshRendererMode.UpdatePosition();
		}
		
		manager.Render_PreRenderMode();
		manager.Render_MeshRenderMode();
	}

	void SetUpCamera() {
		bufferCamera = gameObject.AddComponent<Camera> ();
		bufferCamera.clearFlags = CameraClearFlags.Color;
		bufferCamera.backgroundColor = Color.black;
		bufferCamera.cameraType = CameraType.Game;
		bufferCamera.orthographic = true;
		bufferCamera.targetTexture = renderTexture;
		bufferCamera.farClipPlane = 1f;
		bufferCamera.nearClipPlane = 0f;
		bufferCamera.allowMSAA = false;
		bufferCamera.allowHDR = false;
		bufferCamera.enabled = false;
	}

	void LateUpdate () {
		LightingManager2D manager = LightingManager2D.Get();

		int width = (int)(Screen.width * manager.lightingResolution);
		int height = (int)(Screen.height * manager.lightingResolution);

		if (width != screenWidth || height != screenHeight) {
			SetUpRenderTexture();

			bufferCamera.targetTexture = renderTexture;
		}

		Camera camera = manager.GetCamera();

		if (camera == null) {
			Debug.LogWarning("SmartLighting2D: Main Camera is Missing");
			return;
		}
 
		bufferCamera.orthographicSize = camera.orthographicSize;

		cameraSize = bufferCamera.orthographicSize;

		bufferCamera.backgroundColor = new Color(0.0274f, 0.1019f, 0.1568f); ;

		transform.position = new Vector3(0, 0, camera.transform.position.z - cameraOffset);
		transform.rotation = camera.transform.rotation;

		ForceUpdate();
	}

	public void OnRenderObject() {
		if (Camera.current != bufferCamera) {
			return;
		}

		LightingManager2D manager = LightingManager2D.Get();
		Camera camera = manager.GetCamera();

		if (camera == null) {
			return;
		}

		LightingDebug.LightMainBufferUpdates +=1 ;

		float z = transform.position.z;

		offset.x = -camera.transform.position.x;
		offset.y = -camera.transform.position.y;

		GL.PushMatrix();
		
		if (manager.drawDayShadows) {
			DayLighting.Draw(offset, z);
		}
	
		if (manager.drawRooms) {
			DrawRooms(offset, z);
			
			#if UNITY_2018_1_OR_NEWER
				DrawTilemapRooms(offset, z);
			#endif
		}

		LightingSpriteBuffer.Draw(offset, z);

		DrawLightingBuffers(z);

		if (manager.drawOcclusion) {
			LightingOcclusionCollider.Draw(offset, z);
		}

		GL.PopMatrix();
	}
	
	// Room Mask
	void DrawRooms(Vector2D offset, float z) {
		LightingManager2D manager = LightingManager2D.Get();

		manager.materials.GetAtlasMaterial().SetPass(0);
		GL.Begin(GL.TRIANGLES);

		foreach (LightingRoom2D id in LightingRoom2D.GetList()) {
			LightingRoomCollider.Mask(id, offset, z);
		}

		GL.End();
	}

	#if UNITY_2018_1_OR_NEWER
		void DrawTilemapRooms(Vector2D offset, float z) {
			LightingManager2D manager = LightingManager2D.Get();

			foreach (LightingTilemapRoom2D id in LightingTilemapRoom2D.GetList()) {
				LightingRoomTilemap.MaskSpriteWithoutAtlas(manager.GetCamera(), id, offset, z);
			}
		}
	#endif

	// Lighting Buffers
	void DrawLightingBuffers(float z) {
		LightingManager2D manager = LightingManager2D.Get();

		Camera camera = manager.GetCamera();
		
		Material material = manager.materials.GetAdditive();

		foreach (LightingSource2D id in LightingSource2D.GetList()) {
			if (id.buffer == null) {
				continue;
			}
			if (id.isActiveAndEnabled == false) {
				continue;
			}

			if (id.buffer.bufferCamera == null) {
				continue;
			}

			if (id.InCamera() == false) {
				continue;
			}

			Vector3 pos = id.transform.position - camera.transform.position;
			pos2D.x = pos.x;
			pos2D.y = pos.y;

			float size = id.buffer.bufferCamera.orthographicSize;
			size2D.x = size;
			size2D.y = size;

			Color lightColor = id.lightColor;
			lightColor.a = id.lightAlpha / 2;

			material.mainTexture = id.buffer.renderTexture;

			material.SetColor ("_TintColor", lightColor);
		
			Lighting2D.Max2D.DrawImage (material, pos2D, size2D, z);
		}
	}
}