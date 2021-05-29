using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AltV.Net;
using VenoXV._Preload_.Character_Creator;
using VenoXV.Core;
using VenoXV.Models;
using VenoXV.Zombie.Models;

namespace VenoXV.Zombie.KI
{
    public class Spawner : IScript
    {
        public static List<ZombieModel> CurrentZombies = new List<ZombieModel>();
        private static int _currentZombieCounter;
        private static int _positionCounter;
        private static int _distZombies = 12;
        private static int _maxZombies = 15;

        //
        private static void CreateNewRandomZombie(VnXPlayer player)
        {
            try
            {
                Vector3 position = new Vector3();
                switch (_positionCounter)
                {
                    case 0:
                        position = new Vector3(player.Position.X + _distZombies, player.Position.Y + _distZombies, player.Position.Z - 0.5f);
                        break;
                    case 1:
                        position = new Vector3(player.Position.X - _distZombies, player.Position.Y + _distZombies, player.Position.Z - 0.5f);
                        break;
                    case 2:
                        position = new Vector3(player.Position.X + _distZombies, player.Position.Y - _distZombies, player.Position.Z - 0.5f);
                        break;
                    case 3:
                        position = new Vector3(player.Position.X + _distZombies, player.Position.Y - _distZombies, player.Position.Z - 0.5f);
                        _positionCounter = 0;
                        break;
                }
                Random random = new Random();
                int randomSkin = random.Next(0, Main.CharacterSkins.ToList().Count);
                _positionCounter++;
                ZombieModel zombieClass = new ZombieModel
                {
                    Id = _currentZombieCounter++,
                    SkinName = "mp_m_freemode_01",
                    RandomSkinUid = Main.CharacterSkins.ToList()[randomSkin].Uid,
                    Sex = 0,
                    Armor = 100,
                    Health = 100,
                    IsDead = false,
                    Position = position,
                    TargetEntity = player
                };
                player.Zombies.NearbyZombies.Add(zombieClass);
                CurrentZombies.Add(zombieClass);
            }
            catch (Exception ex) { Debug.CatchExceptions(ex); }
        }

        private static void AddNearbyZombiesIntoList(VnXPlayer player)
        {
            try
            {
                if (!player.Zombies.IsSyncer || player.Zombies.NearbyZombies.Count >= _maxZombies) return;
                CreateNewRandomZombie(player);
                foreach (VnXPlayer nearbyPlayer in player.NearbyPlayers.ToList()) CreateNewRandomZombie(nearbyPlayer);
                //else if (player.Zombies.IsSyncer)
                //Core.Debug.OutputDebugString("[Zombies] : " + player.Username + " hat das Limit von " + MAX_ZOMBIES + " Zombies erreicht.");
            }
            catch (Exception ex) { Debug.CatchExceptions(ex); }
        }

        private static void SpawnZombiesArroundPlayers(VnXPlayer player)
        {
            try
            {
                foreach (var zombieClass in CurrentZombies.ToList().Where(zombieClass => player.Position.Distance(zombieClass.Position) < World.Main.MaxZombieRange))
                {
                    VenoX.TriggerClientEvent(player, "Zombies:SpawnKI", zombieClass.Id, zombieClass.RandomSkinUid, zombieClass.SkinName, zombieClass.Position, zombieClass.TargetEntity);
                    //player?.EmitLocked("Zombies:SpawnKI", zombieClass.ID, zombieClass.SkinName, zombieClass.FaceFeatures, zombieClass.HeadBlendData, zombieClass.HeadOverlays, zombieClass.Position, zombieClass.TargetEntity);
                    //VenoX.TriggerClientEvent(player, "Zombies:SpawnKI", zombieClass.ID, zombieClass.SkinName, zombieClass.FaceFeatures, zombieClass.HeadBlendData, zombieClass.HeadOverlays, zombieClass.Position, zombieClass.TargetEntity);
                    zombieClass.Armor = 200;
                    zombieClass.Health = 200;
                }
            }
            catch (Exception ex) { Debug.CatchExceptions(ex); }
        }
        public static void SpawnZombiesForEveryPlayer()
        {
            try
            {
                foreach (var player in _Globals_.Main.ZombiePlayers.ToList().Where(player => player is not null && player.Exists))
                {
                    AddNearbyZombiesIntoList(player);
                    SpawnZombiesArroundPlayers(player);
                }
            }
            catch (Exception ex) { Debug.CatchExceptions(ex); }
        }


    }
}
