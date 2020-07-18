using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayLighting {
	static Color color = new Color();
	static LightingManager2D manager;
	static Material material;

    public static void Draw(Vector2D offset, float z) {
		DayLightingCollider.Shadow(offset, z);

		DayLightingTilemap.Shadow(offset, z);

		DayLightingCollider.Mask(offset, z);
		
		ShadowDarkness(z);
	}

	static void ShadowDarkness(float z) {
		manager = LightingManager2D.Get();

		color.a = 1f - manager.shadowDarkness;

		if (color.a > 0.01f) {
			color.r = 0.5f;
			color.g = 0.5f;
			color.b = 0.5f;
				
			material = manager.materials.GetAdditive();
			material.mainTexture = null;		
			material.SetColor ("_TintColor", color);

			Lighting2D.Max2D.DrawImage(material, Vector2.zero, LightingManager2D.Render_Size(), 0, z);
		}
	}
}
