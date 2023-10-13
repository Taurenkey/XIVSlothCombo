using Dalamud.Game.ClientState.Objects.Types;
using ECommons.GameFunctions;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace XIVSlothCombo.Extensions
{
    internal static class BattleCharaExtensions
    {
        public unsafe static uint RawShieldValue(this BattleChara chara)
        {
            FFXIVClientStructs.FFXIV.Client.Game.Character.BattleChara* baseVal = (FFXIVClientStructs.FFXIV.Client.Game.Character.BattleChara*)chara.Address;
            var value = baseVal->Character.CharacterData.ShieldValue;
            var rawValue = chara.MaxHp / 100 * value;

            return rawValue;
        }

        public unsafe static byte ShieldPercentage(this BattleChara chara)
        {
            FFXIVClientStructs.FFXIV.Client.Game.Character.BattleChara* baseVal = (FFXIVClientStructs.FFXIV.Client.Game.Character.BattleChara*)chara.Address;
            var value = baseVal->Character.CharacterData.ShieldValue;

            return value;
        }

        public static bool HasShield(this BattleChara chara) => chara.RawShieldValue() > 0;

        public static float RemainingCastTime(this BattleChara chara) => chara.TotalCastTime - chara.CurrentCastTime;

        public static unsafe bool CanAutoAttack(this GameObject chara)
        {
            return ActionManager.CanUseActionOnTarget(7, chara.Struct());
        }
    }
}
