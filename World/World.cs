using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AltV.Net;
using AltV.Net.Data;
using VenoXV.Core;
using VenoXV.Models;
using VenoXV.Zombie.KI;
using VenoXV.Zombie.Models;

namespace VenoXV.Zombie.World
{
    public class Main : IScript
    {
        //public static Position PLAYER_SPAWN_NOOBSPAWN = new Position(-2132.323f, 2821.959f, 34.84159f); // Noobspawn
        public static Position PlayerSpawnNoobspawn = new Position(0, 0, 72); // 0 point of gtav.
        public static int TimeIntervalZombies = 10; // Time in seconds - Interval for spawning zombies.
        public static int ZombieAmmountEachSpawn = 2; // Count of Zombies that will be spawned - in every interval.
        public static int TimeIntervalDeleteZombies = 5;
        public static int TimeIntervalSyncerUpdate = 1; // Time in minutes
        public static int TimeIntervalTargetUpdate = 2; // Time in seconds
        public static int MaxZombieRange = 300;
        // ENTITYDATAS & TIMER
        public static DateTime TimeToSpawnZombies = DateTime.Now;
        public static DateTime TimeToDeleteZombies = DateTime.Now;
        public static DateTime TimeToGetNewSyncer = DateTime.Now;
        public static DateTime TimeToGetNewTarget = DateTime.Now;



        public static void SendPlayerWelcomeNotify(VnXPlayer player)
        {
            try
            {
                player?.SendTranslatedChatMessage("Willkommen im VenoX " + RageApi.GetHexColorcode(255, 0, 0) + " Zombie + " + RageApi.GetHexColorcode(255, 255, 255) + "Modus");
                player?.SendTranslatedChatMessage("Kämpfe um dein Überleben!");
            }
            catch(Exception ex){Core.Debug.CatchExceptions(ex);}
        }

        public static readonly List<Vector3> PlayerSpawns = new List<Vector3>
        {
            new Vector3(-117.03297f,-604.66815f,36.272583f),
            new Vector3(50.861538f,-136.21979f,55.194824f),
            new Vector3(213.27032f,-921.1517f,30.678345f),
            new Vector3(427.0681f,-978.989f,30.69519f),
            new Vector3(-10.892307f,-1118.6901f,27.578003f),
            new Vector3(130.66814f,-1032.8308f,29.431519f),
        };

        public static void OnSelectedZombieGM(VnXPlayer player)
        {
            try
            {
                Random random = new Random();
                int randomNumber = random.Next(0, PlayerSpawns.Count);
                player.SpawnPlayer(PlayerSpawns[randomNumber]);
                player.Dimension = _Globals_.Main.ZombiesDimension;
                //VenoX.TriggerClientEvent(player, "Zombie:OnResourceStart");
                LevelSystem.GivePlayerWeaponsByLevel(player);
                SendPlayerWelcomeNotify(player);
            }
            catch (Exception ex) { Debug.CatchExceptions(ex); }
        }
        public static void OnPlayerDeath(VnXPlayer player)
        {
            try
            {
                Random random = new Random();
                int randomNumber = random.Next(0, PlayerSpawns.Count);
                player.SpawnPlayer(PlayerSpawns[randomNumber]);
                player.Dimension = _Globals_.Main.ZombiesDimension;
                player.Zombies.ZombieDeaths += 1;
                LevelSystem.GivePlayerWeaponsByLevel(player);
            }
            catch (Exception ex) { Debug.CatchExceptions(ex); }
        }

        private static void SetBestPlayerByPing(VnXPlayer player)
        {
            try
            {
                if (player is null || !player.Exists) return;
                uint bestPing = player.Ping;
                //If no one is near you, you are the Syncer.
                if (player.NearbyPlayers.Count <= 0) player.Zombies.IsSyncer = true;
                else
                {
                    // Get New Syncer.
                    foreach (VnXPlayer nearbyPlayers in player.NearbyPlayers.ToList())
                    {
                        if (nearbyPlayers is null || !nearbyPlayers.Exists) continue;
                        if (bestPing < nearbyPlayers.Ping)
                        {
                            bestPing = nearbyPlayers.Ping;
                            nearbyPlayers.Zombies.IsSyncer = true;
                            player.Zombies.IsSyncer = false;
                        }
                        else
                        {
                            nearbyPlayers.Zombies.IsSyncer = false;
                            player.Zombies.IsSyncer = true;
                            VenoX.TriggerClientEvent(player, "Zombies:Sync", false);
                            VenoX.TriggerClientEvent(nearbyPlayers, "Zombies:Sync", false);
                        }
                    }
                }
            }
            catch (Exception ex) { Debug.CatchExceptions(ex); }
        }
        public static void GetBestAreaSyncer()
        {
            try
            {
                foreach (var player in _Globals_.Main.ZombiePlayers.ToList().Where(player => player != null))
                {
                    SetBestPlayerByPing(player);
                    if (player.Zombies.IsSyncer) 
                        VenoX.TriggerClientEvent(player, "Zombies:Sync", true, player.NearbyPlayers.Count);
                }
            }
            catch (Exception ex) { Debug.CatchExceptions(ex); }
        }

        private static void GetNewZombieTarget()
        {
            try
            {
                foreach (ZombieModel zombieClass in Spawner.CurrentZombies.ToList())
                    foreach (var player in _Globals_.Main.ZombiePlayers.ToList().Where(player => player is not null && player.Exists).Where(player => player.Position.Distance(zombieClass.Position) < 50))
                        zombieClass.TargetEntity = player;
            }
            catch (Exception ex) { Debug.CatchExceptions(ex); }
        }
        public static void SyncZombieTargeting()
        {
            try
            {
                long move = Convert.ToInt64(DateTime.UtcNow.AddSeconds(5).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
                foreach (ZombieModel zombieClass in Spawner.CurrentZombies.ToList())
                {
                    foreach (var player in _Globals_.Main.ZombiePlayers.ToList().Where(player => player != null && player.Exists))
                    {
                        if (player.Position.Distance(zombieClass.Position) < 150 && !zombieClass.IsDead)
                        {
                            if (!player.Zombies.NearbyZombies.Contains(zombieClass)) 
                                player.Zombies.NearbyZombies.Add(zombieClass);
                            
                            VenoX.TriggerClientEvent(player, "Zombies:MoveToTarget", zombieClass.Id, zombieClass.SkinName, zombieClass.Position, zombieClass.Rotation, zombieClass.TargetEntity, move);
                        }
                        else
                        {
                            if (!player.Zombies.NearbyZombies.Contains(zombieClass)) continue;
                            player.Zombies.NearbyZombies.Remove(zombieClass);
                            VenoX.TriggerClientEvent(player, "Zombies:DeleteTempZombieById", zombieClass.Id);
                        }
                    }
                }
            }
            catch (Exception ex) { Debug.CatchExceptions(ex); }
        }


        public static void OnUpdate()
        {
            try
            {
                if (TimeToSpawnZombies <= DateTime.Now)
                {
                    TimeToSpawnZombies = DateTime.Now.AddSeconds(TimeIntervalZombies);
                    for (var i = 0; i < ZombieAmmountEachSpawn; i++)
                    {
                        Spawner.SpawnZombiesForEveryPlayer();
                    }
                }
                if (TimeToDeleteZombies <= DateTime.Now)
                {
                    TimeToDeleteZombies = DateTime.Now.AddSeconds(TimeIntervalDeleteZombies);
                    foreach (var zombies in Spawner.CurrentZombies.ToList().Where(zombies => zombies.IsDead))
                    {
                        // remove every nearby zombie for every client.
                        zombies.Destroy();
                    }
                }
                if (TimeToGetNewSyncer <= DateTime.Now)
                {
                    TimeToGetNewSyncer = DateTime.Now.AddMinutes(TimeIntervalSyncerUpdate);
                    GetBestAreaSyncer();
                }

                if (TimeToGetNewTarget > DateTime.Now) return;
                TimeToGetNewTarget = DateTime.Now.AddSeconds(TimeIntervalTargetUpdate);
                GetNewZombieTarget();
                //SyncZombieTargeting();
            }
            catch (Exception ex) { Debug.CatchExceptions(ex); }
        }
    }
}
