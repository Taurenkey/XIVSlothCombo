using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using ECommons.DalamudServices;
using XIVSlothCombo.CustomComboNS.Functions;
using XIVSlothCombo.Combos.PvE;
using XIVSlothCombo.Data;
using Newtonsoft.Json.Serialization;

namespace XIVSlothCombo.Combos.JobHelpers
{
    internal static class Defensives
    {
        public static class AllBuffs
        {
            public const ushort
                 Rampart = 1191,
                 ArmsLength = 1209;
        }

        public static class AllActions
        {
            public const uint
                 Rampart = 7531,
                 ArmsLength = 7548,
                 Reprisal = 7535;
        }

        public static class WARBuffs
        {
            public const ushort
                 ThrillOfBattle = 87,
                 Vengeance = 89,
                 Bloodwhetting = 2678,
                 Holmgang = 409,
                 Equilibrium = 2681,
                 ShakeItOff = 1993,
                 ShakeItOff2 = 1457,
                 RawIntuition = 735,
                 StemOfTheTide = 2680,
                 StemofTheFlow = 2679;

        }


        public static class WARActions
        {
            public const uint
                NascentFlash = 16464,
                Vengeance = 44,
                ShakeItOff = 7388,
                ThrillOfBattle = 40,
                Holmgang = 43,
                Equilibrium = 3552,
                RawIntuition = 3551,
                Bloodwhetting = 25751;
        }

        public static class PLDBuffs
        {
            public const ushort
                Sentinel = 74,
                Bulwark = 77,
                DivineVeil = 1362,
                Hallowed = 82;
        }

        public static class PLDActions
        {
            public const uint
                Sentinel = 17,
                Bulwark = 22,
                DivineVeil = 3540,
                Hallowed = 30;
        }

        private static List<FieldInfo> GetConstants(this Type type)
        {
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public |
                 BindingFlags.Static | BindingFlags.FlattenHierarchy);

            return fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
        }

        public static bool JustUsedCooldown()
        {
            if (ActionWatching.CombatActions.Count == 0) return false;

            foreach (var act in typeof(AllActions).GetConstants())
            {
                uint val = (uint)act.GetValue(null)!;

                if (CustomComboFunctions.WasLastAction(val))
                    return true;
            }

            switch (CustomComboFunctions.LocalPlayer.ClassJob.Id)
            {
                case WAR.JobID:
                    foreach (var act in typeof(WARActions).GetConstants())
                    {
                        uint val = (uint)act.GetValue(null)!;

                        if (CustomComboFunctions.WasLastAction(val))
                            return true;
                    }
                    break;
                case PLD.JobID:
                    foreach (var act in typeof(PLDActions).GetConstants())
                    {
                        uint val = (uint)act.GetValue(null)!;

                        if (CustomComboFunctions.WasLastAction(val))
                            return true;
                    }
                    break;
            }

            return false;
        }

        public static bool HasCooldownBuff()
        {
            foreach (var buff in typeof(AllBuffs).GetConstants())
            {
                ushort val = (ushort)buff.GetValue(null)!;

                if (CustomComboFunctions.HasEffect(val))
                    return true;
            }

            switch (CustomComboFunctions.LocalPlayer.ClassJob.Id)
            {
                case WAR.JobID:
                    foreach (var buff in typeof(WARBuffs).GetConstants())
                    {
                        ushort val = (ushort)buff.GetValue(null)!;

                        if (CustomComboFunctions.HasEffect(val))
                            return true;
                    }
                    break;
                case PLD.JobID:
                    foreach (var buff in typeof(PLDBuffs).GetConstants())
                    {
                        ushort val = (ushort)buff.GetValue(null)!;

                        if (CustomComboFunctions.HasEffect(val))
                            return true;
                    }
                    break;
            }

            return false;
        }

        public static int NumberOfCooldownsActive()
        {
            int output = 0;

            foreach (var buff in typeof(AllBuffs).GetConstants())
            {
                ushort val = (ushort)buff.GetValue(null)!;

                if (CustomComboFunctions.HasEffect(val))
                    output++;
            }

            switch (CustomComboFunctions.LocalPlayer.ClassJob.Id)
            {
                case WAR.JobID:
                    foreach (var buff in typeof(WARBuffs).GetConstants())
                    {
                        ushort val = (ushort)buff.GetValue(null)!;

                        if (CustomComboFunctions.HasEffect(val))
                            output++;
                    }
                    break;
                case PLD.JobID:
                    foreach (var buff in typeof(PLDBuffs).GetConstants())
                    {
                        ushort val = (ushort)buff.GetValue(null)!;

                        if (CustomComboFunctions.HasEffect(val))
                            output++;
                    }
                    break;
            }

            return output;
        }

        public static int GetMatchingConfig(int i, bool isST, out uint action)
        {
            if (CustomComboFunctions.LocalPlayer.ClassJob.Id == WAR.JobID)
            {
                switch (i)
                {
                    case 0:
                        action = AllActions.Rampart;
                        return isST ? WAR.Config.WAR_Adv_Cooldowns_Rampart : WAR.Config.WAR_AoE_Adv_Cooldowns_Rampart;
                    case 1:
                        action = AllActions.ArmsLength;
                        return isST ? WAR.Config.WAR_Adv_Cooldowns_ArmsLength : WAR.Config.WAR_AoE_Adv_Cooldowns_ArmsLength;
                    case 2:
                        action = AllActions.Reprisal;
                        return isST ? WAR.Config.WAR_Adv_Cooldowns_Reprisal : WAR.Config.WAR_AoE_Adv_Cooldowns_Reprisal;
                    case 3:
                        action = WARActions.ThrillOfBattle;
                        return isST ? WAR.Config.WAR_Adv_Cooldowns_ThrillOfBattle : WAR.Config.WAR_AoE_Adv_Cooldowns_ThrillOfBattle;
                    case 4:
                        action = WARActions.Vengeance;
                        return isST ? WAR.Config.WAR_Adv_Cooldowns_Vengeance : WAR.Config.WAR_AoE_Adv_Cooldowns_Vengeance;
                    case 5:
                        action = CustomComboFunctions.OriginalHook(WARActions.RawIntuition);
                        return isST ? WAR.Config.WAR_Adv_Cooldowns_Intuition : WAR.Config.WAR_AoE_Adv_Cooldowns_Intuition;
                    case 6:
                        action = WARActions.ShakeItOff;
                        return isST ? WAR.Config.WAR_Adv_Cooldowns_Shake : WAR.Config.WAR_AoE_Adv_Cooldowns_Shake;
                    case 7:
                        action = WARActions.Equilibrium;
                        return isST ? WAR.Config.WAR_Adv_Cooldowns_Equilibrium : WAR.Config.WAR_AoE_Adv_Cooldowns_Equilibrium;
                }
            }

            if (CustomComboFunctions.LocalPlayer.ClassJob.Id == PLD.JobID)
            {
                switch (i)
                {
                    case 0:
                        action = AllActions.Rampart;
                        return isST ? PLD.Config.PLD_Adv_Cooldowns_Rampart : PLD.Config.PLD_AoE_Adv_Cooldowns_Rampart;
                    case 1:
                        action = AllActions.ArmsLength;
                        return isST ? PLD.Config.PLD_Adv_Cooldowns_ArmsLength : PLD.Config.PLD_AoE_Adv_Cooldowns_ArmsLength;
                    case 2:
                        action = AllActions.Reprisal;
                        return isST ? PLD.Config.PLD_Adv_Cooldowns_Reprisal : PLD.Config.PLD_AoE_Adv_Cooldowns_Reprisal;
                    case 3:
                        action = PLDActions.Sentinel;
                        return isST ? PLD.Config.PLD_Adv_Cooldowns_Sentinel : PLD.Config.PLD_AoE_Adv_Cooldowns_Sentinel;
                    case 4:
                        action = PLDActions.Bulwark;
                        return isST ? PLD.Config.PLD_Adv_Cooldowns_Bulwark : PLD.Config.PLD_AoE_Adv_Cooldowns_Bulwark;
                    case 5:
                        action = CustomComboFunctions.OriginalHook(PLDActions.DivineVeil);
                        return isST ? PLD.Config.PLD_Adv_Cooldowns_DivineVeil : PLD.Config.PLD_AoE_Adv_Cooldowns_DivineVeil;
                    case 6:
                        action = PLDActions.Hallowed;
                        return PLD.Config.PLD_AoE_Adv_Cooldowns_Hallowed;
                }
            }

            action = 0;
            return 0;
        }
    }
}
