using Dalamud.Game.ClientState.JobGauge.Types;
using XIVSlothCombo.Combos.PvE;
using XIVSlothCombo.CustomComboNS;

namespace XIVSlothCombo.Combos.JP
{
    internal class DNC_PoC : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DNC_ST_AdvancedMode;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID is DNC.Cascade)
                return STCombo(actionID, lastComboMove, comboTime);
            if (actionID is DNC.Windmill)
                return AoECombo(actionID, lastComboMove, comboTime);
            return actionID;
        }

        public static uint STCombo(uint actionID, uint lastComboMove, float comboTime)
        {
            // State
            var hasSymmetry = HasEffect(DNC.Buffs.SilkenSymmetry) || HasEffect(DNC.Buffs.FlourishingSymmetry);
            var hasFlow = HasEffect(DNC.Buffs.SilkenFlow) || HasEffect(DNC.Buffs.FlourishingFlow);
            var hasStandardFinish = HasEffect(DNC.Buffs.StandardFinish);
            var hasTechnicalFinish = HasEffect(DNC.Buffs.TechnicalFinish);
            var gauge = GetJobGauge<DNCGauge>();
            var burstCooldown = GetCooldownRemainingTime(DNC.TechnicalStep) + 5;
            var burstIncoming = GetCooldownRemainingTime(DNC.TechnicalStep) <= 5;

            // Dancing
            if (HasEffect(DNC.Buffs.TechnicalStep))
                return gauge.CompletedSteps < 4 ? gauge.NextStep : DNC.TechnicalFinish4;
            if (HasEffect(DNC.Buffs.StandardStep))
                return gauge.CompletedSteps < 2 ? gauge.NextStep : DNC.StandardFinish2;

            // Weaving
            if (CanWeave(actionID))
            {
                if (ActionReady(DNC.Devilment))
                {
                    if (hasTechnicalFinish || !ActionReady(DNC.TechnicalStep))
                        return DNC.Devilment;
                }
                if (ActionReady(DNC.Flourish) && !HasEffect(DNC.Buffs.ThreeFoldFanDance))
                    return DNC.Flourish;
                if (hasTechnicalFinish || hasStandardFinish)
                {
                    if (HasEffect(DNC.Buffs.FourFoldFanDance))
                    {
                        if (hasTechnicalFinish || burstCooldown > GetBuffRemainingTime(DNC.Buffs.FourFoldFanDance))
                            return DNC.FanDance4;
                    }
                    if (HasEffect(DNC.Buffs.ThreeFoldFanDance))
                        return DNC.FanDance3;
                    if (hasTechnicalFinish || gauge.Feathers > 3)
                        return DNC.FanDance1;
                }
            }

            // GCDs
            if (ActionReady(DNC.TechnicalStep) && hasStandardFinish)
                return DNC.TechnicalStep;
            if (HasEffect(DNC.Buffs.LastDanceReady))
            {
                // 1. In burst
                if (HasEffect(DNC.Buffs.TechnicalFinish))
                    return DNC.LastDance;
                // 2. Cannot hold for burst
                if (burstCooldown > GetBuffRemainingTime(DNC.Buffs.LastDanceReady))
                    return DNC.LastDance;
            }
            if (HasEffect(DNC.Buffs.FinishingMoveReady) && ActionReady(DNC.FinishingMove))
                return DNC.FinishingMove;
            if (HasEffect(DNC.Buffs.DanceOfTheDawnReady) && gauge.Esprit >= 50)
                return DNC.DanceOfTheDawn;
            if (ActionReady(DNC.SaberDance) && gauge.Esprit >= 50)
            {
                // 1. In burst
                if (HasEffect(DNC.Buffs.TechnicalFinish))
                    return DNC.SaberDance;
                // 2. Cannot hold for burst
                if (!burstIncoming && gauge.Esprit >= 80)
                    return DNC.SaberDance;
            }
            if (HasEffect(DNC.Buffs.FlourishingStarfall))
                return DNC.StarfallDance;
            if (HasEffect(DNC.Buffs.FlourishingFinish))
                return DNC.Tillana;
            if (ActionReady(DNC.StandardStep) && !burstIncoming)
                return DNC.StandardStep;
            if (ActionReady(DNC.Fountainfall) && hasFlow)
                return DNC.Fountainfall;
            if (ActionReady(DNC.ReverseCascade) && hasSymmetry)
                return DNC.ReverseCascade;
            if (ActionReady(DNC.Fountain) && lastComboMove is DNC.Cascade && comboTime > 0)
                return DNC.Fountain;

            return actionID;
        }

        public static uint AoECombo(uint actionID, uint lastComboMove, float comboTime)
        {
            // State
            var hasSymmetry = HasEffect(DNC.Buffs.SilkenSymmetry) || HasEffect(DNC.Buffs.FlourishingSymmetry);
            var hasFlow = HasEffect(DNC.Buffs.SilkenFlow) || HasEffect(DNC.Buffs.FlourishingFlow);
            var hasStandardFinish = HasEffect(DNC.Buffs.StandardFinish);
            var hasTechnicalFinish = HasEffect(DNC.Buffs.TechnicalFinish);
            var gauge = GetJobGauge<DNCGauge>();
            var burstCooldown = GetCooldownRemainingTime(DNC.TechnicalStep) + 5;
            var burstIncoming = GetCooldownRemainingTime(DNC.TechnicalStep) <= 5;

            // Dancing
            if (HasEffect(DNC.Buffs.TechnicalStep))
                return gauge.CompletedSteps < 4 ? gauge.NextStep : DNC.TechnicalFinish4;
            if (HasEffect(DNC.Buffs.StandardStep))
                return gauge.CompletedSteps < 2 ? gauge.NextStep : DNC.StandardFinish2;

            // Weaving
            if (CanWeave(actionID))
            {
                if (ActionReady(DNC.Devilment))
                {
                    if (hasTechnicalFinish || !ActionReady(DNC.TechnicalStep))
                        return DNC.Devilment;
                }
                if (ActionReady(DNC.Flourish) && !HasEffect(DNC.Buffs.ThreeFoldFanDance))
                    return DNC.Flourish;
                if (hasTechnicalFinish || hasStandardFinish)
                {
                    if (HasEffect(DNC.Buffs.FourFoldFanDance))
                    {
                        if (hasTechnicalFinish || burstCooldown > GetBuffRemainingTime(DNC.Buffs.FourFoldFanDance))
                            return DNC.FanDance4;
                    }
                    if (HasEffect(DNC.Buffs.ThreeFoldFanDance))
                        return DNC.FanDance3;
                    if (hasTechnicalFinish || gauge.Feathers > 3)
                        return DNC.FanDance2;
                }
            }

            // GCDs
            if (ActionReady(DNC.TechnicalStep) && hasStandardFinish)
                return DNC.TechnicalStep;
            if (HasEffect(DNC.Buffs.LastDanceReady))
            {
                // 1. In burst
                if (HasEffect(DNC.Buffs.TechnicalFinish))
                    return DNC.LastDance;
                // 2. Cannot hold for burst
                if (burstCooldown > GetBuffRemainingTime(DNC.Buffs.LastDanceReady))
                    return DNC.LastDance;
            }
            if (HasEffect(DNC.Buffs.FinishingMoveReady) && ActionReady(DNC.FinishingMove))
                return DNC.FinishingMove;
            if (HasEffect(DNC.Buffs.DanceOfTheDawnReady) && gauge.Esprit >= 50)
                return DNC.DanceOfTheDawn;
            if (ActionReady(DNC.SaberDance) && gauge.Esprit >= 50)
            {
                // 1. In burst
                if (HasEffect(DNC.Buffs.TechnicalFinish))
                    return DNC.SaberDance;
                // 2. Cannot hold for burst
                if (!burstIncoming && gauge.Esprit >= 80)
                    return DNC.SaberDance;
            }
            if (HasEffect(DNC.Buffs.FlourishingStarfall))
                return DNC.StarfallDance;
            if (HasEffect(DNC.Buffs.FlourishingFinish))
                return DNC.Tillana;
            if (ActionReady(DNC.StandardStep) && !burstIncoming)
                return DNC.StandardStep;
            if (ActionReady(DNC.Bloodshower) && hasFlow)
                return DNC.Bloodshower;
            if (ActionReady(DNC.RisingWindmill) && hasSymmetry)
                return DNC.RisingWindmill;
            if (ActionReady(DNC.Bladeshower) && lastComboMove is DNC.Windmill && comboTime > 0)
                return DNC.Bladeshower;

            return actionID;
        }
    }
}
