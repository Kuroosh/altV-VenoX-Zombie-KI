using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AltV.Net;
using AltV.Net.Async;
using VenoXV._RootCore_.Models;
using VenoXV.Core;
using VenoXV.Models;
using VenoXV.Zombie.KI;
using VenoXV.Zombie.Models;

namespace VenoXV.Zombie.Globals
{
    public class Events : IScript
    {
        public static List<VehicleModel> ZombieVehicles = new List<VehicleModel>();
        private static List<AltV.Net.Enums.VehicleModel> _randomVehicleList = new List<AltV.Net.Enums.VehicleModel>
        {
            AltV.Net.Enums.VehicleModel.Adder,
            AltV.Net.Enums.VehicleModel.Voltic2,
            AltV.Net.Enums.VehicleModel.Hakuchou,
            AltV.Net.Enums.VehicleModel.F620,
            AltV.Net.Enums.VehicleModel.Sultan,
            AltV.Net.Enums.VehicleModel.Faction,
            AltV.Net.Enums.VehicleModel.Raiden,
            AltV.Net.Enums.VehicleModel.Elegy,
        };


        //public static List<int> KilledZombieIds = new List<int>();
        [AsyncClientEvent("Zombies:OnZombieDeath")]
        public static void OnZombieDeath(VnXPlayer player = null, int id = 0, float zombiePosX = 0, float zombiePosY = 0, float zombiePosZ = 0, float zombieRotX = 0, float zombieRotY = 0, float zombieRotZ = 0)
        {
            try
            {
                ZombieModel zombie = Spawner.CurrentZombies.FirstOrDefault(z => z.Id == id);
                if (zombie != null) zombie.IsDead = true;
                if (player is null || !player.Exists) return;
                lock (player)
                {
                    if (zombie != null) zombie.Killer = player;
                    player.Zombies.ZombieKills += 1;
                    if (LevelSystem.LevelWeapons.ContainsKey(player.Zombies.ZombieKills))
                    {
                        player.SendTranslatedChatMessage("Neue Waffen freigeschaltet! [" + RageApi.GetHexColorcode(0, 200, 255) + player.Zombies.ZombieKills + RageApi.GetHexColorcode(255, 255, 255) + " - " + LevelSystem.LevelWeapons[player.Zombies.ZombieKills] + "]");
                        LevelSystem.GivePlayerWeaponsByLevel(player);
                    }
                    CreateVehicleSpawnChance(player, new Vector3(zombiePosX, zombiePosY, zombiePosZ), new Vector3(zombieRotX, zombieRotY, zombieRotZ));
                }
            }
            catch (Exception ex) { Debug.CatchExceptions(ex); }
        }

        public static void CreateVehicleSpawnChance(VnXPlayer player, Vector3 position, Vector3 rotation)
        {
            try
            {
                Random random = new Random();
                int randomnumb = random.Next(0, 20);
                Debug.OutputDebugString("Chance  : " + randomnumb);
                if (randomnumb == 10)
                {
                    Debug.OutputDebugString("Position  : " + position);
                    Debug.OutputDebugString("Rotation  : " + rotation);
                    Random random1 = new Random();
                    int randomVehIndex = random1.Next(0, _randomVehicleList.Count);
                    VehicleModel veh = (VehicleModel)Alt.CreateVehicle(_randomVehicleList[randomVehIndex], position, rotation);
                    veh.Dimension = _Globals_.Main.ZombiesDimension;
                    veh.NotSave = true;
                    veh.EngineOn = true;
                    veh.Owner = player.Username;
                    veh.Kms = 0;
                    veh.Gas = 1000f;
                    ZombieVehicles.Add(veh);
                }
            }
            catch (Exception ex) { Debug.CatchExceptions(ex); }
        }
        public static void OnPlayerDisconnect(VnXPlayer player)
        {
            try
            {
                player.Zombies.IsSyncer = false;
                if (_Globals_.Main.ZombiePlayers.Count <= 0)
                {
                    foreach (VehicleModel vehicleClass in ZombieVehicles.ToList())
                        RageApi.DeleteVehicleThreadSafe(vehicleClass);
                    ZombieVehicles.Clear();
                }
            }
            catch (Exception ex) { Debug.CatchExceptions(ex); }
        }

        [VenoXRemoteEvent("Zombies:OnSyncerCall")]
        public static void OnZombiesSyncerCall(VnXPlayer player = null, int zombieId = 0, float zombiePosX = 0, float zombiePosY = 0, float zombiePosZ = 0, float zombieRotX = 0, float zombieRotY = 0, float zombieRotZ = 0)
        {
            try
            {
                ZombieModel zombie = Spawner.CurrentZombies.FirstOrDefault(z => z.Id == zombieId);
                if (zombie != null) zombie.UpdatePositionAndRotation(new Vector3(zombiePosX, zombiePosY, zombiePosZ), new Vector3(zombieRotX, zombieRotY, zombieRotZ));
                World.Main.SyncZombieTargeting();
            }
            catch (Exception ex) { Debug.CatchExceptions(ex); }
        }
    }
}
