﻿using System;
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
        [ResourceAttr("Assets/Char1Spritesheet.png")] CheerfulAssasinChar,
        [ResourceAttr("Assets/Char2Spritesheet.png")] TriggerHappyHipsterChar,
        [ResourceAttr("Assets/Char3Spritesheet.png")] HawaiianManiacChar,
        [ResourceAttr("Assets/Char4Spritesheet.png")] ProfoundAsianChar,
        // UI
        [ResourceAttr("Assets/cursor.png")] AimCursor,
        [ResourceAttr("Assets/ButtonDefault.png")] ButtonDefault,
        [ResourceAttr("Assets/ButtonSelected.png")] ButtonSelected,
        [ResourceAttr("Assets/ButtonClicked.png")] ButtonClicked,
        //[ResourceAttr("Assets/ScoreboardBox.png")] ScoreboardBox,
        // Projectiles
        [ResourceAttr("Assets/bullet.png")] Bullet,
        // Guns
        [ResourceAttr("Assets/gunAk47.png")] GunAk47,
        [ResourceAttr("Assets/Minigun.png")] Minigun,
        [ResourceAttr("Assets/Pistol.png")] Pistol,
        [ResourceAttr("Assets/Shotgun.png")] Shotgun,
        [ResourceAttr("Assets/SniperRifle.png")] SniperRifle,
        [ResourceAttr("Assets/Flamethrower.png")] Flamethrower,
        // Laser Colors
        [ResourceAttr("Assets/RedLaser.png")] RedLaser,
        [ResourceAttr("Assets/GreenLaser.png")] GreenLaser,
        // Guns with lasers
        [ResourceAttr("Assets/Pistol_RedLaser.png")] RedPistolLaser,
        [ResourceAttr("Assets/SniperRifle_RedLaser.png")] RedSniperLaser,
        [ResourceAttr("Assets/gunAk47_RedLaser.png")] RedGunAk47Laser,
        [ResourceAttr("Assets/Pistol_GreenLaser.png")] GreenPistolLaser,
        [ResourceAttr("Assets/SniperRifle_GreenLaser.png")] GreenSniperLaser,
        [ResourceAttr("Assets/gunAk47_GreenLaser.png")] GreenGunAk47Laser,
        // HUD
        [ResourceAttr("Assets/hudPlayerBar.png")] PlayerBar,
        [ResourceAttr("Assets/hudPlayerBarMask.png")] PlayerBarMask,
        [ResourceAttr("Assets/hudPlayerBarAmmoMask.png")] PlayerBarAmmoMask,
        // Destructible Objects
        [ResourceAttr("Assets/MedkitBoxBrown.png")] MedkitCrateBrown,
        [ResourceAttr("Assets/MedkitBoxYellow.png")] MedkitCrateYellow,
        [ResourceAttr("Assets/BoxBrown.png")] CrateBrown,
        [ResourceAttr("Assets/BoxYellow.png")] CrateYellow,
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
        // Traps
        [ResourceAttr("Assets/FreezingTrap.png")] FreezeTrap,
        [ResourceAttr("Assets/DamageTrap.png")] DamageTrap,
        [ResourceAttr("Assets/RemoveAmmoTrap.png")] RemoveAmmoTrap,
        // Tilemaps
        [ResourceAttr("Assets/tilemap.png")] TileMap,
        // Particles
        [ResourceAttr("Assets/Explosion.png")] Explosion,
        [ResourceAttr("Assets/Portal.png")] Portal,
        [ResourceAttr("Assets/PortalRed.png")] PortalRed
    }

    public enum FontIdentifier
    {
        [ResourceAttr("Assets/pixelFontSmall.ttf")] PixelatedSmall,
    }
}
