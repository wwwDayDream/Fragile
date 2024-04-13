using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Fragile.Patches;

[HarmonyPatch(typeof(PlayerRagdoll))]
public class PlayerRagdollBodyPartCollisionPatch {
    [HarmonyPatch(nameof(PlayerRagdoll.BodyPartCollision))]
    [HarmonyPostfix]
    private static void MakeMeHurtBodyPartCollision(PlayerRagdoll __instance, Collision collision, Bodypart bodypart)
    {
        float magnitude = default;
        if ((magnitude = collision.relativeVelocity.magnitude) < (__instance.player.data.fallTime <= 0.5f ? 10f : 5f) || !Player.localPlayer.refs.view.Controller.IsMasterClient) return;
        var multiplier = bodypart.bodypartType switch {
            BodypartType.Hip => 0.5f,
            BodypartType.Mid => 0.5f,
            BodypartType.Torso => 0.7f,
            BodypartType.Neck => 0.8f,
            BodypartType.Mouth => 1f,
            BodypartType.Head => 1f,
            BodypartType.Arm_L => 0.1f,
            BodypartType.Elbow_L => 0.35f,
            BodypartType.Hand_L => 0.25f,
            BodypartType.Arm_R => 0.1f,
            BodypartType.Elbow_R => 0.35f,
            BodypartType.Hand_R => 0.25f,
            BodypartType.Leg_L => 0.1f,
            BodypartType.Knee_L => 0.35f,
            BodypartType.Leg_R => 0.1f,
            BodypartType.Knee_R => 0.35f,
            _ => 0f
        };
        if (multiplier <= 0) return;

        __instance.player.CallTakeDamageAndTase(magnitude * multiplier, 0f);
    }
}