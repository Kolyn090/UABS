using UnityEngine;
using UABS.Assets.Script.Reader;
using UABS.Assets.Script.View;
using System.Collections.Generic;

namespace UABS.Assets.Script.Controller
{
    public class TextureDisplayForBundle : MonoBehaviour
    {
        // private const string TestBundlePath = @"\\?\C:\Program Files (x86)\Steam\steamapps\common\Otherworld Legends\Otherworld Legends_Data\StreamingAssets\aa\StandaloneWindows64\graphiceffecttextureseparatelygroup_assets_assets\sprites\uniteffect_0.spriteatlas_66b2db9fb94b5bda5b7794c6ed82cf3f.bundle";
        private const string TestBundlePath = @"\\?\C:\Program Files (x86)\Steam\steamapps\common\Otherworld Legends\Otherworld Legends_Data\StreamingAssets\aa\StandaloneWindows64\graphiceffecttextureseparatelygroup_assets_assets\sprites\unit\unit_other_pile.psd_0678876b821c494df01ee1384bec84f2.bundle";

        private const string TestBundlePath2 = @"\\?\C:\Program Files (x86)\Steam\steamapps\common\Otherworld Legends\Otherworld Legends_Data\StreamingAssets\aa\StandaloneWindows64\spriteassetgroup_assets_assets\needdynamicloadresources\spritereference\unit_hero_quanhuying_bartender.asset_d6edc88258e9f90ae565393468c0fc94.bundle";

        [SerializeField]
        private TextureView _textureView;

        private void Start()
        {
            // _textureView.AssignTextureToImage(ReadTexturesFromBundle.ReadTextures(TestBundlePath)[0]);
            List<Texture2D> textures = ReadTexturesFromBundle.ReadSpritesInAtlas(TestBundlePath2);
            _textureView.AssignTextures(textures);
        }
    }
}