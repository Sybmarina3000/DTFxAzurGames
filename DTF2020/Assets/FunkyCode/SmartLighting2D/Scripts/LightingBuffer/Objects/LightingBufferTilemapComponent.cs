using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_2018_1_OR_NEWER

public class LightingBufferTilemapComponent {
	public static Vector2D polyOffset = Vector2D.Zero();

	static Polygon2D poly = null;
	public static List<Pair2D> pairList;
	public static List<List<Pair2D>> pairsList = new List<List<Pair2D>>();

	static Vector2 scale = new Vector2(1, 1);

	static public void Shadow(LightingBuffer2D buffer, LightingTilemapCollider2D id, float lightSizeSquared, float z) {
		if (id.colliderType != LightingTilemapCollider2D.ColliderType.Collider) {
			return;
		}

		polyOffset.x = -buffer.lightSource.transform.position.x;
		polyOffset.y = -buffer.lightSource.transform.position.y;

		ShadowEdge(buffer, id, lightSizeSquared, z);
		ShadowPolygon(buffer, id, lightSizeSquared, z);
	}

	static public void ShadowPolygon(LightingBuffer2D buffer, LightingTilemapCollider2D id, float lightSizeSquared, float z) {
		pairsList = new List<List<Pair2D>>();
		
		for(int i = 0; i < id.polygonColliders.Count; i++) {
			poly = id.polygonColliders[i];

			pairList = Pair2D.GetList(poly.pointsList);
			pairsList.Add(pairList);
		}

		LightingBufferShadow.Draw(buffer, id.polygonColliders, pairsList, lightSizeSquared, z, polyOffset, scale);
	}

	static public void ShadowEdge(LightingBuffer2D buffer, LightingTilemapCollider2D id, float lightSizeSquared, float z) {
		pairsList = new List<List<Pair2D>>();

		for(int i = 0; i < id.edgeColliders.Count; i++) {
			poly = id.edgeColliders[i];

			pairList = Pair2D.GetList(poly.pointsList, false);
			pairsList.Add(pairList);
		}

		LightingBufferShadow.Draw(buffer, id.edgeColliders, pairsList, lightSizeSquared, z, polyOffset, scale);
	}
}

#endif