// file:	Assets\DownloadedContent\FREE Footsteps System\scripts\SurfaceManager.cs
//
// summary:	Implements the surface manager class

using UnityEngine;

/// <summary>   A surface definition. </summary>
///
/// <remarks>    . </remarks>

[System.Serializable]
public struct SurfaceDefinition {
    /// <summary>   The name. </summary>
	public string name;
    /// <summary>   The footsteps. </summary>
	public AudioClip[] footsteps;
}

/// <summary>   A registered material. </summary>
///
/// <remarks>    . </remarks>

[System.Serializable]
public struct RegisteredMaterial {
    /// <summary>   The texture. </summary>
	public Texture texture;
    /// <summary>   Zero-based index of the surface. </summary>
	public int surfaceIndex;
}

/// <summary>   Manager for surfaces. </summary>
///
/// <remarks>    . </remarks>

public class SurfaceManager : MonoBehaviour {

    /// <summary>   The singleton. </summary>
	public static SurfaceManager singleton;

    /// <summary>   Gets the defined surfaces. </summary>
    ///
    /// <value> The defined surfaces. </value>

	[SerializeField] SurfaceDefinition[] definedSurfaces;

    /// <summary>   Gets the registered textures. </summary>
    ///
    /// <value> The registered textures. </value>

	[SerializeField] RegisteredMaterial[] registeredTextures;

    /// <summary>   The int to process. </summary>
	int n;

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start() {
		if(!singleton) singleton = this;
		else if(singleton != this) Destroy(gameObject);
	}

    /// <summary>   Gets a footstep. </summary>
    ///
 
    ///
    /// <param name="groundCollider">   The ground collider. </param>
    /// <param name="worldPosition">    The world position. </param>
    ///
    /// <returns>   The footstep. </returns>

	public AudioClip GetFootstep(Collider groundCollider, Vector3 worldPosition) {
		int surfaceIndex = GetSurfaceIndex(groundCollider, worldPosition);

		if(surfaceIndex == -1) {
			return null;
		}

		// Getting the footstep sounds based on surface index.
		AudioClip[] footsteps = definedSurfaces[surfaceIndex].footsteps;
		n = Random.Range(1, footsteps.Length);

		// Move picked sound to index 0 so it's not picked next time.
		AudioClip temp = footsteps[n];
		footsteps[n] = footsteps[0];
		footsteps[0] = temp;

		return temp;
	}

    /// <summary>   Gets all surface names. </summary>
    ///
 
    ///
    /// <returns>   An array of string. </returns>

	public string[] GetAllSurfaceNames() {
		string[] names = new string[definedSurfaces.Length];

		for(int i = 0;i < names.Length;i ++) names[i] = definedSurfaces[i].name;

		return names;
	}

	// This is for bullet hit particles

    /// <summary>   Gets surface index. </summary>
    ///
 
    ///
    /// <param name="ray">      The ray. </param>
    /// <param name="col">      The col. </param>
    /// <param name="worldPos"> The world position. </param>
    ///
    /// <returns>   The surface index. </returns>

	int GetSurfaceIndex(Ray ray, Collider col, Vector3 worldPos) {
		string textureName = "";

		// Case when the ground is a terrain.
		if(col.GetType() == typeof(TerrainCollider)) {
			Terrain terrain = col.GetComponent<Terrain>();
			TerrainData terrainData = terrain.terrainData;
			float[] textureMix = GetTerrainTextureMix(worldPos, terrainData, terrain.GetPosition());
			int textureIndex = GetTextureIndex(worldPos, textureMix);
			textureName = terrainData.splatPrototypes[textureIndex].texture.name;
		}
		// Case when the ground is a normal mesh.
		else {
			textureName = GetMeshMaterialAtPoint(worldPos, ray);
		}
		// Searching for the found texture / material name in registered materials.
		foreach(var material in registeredTextures) {
			if(material.texture.name == textureName) {
				return material.surfaceIndex;
			}
		}

		return -1;
	}

	// This is for footsteps

    /// <summary>   Gets surface index. </summary>
    ///
 
    ///
    /// <param name="col">      The col. </param>
    /// <param name="worldPos"> The world position. </param>
    ///
    /// <returns>   The surface index. </returns>

	int GetSurfaceIndex(Collider col, Vector3 worldPos) {
		string textureName = "";

		// Case when the ground is a terrain.
		if(col.GetType() == typeof(TerrainCollider)) {
			Terrain terrain = col.GetComponent<Terrain>();
			TerrainData terrainData = terrain.terrainData;
			float[] textureMix = GetTerrainTextureMix(worldPos, terrainData, terrain.GetPosition());
			int textureIndex = GetTextureIndex(worldPos, textureMix);
			textureName = terrainData.splatPrototypes[textureIndex].texture.name;
		}
		// Case when the ground is a normal mesh.
		else {
			textureName = GetMeshMaterialAtPoint(worldPos, new Ray(Vector3.zero, Vector3.zero));
		}
		// Searching for the found texture / material name in registered materials.
		foreach(var material in registeredTextures) {
			if(material.texture.name == textureName) {
				return material.surfaceIndex;
			}
		}

		return -1;
	}

    /// <summary>   Gets mesh material at point. </summary>
    ///
 
    ///
    /// <param name="worldPosition">    The world position. </param>
    /// <param name="ray">              The ray. </param>
    ///
    /// <returns>   The mesh material at point. </returns>

	string GetMeshMaterialAtPoint(Vector3 worldPosition, Ray ray) {
		if(ray.direction == Vector3.zero) {
			ray = new Ray(worldPosition + Vector3.up * 0.01f, Vector3.down);
		}

		RaycastHit hit;

		if (!Physics.Raycast (ray, out hit)) {
			return "";
		}

		Renderer r = hit.collider.GetComponent<Renderer>();
		MeshCollider mc = hit.collider as MeshCollider;

		if (r == null || r.sharedMaterial == null || r.sharedMaterial.mainTexture == null || r == null) {
			return "";
		}
		else if(!mc || mc.convex) {
			return r.material.mainTexture.name;
		}

		int materialIndex = -1;
		Mesh m = mc.sharedMesh;
		int triangleIdx = hit.triangleIndex;
		int lookupIdx1 = m.triangles[triangleIdx * 3];
		int lookupIdx2 = m.triangles[triangleIdx * 3 + 1];
		int lookupIdx3 = m.triangles[triangleIdx * 3 + 2];
		int subMeshesNr = m.subMeshCount;

		for(int i = 0;i < subMeshesNr;i ++) {
			int[] tr = m.GetTriangles(i);

			for(int j = 0;j < tr.Length;j += 3) {
				if (tr[j] == lookupIdx1 && tr[j+1] == lookupIdx2 && tr[j+2] == lookupIdx3) {
					materialIndex = i;

					break;
				}
			}

			if (materialIndex != -1) break;
		}

		string textureName = r.materials[materialIndex].mainTexture.name;

		return textureName;
	}

    /// <summary>   Gets terrain texture mix. </summary>
    ///
 
    ///
    /// <param name="worldPos">     The world position. </param>
    /// <param name="terrainData">  Information describing the terrain. </param>
    /// <param name="terrainPos">   The terrain position. </param>
    ///
    /// <returns>   An array of float. </returns>

	float[] GetTerrainTextureMix(Vector3 worldPos, TerrainData terrainData, Vector3 terrainPos) {
		// returns an array containing the relative mix of textures
		// on the main terrain at this world position.

		// The number of values in the array will equal the number
		// of textures added to the terrain.

		// calculate which splat map cell the worldPos falls within (ignoring y)
		int mapX = (int)(((worldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
		int mapZ = (int)(((worldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);

		// get the splat data for this cell as a 1x1xN 3d array (where N = number of textures)
		float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

		// extract the 3D array data to a 1D array:
		float[] cellMix = new float[splatmapData.GetUpperBound(2) + 1];

		for(int n = 0;n < cellMix.Length;n++) {
			cellMix[n] = splatmapData[0, 0, n];
		}

		return cellMix;
	}

    /// <summary>   Gets texture index. </summary>
    ///
 
    ///
    /// <param name="worldPos">     The world position. </param>
    /// <param name="textureMix">   The texture mix. </param>
    ///
    /// <returns>   The texture index. </returns>

	int GetTextureIndex(Vector3 worldPos, float[] textureMix) {
		// returns the zero-based index of the most dominant texture
		// on the terrain at this world position.
		float maxMix = 0;
		int maxIndex = 0;

		// loop through each mix value and find the maximum
		for(int n = 0;n < textureMix.Length;n ++){
			if (textureMix[n] > maxMix){
				maxIndex = n;
				maxMix = textureMix[n];
			}
		}

		return maxIndex;
	}
}


