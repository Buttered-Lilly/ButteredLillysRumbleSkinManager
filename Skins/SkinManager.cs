using MelonLoader;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using RUMBLE.Managers;
using RUMBLE.Players;

namespace LillysSkinManager
{
	public class SkinManager : MelonMod
	{
		public byte[] Bytes;
        public Texture2D[] textures = new Texture2D[6];
        //public Texture2D playerMain, playerNormal, playerMat;
        //SkinnedMeshRenderer suit;

        string[] types = new string[5];
        string[] poolPaths = new string[5];
        string[] texPaths = new string[5];
        bool ran = false;
        PlayerTexturing playertexturing;

        public override void OnLateInitializeMelon()
        {
            base.OnLateInitializeMelon();

            //playerMain = null;
            //playerNormal = null;
            //playerMat = null;

            types[0] = "Pillar";
            types[1] = "Wall";
            types[2] = "Disc";
            types[3] = "Cube";
            types[4] = "Ball";

            poolPaths[0] = "Game Instance/Pre-Initializable/PoolManager/Pool: Pillar (RUMBLE.MoveSystem.Structure)";
            poolPaths[1] = "Game Instance/Pre-Initializable/PoolManager/Pool: Wall (RUMBLE.MoveSystem.Structure)";
            poolPaths[2] = "Game Instance/Pre-Initializable/PoolManager/Pool: Disc (RUMBLE.MoveSystem.Structure)";
            poolPaths[3] = "Game Instance/Pre-Initializable/PoolManager/Pool: RockCube (RUMBLE.MoveSystem.Structure)";
            poolPaths[4] = "Game Instance/Pre-Initializable/PoolManager/Pool: Ball (RUMBLE.MoveSystem.Structure)";

            texPaths[0] = "/Skins/Pillar/";
            texPaths[1] = "/Skins/Wall/";
            texPaths[2] = "/Skins/Disc/";
            texPaths[3] = "/Skins/Cube/";
            texPaths[4] = "/Skins/Ball/";

            MelonLogger.Msg("Passed init");
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            /*suit = null;
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
            }*/
            if (ran)
            {
                playertexturing.OnSceneLoaded(sceneName);
                return;
            }
            else
            {
                ran = true;
                MelonLogger.Msg("Loading Textures");
                ApplyStructureScriptTex();
                ApplyPlayerScriptTex();
            }
        }

        void ApplyPlayerScriptTex()
        {
            GameObject playerManager = GameObject.Find("Game Instance/Initializable/PlayerManager");
            playertexturing = playerManager.AddComponent<PlayerTexturing>();
        }

        void ApplyStructureScriptTex()
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
                    StructureTextureing g = pool.gameObject.AddComponent<StructureTextureing>();
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
    public class StructureTextureing : MonoBehaviour
    {
        public StructureTextureing(IntPtr ptr) : base(ptr) { }

        public string type;
        public int typeInt;
        public string texPath;

        private Texture2D texNormal, texMain, texMat, texGrounded;
        private byte[] Bytes;
        private int lastCount;

        void Start()
        {
            if (texNormal != null || texMain != null || texMat != null || texGrounded != null)
                return;
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


        System.Collections.IEnumerator delayTex()
        {
            MeshRenderer meshRenderer;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            meshRenderer = gameObject.transform.GetChild(lastCount).gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
            updateTex(meshRenderer);
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
            MelonCoroutines.Start(delayTex());
            //Preploaded();
        }

        void updateTex(MeshRenderer meshRenderer)
        {
            MelonLogger.Msg("Child Being Updated");
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
            lastCount++;
        }
    }

    [RegisterTypeInIl2Cpp]
    public class PlayerTexturing : MonoBehaviour
    {
        public PlayerTexturing(IntPtr ptr) : base(ptr) { }

        private RUMBLE.Managers.PlayerManager playerManager;
        private byte[] Bytes;

        private const int texCount = 9;
        private Texture2D[] playerTextures = new Texture2D[texCount];
        private string[] filetypes = new string[texCount];
        private string[] texProps = new string[texCount];
        private string[] objectPaths = new string[texCount];

        void Start()
        {
            filetypes[0] = "Suit/Main";
            filetypes[1] = "Suit/Mat";
            filetypes[2] = "Suit/Normal";
            filetypes[3] = "Shiftsocket/Main_R";
            filetypes[4] = "Shiftsocket/Mat_R";
            filetypes[5] = "Shiftsocket/Normal_R";
            filetypes[6] = "Shiftsocket/Main_L";
            filetypes[7] = "Shiftsocket/Mat_L";
            filetypes[8] = "Shiftsocket/Normal_L";

            texProps[0] = "_Color_Map";
            texProps[1] = "_Metal_Map";
            texProps[2] = "_Normal_Map";
            texProps[3] = "_Color_Map";
            texProps[4] = "_Metal_Map";
            texProps[5] = "_Normal_Map";
            texProps[6] = "_Color_Map";
            texProps[7] = "_Metal_Map";
            texProps[8] = "_Normal_Map";

            objectPaths[0] = "Visuals/Suit";
            objectPaths[1] = "Visuals/Suit";
            objectPaths[2] = "Visuals/Suit";
            objectPaths[3] = "Visuals/RIG/Bone_Pelvis/Bone_Spine/Bone_Chest/Bone_Shoulderblade_R/Bone_Shoulder_R/Bone_Lowerarm_R/Bone_Hand_R/ShiftstoneSocket_R/Shiftsocket";
            objectPaths[4] = "Visuals/RIG/Bone_Pelvis/Bone_Spine/Bone_Chest/Bone_Shoulderblade_R/Bone_Shoulder_R/Bone_Lowerarm_R/Bone_Hand_R/ShiftstoneSocket_R/Shiftsocket";
            objectPaths[5] = "Visuals/RIG/Bone_Pelvis/Bone_Spine/Bone_Chest/Bone_Shoulderblade_R/Bone_Shoulder_R/Bone_Lowerarm_R/Bone_Hand_R/ShiftstoneSocket_R/Shiftsocket";
            objectPaths[6] = "Visuals/RIG/Bone_Pelvis/Bone_Spine/Bone_Chest/Bone_Shoulderblade_L/Bone_Shoulder_L/Bone_Lowerarm_L/Bone_Hand_L/ShiftstoneSocket_L/Shiftsocket";
            objectPaths[7] = "Visuals/RIG/Bone_Pelvis/Bone_Spine/Bone_Chest/Bone_Shoulderblade_L/Bone_Shoulder_L/Bone_Lowerarm_L/Bone_Hand_L/ShiftstoneSocket_L/Shiftsocket";
            objectPaths[8] = "Visuals/RIG/Bone_Pelvis/Bone_Spine/Bone_Chest/Bone_Shoulderblade_L/Bone_Shoulder_L/Bone_Lowerarm_L/Bone_Hand_L/ShiftstoneSocket_L/Shiftsocket";

            for (int i = 0; i < playerTextures.Length; i++)
            {
                if (System.IO.File.Exists(MelonUtils.UserDataDirectory + "/Skins/Player/" + filetypes[i] + ".png"))
                {
                    playerTextures[i] = new Texture2D(2, 2);
                    Bytes = System.IO.File.ReadAllBytes(MelonUtils.UserDataDirectory + "/Skins/Player/" + filetypes[i] + ".png");
                    ImageConversion.LoadImage(playerTextures[i], Bytes);
                    playerTextures[i].hideFlags = HideFlags.HideAndDontSave;
                }
                else
                {
                    playerTextures[i] = null;
                }
            }
            playerManager = this.gameObject.GetComponent<RUMBLE.Managers.PlayerManager>();
        }

        void updateTex(GameObject localPlayer)
        {
            try
            {
                MelonLogger.Msg("trying to skin player");
                MaterialPropertyBlock block = new MaterialPropertyBlock();
                Renderer skinned = localPlayer.transform.Find(objectPaths[0]).GetComponent<Renderer>();
                string last = objectPaths[0];
                skinned.GetPropertyBlock(block);
                for (int i = 0; i < playerTextures.Length; i++)
                {
                    if (objectPaths[i] != last)
                    {
                        skinned.SetPropertyBlock(block);
                        skinned = localPlayer.transform.Find(objectPaths[i]).GetComponent<Renderer>();
                        skinned.GetPropertyBlock(block);
                    }
                    if (playerTextures[i] != null)
                    {
                        block.SetTexture(texProps[i], playerTextures[i]);
                    }
                    last = objectPaths[i];
                }
                skinned.SetPropertyBlock(block);
            }
            catch (Exception e)
            {
                MelonLogger.Msg(e.ToString());
                MelonCoroutines.Start(delayTex());
            }
        }

        System.Collections.IEnumerator delayTex()
        {
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            try
            {
                GameObject player = null;
                player = playerManager.LocalPlayer.Controller.gameObject;
                updateTex(player);
            }
            catch (Exception e)
            {
                MelonLogger.Msg(e.ToString());
                MelonCoroutines.Start(delayTex());
            }
        }

        public void OnSceneLoaded(string sceneName)
        {
            if(sceneName != "Loader")
            {
                try
                {
                    MelonLogger.Msg("trying to find local player");
                    MelonCoroutines.Start(delayTex());
                }
                catch(Exception e)
                {
                    MelonLogger.Msg(e.ToString());
                }
            }
        }
    }
}