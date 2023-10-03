using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Logging;
using ECommons.DalamudServices;
using ECommons.GameFunctions;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using System.Collections.Generic;
using System.Linq;
using XIVSlothCombo.Services;
using BattleChara = Dalamud.Game.ClientState.Objects.Types.BattleChara;

namespace XIVSlothCombo.CustomComboNS.Functions
{
    internal abstract partial class CustomComboFunctions
    {
        /// <summary> Checks if player is in a party </summary>
        public static bool IsInParty() => (Service.PartyList.PartyId > 0);

        /// <summary> Gets the party list </summary>
        /// <returns> Current party list. </returns>
        public static List<BattleChara> GetPartyMembers()
        {
            List<BattleChara> party = Svc.Party.Any() ? Svc.Party.Where(x => x.GameObject is not null).Select(x => x.GameObject).Cast<BattleChara>().ToList() : Svc.Buddies.Where(x => x.GameObject is not null).Select(x => x.GameObject).Cast<BattleChara>().ToList();

            if (!party.Any(x => x.ObjectId == Svc.ClientState.LocalPlayer.ObjectId))
                party.Add(Svc.ClientState.LocalPlayer!);

            return party;
        }

        public unsafe static GameObject? GetPartySlot(int slot)
        {
            try
            {
                var o = slot switch
                {
                    1 => GetTarget(TargetType.Self),
                    2 => GetTarget(TargetType.P2),
                    3 => GetTarget(TargetType.P3),
                    4 => GetTarget(TargetType.P4),
                    5 => GetTarget(TargetType.P5),
                    6 => GetTarget(TargetType.P6),
                    7 => GetTarget(TargetType.P7),
                    8 => GetTarget(TargetType.P8),
                    _ => GetTarget(TargetType.Self),
                };
                long i = PartyTargetingService.GetObjectID(o);
                return Service.ObjectTable.Where(x => x.ObjectId == i).Any()
                    ? Service.ObjectTable.Where(x => x.ObjectId == i).First()
                    : null;
            }

            catch
            {
                return null;
            }
        }

        public unsafe static bool PartyInCombat()
        {
            var party = GetPartyMembers();
            foreach (var member in party)
            {
                if (member is null) continue;
                var baseChara = CharacterManager.Instance()->LookupBattleCharaByObjectId(member.ObjectId);
                //PluginLog.Debug($"{member.Name} {baseChara->Character.InCombat}");
                if (baseChara->Character.InCombat) return true;
            }

            return false;
        }
    }
}
