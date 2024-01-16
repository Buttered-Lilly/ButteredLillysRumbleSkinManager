using MelonLoader;
using System;
using HarmonyLib;
using UnityEngine;
using RUMBLE.Managers;
using RUMBLE.Players;
using RUMBLE.MoveSystem;
using RUMBLE.Players.Subsystems;
using LillysSkinManager;
using Photon.Pun;
using System.Management.Instrumentation;

namespace Skins
{
    internal class Structure_Texturing
    {
        public static string[] types = new string[5];
        string[] texTypes = new string[4];
        string[] texPaths = new string[5];
        Texture2D[][] texturesLocal = new Texture2D[5][];
        Texture2D[][] texturesRemote = new Texture2D[5][];
        private byte[] Bytes;
        public static Structure_Texturing Struc;

        public void init()
        {
            Struc = this;
            types[0] = "Pillar";
            types[1] = "Wall";
            types[2] = "Disc";
            types[3] = "RockCube";
            types[4] = "Ball";

            texPaths[0] = "/Skins/Pillar/";
            texPaths[1] = "/Skins/Wall/";
            texPaths[2] = "/Skins/Disc/";
            texPaths[3] = "/Skins/Cube/";
            texPaths[4] = "/Skins/Ball/";

            texTypes[0] = "Normal";
            texTypes[1] = "Main";
            texTypes[2] = "Mat";
            texTypes[3] = "Grounded";

            for (int i = 0; i < texturesLocal.Length; i++)
            {
                texturesLocal[i] = new Texture2D[4];
                texturesRemote[i] = new Texture2D[4];

                for (int x = 0; x < texTypes.Length; x++)
                {
                    if (System.IO.File.Exists(MelonUtils.UserDataDirectory + texPaths[i] + texTypes[x] + ".png"))
                    {
                        texturesLocal[i][x] = new Texture2D(2, 2);
                        Bytes = System.IO.File.ReadAllBytes(MelonUtils.UserDataDirectory + texPaths[i] + texTypes[x] + ".png");
                        ImageConversion.LoadImage(texturesLocal[i][x], Bytes);
                        texturesLocal[i][x].hideFlags = HideFlags.HideAndDontSave;
                    }
                    else
                    {
                        texturesLocal[i][x] = null;
                    }
                    /*if (System.IO.File.Exists(MelonUtils.UserDataDirectory + texPaths[i] + texTypes[x] + "_Alt.png"))
                    {
                        texturesRemote[i][x] = new Texture2D(2, 2);
                        Bytes = System.IO.File.ReadAllBytes(MelonUtils.UserDataDirectory + texPaths[i] + texTypes[x] + "_Alt.png");
                        ImageConversion.LoadImage(texturesRemote[i][x], Bytes);
                        texturesRemote[i][x].hideFlags = HideFlags.HideAndDontSave;
                    }
                    else
                    {
                        texturesRemote[i][x] = texturesLocal[i][x];
                        if (texturesRemote[i][x] != null)
                            texturesRemote[i][x].hideFlags = HideFlags.HideAndDontSave;
                    }*/
                }
            }
            MelonLogger.Msg("Structures Passed File loading");
        }

        public void texture(GameObject obj, int type, bool isLocal)
        {
            Texture2D[][] textures;
            if (isLocal)
                textures = texturesLocal;
            else
                textures = texturesRemote;
            MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            meshRenderer.GetPropertyBlock(block);
            if (textures[type][0] != null)
                block.SetTexture("Texture2D_2058E65A", textures[type][0]);
            if (textures[type][1] != null)
                block.SetTexture("Texture2D_3812B1EC", textures[type][1]);
            if (textures[type][2] != null)
                block.SetTexture("Texture2D_8F187FEF", textures[type][2]);
            if (textures[type][3] != null)
                block.SetTexture("_GroundTexture", textures[type][3]);
            meshRenderer.SetPropertyBlock(block);
        }

        [HarmonyPatch(typeof(Structure), "Start")]
        public static class strutuerespawn
        {
            private static void Postfix(ref Structure __instance)
            {
                //MelonLogger.Msg("stuc spawned");
                int index;
                GameObject obj;
                bool isLocal = true;
                try
                {
                    //MelonLogger.Msg("checking if on list");
                    index = Array.IndexOf(types, __instance.processableComponent.gameObject.name);
                    if (index < 0 || __instance.isSceneStructure)
                    {
                        //MelonLogger.Msg("not on list");
                        return;
                    }
                    //MelonLogger.Msg("getting object");
                    obj = __instance.processableComponent.gameObject.transform.GetChild(0).gameObject;
                    //PlayerControllerSubsystem.
                    //__instance.processableComponent.latestInfluencedProcessor;
                    //MelonLogger.Msg("getting is local");
                    //MelonLogger.Msg("is mine: " + __instance.processableComponent.gameObject.GetComponent<PhotonView>().IsMine);
                    //isLocal = __instance.processableComponent.gameObject.GetComponent<PhotonView>().IsMine;
                    //isLocal = true;
                    //isLocal = __instance.processableComponent.gameObject.GetComponent<>
                }
                catch (Exception e)
                {
                    MelonLogger.Msg(e);
                    return;
                }
                //MelonLogger.Msg(isLocal);
                Struc.texture(obj, index, isLocal);
            }
        }
    }
}
