using System;
using System.Linq;
using System.Numerics;
using VenoXV._Globals_;
using VenoXV.Core;
using VenoXV.Models;
using VenoXV.Zombie.KI;

namespace VenoXV.Zombie.Models
{
    public class ZombieModel
    {
        public int Id { get; set; }
        public int Sex { get; set; }
        public int RandomSkinUid { get; set; }
        public string SkinName { get; set; }
        private Vector3 _Position { get; set; }
        public VnXPlayer Killer { get; set; }
        public Vector3 Position
        {
            get => _Position;
            init
            {
                _Position = value;
                foreach (var players in Main.ZombiePlayers.ToList().Where(players => players.Zombies.NearbyZombies.Contains(this) && !players.Zombies.IsSyncer)) VenoX.TriggerClientEvent(players, "Zombies:SetPosition", Id, value.X, value.Y, value.Z);
            }
        }
        private Vector3 _Rotation { get; set; }
        public Vector3 Rotation
        {
            get => _Rotation;
            set
            {
                _Rotation = value;
                foreach (var players in Main.ZombiePlayers.ToList().Where(players => players.Zombies.NearbyZombies.Contains(this) && !players.Zombies.IsSyncer)) VenoX.TriggerClientEvent(players, "Zombies:SetRotation", Id, value.X, value.Y, value.Z);
            }
        }
        public void UpdatePositionAndRotation(Vector3 position, Vector3 rotation, bool sync = false)
        {
            try
            {
                _Position = position;
                _Rotation = rotation;
                foreach (var players in Main.ZombiePlayers.ToList().Where(players => players.Zombies.NearbyZombies.Contains(this) && !players.Zombies.IsSyncer && sync)) 
                    VenoX.TriggerClientEvent(players, "Zombies:UpdatePositionAndRotation", Id, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z);
            }
            catch(Exception ex){Core.Debug.CatchExceptions(ex);}
        }
        private int _Armor { get; set; }
        public int Armor
        {
            get => _Armor;
            set
            {
                _Armor = value;
                foreach (var players in Main.ZombiePlayers.ToList().Where(players => players.Zombies.NearbyZombies.Contains(this))) VenoX.TriggerClientEvent(players, "Zombies:SetArmor", Id, value);
            }
        }
        private int _Health { get; set; }
        public int Health
        {
            get => _Health;
            set
            {
                _Health = value;
                foreach (var players in Main.ZombiePlayers.ToList().Where(players => players.Zombies.NearbyZombies.Contains(this))) VenoX.TriggerClientEvent(players, "Zombies:SetHealth", Id, value);
            }
        }
        private bool _IsDead { get; set; }
        public bool IsDead
        {
            get => _IsDead;
            set
            {
                _IsDead = value;
                foreach (var players in Main.ZombiePlayers.ToList().Where(players => players.Zombies.NearbyZombies.Contains(this) && Killer != players)) VenoX.TriggerClientEvent(players, "Zombies:SetDead", Id, value);
            }
        }
        public void Destroy()
        {
            try
            {
                foreach (var players in Main.ZombiePlayers.ToList().Where(players => players is not null && players.Zombies.NearbyZombies.Contains(this)))
                {
                    VenoX.TriggerClientEvent(players, "Zombies:Destroy", Id); 
                    players?.Zombies.NearbyZombies.Remove(this);
                }
                Spawner.CurrentZombies.Remove(this);
            }
            catch (Exception ex) { Debug.CatchExceptions(ex); }
        }
        public VnXPlayer TargetEntity { get; set; }
    }
}
