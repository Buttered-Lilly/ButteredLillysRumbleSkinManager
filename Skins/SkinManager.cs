using MelonLoader;
using UnityEngine;
using System;
using UnityEditor;
using static UnityEngine.Rendering.Universal.UniversalRenderPipeline.Profiling.Pipeline;

namespace LillysSkinManager
{
	public class SkinManager : MelonMod
	{
		public byte[] Bytes;
        public Texture2D[] textures = new Texture2D[6];
        public Texture2D playerMain, playerNormal, playerMat;
        SkinnedMeshRenderer suit;

        string[] types = new string[5];
        string[] poolPaths = new string[6];
        string[] texPaths = new string[6];
        bool ran = false;

        public override void OnLateInitializeMelon()
        {
            base.OnLateInitializeMelon();

            playerMain = null;
            playerNormal = null;
            playerMat = null;

            types[0] = "pillar";
            types[1] = "wall";
            types[2] = "disc";
            types[3] = "cube";
            types[4] = "ball";

            poolPaths[0] = "Game Instance/Pre-Initializable/PoolManager/Pool: Pillar (RUMBLE.MoveSystem.Structure)";
            poolPaths[1] = "Game Instance/Pre-Initializable/PoolManager/Pool: Wall (RUMBLE.MoveSystem.Structure)";
            poolPaths[2] = "Game Instance/Pre-Initializable/PoolManager/Pool: Disc (RUMBLE.MoveSystem.Structure)";
            poolPaths[3] = "Game Instance/Pre-Initializable/PoolManager/Pool: RockCube (RUMBLE.MoveSystem.Structure)";
            poolPaths[4] = "Game Instance/Pre-Initializable/PoolManager/Pool: Ball (RUMBLE.MoveSystem.Structure)";
            poolPaths[5] = "Game Instance/Pre-Initializable/PoolManager/Pool: BoulderBall (RUMBLE.MoveSystem.Structure)";

            texPaths[0] = "/Skins/Pillar/";
            texPaths[1] = "/Skins/Wall/";
            texPaths[2] = "/Skins/Disc/";
            texPaths[3] = "/Skins/Cube/";
            texPaths[4] = "/Skins/Ball/";
            texPaths[5] = "/Skins/Ball/";

            MelonLogger.Msg("Passed init");
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            suit = null;
            if (sceneName != "Loader")
            {
                try
                {
                    if (System.IO.File.Exists(MelonUtils.UserDataDirectory + "/Skins/Player/Normal.png"))
                    {
                        playerNormal = new Texture2D(2, 2);
                        Bytes = System.IO.File.ReadAllBytes(MelonUtils.UserDataDirectory + "/Skins/Player/Normal.png");
                        ImageConversion.LoadImage(playerNormal, Bytes);
                    }
                    if (System.IO.File.Exists(MelonUtils.UserDataDirectory + "/Skins/Player/Main.png"))
                    {
                        playerMain = new Texture2D(2, 2);
                        Bytes = System.IO.File.ReadAllBytes(MelonUtils.UserDataDirectory + "/Skins/Player/Main.png");
                        ImageConversion.LoadImage(playerMain, Bytes);
                    }
                    if (System.IO.File.Exists(MelonUtils.UserDataDirectory + "/Skins/Player/Mat.png"))
                    {
                        playerMat = new Texture2D(2, 2);
                        Bytes = System.IO.File.ReadAllBytes(MelonUtils.UserDataDirectory + "/Skins/Player/Mat.png");
                        ImageConversion.LoadImage(playerMat, Bytes);
                    }
                    foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll<GameObject>())
                    {
                        if (gameObject.name == "Suit")
                        {
                            suit = gameObject.GetComponent<SkinnedMeshRenderer>();
                            if (playerMain != null)
                            {
                                suit.material.SetTexture("_Color_Map", playerMain);
                            }
                            if (playerMat != null)
                            {
                                suit.material.SetTexture("_Metal_Map", playerMat);
                            }
                            if (playerNormal != null)
                            {
                                suit.material.SetTexture("_Normal_Map", playerNormal);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MelonLogger.Msg(ex.Message);
                }
            }
            if (ran)
            {
                return;
            }
            else
            {
                ran = true;
                MelonLogger.Msg("Loading Textures");
                loadTex();
            }
        }

        void loadTex()
        {
            try
            {
                for (int i = 0; i < texPaths.Length; i++)
                {
                    /*for (int n = 0; n < textures.Length; n++)
                    {
                        textures[n] = null;
                    }*/
                    GameObject pool = GameObject.Find(poolPaths[i]).gameObject;
                    Child g = pool.gameObject.AddComponent<Child>();
                    //g.tex = textures;
                    g.type = types[i];
                    g.typeInt = i;
                    g.texPath = MelonUtils.UserDataDirectory + texPaths[i];
                }
            }
            catch (Exception ex) { }
        }
    }

    [RegisterTypeInIl2Cpp]
    public class Child : MonoBehaviour
    {
        public Child(IntPtr ptr) : base(ptr) { }

        public string type;
        public int typeInt;
        public string texPath;

        private Texture2D texNormal, texMain, texMat, texGrounded;
        private byte[] Bytes;
        private int lastCount;
        private Material temp;

        void Start()
        {
            if (texNormal != null || texMain != null || texMat != null || texGrounded != null)
                return;
            temp = gameObject.transform.GetChild(lastCount).gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial;
            if (System.IO.File.Exists(texPath + "Normal.png"))
            {
                texNormal = new Texture2D(2, 2);
                Bytes = System.IO.File.ReadAllBytes(texPath + "Normal.png");
                ImageConversion.LoadImage(texNormal, Bytes);
                texNormal.hideFlags = HideFlags.HideAndDontSave;
            }
            if (System.IO.File.Exists(texPath + "Main.png"))
            {
                texMain = new Texture2D(2, 2);
                Bytes = System.IO.File.ReadAllBytes(texPath + "Main.png");
                ImageConversion.LoadImage(texMain, Bytes);
                texMain.hideFlags = HideFlags.HideAndDontSave;
            }
            if (System.IO.File.Exists(texPath + "Mat.png"))
            {
                texMat = new Texture2D(2, 2);
                Bytes = System.IO.File.ReadAllBytes(texPath + "Mat.png");
                ImageConversion.LoadImage(texMat, Bytes);
                texMat.hideFlags = HideFlags.HideAndDontSave;
            }
            if (System.IO.File.Exists(texPath + "Grounded.png"))
            {
                texGrounded = new Texture2D(2, 2);
                Bytes = System.IO.File.ReadAllBytes(texPath + "Grounded.png");
                ImageConversion.LoadImage(texGrounded, Bytes);
                texGrounded.hideFlags = HideFlags.HideAndDontSave;
            }
            MelonLogger.Msg("Passed File loading");
            Preploaded();
        }

        void Preploaded()
        {
            MeshRenderer meshRenderer;
            for (lastCount = 0; lastCount < gameObject.transform.childCount; lastCount++)
            {
                meshRenderer = gameObject.transform.GetChild(lastCount).gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
                MaterialPropertyBlock block = new MaterialPropertyBlock();
                meshRenderer.GetPropertyBlock(block);
                if (texNormal != null)
                    block.SetTexture("Texture2D_2058E65A", texNormal);
                if (texMain != null)
                    block.SetTexture("Texture2D_3812B1EC", texMain);
                if (texMat != null)
                    block.SetTexture("Texture2D_8F187FEF", texMat);
                if (texGrounded != null)
                    block.SetTexture("_GroundTexture", texGrounded);
                meshRenderer.SetPropertyBlock(block);
            }
        }

        public void OnTransformChildrenChanged()
        {
            Preploaded();
        }

        void updateTex()
        {
            MelonLogger.Msg("Child Being Updated");
            MeshRenderer meshRenderer;
            meshRenderer = gameObject.transform.GetChild(lastCount).gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            meshRenderer.GetPropertyBlock(block);
            if (texNormal != null)
                block.SetTexture("Texture2D_2058E65A", texNormal);
            if (texMain != null)
                block.SetTexture("Texture2D_3812B1EC", texMain);
            if (texMat != null)
                block.SetTexture("Texture2D_8F187FEF", texMat);
            if (texGrounded != null)
                block.SetTexture("_GroundTexture", texGrounded);
            meshRenderer.SetPropertyBlock(block);
        }
    }
}