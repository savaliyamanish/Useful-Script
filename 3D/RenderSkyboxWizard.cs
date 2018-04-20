#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
namespace MS
{
	public class RenderSkyboxWizard : ScriptableWizard
		{
		public enum SkyboxSize
		{
			Size_64=64,
			Size_128=128,
			Size_256=256,
			Size_512=512,
			Size_1024=1024,
			Size_2048=2048
		}
		public Camera renderCamera;
			public TextureFormat textureFormat=TextureFormat.RGB24;
		public SkyboxSize size=SkyboxSize.Size_512;
		public bool createMaterial=true;

		void OnWizardUpdate()
		{
			string helpString = "Select Camera to render from";
			bool isValid = (renderCamera != null);
		}

		void OnWizardCreate()
		{
			
			
				Cubemap cubemap=new Cubemap((int)size,textureFormat,false);
				renderCamera.RenderToCubemap(cubemap);

				if (!AssetDatabase.IsValidFolder ("Assets/MS_Skybox"))
					AssetDatabase.CreateFolder ("Assets", "MS_Skybox");
					string path = "Assets/MS_Skybox";
		
				Texture2D NegativeX = new Texture2D (cubemap.width, cubemap.height);
				NegativeX.SetPixels (cubemap.GetPixels (CubemapFace.NegativeX));	
				File.WriteAllBytes(path+"/-X.png",FlipTexture(NegativeX,true).EncodeToPNG ());

				Texture2D PositiveX = new Texture2D (cubemap.width, cubemap.height);
				PositiveX.SetPixels (cubemap.GetPixels (CubemapFace.PositiveX));
				File.WriteAllBytes(path+"/+X.png",FlipTexture(PositiveX,true).EncodeToPNG ());

				Texture2D NegativeY = new Texture2D (cubemap.width, cubemap.height);
				NegativeY.SetPixels (cubemap.GetPixels (CubemapFace.NegativeY));
				File.WriteAllBytes(path+"/-Y.png",FlipTexture(NegativeY,true).EncodeToPNG ());

				Texture2D PositiveY = new Texture2D (cubemap.width, cubemap.height);
				PositiveY.SetPixels (cubemap.GetPixels (CubemapFace.PositiveY));
				File.WriteAllBytes(path+"/+Y.png",FlipTexture(PositiveY,true).EncodeToPNG ());

				Texture2D NegativeZ = new Texture2D (cubemap.width, cubemap.height);
				NegativeZ.SetPixels (cubemap.GetPixels (CubemapFace.NegativeZ));
				File.WriteAllBytes(path+"/-Z.png",FlipTexture(NegativeZ,true).EncodeToPNG ());

				Texture2D PositiveZ = new Texture2D (cubemap.width, cubemap.height);
				PositiveZ.SetPixels (cubemap.GetPixels (CubemapFace.PositiveZ));
				File.WriteAllBytes(path+"/+Z.png",FlipTexture(PositiveZ,true).EncodeToPNG ());
				AssetDatabase.Refresh ();
			

				if (createMaterial) {
									Material material = new Material (Shader.Find ("Skybox/6 Sided"));

					material.SetTexture ("_FrontTex", AssetDatabase.LoadAssetAtPath<Texture> (path+"/+Z.png"));
					material.SetTextureScale ("_FrontTex", new Vector2(1,-1));

					material.SetTexture ("_BackTex", AssetDatabase.LoadAssetAtPath<Texture> (path+"/-Z.png"));
					material.SetTextureScale ("_BackTex", new Vector2(1,-1));

					material.SetTexture ("_LeftTex", AssetDatabase.LoadAssetAtPath<Texture> (path+"/+X.png"));
					material.SetTextureScale ("_LeftTex", new Vector2(1,-1));

					material.SetTexture ("_RightTex", AssetDatabase.LoadAssetAtPath<Texture> (path+"/-X.png"));
					material.SetTextureScale ("_RightTex", new Vector2(1,-1));

					material.SetTexture ("_UpTex", AssetDatabase.LoadAssetAtPath<Texture> (path+"/+Y.png"));
					material.SetTexture ("_DownTex", AssetDatabase.LoadAssetAtPath<Texture> (path+"/-Y.png"));
					AssetDatabase.CreateAsset (material, "Assets/MS_Skybox/SkyboxImageMat.mat");
				}
		}
			Texture2D FlipTexture(Texture2D original, bool upSideDown = true)
			{

				Texture2D flipped = new Texture2D(original.width, original.height);

				int xN = original.width;
				int yN = original.height;


				for (int i = 0; i < xN; i++)
				{
					for (int j = 0; j < yN; j++)
					{
						if (upSideDown)
						{
							flipped.SetPixel(j, xN - i - 1, original.GetPixel(j, i));
						}
						else
						{
							flipped.SetPixel(xN - i - 1, j, original.GetPixel(i, j));
						}
					}
				}
				flipped.Apply();

				return flipped;
			}
		void OnWizardOtherButton()
		{
				Cubemap cubemap=new Cubemap((int)size,textureFormat,false);
				renderCamera.RenderToCubemap(cubemap);			
				if (!AssetDatabase.IsValidFolder ("Assets/MS_Skybox"))
					AssetDatabase.CreateFolder ("Assets", "MS_Skybox");
			
				string path = "Assets/MS_Skybox/SkyboxCube.cubemap";
				AssetDatabase.CreateAsset(cubemap,path);
				Selection.activeObject = AssetDatabase.LoadAssetAtPath<Cubemap> (path);
				if (createMaterial) {
					Material material = new Material (Shader.Find ("Skybox/Cubemap"));
					material.SetTexture ("_Tex", AssetDatabase.LoadAssetAtPath<Cubemap> (path));
					AssetDatabase.CreateAsset (material, "Assets/MS_Skybox/SkyboxCubeMat.mat");
				}
				AssetDatabase.Refresh ();

		}
		[MenuItem("GameObject/Render Skybox")]
		static void RenderCubemap()
		{
				ScriptableWizard.DisplayWizard<RenderSkyboxWizard>("Render Skybox", "Render Image!","Render CubeMap").renderCamera=Camera.main;
		}

	}
}
#endif