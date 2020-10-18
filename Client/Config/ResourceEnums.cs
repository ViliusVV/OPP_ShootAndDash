using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Config
{
    public enum SoundIdentifier
    {
        [ResourceAttr("Assets/soundGenericGun.ogg")] GenericGun,
        [ResourceAttr("Assets/soundSniper.ogg")] Sniper,
        [ResourceAttr("Assets/soundReload.ogg")] Reload
    }

    public enum TextureIdentifier
    {
        // Tiles
        [ResourceAttr("Assets/tileGrass.png")] Background,
        // Characters
        [ResourceAttr("Assets/Char4Spritesheet.png")] MainCharacter,
        // UI
        [ResourceAttr("Assets/cursor.png")] AimCursor,
        //[ResourceAttr("Assets/ScoreboardBox.png")] ScoreboardBox,
        // Projectiles
        [ResourceAttr("Assets/bullet.png")] Bullet,
        // Guns
        [ResourceAttr("Assets/gunAk47.png")] GunAk47,
        [ResourceAttr("Assets/Minigun.png")] Minigun,
        [ResourceAttr("Assets/Pistol.png")] Pistol,
        [ResourceAttr("Assets/Shotgun.png")] Shotgun,
        [ResourceAttr("Assets/SniperRifle.png")] SniperRifle,
        // HUD
        [ResourceAttr("Assets/hudPlayerBar.png")] PlayerBar,
        [ResourceAttr("Assets/hudPlayerBarMask.png")] PlayerBarMask,
        [ResourceAttr("Assets/hudPlayerBarAmmoMask.png")] PlayerBarAmmoMask,
        // Destructible Objects
        [ResourceAttr("Assets/ExplosiveBarrel.png")] ExplosiveBarrel,
        [ResourceAttr("Assets/Box.png")] Crate,
        // Indestructible Objects
        [ResourceAttr("Assets/BushLong.png")] Bush,
        [ResourceAttr("Assets/Wall.png")] Wall,
        [ResourceAttr("Assets/BarbWire.png")] BarbWire,
        // Powerups
        [ResourceAttr("Assets/Medkit.png")] Medkit,
        [ResourceAttr("Assets/energyDrink.png")] MovementSyringe,
        [ResourceAttr("Assets/syringeReload.png")] ReloadSyringe,
        [ResourceAttr("Assets/syringeHealing.png")] HealingSyringe,
        [ResourceAttr("Assets/syringeDeflection.png")] DeflectionSyringe,
        // Tilemaps
        [ResourceAttr("Assets/tilemap.png")] TileMap
    }

    public enum FontIdentifier
    {
        [ResourceAttr("Assets/pixelFontSmall.ttf")] PixelatedSmall,
    }
}
