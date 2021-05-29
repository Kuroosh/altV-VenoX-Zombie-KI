//----------------------------------//
///// VenoX Gaming & Fun 2020 © ///////
//////By Solid_Snake & VnX RL Crew////
////////www.venox-reallife.com////////
//----------------------------------//

import * as alt from 'alt-client';
import * as game from "natives";
import {
    DrawText
} from '../../Globals/VnX-Lib';

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Settings

// Needed variables
let IsSyncer = false;
let DestroyedOne = false;
let Zombies = {};
let SyncInterval;
let ZombieClothes = {};
let ZombieAccessories = {};
let ZombieSkins = {};
var CurrentDateTime = new Date().toUTCString();
// Require 
game.requestAnimSet('move_m@drunk@verydrunk');
game.requestAnimSet('facials@gen_male@base');
game.requestAnimSet('move_m@injured');

game.requestAnimDict("special_ped@zombie@monologue_6@monologue_6a");

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

alt.onServer('Zombies:Sync', async (state, playerCount = 0) => {
    try {
        IsSyncer = state;
        if (IsSyncer) {
            if (playerCount <= 0) return;
            SyncInterval = alt.setInterval(() => {
                let numb = 0;
                for (var counter in Zombies) {
                    let Zombie = Zombies[counter];
                    if (!Zombie) continue;
                    if (numb >= 25) return;
                    if (Zombie.Entity != null && !Zombie.OutOfStreamingRange) {
                        let zombiePos = game.getEntityCoords(Zombie.Entity, true);
                        let zombieRot = game.getEntityRotation(Zombie.Entity, 2);
                        alt.emitServer('Zombies:OnSyncerCall', Zombie.Id, zombiePos.x, zombiePos.y, zombiePos.z - 1.0, zombieRot.x, zombieRot.y, zombieRot.z);
                        numb++;
                    }
                };
            }, 5000);

        } else {
            if (SyncInterval) {
                alt.clearInterval(SyncInterval);
                SyncInterval = null;
            }
        }
    } catch {}
});

alt.onServer('Zombies:SpawnKI', async (Id, RandomSkinUID, Hash, Position, Target) => {
    SpawnZombie(parseInt(Id), parseInt(RandomSkinUID), Hash, Position, Target);
});

alt.onServer('Zombies:SetDead', (Id, state) => {
    if (!Zombies[parseInt(Id)]) return;
    Zombies[parseInt(Id)].IsDead = state;
    game.setEntityAsMissionEntity(Zombies[parseInt(Id)].Entity, false, true);
})


alt.setInterval(() => {
    if (DestroyedOne) DestroyedOne = false;
    else {
        for (var _c in Zombies) {
            if (Zombies[_c].IsDead === true) {
                DestroyedOne = true;
                if (Zombies[_c].Entity == null) return delete Zombies[_c];;
                game.deletePed(Zombies[_c].Entity);
                delete Zombies[_c];
            }
        }
    }
}, 250);

alt.onServer('Zombies:SetArmor', async (Id, Armour) => {
    if (!Zombies[parseInt(Id)]) return;
    game.setPedArmour(Zombies[parseInt(Id)].Entity, parseInt(Armour));
});

alt.onServer('Zombies:SetHealth', async (Id, Health) => {
    if (!Zombies[parseInt(Id)]) return;
    game.setEntityHealth(Zombies[parseInt(Id)].Entity, parseInt(Health));
});

alt.onServer("Zombies:UpdatePositionAndRotation", async (Id, PosX, PosY, PosZ, RotX, RotY, RotZ) => {
    if (!Zombies[parseInt(Id)]) return;
    game.setEntityCoords(Zombies[parseInt(Id)].Entity, PosX, PosY, PosZ);
    game.setEntityRotation(Zombies[parseInt(Id)].Entity, RotX, RotY, RotZ, 2, true);
});

alt.onServer("Zombies:SetPosition", async (Id, PosX, PosY, PosZ) => {
    if (!Zombies[parseInt(Id)]) return;
    game.setEntityCoords(Zombies[parseInt(Id)].Entity, PosX, PosY, PosZ);
});

alt.onServer("Zombies:SetRotation", async (Id, RotX, RotY, RotZ) => {
    if (!Zombies[parseInt(Id)]) return;
    game.setEntityRotation(Zombies[parseInt(Id)].Entity, RotX, RotY, RotZ, 2, true);
});

alt.onServer('Zombies:Destroy', async (Id) => {
    if (!Zombies[parseInt(Id)]) return;
    Zombies[parseInt(Id)].IsDead = true;
});

alt.onServer('Zombies:DeleteTempZombieById', async (ID) => {
    if (!Zombies[parseInt(ID)]) return;
    Zombies[parseInt(ID)].OutOfStreamingRange = true;
    DeleteZombieById(parseInt(ID));
});


alt.onServer('Zombies:MoveToTarget', async (ID, Hash, Position, Rotation, TargetEntity, move) => {
    if (!Zombies[parseInt(ID)]) {
        SpawnZombie(parseInt(ID), Hash, Position, TargetEntity);
        return;
    }
    if (!TargetEntity) return;
    var dotNetDate = "/Date(" + move + ")/";
    var javaScriptDate = new Date(parseInt(dotNetDate.substr(6))).toUTCString();
    Zombies[parseInt(ID)].MoveTime = javaScriptDate;
    game.setEntityCoords(Zombies[parseInt(ID)].Entity, Position.x, Position.y, Position.z);
    game.setEntityRotation(Zombies[parseInt(ID)].Entity, Rotation.x, Rotation.y, Rotation.z, 2, true);
});



alt.onServer('Zombies:LoadEntityClassClothes', async (ID, Slot1Drawable, Slot1Texture, Slot2Drawable, Slot2Texture, Slot3Drawable, Slot3Texture, Slot4Drawable, Slot4Texture, Slot5Drawable, Slot5Texture, Slot6Drawable, Slot6Texture, Slot7Drawable, Slot7Texture, Slot8Drawable, Slot8Texture, Slot9Drawable, Slot9Texture, Slot10Drawable, Slot10Texture, Slot11Drawable, Slot11Texture) => {
    if (ZombieClothes[parseInt(ID)]) return;
    ZombieClothes[parseInt(ID)] = {
        Slot1Drawable: parseInt(Slot1Drawable),
        Slot1Texture: parseInt(Slot1Texture),
        Slot2Drawable: parseInt(Slot2Drawable),
        Slot2Texture: parseInt(Slot2Texture),
        Slot3Drawable: parseInt(Slot3Drawable),
        Slot3Texture: parseInt(Slot3Texture),
        Slot4Drawable: parseInt(Slot4Drawable),
        Slot4Texture: parseInt(Slot4Texture),
        Slot5Drawable: parseInt(Slot5Drawable),
        Slot5Texture: parseInt(Slot5Texture),
        Slot6Drawable: parseInt(Slot6Drawable),
        Slot6Texture: parseInt(Slot6Texture),
        Slot7Drawable: parseInt(Slot7Drawable),
        Slot7Texture: parseInt(Slot7Texture),
        Slot8Drawable: parseInt(Slot8Drawable),
        Slot8Texture: parseInt(Slot8Texture),
        Slot9Drawable: parseInt(Slot9Drawable),
        Slot9Texture: parseInt(Slot9Texture),
        Slot10Drawable: parseInt(Slot10Drawable),
        Slot11Texture: parseInt(Slot11Texture)
    }
    //alt.log('Added Clothes for Index : ' + parseInt(ID));
});

alt.onServer('Zombies:LoadEntityClassAccessories', async (ID, Slot1Drawable, Slot1Texture, Slot2Drawable, Slot2Texture, Slot3Drawable, Slot3Texture, Slot4Drawable, Slot4Texture, Slot5Drawable, Slot5Texture, Slot6Drawable, Slot6Texture, Slot7Drawable, Slot7Texture, Slot8Drawable, Slot8Texture, Slot9Drawable, Slot9Texture, Slot10Drawable, Slot10Texture, Slot11Drawable, Slot11Texture) => {
    if (ZombieAccessories[parseInt(ID)]) return;
    ZombieAccessories[parseInt(ID)] = {
        Slot1Drawable: parseInt(Slot1Drawable),
        Slot1Texture: parseInt(Slot1Texture),
        Slot2Drawable: parseInt(Slot2Drawable),
        Slot2Texture: parseInt(Slot2Texture),
        Slot3Drawable: parseInt(Slot3Drawable),
        Slot3Texture: parseInt(Slot3Texture),
        Slot4Drawable: parseInt(Slot4Drawable),
        Slot4Texture: parseInt(Slot4Texture),
        Slot5Drawable: parseInt(Slot5Drawable),
        Slot5Texture: parseInt(Slot5Texture),
        Slot6Drawable: parseInt(Slot6Drawable),
        Slot6Texture: parseInt(Slot6Texture),
        Slot7Drawable: parseInt(Slot7Drawable),
        Slot7Texture: parseInt(Slot7Texture),
        Slot8Drawable: parseInt(Slot8Drawable),
        Slot8Texture: parseInt(Slot8Texture),
        Slot9Drawable: parseInt(Slot9Drawable),
        Slot9Texture: parseInt(Slot9Texture),
        Slot10Drawable: parseInt(Slot10Drawable),
        Slot11Texture: parseInt(Slot11Texture)
    }
    //alt.log('Added Accessories for Index : ' + parseInt(ID));
});


alt.onServer('Zombies:LoadEntityClass', async (ID, FaceFeatures, HeadBlendData, HeadOverlays) => {
    if (ZombieSkins[parseInt(ID)]) return;
    ZombieSkins[parseInt(ID)] = {
        FaceFeatures: FaceFeatures,
        HeadBlendData: HeadBlendData,
        HeadOverlays: HeadOverlays
    }
    //alt.log('Added LoadEntityClass for Index : ' + ID);
});


alt.on("disconnect", () => {
    DeleteEveryZombie();
});

alt.onServer('Zombies:OnGamemodeDisconnect', () => {
    DeleteEveryZombie();
});

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function DeleteEveryZombie() {
    for (var Id in Zombies) {
        if (Zombies[Id].Entity != null) {
            game.deletePed(Zombies[Id].Entity);
        }
        delete Zombies[Id];
    };
}


function DeleteZombieById(ID) {
    if (!Zombies[ID]) return;
    for (var _c in Zombies) {
        if (Zombies[_c].Id == ID) {
            Zombies[_c].IsDead = true;
            game.setEntityAsMissionEntity(Zombies[_c].Entity, false, true);
        }
    }
}

function CheckZombieHealths() {
    for (var Id in Zombies) {
        if (Zombies[Id].Entity != null && !Zombies[Id].IsDead) {
            if (game.hasEntityBeenDamagedByEntity(Zombies[Id].Entity, alt.Player.local.scriptID)) {
                game.clearEntityLastDamageEntity(Zombies[Id].Entity);
                if (game.getEntityHealth(Zombies[Id].Entity) <= 0 && !Zombies[Id].OutOfStreamingRange) {
                    let zombiePos = game.getEntityCoords(Zombies[Id].Entity, true);
                    let zombieRot = game.getEntityRotation(Zombies[Id].Entity, 2);
                    Zombies[Id].IsDead = true;
                    game.setEntityAsMissionEntity(Zombies[Id].Entity, false, true);
                    alt.emitServer("Zombies:OnZombieDeath", parseInt(Zombies[Id].Id), zombiePos.x, zombiePos.y, zombiePos.z - 1.0, zombieRot.x, zombieRot.y, zombieRot.z);
                }
            }
        }
    };
}

alt.setInterval(() => {
    CheckZombieHealths();
    CurrentDateTime = new Date().toUTCString();
}, 500);


function DrawNametags() {
    for (var Id in Zombies) {
        if (Zombies[Id].Entity != null) {
            let playerPos2 = game.getEntityCoords(Zombies[Id].Entity);
            let distance = game.getDistanceBetweenCoords(alt.Player.local.pos.x, alt.Player.local.pos.y, alt.Player.local.pos.z, playerPos2.x, playerPos2.y, playerPos2.z, true);
            let screenPos = game.getScreenCoordFromWorldCoord(playerPos2.x, playerPos2.y, playerPos2.z + 1);
            if (distance <= 30) DrawText("Zombie ~r~[" + Zombies[Id].Id + "]\n~b~ Move : ~w~" + Zombies[Id].MoveTime + "\nCDate : " + CurrentDateTime, [screenPos[1], screenPos[2] - 0.035], [0.55, 0.55], 4, [255, 255, 255, 255], true, true);
        }
    }
}


function SetZombieCorrectSkin(zombieEntity, RandomSkinUID) {
    RandomSkinUID = parseInt(RandomSkinUID);
    if (!ZombieSkins[RandomSkinUID]) return;
    let facefeaturesarray = ZombieSkins[RandomSkinUID].FaceFeatures;
    let headblendsarray = ZombieSkins[RandomSkinUID].HeadBlendData;
    let headoverlaysarray = ZombieSkins[RandomSkinUID].HeadOverlays;

    let facefeatures = JSON.parse(facefeaturesarray);
    let headblends = JSON.parse(headblendsarray);
    let headoverlays = JSON.parse(headoverlaysarray);

    game.setPedHeadBlendData(zombieEntity, headblends[0], headblends[1], 0, headblends[2], headblends[5], 0, headblends[3], headblends[4], 0, 0);
    game.setPedHeadOverlayColor(zombieEntity, 1, 1, parseInt(headoverlays[2][1]), 1);
    game.setPedHeadOverlayColor(zombieEntity, 2, 1, parseInt(headoverlays[2][2]), 1);
    game.setPedHeadOverlayColor(zombieEntity, 5, 2, parseInt(headoverlays[2][5]), 1);
    game.setPedHeadOverlayColor(zombieEntity, 8, 2, parseInt(headoverlays[2][8]), 1);
    game.setPedHeadOverlayColor(zombieEntity, 10, 1, parseInt(headoverlays[2][10]), 1);
    game.setPedEyeColor(zombieEntity, parseInt(headoverlays[0][14]));
    game.setPedHeadOverlay(zombieEntity, 0, parseInt(headoverlays[0][0]), parseInt(headoverlays[1][0]));
    game.setPedHeadOverlay(zombieEntity, 1, parseInt(headoverlays[0][1]), parseFloat(headoverlays[1][1]));
    game.setPedHeadOverlay(zombieEntity, 2, parseInt(headoverlays[0][2]), parseFloat(headoverlays[1][2]));
    game.setPedHeadOverlay(zombieEntity, 3, parseInt(headoverlays[0][3]), parseInt(headoverlays[1][3]));
    game.setPedHeadOverlay(zombieEntity, 4, parseInt(headoverlays[0][4]), parseInt(headoverlays[1][4]));
    game.setPedHeadOverlay(zombieEntity, 5, parseInt(headoverlays[0][5]), parseInt(headoverlays[1][5]));
    game.setPedHeadOverlay(zombieEntity, 6, parseInt(headoverlays[0][6]), parseInt(headoverlays[1][6]));
    game.setPedHeadOverlay(zombieEntity, 7, parseInt(headoverlays[0][7]), parseInt(headoverlays[1][7]));
    game.setPedHeadOverlay(zombieEntity, 8, parseInt(headoverlays[0][8]), parseInt(headoverlays[1][8]));
    game.setPedHeadOverlay(zombieEntity, 9, parseInt(headoverlays[0][9]), parseInt(headoverlays[1][9]));
    game.setPedHeadOverlay(zombieEntity, 10, parseInt(headoverlays[0][10]), parseInt(headoverlays[1][10]));
    game.setPedComponentVariation(zombieEntity, 2, parseInt(headoverlays[0][13]), 0, 0);
    game.setPedHairColor(zombieEntity, parseInt(headoverlays[2][13]), parseInt(headoverlays[1][13]));
    for (let i = 0; i < 20; i++) {
        game.setPedFaceFeature(zombieEntity, i, facefeatures[i]);
    }

    if (!ZombieClothes[RandomSkinUID]) return;
    game.setPedComponentVariation(zombieEntity, 1, ZombieClothes[RandomSkinUID].Slot1Drawable, ZombieClothes[RandomSkinUID].Slot1Texture);
    game.setPedComponentVariation(zombieEntity, 2, ZombieClothes[RandomSkinUID].Slot2Drawable, ZombieClothes[RandomSkinUID].Slot2Texture);
    game.setPedComponentVariation(zombieEntity, 3, ZombieClothes[RandomSkinUID].Slot3Drawable, ZombieClothes[RandomSkinUID].Slot3Texture);
    game.setPedComponentVariation(zombieEntity, 4, ZombieClothes[RandomSkinUID].Slot4Drawable, ZombieClothes[RandomSkinUID].Slot4Texture);
    game.setPedComponentVariation(zombieEntity, 5, ZombieClothes[RandomSkinUID].Slot5Drawable, ZombieClothes[RandomSkinUID].Slot5Texture);
    game.setPedComponentVariation(zombieEntity, 6, ZombieClothes[RandomSkinUID].Slot6Drawable, ZombieClothes[RandomSkinUID].Slot6Texture);
    game.setPedComponentVariation(zombieEntity, 7, ZombieClothes[RandomSkinUID].Slot7Drawable, ZombieClothes[RandomSkinUID].Slot7Texture);
    game.setPedComponentVariation(zombieEntity, 8, ZombieClothes[RandomSkinUID].Slot8Drawable, ZombieClothes[RandomSkinUID].Slot8Texture);
    game.setPedComponentVariation(zombieEntity, 9, ZombieClothes[RandomSkinUID].Slot9Drawable, ZombieClothes[RandomSkinUID].Slot9Texture);
    game.setPedComponentVariation(zombieEntity, 10, ZombieClothes[RandomSkinUID].Slot10Drawable, ZombieClothes[RandomSkinUID].Slot10Texture);
    game.setPedComponentVariation(zombieEntity, 11, ZombieClothes[RandomSkinUID].Slot11Drawable, ZombieClothes[RandomSkinUID].Slot11Texture);

    if (!ZombieAccessories[RandomSkinUID]) return;
    game.setPedPreloadVariationData(zombieEntity, 1, ZombieAccessories[RandomSkinUID].Slot1Drawable, ZombieAccessories[RandomSkinUID].Slot1Texture);
    game.setPedPreloadVariationData(zombieEntity, 2, ZombieAccessories[RandomSkinUID].Slot2Drawable, ZombieAccessories[RandomSkinUID].Slot2Texture);
    game.setPedPreloadVariationData(zombieEntity, 3, ZombieAccessories[RandomSkinUID].Slot3Drawable, ZombieAccessories[RandomSkinUID].Slot3Texture);
    game.setPedPreloadVariationData(zombieEntity, 4, ZombieAccessories[RandomSkinUID].Slot4Drawable, ZombieAccessories[RandomSkinUID].Slot4Texture);
    game.setPedPreloadVariationData(zombieEntity, 5, ZombieAccessories[RandomSkinUID].Slot5Drawable, ZombieAccessories[RandomSkinUID].Slot5Texture);
    game.setPedPreloadVariationData(zombieEntity, 6, ZombieAccessories[RandomSkinUID].Slot6Drawable, ZombieAccessories[RandomSkinUID].Slot6Texture);
    game.setPedPreloadVariationData(zombieEntity, 7, ZombieAccessories[RandomSkinUID].Slot7Drawable, ZombieAccessories[RandomSkinUID].Slot7Texture);
    game.setPedPreloadVariationData(zombieEntity, 8, ZombieAccessories[RandomSkinUID].Slot8Drawable, ZombieAccessories[RandomSkinUID].Slot8Texture);
    game.setPedPreloadVariationData(zombieEntity, 9, ZombieAccessories[RandomSkinUID].Slot9Drawable, ZombieAccessories[RandomSkinUID].Slot9Texture);
    game.setPedPreloadVariationData(zombieEntity, 10, ZombieAccessories[RandomSkinUID].Slot10Drawable, ZombieAccessories[RandomSkinUID].Slot10Texture);
    game.setPedPreloadVariationData(zombieEntity, 11, ZombieAccessories[RandomSkinUID].Slot11Drawable, ZombieAccessories[RandomSkinUID].Slot11Texture);


}

function SetZombieAttributes(zombie) {
    game.setPedPathCanUseLadders(zombie, false);
    game.setPedPathCanUseClimbovers(zombie, false);

    game.setPedCanRagdoll(zombie, false);
    game.setEntityAsMissionEntity(zombie, true, false);
    game.setPedRelationshipGroupHash(zombie, game.getHashKey('zombeez'));
    game.setPedAccuracy(zombie, 25);
    game.setPedSeeingRange(zombie, 100.0);
    game.setPedHearingRange(zombie, 100.0);
    game.setPedFleeAttributes(zombie, 0, 0);
    game.setPedCombatAttributes(zombie, 16, 1);
    game.setPedCombatAttributes(zombie, 17, 0);
    game.setPedCombatAttributes(zombie, 46, 1);
    game.setPedCombatAttributes(zombie, 1424, 0);
    game.setPedCombatAttributes(zombie, 5, 1);
    game.setPedCombatRange(zombie, 2);
    game.setAmbientVoiceName(zombie, 'ALIENS');
    game.setPedEnableWeaponBlocking(zombie, true);
    game.disablePedPainAudio(zombie, true);
    game.setPedDiesInWater(zombie, false);
    game.setPedDiesWhenInjured(zombie, false);
    if (game.hasAnimSetLoaded('move_m@drunk@verydrunk')) {
        game.setPedMovementClipset(zombie, 'move_m@drunk@verydrunk');
    }
    game.applyPedDamagePack(zombie, 'BigHitByVehicle', 0.0, 9.0);
    game.applyPedDamagePack(zombie, 'SCR_Dumpster', 0.0, 9.0);
    game.applyPedDamagePack(zombie, 'SCR_Torture', 0.0, 9.0);
    game.stopPedSpeaking(zombie, true);
    game.setAiMeleeWeaponDamageModifier(10000);
}


async function loadModel(dict) {
    return new Promise(resolve => {
        if (game.hasModelLoaded(dict)) return resolve(true);
        game.requestModel(dict);
        let inter = alt.setInterval(() => {
            if (game.hasModelLoaded(dict)) {
                alt.clearInterval(inter);
                return resolve(true);
            }
        }, 5);
    });
}

async function SpawnZombie(Id, RandomSkinUID, Hash, Position, Target) {
    if (Zombies[Id]) return;
    Hash = alt.hash(Hash);
    let res = loadModel(Hash);
    res.then(() => {
        let zombieEntity = game.createPed(2, Hash, Position.x, Position.y, Position.z, 0, false, false);
        Zombies[Id] = {
            Id: Id,
            Entity: zombieEntity,
            IsZombieKI: true,
            Position: Position,
            IsDead: false,
            Frozen: true,
            OutOfStreamingRange: false,
            TargetEntity: Target,
            MoveTime: null
        }
        game.freezeEntityPosition(Zombies[Id].Entity, true);
        SetZombieCorrectSkin(Zombies[Id].Entity, RandomSkinUID);
        SetZombieAttributes(Zombies[Id].Entity);
    });
}



function MoveZombieToTarget(ID, TargetEntity) {
    if (Zombies[ID].IsDead) return;
    if (Zombies[ID].Frozen)
        game.freezeEntityPosition(Zombies[ID].Entity, false);
    Zombies[ID].Frozen = false;

    Zombies[ID].TargetEntity = TargetEntity;
    Zombies[ID].OutOfStreamingRange = false;

    let playerPos = game.getEntityCoords(Zombies[ID].TargetEntity.scriptID, true);
    game.taskGoToCoordAnyMeans(Zombies[ID].Entity, playerPos.x, playerPos.y, playerPos.z, 5, 0, false, 786603, 0);
    game.taskPutPedDirectlyIntoMelee(Zombies[ID].Entity, Zombies[ID].TargetEntity.scriptID, 0.0, -5.0, 1.0, false);


    if (!game.isPedRunning(Zombies[ID].Entity)) {
        game.disablePedPainAudio(Zombies[ID].Entity, false);
        game.playPain(Zombies[ID].Entity, 8);
    }

}


alt.everyTick(() => {
    //DrawNametags();
    for (var Id in Zombies)
        if (Zombies[Id].MoveTime && Zombies[Id].MoveTime <= CurrentDateTime)
            MoveZombieToTarget(Id, Zombies[Id].TargetEntity);
});


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////