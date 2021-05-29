using System;
using AltV.Net;
using VenoXV._RootCore_.Models;
using VenoXV.Models;

namespace VenoXV.Zombie.Globals
{
    public class Main : IScript
    {

        public static void OnUpdate()
        {
            try
            {
                if (_Globals_.Main.ZombiePlayers.Count <= 0) return;
                World.Main.OnUpdate();
            }
            catch(Exception ex){Core.Debug.CatchExceptions(ex);}
        }

        [VenoXRemoteEvent("OnZombieKill")]
        public static void OnZombieKill(VnXPlayer player)
        {
            try
            {
                if (player is null) return;
                player.Zombies.ZombieKills += 1;
                // debug
                Console.WriteLine(player.Username + " killed a Zombie!");
            }
            catch(Exception ex){Core.Debug.CatchExceptions(ex);}
        }
    }
}
