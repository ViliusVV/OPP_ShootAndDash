using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Config
{
    public enum SoundResource
    {
        [ResourceAttr("Assets/soundGenericGun.ogg")] GenericGun,
        [ResourceAttr("Assets/soundSniper.ogg")] Sniper
    }

    public enum TextureIdentifier
    {
        // Tiles
        [ResourceAttr("Assets/tileGrass.png")] Background,
        // Characters
        [ResourceAttr("Assets/char.png")] MainCharacter,
        // UI
        [ResourceAttr("Assets/cursor.png")] AimCursor,
        // Projectiles
        [ResourceAttr("Assets/bullet.png")] Bullet,
        // Guns
        [ResourceAttr("Assets/gunAk47.png")] GunAk47,
        // HUD
        [ResourceAttr("Assets/hudPlayerBar.png")] PlayerBar
    }

    public enum FontResource
    {
        [ResourceAttr("Assets/pixelFontSmall.ogg")] PixelatedSmall,
    }
}
