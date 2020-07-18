using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingBufferCollider {
	static List<Polygon2D> polygons = null;
	static List<List<Pair2D>> polygonPairs = null;
	static List<Pair2D> pairList = null;
	
	static Vector2 scale = Vector2.zero;
	static Color color = Color.white;
	static VirtualSpriteRenderer spriteRenderer = new VirtualSpriteRenderer();

    public static void Shadow(LightingBuffer2D buffer, LightingCollider2D id, float lightSizeSquared, float z, Vector2D offset) {
		if (false == (id.shape.colliderType == LightingCollider2D.ColliderType.Collider || id.shape.colliderType == LightingCollider2D.ColliderType.SpriteCustomPhysicsShape)) {	
			return;
		}
		
		if (id.isVisibleForLight(buffer) == false) {
			return;
		}

		spriteRenderer.sprite = id.shape.GetOriginalSprite();

		if (id.spriteRenderer != null) {
			spriteRenderer.flipX = id.spriteRenderer.flipX;
			spriteRenderer.flipY = id.spriteRenderer.flipY;
		} else {
			spriteRenderer.flipX = false;
			spriteRenderer.flipY = false;
		}
		
		polygons = id.shape.GetPolygons_World_ColliderType(id.transform, spriteRenderer);
		
		if (polygons.Count < 1) {
			return;
		}

		polygonPairs = id.shape.GetPolygons_Pair_World_ColliderType(id.transform, spriteRenderer);

		scale.x = 1;
		scale.y = 1;

		LightingBufferShadow.Draw(buffer, polygons, polygonPairs, lightSizeSquared, z, offset, scale);
	}

	public static void Mask(LightingBuffer2D buffer, LightingCollider2D id, LayerSetting layerSetting, Vector2D offset, float z) {
		if (false == (id.shape.maskType == LightingCollider2D.MaskType.Collider || id.shape.maskType == LightingCollider2D.MaskType.SpriteCustomPhysicsShape)) {	
			return;
		}
		
		if (id.isVisibleForLight(buffer) == false) {
			return;
		}

		MeshVertices vertices = id.shape.GetMesh_Vertices_MaskType(id.transform);

		if (vertices == null || vertices.veclist == null || vertices.veclist.Count < 1) {
			return;
		}

		bool maskEffect = (layerSetting.effect == LightingLayerEffect.InvisibleBellow);

		LightingMaskMode maskMode = id.maskMode;
		MeshVertice vertice;

		if (maskMode == LightingMaskMode.Invisible) {
			GL.Color(Color.black);

		} else if (layerSetting.effect == LightingLayerEffect.InvisibleBellow) {
			float c = (float)offset.y / layerSetting.maskEffectDistance +  layerSetting.maskEffectDistance * 2;
			if (c < 0) {
				c = 0;
			}

			color.r = c;
			color.g = c;
			color.b = c;
			color.a = 1;

			GL.Color(color);
		} else {
			GL.Color(Color.white);
		}

		for (int i = 0; i < vertices.veclist.Count; i ++) {
			vertice = vertices.veclist[i];

			GL.TexCoord3(Max2DMatrix.c_x, Max2DMatrix.c_y, 0);
			GL.Vertex3((float)vertice.a.x + (float)offset.x, (float)vertice.a.y + (float)offset.y, z);
			GL.TexCoord3(Max2DMatrix.c_x, Max2DMatrix.c_y, 0);
			GL.Vertex3((float)vertice.b.x + (float)offset.x, (float)vertice.b.y + (float)offset.y, z);
			GL.TexCoord3(Max2DMatrix.c_x, Max2DMatrix.c_y, 0);
			GL.Vertex3((float)vertice.c.x + (float)offset.x, (float)vertice.c.y + (float)offset.y, z);
		}

		LightingDebug.maskGenerations ++;		
	}	
}