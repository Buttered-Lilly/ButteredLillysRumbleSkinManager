using MelonLoader;
using UnityEngine;
using System;
using Skins;
using RUMBLE.Managers;
using RUMBLE.Players;
using RUMBLE.Social.Phone;
using Unity.Mathematics;
using HarmonyLib;

namespace LillysSkinManager
{
    public class _SkinManager : MelonMod
    {
        public byte[] Bytes;
        public Texture2D[] texturesLocal = new Texture2D[6];

        public static PlayerManager lilsTexPlayerMan;
        //public Texture2D playerMain, playerNormal, playerMat;
        //SkinnedMeshRenderer suit;

        Player_Texturing playerTex = new Player_Texturing();
        Structure_Texturing strucTex = new Structure_Texturing();

        public override void OnLateInitializeMelon()
        {
            base.OnLateInitializeMelon();
            //lilsTexPlayerMan = GameObject.Find("Game Instance/Initializable/PlayerManager").GetComponent<PlayerManager>();
            playerTex.init();
            strucTex.init();
        }

        [HarmonyPatch(typeof(PlayerManager), "Initialize")]
        public static class playerManInit
        {
            private static void Postfix(ref PlayerManager __instance)
            {
                lilsTexPlayerMan = __instance;
            }
        }
    }
}