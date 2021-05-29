using System;
using System.Linq;
using VenoXV._Gamemodes_.Reallife.model;
using VenoXV._Preload_.Character_Creator;
using VenoXV.Core;
using VenoXV.Models;
using Main = VenoXV._Gamemodes_.Reallife.Globals.Main;

namespace VenoXV.Zombie.Assets
{
    public class ZombieAssets
    {
        private static void LoadZombieClothesByIndex(VnXPlayer player, int index)
        {
            try
            {
                int slot1Drawable = 0;
                int slot1Texture = 0;

                int slot2Drawable = 0;
                int slot2Texture = 0;

                int slot3Drawable = 0;
                int slot3Texture = 0;

                int slot4Drawable = 0;
                int slot4Texture = 0;

                int slot5Drawable = 0;
                int slot5Texture = 0;

                int slot6Drawable = 0;
                int slot6Texture = 0;

                int slot7Drawable = 0;
                int slot7Texture = 0;

                int slot8Drawable = 0;
                int slot8Texture = 0;

                int slot9Drawable = 0;
                int slot9Texture = 0;

                int slot10Drawable = 0;
                int slot10Texture = 0;

                int slot11Drawable = 0;
                int slot11Texture = 0;
                foreach (var clothes in Main.ClothesList.ToList().Where(clothes => clothes.Player == index && clothes.Dressed && clothes.Type == 0))
                {
                    switch (clothes.Slot)
                    {
                        case 1:
                            slot1Drawable = clothes.Drawable;
                            slot1Texture = clothes.Texture;
                            break;
                        case 2:
                            slot2Drawable = clothes.Drawable;
                            slot2Texture = clothes.Texture;
                            break;
                        case 3:
                            slot3Drawable = clothes.Drawable;
                            slot3Texture = clothes.Texture;
                            break;
                        case 4:
                            slot4Drawable = clothes.Drawable;
                            slot4Texture = clothes.Texture;
                            break;
                        case 5:
                            slot5Drawable = clothes.Drawable;
                            slot5Texture = clothes.Texture;
                            break;
                        case 6:
                            slot6Drawable = clothes.Drawable;
                            slot6Texture = clothes.Texture;
                            break;
                        case 7:
                            slot7Drawable = clothes.Drawable;
                            slot7Texture = clothes.Texture;
                            break;
                        case 8:
                            slot8Drawable = clothes.Drawable;
                            slot8Texture = clothes.Texture;
                            break;
                        case 9:
                            slot9Drawable = clothes.Drawable;
                            slot9Texture = clothes.Texture;
                            break;
                        case 10:
                            slot10Drawable = clothes.Drawable;
                            slot10Texture = clothes.Texture;
                            break;
                        case 11:
                            slot11Drawable = clothes.Drawable;
                            slot11Texture = clothes.Texture;
                            break;
                    }
                }
                /*
                Core.Debug.OutputDebugString("ID1 : " + Slot1Drawable + " | " + Slot1Texture);
                Core.Debug.OutputDebugString("ID2 : " + Slot2Drawable + " | " + Slot2Texture);
                Core.Debug.OutputDebugString("ID3 : " + Slot3Drawable + " | " + Slot3Texture);
                Core.Debug.OutputDebugString("ID4 : " + Slot4Drawable + " | " + Slot4Texture);
                Core.Debug.OutputDebugString("ID5 : " + Slot5Drawable + " | " + Slot5Texture);
                Core.Debug.OutputDebugString("ID6 : " + Slot6Drawable + " | " + Slot6Texture);
                Core.Debug.OutputDebugString("ID7 : " + Slot7Drawable + " | " + Slot7Texture);
                Core.Debug.OutputDebugString("ID8 : " + Slot8Drawable + " | " + Slot8Texture);
                Core.Debug.OutputDebugString("ID9 : " + Slot9Drawable + " | " + Slot9Texture);
                Core.Debug.OutputDebugString("ID10 : " + Slot10Drawable + " | " + Slot10Texture);
                Core.Debug.OutputDebugString("ID11 : " + Slot11Drawable + " | " + Slot11Texture);
                */
                VenoX.TriggerPreloadEvent(player, "Zombie Clothes [" + index + "]", "Zombies:LoadEntityClassClothes", index,
                    slot1Drawable, slot1Texture,
                    slot2Drawable, slot2Texture,
                    slot3Drawable, slot3Texture,
                    slot4Drawable, slot4Texture,
                    slot5Drawable, slot5Texture,
                    slot6Drawable, slot6Texture,
                    slot7Drawable, slot7Texture,
                    slot8Drawable, slot8Texture,
                    slot9Drawable, slot9Texture,
                    slot10Drawable, slot10Texture,
                    slot11Drawable, slot11Texture);
            }
            catch (Exception ex) { Debug.CatchExceptions(ex); }
        }
        private static void LoadZombieAccessoriesByIndex(VnXPlayer player, int index)
        {
            try
            {
                int slot1Drawable = 0;
                int slot1Texture = 0;

                int slot2Drawable = 0;
                int slot2Texture = 0;

                int slot3Drawable = 0;
                int slot3Texture = 0;

                int slot4Drawable = 0;
                int slot4Texture = 0;

                int slot5Drawable = 0;
                int slot5Texture = 0;

                int slot6Drawable = 0;
                int slot6Texture = 0;

                int slot7Drawable = 0;
                int slot7Texture = 0;

                int slot8Drawable = 0;
                int slot8Texture = 0;

                int slot9Drawable = 0;
                int slot9Texture = 0;

                int slot10Drawable = 0;
                int slot10Texture = 0;

                int slot11Drawable = 0;
                int slot11Texture = 0;
                foreach (ClothesModel clothes in Main.ClothesList.ToList().Where(clothes => clothes.Player == index && clothes.Dressed && clothes.Type != 0))
                {
                    switch (clothes.Slot)
                    {
                        case 1:
                            slot1Drawable = clothes.Drawable;
                            slot1Texture = clothes.Texture;
                            break;
                        case 2:
                            slot2Drawable = clothes.Drawable;
                            slot2Texture = clothes.Texture;
                            break;
                        case 3:
                            slot3Drawable = clothes.Drawable;
                            slot3Texture = clothes.Texture;
                            break;
                        case 4:
                            slot4Drawable = clothes.Drawable;
                            slot4Texture = clothes.Texture;
                            break;
                        case 5:
                            slot5Drawable = clothes.Drawable;
                            slot5Texture = clothes.Texture;
                            break;
                        case 6:
                            slot6Drawable = clothes.Drawable;
                            slot6Texture = clothes.Texture;
                            break;
                        case 7:
                            slot7Drawable = clothes.Drawable;
                            slot7Texture = clothes.Texture;
                            break;
                        case 8:
                            slot8Drawable = clothes.Drawable;
                            slot8Texture = clothes.Texture;
                            break;
                        case 9:
                            slot9Drawable = clothes.Drawable;
                            slot9Texture = clothes.Texture;
                            break;
                        case 10:
                            slot10Drawable = clothes.Drawable;
                            slot10Texture = clothes.Texture;
                            break;
                        case 11:
                            slot11Drawable = clothes.Drawable;
                            slot11Texture = clothes.Texture;
                            break;
                    }
                }
                /*Core.Debug.OutputDebugString("ID1 : " + Slot1Drawable + " | " + Slot1Texture);
                Core.Debug.OutputDebugString("ID2 : " + Slot2Drawable + " | " + Slot2Texture);
                Core.Debug.OutputDebugString("ID3 : " + Slot3Drawable + " | " + Slot3Texture);
                Core.Debug.OutputDebugString("ID4 : " + Slot4Drawable + " | " + Slot4Texture);
                Core.Debug.OutputDebugString("ID5 : " + Slot5Drawable + " | " + Slot5Texture);
                Core.Debug.OutputDebugString("ID6 : " + Slot6Drawable + " | " + Slot6Texture);
                Core.Debug.OutputDebugString("ID7 : " + Slot7Drawable + " | " + Slot7Texture);
                Core.Debug.OutputDebugString("ID8 : " + Slot8Drawable + " | " + Slot8Texture);
                Core.Debug.OutputDebugString("ID9 : " + Slot9Drawable + " | " + Slot9Texture);
                Core.Debug.OutputDebugString("ID10 : " + Slot10Drawable + " | " + Slot10Texture);
                Core.Debug.OutputDebugString("ID11 : " + Slot11Drawable + " | " + Slot11Texture);*/
                VenoX.TriggerPreloadEvent(player, "Zombie Accessories [" + index + "]", "Zombies:LoadEntityClassAccessories", index,
                    slot1Drawable, slot1Texture,
                    slot2Drawable, slot2Texture,
                    slot3Drawable, slot3Texture,
                    slot4Drawable, slot4Texture,
                    slot5Drawable, slot5Texture,
                    slot6Drawable, slot6Texture,
                    slot7Drawable, slot7Texture,
                    slot8Drawable, slot8Texture,
                    slot9Drawable, slot9Texture,
                    slot10Drawable, slot10Texture,
                    slot11Drawable, slot11Texture);
            }
            catch (Exception ex) { Debug.CatchExceptions(ex); }
        }
        public static void LoadZombieEntityData(VnXPlayer player)
        {
            try
            {
                int i = 0;
                foreach (CharacterModel entityClass in _Preload_.Character_Creator.Main.CharacterSkins.ToList())
                {
                    i++;
                    VenoX.TriggerPreloadEvent(player, "Zombie Skins [" + i + "/" + _Preload_.Character_Creator.Main.CharacterSkins.ToList().Count + "]", "Zombies:LoadEntityClass", entityClass.Uid, entityClass.FaceFeatures, entityClass.HeadBlendData, entityClass.HeadOverlays);
                    LoadZombieClothesByIndex(player, entityClass.Uid);
                    LoadZombieAccessoriesByIndex(player, entityClass.Uid);
                    //Core.Debug.OutputDebugString("Index : " + EntityClass.UID);
                }
                //Core.Debug.OutputDebugString("Count Value : " + _Preload_.Character_Creator.Main.CharacterSkins.Count);
            }
            catch (Exception ex) { Debug.CatchExceptions(ex); }
        }
    }
}
