using System;
using System.Collections.Generic;
using System.Linq;
using AltV.Net.Enums;
using VenoXV.Models;

namespace VenoXV.Zombie.KI
{
    public class LevelSystem
    {
        public static Dictionary<int, WeaponModel> LevelWeapons = new Dictionary<int, WeaponModel>
        {
            { 0, WeaponModel.Switchblade },
            { 5, WeaponModel.Snowballs },
            { 10, WeaponModel.Pistol },
            { 20, WeaponModel.APPistol },
            { 30, WeaponModel.HeavyRevolver },
            { 50, WeaponModel.MicroSMG },
            { 75,  WeaponModel.CombatPDW },
            { 100,  WeaponModel.DoubleActionRevolver },
            { 120, WeaponModel.PumpShotgun },
            { 200, WeaponModel.BullpupRifle },
            { 250, WeaponModel.AssaultRifle },
            { 300, WeaponModel.CombatMG },
            { 350, WeaponModel.HeavySniper },
            { 400, WeaponModel.FireworkLauncher },
            { 500, WeaponModel.GrenadeLauncher },
            { 525, WeaponModel.MolotovCocktail },
            { 550, WeaponModel.Grenade },
            { 800, WeaponModel.GrenadeLauncher },
            { 1000, WeaponModel.RPG },
            { 1500, WeaponModel.Railgun },
            { 2000, WeaponModel.Minigun },
            { 20000, WeaponModel.Widowmaker },
        };
        public static void GivePlayerWeaponsByLevel(VnXPlayer player)
        {
            try
            {
                if (player is null) return;
                foreach (var weapon in LevelWeapons.Where(weapon => weapon.Key <= player.Zombies.ZombieKills))
                    player.GivePlayerWeapon(weapon.Value, 9999);
            }
            catch(Exception ex){Core.Debug.CatchExceptions(ex);}
        }
    }
}
