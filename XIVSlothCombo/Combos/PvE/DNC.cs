﻿using Dalamud.Game.ClientState.JobGauge.Types;
using XIVSlothCombo.Combos.PvE.Content;
using XIVSlothCombo.Core;
using XIVSlothCombo.CustomComboNS;
using XIVSlothCombo.CustomComboNS.Functions;
using XIVSlothCombo.Services;

namespace XIVSlothCombo.Combos.PvE
{
    internal static class DNC
    {
        public const byte JobID = 38;

        #region Actions

        public const uint
            // Single Target
            Cascade = 15989,
            Fountain = 15990,
            ReverseCascade = 15991,
            Fountainfall = 15992,
            StarfallDance = 25792,
            // AoE
            Windmill = 15993,
            Bladeshower = 15994,
            RisingWindmill = 15995,
            Bloodshower = 15996,
            Tillana = 25790,
            // Dancing
            StandardStep = 15997,
            TechnicalStep = 15998,
            StandardFinish0 = 16003,
            StandardFinish1 = 16191,
            StandardFinish2 = 16192,
            TechnicalFinish0 = 16004,
            TechnicalFinish1 = 16193,
            TechnicalFinish2 = 16194,
            TechnicalFinish3 = 16195,
            TechnicalFinish4 = 16196,
            // Fan Dances
            FanDance1 = 16007,
            FanDance2 = 16008,
            FanDance3 = 16009,
            FanDance4 = 25791,
            // Other
            Peloton = 7557,
            SaberDance = 16005,
            EnAvant = 16010,
            Devilment = 16011,
            ShieldSamba = 16012,
            Flourish = 16013,
            Improvisation = 16014,
            CuringWaltz = 16015,
            LastDance = 36983,
            FinishingMove = 36984,
            DanceOfTheDawn = 36985;

        #endregion

        public static class Buffs
        {
            public const ushort
                // Flourishing & Silken (Procs)
                FlourishingCascade = 1814,
                FlourishingFountain = 1815,
                FlourishingWindmill = 1816,
                FlourishingShower = 1817,
                FlourishingFanDance = 2021,
                SilkenSymmetry = 2693,
                SilkenFlow = 2694,
                FlourishingFinish = 2698,
                FlourishingStarfall = 2700,
                FlourishingSymmetry = 3017,
                FlourishingFlow = 3018,
                // Dances
                StandardStep = 1818,
                TechnicalStep = 1819,
                StandardFinish = 1821,
                TechnicalFinish = 1822,
                // Fan Dances
                ThreeFoldFanDance = 1820,
                FourFoldFanDance = 2699,
                // Other
                Peloton = 1199,
                ShieldSamba = 1826,
                LastDanceReady = 3867,
                FinishingMoveReady = 3868,
                DanceOfTheDawnReady = 3869,
                Devilment = 1825;
        }

        public static class Config
        {
            public static readonly UserInt
                DNCEspritThreshold_ST = new UserInt("DNCEspritThreshold_ST", 50); // ST - Esprit threshold
            public static readonly UserInt
                DNCEspritThreshold_AoE = new UserInt("DNCEspritThreshold_AoE", 50); // AoE - Esprit threshold

            #region Advanced ST Sliders
            public static readonly UserInt
                DNC_ST_Adv_SSBurstPercent = new UserInt("DNC_ST_Adv_SSBurstPercent", 0), // Standard Step - target HP% threshold
                DNC_ST_Adv_TSBurstPercent = new UserInt("DNC_ST_Adv_TSBurstPercent", 0), // Technical Step - target  HP% threshold
                DNC_ST_Adv_FeatherBurstPercent = new UserInt("DNC_ST_Adv_FeatherBurstPercent", 0), // Feather burst -  target HP% threshold
                DNC_ST_Adv_SaberThreshold = new UserInt("DNC_ST_Adv_SaberThreshold", 50), // Saber Dance - Esprit  threshold
                DNC_ST_Adv_PanicHealWaltzPercent = new UserInt("DNC_ST_Adv_PanicHealWaltzPercent", 30), // Curing Waltz - player HP% threshold
                DNC_ST_Adv_PanicHealWindPercent = new UserInt("DNC_ST_Adv_PanicHealWindPercent", 20); // Second Wind - player HP% threshold
            #endregion

            #region Advanced AoE Sliders
            public static readonly UserInt
                DNC_AoE_Adv_SSBurstPercent = new UserInt("DNC_AoE_Adv_SSBurstPercent", 0), // Standard Step - target HP% threshold
                DNC_AoE_Adv_TSBurstPercent = new UserInt("DNC_AoE_Adv_TSBurstPercent", 0), // Technical Step - target HP% threshold
                DNC_AoE_Adv_SaberThreshold = new UserInt("DNC_AoE_Adv_SaberThreshold", 50), // Saber Dance - Esprit threshold
                DNC_AoE_Adv_PanicHealWaltzPercent = new UserInt("DNC_AoE_Adv_PanicHealWaltzPercent", 30), // Curing Waltz - player HP% threshold
                DNC_AoE_Adv_PanicHealWindPercent = new UserInt("DNC_AoE_Adv_PanicHealWindPercent", 20); // Second Wind - player HP% threshold
            #endregion

            public static readonly UserInt
                DNCVariantCurePercent = new UserInt("DNCVariantCurePercent"); // Variant Cure - player HP% threshold
        }

        internal class DNC_DanceComboReplacer : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DNC_DanceComboReplacer;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (GetJobGauge<DNCGauge>().IsDancing)
                {
                    uint[]? actionIDs = Service.Configuration.DancerDanceCompatActionIDs;

                    if (actionID == actionIDs[0] || (actionIDs[0] == 0 && actionID == Cascade))     // Cascade replacement
                        return OriginalHook(Cascade);
                    if (actionID == actionIDs[1] || (actionIDs[1] == 0 && actionID == Flourish))    // Fountain replacement
                        return OriginalHook(Fountain);
                    if (actionID == actionIDs[2] || (actionIDs[2] == 0 && actionID == FanDance1))   // Reverse Cascade replacement
                        return OriginalHook(ReverseCascade);
                    if (actionID == actionIDs[3] || (actionIDs[3] == 0 && actionID == FanDance2))   // Fountainfall replacement
                        return OriginalHook(Fountainfall);
                }

                return actionID;
            }
        }

        internal class DNC_FanDanceCombos : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DNC_FanDanceCombos;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                // FD 1 --> 3, FD 1 --> 4
                if (actionID is FanDance1)
                {
                    if (IsEnabled(CustomComboPreset.DNC_FanDance_1to3_Combo) &&
                        HasEffect(Buffs.ThreeFoldFanDance))
                        return FanDance3;
                    if (IsEnabled(CustomComboPreset.DNC_FanDance_1to4_Combo) &&
                        HasEffect(Buffs.FourFoldFanDance))
                        return FanDance4;
                }

                // FD 2 --> 3, FD 2 --> 4
                if (actionID is FanDance2)
                {
                    if (IsEnabled(CustomComboPreset.DNC_FanDance_2to3_Combo) &&
                        HasEffect(Buffs.ThreeFoldFanDance))
                        return FanDance3;
                    if (IsEnabled(CustomComboPreset.DNC_FanDance_2to4_Combo) &&
                        HasEffect(Buffs.FourFoldFanDance))
                        return FanDance4;
                }

                return actionID;
            }
        }

        internal class DNC_DanceStepCombo : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DNC_DanceStepCombo;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                DNCGauge? gauge = GetJobGauge<DNCGauge>();

                // Standard Step
                if (actionID is StandardStep && gauge.IsDancing && HasEffect(Buffs.StandardStep))
                    return gauge.CompletedSteps < 2
                        ? gauge.NextStep
                        : StandardFinish2;

                // Technical Step
                if (actionID is TechnicalStep && gauge.IsDancing && HasEffect(Buffs.TechnicalStep))
                    return gauge.CompletedSteps < 4
                        ? gauge.NextStep
                        : TechnicalFinish4;

                return actionID;
            }
        }

        internal class DNC_FlourishingFanDances : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DNC_FlourishingFanDances;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                // Fan Dance 3 & 4 on Flourish
                if (actionID is Flourish && CanWeave(actionID))
                {
                    if (HasEffect(Buffs.ThreeFoldFanDance))
                        return FanDance3;
                    if (HasEffect(Buffs.FourFoldFanDance))
                        return FanDance4;
                }

                return actionID;
            }
        }

        internal class DNC_Starfall_Devilment : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DNC_Starfall_Devilment;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level) =>
                actionID is Devilment && HasEffect(Buffs.FlourishingStarfall)
                    ? StarfallDance
                    : actionID;
        }

        internal class DNC_StandardStep_LastDance : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DNC_StandardStep_LastDance;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level) =>
                actionID is StandardStep or FinishingMove &&
                HasEffect(Buffs.LastDanceReady)
                    ? LastDance
                    : actionID;
        }

        internal class DNC_TechnicalStep_Devilment : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DNC_TechnicalStep_Devilment;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level) =>
                actionID is TechnicalStep &&
                WasLastWeaponskill(TechnicalFinish4) &&
                HasEffect(Buffs.TechnicalFinish)
                    ? Devilment
                    : actionID;
        }

        internal class DNC_ST_AdvancedMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DNC_ST_AdvancedMode;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                // OK so just to be on the same page here are some notes that I made
                // The whole spirit of this version is to focus on DNC core abilities (kinda like TNKs where sloth gives the rotation but it's your job to keep yourself alive)
                // There is some room for auto-adjustment in case the rotation goes off the rails, but didn't focus on that initially
                // 1. The following priority system will revolve around raid buffs, nothing is more important than that
                // 2. Stats have NO impact in the timing of said raid buffs (This is probably a huge mistake since skill speed is a thing, but working around it shouldn't be that hard)      
                // 3. I'm making every tick box enabled and have no care about enemy hp, etc. (Since those are just conditions you add depending on the settings)
                // 4. I don't include healing, interrupts, mitigation, variant dungeons, etc.
                // 5. I'll try not to mess up with this code base too much to keep the spirit of the changes intact
                // 6. I'll add my own class for comparisson sake
                if (actionID is not Cascade) return actionID;

                #region Variables
                DNCGauge? gauge = GetJobGauge<DNCGauge>();

                var flow = HasEffect(Buffs.SilkenFlow) || HasEffect(Buffs.FlourishingFlow);
                var symmetry = HasEffect(Buffs.SilkenSymmetry) || HasEffect(Buffs.FlourishingSymmetry);
                var targetHpThresholdFeather = Config.DNC_ST_Adv_FeatherBurstPercent;
                var targetHpThresholdStandard = Config.DNC_ST_Adv_SSBurstPercent;
                var targetHpThresholdTechnical = Config.DNC_ST_Adv_TSBurstPercent;
                var gcd = GetCooldown(Fountain).CooldownTotal;

                var needToTech =
                    IsEnabled(CustomComboPreset.DNC_ST_Adv_TS) && // Enabled
                    GetCooldownRemainingTime(TechnicalStep) < 0.05f && // Up or about to be (some anti-drift)
                    !HasEffect(Buffs.StandardStep) && // After Standard
                    IsOnCooldown(StandardStep) &&
                    GetTargetHPPercent() > targetHpThresholdTechnical &&// HP% check
                    LevelChecked(TechnicalStep);

                var needToStandardOrFinish =
                    IsEnabled(CustomComboPreset.DNC_ST_Adv_SS) && // Enabled
                    GetTargetHPPercent() > targetHpThresholdStandard && // HP% check
                    (IsOffCooldown(TechnicalStep) || // Checking burst is ready for standard
                     GetCooldownRemainingTime(TechnicalStep) > 5) && // Don't mangle
                    LevelChecked(StandardStep);

                var needToFinish =
                    HasEffect(Buffs.FinishingMoveReady) &&
                    !HasEffect(Buffs.LastDanceReady) &&
                    ((GetCooldownRemainingTime(StandardStep) < 0.3f && // About to be up - some more aggressive anti-drift
                      HasEffect(Buffs.TechnicalFinish)) ||
                     (!HasEffect(Buffs.TechnicalFinish) && // Anti-Drift outside of Tech
                      GetCooldownRemainingTime(StandardStep) < 0.05f));

                var needToStandard =
                    ((!IsEnabled(CustomComboPreset.DNC_ST_Adv_SS_Hold) &&
                      GetCooldownRemainingTime(StandardStep) < 0.1f) || // Up or about to be (some anti-drift)
                     IsEnabled(CustomComboPreset.DNC_ST_Adv_SS_Hold) &&
                     GetCooldownRemainingTime(StandardStep) < gcd) // About to be up next GCD
                    &&
                    !HasEffect(Buffs.FinishingMoveReady) &&
                    (IsOffCooldown(Flourish) ||
                     GetCooldownRemainingTime(Flourish) > 5) &&
                    !HasEffect(Buffs.TechnicalFinish);
                #endregion

                // I actually removed all of this from my side, since prepull is basically SS + Peloton
                // And the priority system forces you to have SF at all times (This is probably a min/max thing so I didn't worry too much)
                #region Pre-pull

                if (!InCombat())
                {
                    // ST Standard Step (Pre-pull)
                    if (IsEnabled(CustomComboPreset.DNC_ST_Adv_SS_Prepull) &&
                        ActionReady(StandardStep) &&
                        !HasEffect(Buffs.FinishingMoveReady) &&
                        !HasEffect(Buffs.TechnicalFinish) &&
                        IsOffCooldown(TechnicalStep) &&
                        IsOffCooldown(StandardStep) &&
                        !HasTarget())
                        return StandardStep;

                    // ST Standard Steps (Pre-pull)
                    if ((IsEnabled(CustomComboPreset.DNC_ST_Adv_SS) ||
                         IsEnabled(CustomComboPreset.DNC_ST_Adv_StandardFill) ||
                         IsEnabled(CustomComboPreset.DNC_ST_Adv_SS_Prepull)) &&
                        HasEffect(Buffs.StandardStep) &&
                        gauge.CompletedSteps < 2 &&
                        !HasTarget())
                        return gauge.NextStep;

                    // ST Peloton
                    if (IsEnabled(CustomComboPreset.DNC_ST_Adv_Peloton) &&
                        !HasEffectAny(Buffs.Peloton) &&
                        GetBuffRemainingTime(Buffs.StandardStep) > 5)
                        return Peloton;
                }
                #endregion

                #region Dance Fills
                // OK so I decided to code this "state machine" from Most Priority/Less likely to happen to Less Priority/More likely to happen
                // No matter what is happening anywhere, you hit TS on cooldown
                // That means moving the ST GCD TD up in the code
                // ST Technical (Dance) Steps & Fill
                if ((IsEnabled(CustomComboPreset.DNC_ST_Adv_TS) || IsEnabled(CustomComboPreset.DNC_ST_Adv_TechFill)) &&
                    HasEffect(Buffs.TechnicalStep))
                    return gauge.CompletedSteps < 4
                        ? gauge.NextStep
                        : TechnicalFinish4;

                // ST Standard (Dance) Steps & Fill
                if ((IsEnabled(CustomComboPreset.DNC_ST_Adv_SS) ||
                     IsEnabled(CustomComboPreset.DNC_ST_Adv_StandardFill)) &&
                    HasEffect(Buffs.StandardStep))
                    return gauge.CompletedSteps < 2
                        ? gauge.NextStep
                        : StandardFinish2;
                #endregion

                #region Weaves
                // OK here is where things get tricky
                // Devilment -> Flourish -> FanDance4 -> FanDance3 -> FanDance1 
                // ST Devilment => To me Devilment only has two conditions: Off cooldown and while burst (TF buff)
                // 1. I don't add a gap to the cooldown remaining time check
                // 2. I don't check the last action performed
                // 3. I only care that I have TechnicalFinish or that I'm level locked out of TS
                if (IsEnabled(CustomComboPreset.DNC_ST_Adv_Devilment) &&
                    CanWeave(actionID) &&
                    LevelChecked(Devilment) &&
                    GetCooldownRemainingTime(Devilment) < 0.05 &&
                    (HasEffect(Buffs.TechnicalFinish) ||
                     WasLastAction(TechnicalFinish4) ||
                     !LevelChecked(TechnicalStep)))
                    return Devilment;

                // ST Flourish => To me Flourish only has two conditions: Off cooldown and prevent overcapping FanDance3             
                // 1. I don't check the last action performed
                // 2. I don't check burst status (Flourish doesn't do damage, and due to being 60s, it's impossible to miss raid windows on both (at least one will fit)
                // 3. FinishingMoveReady, FourFoldDance, Floursihing Symmetry and Florishing Flow cannot be stacked so no need to check for those
                if (IsEnabled(CustomComboPreset.DNC_ST_Adv_Flourish) &&
                    CanWeave(actionID) &&
                    ActionReady(Flourish) &&
                    !WasLastWeaponskill(TechnicalFinish4) &&
                    IsOnCooldown(Devilment) &&
                    (GetCooldownRemainingTime(Devilment) > 50 ||
                     (HasEffect(Buffs.Devilment) &&
                      GetBuffRemainingTime(Buffs.Devilment) < 19)) &&
                    !HasEffect(Buffs.ThreeFoldFanDance) &&
                    !HasEffect(Buffs.FourFoldFanDance) &&
                    !HasEffect(Buffs.FlourishingSymmetry) &&
                    !HasEffect(Buffs.FlourishingFlow) &&
                    !HasEffect(Buffs.FinishingMoveReady) &&
                    ((CombatEngageDuration().TotalSeconds < 20 &&
                      HasEffect(Buffs.TechnicalFinish)) ||
                     CombatEngageDuration().TotalSeconds > 20))
                    return Flourish;

                if (IsEnabled(CustomComboPreset.DNC_ST_Adv_Flourish_ForcedTripleWeave) &&
                    (HasEffect(Buffs.ThreeFoldFanDance) ||
                     HasEffect(Buffs.FourFoldFanDance)) &&
                     CombatEngageDuration().TotalSeconds > 20 &&
                     HasEffect(Buffs.TechnicalFinish) &&
                     GetCooldownRemainingTime(Flourish) > 58)
                {
                    if (HasEffect(Buffs.ThreeFoldFanDance) && CanDelayedWeave(actionID))
                        return FanDance3;
                    if (HasEffect(Buffs.FourFoldFanDance))
                        return FanDance4;
                }

                // ST Interrupt
                if (IsEnabled(CustomComboPreset.DNC_ST_Adv_Interrupt) &&
                CanInterruptEnemy() &&
                ActionReady(All.HeadGraze) &&
                !HasEffect(Buffs.TechnicalFinish))
                    return All.HeadGraze;

                // Variant Cure
                if (IsEnabled(CustomComboPreset.DNC_Variant_Cure) &&
                    IsEnabled(Variant.VariantCure) &&
                    PlayerHealthPercentageHp() <= GetOptionValue(Config.DNCVariantCurePercent))
                    return Variant.VariantCure;

                // Variant Rampart
                if (IsEnabled(CustomComboPreset.DNC_Variant_Rampart) &&
                    IsEnabled(Variant.VariantRampart) &&
                    IsOffCooldown(Variant.VariantRampart) &&
                    CanWeave(actionID))
                    return Variant.VariantRampart;

                // Feahers => Here I just check whether I'm buffed and in burst or close to burst (for FanDance4)
                // 1. I don't check the last action performed
                // 2. For FourFoldFanDance I check If I'm in burst or far from burst (5s check for TS)
                // 3. For FanDance1 I consumed them when capped (always) instead of using them always when out of TF (early levels)
                //  An advantage of this approach is that in fights where there are raidbuffs outside of TF, you can bank them
                if (CanWeave(actionID) && !WasLastWeaponskill(TechnicalFinish4))
                {
                    if (HasEffect(Buffs.ThreeFoldFanDance))
                        return FanDance3;

                    if (HasEffect(Buffs.FourFoldFanDance))
                        return FanDance4;

                    // ST Feathers & Fans
                    if (IsEnabled(CustomComboPreset.DNC_ST_Adv_Feathers) &&
                        LevelChecked(FanDance1))
                    {
                        // FD1 HP% Dump
                        if (GetTargetHPPercent() <= targetHpThresholdFeather && gauge.Feathers > 0)
                            return FanDance1;

                        if (LevelChecked(TechnicalStep))
                        {
                            // Burst FD1
                            if (HasEffect(Buffs.TechnicalFinish) && gauge.Feathers > 0)
                                return FanDance1;

                            // FD1 Pooling
                            if (gauge.Feathers > 3 &&
                                (HasEffect(Buffs.SilkenSymmetry) ||
                                 HasEffect(Buffs.SilkenFlow))
                                )

                                return FanDance1;
                        }

                        // FD1 Non-pooling & under burst level
                        if (!LevelChecked(TechnicalStep) && gauge.Feathers > 0)
                            return FanDance1;
                    }

                    // ST Panic Heals
                    if (IsEnabled(CustomComboPreset.DNC_ST_Adv_PanicHeals))
                    {
                        if (ActionReady(CuringWaltz) &&
                            PlayerHealthPercentageHp() < Config.DNC_ST_Adv_PanicHealWaltzPercent)
                            return CuringWaltz;

                        if (ActionReady(All.SecondWind) &&
                            PlayerHealthPercentageHp() < Config.DNC_ST_Adv_PanicHealWindPercent)
                            return All.SecondWind;
                    }

                    // ST Improvisation
                    if (IsEnabled(CustomComboPreset.DNC_ST_Adv_Improvisation) &&
                        ActionReady(Improvisation) &&
                        !HasEffect(Buffs.TechnicalFinish))
                        return Improvisation;
                }
                #endregion

                #region GCD
                // OK so here is where weird stuff started happening to me and why I decided to check the code
                // I simplified the logic of needToTech, removed the margins of error and so on but it shouldn't have an impact
                // 1. Technical Step is king (nothing else should get in the way of pressing it
                // ST Technical Step
                if (needToTech)
                    return TechnicalStep;
                // 2. Last Dance vs Dance of the Dawn
                // Here I couldn't decide on priority, my mind tells me Dance of the Dawn is more important
                // Because using LastDance means another GCD, meaning more Esprit generation that could overcap you
                // But on the other hand using Dance of the Dawn (which only happens in burst - meaning you have time) could screw over the timeout of LastDance
                // So I'm risking overcapping Esprit on 1 GCD rather than risk losing LastDance (No 10 moves ahead 4D chess here, just coding what I would do playing normally)
                // -- For Last Dance I check If I'm in burst or far from burst (5s check for TS)
                // ST Last Dance
                if (IsEnabled(CustomComboPreset.DNC_ST_Adv_LD) && // Enabled
                    HasEffect(Buffs.LastDanceReady) && // Ready
                    (HasEffect(Buffs.TechnicalFinish) || // Has Tech
                     !(IsOnCooldown(TechnicalStep) && // Or can't hold it for tech
                       GetCooldownRemainingTime(TechnicalStep) < 20 &&
                       GetBuffRemainingTime(Buffs.LastDanceReady) > GetCooldownRemainingTime(TechnicalStep) + 4) ||
                     GetBuffRemainingTime(Buffs.LastDanceReady) < 4)) // Or last second
                    return LastDance;

                // 3. Dance of the Dawn
                // This is one of the weird things that happened to me, so TL;DR SimpleDNC prioritized Tillana (which gives 50 Esprit)
                // Before consuming Dance of the Dawn (only way to salvage this is to set the threshold to 50, but if 70-80 Tillana happens first in some cases)
                // So Dance of the Dawn kicks in if I have resources, period
                // ST Dance of the Dawn
                if (IsEnabled(CustomComboPreset.DNC_ST_Adv_DawnDance) &&
                    HasEffect(Buffs.DanceOfTheDawnReady) &&
                    LevelChecked(DanceOfTheDawn) &&
                    (GetCooldownRemainingTime(TechnicalStep) > 5 ||
                     IsOffCooldown(TechnicalStep)) && // Tech is up
                    (gauge.Esprit >= Config.DNC_ST_Adv_SaberThreshold || // above esprit threshold use
                     (HasEffect(Buffs.TechnicalFinish) && gauge.Esprit >= 50) || // will overcap with Tillana if not used
                     (GetBuffRemainingTime(Buffs.DanceOfTheDawnReady) < 5 && gauge.Esprit >= 50))) // emergency use
                    return OriginalHook(DanceOfTheDawn);

                // 4. Finishing Move
                // Here I just check that I have the buff and the action is ready (Don't care about the rest)
                // Again This is a consequence of Flourish, so it will land in raid buffs
                // ST Standard Step (Finishing Move)
                if (needToStandardOrFinish && needToFinish)
                    return OriginalHook(FinishingMove);

                // 5. Saber Dance
                // Above Threshold or in burst (this is ok, but still moved it up in the priority
                // ST Saber Dance
                if (IsEnabled(CustomComboPreset.DNC_ST_Adv_SaberDance) &&
                    LevelChecked(SaberDance) &&
                    gauge.Esprit >= Config.DNC_ST_Adv_SaberThreshold || // Above esprit threshold use
                    (HasEffect(Buffs.TechnicalFinish) && gauge.Esprit >= 50) && // Burst
                    (GetCooldownRemainingTime(TechnicalStep) > 5 ||
                     IsOffCooldown(TechnicalStep))) // Tech is up
                    return SaberDance;

                // 6. Starfall Dance
                // On cooldown
                if (HasEffect(Buffs.FlourishingStarfall))
                    return StarfallDance;

                // 7. Tillana
                // On cooldown
                if (HasEffect(Buffs.FlourishingFinish) &&
                    IsEnabled(CustomComboPreset.DNC_ST_Adv_Tillana))
                    return Tillana;

                // 8. Standard Step
                // As low as this looks, all of the above have such dependency on this that it flows naturally as far as I've tested
                if (needToStandardOrFinish && needToStandard)
                    return StandardStep;

                // No need for emergency workarounds in my case as far as I could tell.
                // ST combos and burst attacks
                if (LevelChecked(Fountain) &&
                    lastComboMove is Cascade &&
                    comboTime is < 2 and > 0)
                    return Fountain;

                if (LevelChecked(Fountainfall) && flow)
                    return Fountainfall;
                if (LevelChecked(ReverseCascade) && symmetry)
                    return ReverseCascade;
                if (LevelChecked(Fountain) && lastComboMove is Cascade && comboTime > 0)
                    return Fountain;
                #endregion

                return actionID;
            }
        }

        internal class DNC_ST_MultiButton : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DNC_ST_MultiButton;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Cascade)
                {
                    #region Types
                    DNCGauge? gauge = GetJobGauge<DNCGauge>();
                    bool flow = HasEffect(Buffs.SilkenFlow) || HasEffect(Buffs.FlourishingFlow);
                    bool symmetry = HasEffect(Buffs.SilkenSymmetry) || HasEffect(Buffs.FlourishingSymmetry);
                    #endregion

                    // ST Esprit overcap protection
                    if (IsEnabled(CustomComboPreset.DNC_ST_EspritOvercap) &&
                        LevelChecked(DanceOfTheDawn) &&
                        HasEffect(Buffs.DanceOfTheDawnReady) &&
                        gauge.Esprit >= Config.DNCEspritThreshold_ST)
                        return OriginalHook(DanceOfTheDawn);
                    if (IsEnabled(CustomComboPreset.DNC_ST_EspritOvercap) &&
                        LevelChecked(SaberDance) &&
                        gauge.Esprit >= Config.DNCEspritThreshold_ST)
                        return SaberDance;

                    if (CanWeave(actionID))
                    {
                        // ST Fan Dance overcap protection
                        if (IsEnabled(CustomComboPreset.DNC_ST_FanDanceOvercap) &&
                            LevelChecked(FanDance1) && gauge.Feathers is 4 && (HasEffect(Buffs.SilkenSymmetry) ||
                            HasEffect(Buffs.SilkenFlow)))
                            return FanDance1;

                        // ST Fan Dance 3/4 on combo
                        if (IsEnabled(CustomComboPreset.DNC_ST_FanDance34))
                        {
                            if (HasEffect(Buffs.ThreeFoldFanDance))
                                return FanDance3;
                            if (HasEffect(Buffs.FourFoldFanDance))
                                return FanDance4;
                        }
                    }

                    // ST base combos
                    if (LevelChecked(Fountainfall) && flow)
                        return Fountainfall;
                    if (LevelChecked(ReverseCascade) && symmetry)
                        return ReverseCascade;
                    if (LevelChecked(Fountain) && lastComboMove is Cascade)
                        return Fountain;
                }

                return actionID;
            }
        }

        internal class DNC_AoE_AdvancedMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DNC_AoE_AdvancedMode;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is not Windmill) return actionID;

                #region Variables
                DNCGauge? gauge = GetJobGauge<DNCGauge>();

                bool flow = HasEffect(Buffs.SilkenFlow) || HasEffect(Buffs.FlourishingFlow);
                bool symmetry = HasEffect(Buffs.SilkenSymmetry) || HasEffect(Buffs.FlourishingSymmetry);
                var targetHpThresholdStandard = Config.DNC_AoE_Adv_SSBurstPercent;
                var targetHpThresholdTechnical = Config.DNC_AoE_Adv_TSBurstPercent;

                var needToTech =
                    IsEnabled(CustomComboPreset.DNC_AoE_Adv_TS) && // Enabled
                    ActionReady(TechnicalStep) && // Up
                    !HasEffect(Buffs.StandardStep) && // After Standard
                    IsOnCooldown(StandardStep) &&
                    GetTargetHPPercent() > targetHpThresholdTechnical && // HP% check
                    LevelChecked(TechnicalStep);

                var needToStandardOrFinish =
                    IsEnabled(CustomComboPreset.DNC_AoE_Adv_SS) && // Enabled
                    ActionReady(StandardStep) && // Up
                    GetTargetHPPercent() > targetHpThresholdStandard && // HP% check
                    (IsOffCooldown(TechnicalStep) || // Checking burst is ready for standard
                     GetCooldownRemainingTime(TechnicalStep) > 5) && // Don't mangle
                    LevelChecked(StandardStep);

                var needToFinish =
                    HasEffect(Buffs.FinishingMoveReady) &&
                    !HasEffect(Buffs.LastDanceReady);

                var needToStandard =
                    !HasEffect(Buffs.FinishingMoveReady) &&
                    (IsOffCooldown(Flourish) ||
                     GetCooldownRemainingTime(Flourish) > 5) &&
                    !HasEffect(Buffs.TechnicalFinish);
                #endregion

                #region Dance Fills
                // AoE Standard (Dance) Steps & Fill
                if ((IsEnabled(CustomComboPreset.DNC_AoE_Adv_SS) ||
                     IsEnabled(CustomComboPreset.DNC_AoE_Adv_StandardFill)) &&
                    HasEffect(Buffs.StandardStep))
                    return gauge.CompletedSteps < 2
                        ? gauge.NextStep
                        : StandardFinish2;

                // AoE Technical (Dance) Steps & Fill
                if ((IsEnabled(CustomComboPreset.DNC_AoE_Adv_TS) ||
                     IsEnabled(CustomComboPreset.DNC_AoE_Adv_TechFill)) &&
                    HasEffect(Buffs.TechnicalStep))
                    return gauge.CompletedSteps < 4
                        ? gauge.NextStep
                        : TechnicalFinish4;
                #endregion

                #region Weaves
                // AoE Devilment
                if (IsEnabled(CustomComboPreset.DNC_AoE_Adv_Devilment) &&
                    CanWeave(actionID) &&
                    LevelChecked(Devilment) &&
                    GetCooldownRemainingTime(Devilment) < 0.05 &&
                    (HasEffect(Buffs.TechnicalFinish) ||
                     WasLastAction(TechnicalFinish4) ||
                     !LevelChecked(TechnicalStep)))
                    return Devilment;

                // AoE Flourish
                if (IsEnabled(CustomComboPreset.DNC_AoE_Adv_Flourish) &&
                    CanWeave(actionID) &&
                    ActionReady(Flourish) &&
                    !WasLastWeaponskill(TechnicalFinish4) &&
                    IsOnCooldown(Devilment) &&
                    (GetCooldownRemainingTime(Devilment) > 50 ||
                     (HasEffect(Buffs.Devilment) &&
                      GetBuffRemainingTime(Buffs.Devilment) < 19)) &&
                    !HasEffect(Buffs.ThreeFoldFanDance) &&
                    !HasEffect(Buffs.FourFoldFanDance) &&
                    !HasEffect(Buffs.FlourishingSymmetry) &&
                    !HasEffect(Buffs.FlourishingFlow) &&
                    !HasEffect(Buffs.FinishingMoveReady))
                    return Flourish;

                // AoE Interrupt
                if (IsEnabled(CustomComboPreset.DNC_AoE_Adv_Interrupt) &&
                    CanInterruptEnemy() && ActionReady(All.HeadGraze) &&
                    !HasEffect(Buffs.TechnicalFinish))
                    return All.HeadGraze;

                if (IsEnabled(CustomComboPreset.DNC_Variant_Cure) &&
                    IsEnabled(Variant.VariantCure) &&
                    PlayerHealthPercentageHp() <= GetOptionValue(Config.DNCVariantCurePercent))
                    return Variant.VariantCure;

                if (IsEnabled(CustomComboPreset.DNC_Variant_Rampart) &&
                    IsEnabled(Variant.VariantRampart) &&
                    IsOffCooldown(Variant.VariantRampart) &&
                    CanWeave(actionID))
                    return Variant.VariantRampart;

                if (CanWeave(actionID) && !WasLastWeaponskill(TechnicalFinish4))
                {
                    // AoE Feathers & Fans
                    if (IsEnabled(CustomComboPreset.DNC_AoE_Adv_Feathers) &&
                        LevelChecked(FanDance1))
                    {
                        // FD3
                        if (HasEffect(Buffs.ThreeFoldFanDance))
                            return FanDance3;

                        if (LevelChecked(FanDance2))
                        {
                            if (LevelChecked(TechnicalStep))
                            {
                                // Burst FD2
                                if (HasEffect(Buffs.TechnicalFinish) &&
                                    gauge.Feathers > 0)
                                    return FanDance2;

                                // FD2 Pooling
                                if (gauge.Feathers > 3 &&
                                    (HasEffect(Buffs.SilkenSymmetry) ||
                                     HasEffect(Buffs.SilkenFlow)))
                                    return FanDance2;
                            }

                            // FD2 Non-pooling & under burst level
                            if (!LevelChecked(TechnicalStep) &&
                                gauge.Feathers > 0)
                                return FanDance2;
                        }

                        // FD1 Replacement for Lv.30-49
                        if (!LevelChecked(FanDance2) &&
                            gauge.Feathers > 0)
                            return FanDance1;
                    }

                    if (HasEffect(Buffs.FourFoldFanDance))
                        return FanDance4;

                    // AoE Panic Heals
                    if (IsEnabled(CustomComboPreset.DNC_AoE_Adv_PanicHeals))
                    {
                        if (ActionReady(CuringWaltz) &&
                            PlayerHealthPercentageHp() < Config.DNC_AoE_Adv_PanicHealWaltzPercent)
                            return CuringWaltz;

                        if (ActionReady(All.SecondWind) &&
                            PlayerHealthPercentageHp() < Config.DNC_AoE_Adv_PanicHealWindPercent)
                            return All.SecondWind;
                    }

                    // AoE Improvisation
                    if (IsEnabled(CustomComboPreset.DNC_AoE_Adv_Improvisation) &&
                        ActionReady(Improvisation) &&
                        !HasEffect(Buffs.TechnicalStep))
                        return Improvisation;
                }
                #endregion

                #region GCD
                // AoE Technical Step
                if (needToTech)
                    return TechnicalStep;

                // AoE Last Dance
                if (IsEnabled(CustomComboPreset.DNC_AoE_Adv_LD) && // Enabled
                    HasEffect(Buffs.LastDanceReady) && // Ready
                    (HasEffect(Buffs.TechnicalFinish) || // Has Tech
                     !(IsOnCooldown(TechnicalStep) && // Or can't hold it for tech
                       GetCooldownRemainingTime(TechnicalStep) < 20 &&
                       GetBuffRemainingTime(Buffs.LastDanceReady) > GetCooldownRemainingTime(TechnicalStep) + 4) ||
                     GetBuffRemainingTime(Buffs.LastDanceReady) < 4)) // Or last second
                    return LastDance;

                // AoE Standard Step (Finishing Move)
                if (needToStandardOrFinish && needToFinish)
                    return OriginalHook(FinishingMove);

                // AoE Standard Step
                if (needToStandardOrFinish && needToStandard)
                    return StandardStep;

                // Emergency Starfall usage
                if (HasEffect(Buffs.FlourishingStarfall) &&
                    GetBuffRemainingTime(Buffs.FlourishingStarfall) < 4)
                    return StarfallDance;

                // AoE Dance of the Dawn
                if (IsEnabled(CustomComboPreset.DNC_AoE_Adv_DawnDance) &&
                    HasEffect(Buffs.DanceOfTheDawnReady) &&
                    LevelChecked(DanceOfTheDawn) &&
                    (GetCooldownRemainingTime(TechnicalStep) > 5 ||
                     IsOffCooldown(TechnicalStep)) && // Tech is up
                    (gauge.Esprit >= Config.DNC_AoE_Adv_SaberThreshold || // above esprit threshold use
                     (HasEffect(Buffs.TechnicalFinish) && gauge.Esprit >= 50) || // will overcap with Tillana if not used
                     (GetBuffRemainingTime(Buffs.DanceOfTheDawnReady) < 5 && gauge.Esprit >= 50))) // emergency use
                    return OriginalHook(DanceOfTheDawn);

                // AoE Saber Dance (Emergency Use)
                if (IsEnabled(CustomComboPreset.DNC_AoE_Adv_SaberDance) &&
                    LevelChecked(SaberDance) &&
                    (gauge.Esprit >= Config.DNC_AoE_Adv_SaberThreshold || // above esprit threshold use
                     (HasEffect(Buffs.TechnicalFinish) && gauge.Esprit >= 50)) && // will overcap with Tillana if not used
                    ActionReady(SaberDance))
                    return SaberDance;

                if (HasEffect(Buffs.FlourishingStarfall))
                    return StarfallDance;

                // AoE Tillana
                if (HasEffect(Buffs.FlourishingFinish) &&
                    IsEnabled(CustomComboPreset.DNC_AoE_Adv_Tillana))
                    return Tillana;

                // AoE Saber Dance
                if (IsEnabled(CustomComboPreset.DNC_AoE_Adv_SaberDance) &&
                    LevelChecked(SaberDance) &&
                    gauge.Esprit >= Config.DNC_ST_Adv_SaberThreshold || // Above esprit threshold use
                    (HasEffect(Buffs.TechnicalFinish) && gauge.Esprit >= 50) && // Burst
                    (GetCooldownRemainingTime(TechnicalStep) > 5 ||
                     IsOffCooldown(TechnicalStep))) // Tech is up
                    return SaberDance;

                // AoE combos and burst attacks
                if (LevelChecked(Bladeshower) &&
                    lastComboMove is Windmill &&
                    comboTime is < 2 and > 0)
                    return Bladeshower;

                if (LevelChecked(Bloodshower) && flow)
                    return Bloodshower;
                if (LevelChecked(RisingWindmill) && symmetry)
                    return RisingWindmill;
                if (LevelChecked(Bladeshower) && lastComboMove is Windmill && comboTime > 0)
                    return Bladeshower;
                #endregion

                return actionID;
            }
        }

        internal class DNC_AoE_MultiButton : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DNC_AoE_MultiButton;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Windmill)
                {
                    #region Types
                    DNCGauge? gauge = GetJobGauge<DNCGauge>();
                    bool flow = HasEffect(Buffs.SilkenFlow) || HasEffect(Buffs.FlourishingFlow);
                    bool symmetry = HasEffect(Buffs.SilkenSymmetry) || HasEffect(Buffs.FlourishingSymmetry);
                    #endregion

                    // AoE Esprit overcap protection
                    if (IsEnabled(CustomComboPreset.DNC_AoE_EspritOvercap) &&
                        LevelChecked(DanceOfTheDawn) &&
                        HasEffect(Buffs.DanceOfTheDawnReady) &&
                        gauge.Esprit >= Config.DNCEspritThreshold_ST)
                        return OriginalHook(DanceOfTheDawn);
                    if (IsEnabled(CustomComboPreset.DNC_AoE_EspritOvercap) &&
                        LevelChecked(SaberDance) &&
                        gauge.Esprit >= Config.DNCEspritThreshold_AoE)
                        return SaberDance;

                    if (CanWeave(actionID))
                    {
                        // AoE Fan Dance overcap protection
                        if (IsEnabled(CustomComboPreset.DNC_AoE_FanDanceOvercap) &&
                            LevelChecked(FanDance2) && gauge.Feathers is 4 &&
                            (HasEffect(Buffs.SilkenSymmetry) || HasEffect(Buffs.SilkenFlow)))
                            return FanDance2;

                        // AoE Fan Dance 3/4 on combo
                        if (IsEnabled(CustomComboPreset.DNC_AoE_FanDance34))
                        {
                            if (HasEffect(Buffs.ThreeFoldFanDance))
                                return FanDance3;
                            if (HasEffect(Buffs.FourFoldFanDance))
                                return FanDance4;
                        }
                    }

                    // AoE base combos
                    if (LevelChecked(Bloodshower) && flow)
                        return Bloodshower;
                    if (LevelChecked(RisingWindmill) && symmetry)
                        return RisingWindmill;
                    if (LevelChecked(Bladeshower) && lastComboMove is Windmill)
                        return Bladeshower;
                }

                return actionID;
            }
        }
    }
}
