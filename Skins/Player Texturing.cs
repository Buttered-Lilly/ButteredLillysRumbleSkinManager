using MelonLoader;
using System;
using UnityEngine;
using HarmonyLib;
using RUMBLE.Players;
using RUMBLE.Managers;

namespace Skins
{
    internal class Player_Texturing
    {
        private byte[] Bytes;

        private const int texCount = 9;
        private Texture2D[] playerTextures = new Texture2D[texCount];
        private string[] filetypes = new string[texCount];
        private string[] texProps = new string[texCount];
        private string[] objectPaths = new string[texCount];
        public static Player_Texturing PlayerTex;

        public void init()
        {
            PlayerTex = this;
            MelonLogger.Msg(PlayerTex.texProps[1]);
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
            MelonLogger.Msg("Player Passed File loading");
        }

        public void updateTex(GameObject player)
        {
            try
            {
                //GameObject player = LRL.playerManager.LocalPlayer.Controller.gameObject;
                MelonLogger.Msg("trying to skin player");
                MaterialPropertyBlock block = new MaterialPropertyBlock();
                Renderer skinned = player.transform.Find(objectPaths[0]).GetComponent<Renderer>();
                string last = objectPaths[0];
                skinned.GetPropertyBlock(block);
                for (int i = 0; i < playerTextures.Length; i++)
                {
                    if (objectPaths[i] != last)
                    {
                        skinned.SetPropertyBlock(block);
                        skinned = player.transform.Find(objectPaths[i]).GetComponent<Renderer>();
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
            }
        }


        //PlayerController.Initialize(RUMBLE.Players.Player)

        [HarmonyPatch(typeof(PlayerManager), "SpawnPlayerController", new Type[] { typeof(Player), typeof(SpawnPointHandler.SpawnPointType) })]
        public static class playerspawn
        {
            private static void Postfix(ref Player player, ref SpawnPointHandler.SpawnPointType spawnPoint)
            {
                PlayerTex.updateTex(player.Controller.gameObject);
            }
        }
    }
}
