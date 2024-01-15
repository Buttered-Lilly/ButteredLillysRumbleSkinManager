using MelonLoader;
using UnityEngine;
using System;
using Skins;

namespace LillysSkinManager
{
    public class _SkinManager : MelonMod
    {
        public byte[] Bytes;
        public Texture2D[] textures = new Texture2D[6];
        //public Texture2D playerMain, playerNormal, playerMat;
        //SkinnedMeshRenderer suit;

        Player_Texturing playerTex = new Player_Texturing();
        Structure_Texturing strucTex = new Structure_Texturing();

        public override void OnLateInitializeMelon()
        {
            base.OnLateInitializeMelon();
            playerTex.init();
            strucTex.init();
        }
    }
}