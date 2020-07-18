using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[System.Serializable]
public class LightingManager2DMaterials {
	private Sprite penumbraSprite;
	private Sprite atlasPenumbraSprite;

	private Sprite whiteMaskSprite;
	private Sprite atlasWhiteMaskSprite;

	private LightingMaterial occlusionEdge = null;
	private LightingMaterial occlusionBlur = null;
	private LightingMaterial shadowBlur = null;
	private LightingMaterial additive = null;
	private LightingMaterial multiply = null;

	private LightingMaterial whiteSprite = null;
	private LightingMaterial blackSprite = null;

	private LightingMaterial atlasMaterial = null;

	public Sprite GetPenumbraSprite() {
		if (penumbraSprite == null) {
			penumbraSprite = Resources.Load<Sprite>("textures/penumbra"); 
		}
		return(penumbraSprite);
	}

	public Sprite GetAtlasPenumbraSprite() {
		if (atlasPenumbraSprite == null) {
			atlasPenumbraSprite = SpriteAtlasManager.RequestSprite(GetPenumbraSprite(), SpriteRequest.Type.BlackAlpha);
		}
		return(atlasPenumbraSprite);
	}

	public Sprite GetWhiteMaskSprite() {
		if (whiteMaskSprite == null) {
			whiteMaskSprite = Resources.Load<Sprite>("textures/white"); 
		}
		return(whiteMaskSprite);
	}

	public Sprite GetAtlasWhiteMaskSprite() {
		if (atlasWhiteMaskSprite == null) {
			atlasWhiteMaskSprite = SpriteAtlasManager.RequestSprite(GetWhiteMaskSprite(), SpriteRequest.Type.WhiteMask);
		}
		return(atlasWhiteMaskSprite);
	}

	public void Initialize() {
		GetPenumbraSprite();
		GetAtlasPenumbraSprite();

		GetWhiteMaskSprite();

		GetAtlasWhiteMaskSprite();

		GetAdditive();
		GetOcclusionBlur();
		GetOcclusionEdge();
		GetShadowBlur();
		GetWhiteSprite();
		GetBlackSprite();
		GetAtlasMaterial();
	}

	public Material GetAtlasMaterial() {
		if (atlasMaterial == null || atlasMaterial.Get() == null) {
			atlasMaterial = LightingMaterial.Load(Max2D.shaderPath + "Particles/Alpha Blended");
		}
		
		atlasMaterial.SetTexture(SpriteAtlasManager.GetAtlasPage().texture);

		return(atlasMaterial.Get());
	}
	
	public Material GetAdditive() {
		if (additive == null || additive.Get() == null) {
			additive = LightingMaterial.Load(Max2D.shaderPath + "Particles/Additive");
		}
		return(additive.Get());
	}

	public Material GetMultiply() {
		if (multiply == null || multiply.Get() == null) {
			multiply = LightingMaterial.Load(Max2D.shaderPath + "Particles/Multiply");
		}
		return(multiply.Get());
	}

	public Material GetOcclusionEdge() {
		if (occlusionEdge == null || occlusionEdge.Get() == null) {
			occlusionEdge = LightingMaterial.Load(Max2D.shaderPath + "Particles/Multiply");
			occlusionEdge.SetTexture("textures/occlusionedge");
		}
		return(occlusionEdge.Get());
	}

	public Material GetShadowBlur() {
		if (shadowBlur == null || shadowBlur.Get() == null) {
			shadowBlur = LightingMaterial.Load(Max2D.shaderPath + "Particles/Multiply");
			shadowBlur.SetTexture("textures/shadowblur");
		}
		return(shadowBlur.Get());
	}

	public Material GetOcclusionBlur() {
		if (occlusionBlur == null || occlusionBlur.Get() == null) {
			occlusionBlur = LightingMaterial.Load(Max2D.shaderPath + "Particles/Multiply");
			occlusionBlur.SetTexture("textures/occlussionblur");
		}
		return(occlusionBlur.Get());
	}

	public Material GetWhiteSprite() {
		if (whiteSprite == null || whiteSprite.Get() == null) {
			whiteSprite = LightingMaterial.Load("SmartLighting2D/SpriteWhite");
		}
		return(whiteSprite.Get());
	}

	public Material GetBlackSprite() {
		if (blackSprite == null || blackSprite.Get() == null) {
			blackSprite = LightingMaterial.Load("SmartLighting2D/SpriteBlack");
		}
		return(blackSprite.Get());
	}
}