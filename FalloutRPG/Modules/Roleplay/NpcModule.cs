using Discord.Commands;
using FalloutRPG.Constants;
using FalloutRPG.Services.Roleplay;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules.Roleplay
{
    [Group("npc")]
    public class NpcModule : ModuleBase<SocketCommandContext>
    {
        private readonly NpcService _npcService;

        public NpcModule(NpcService npcService)
        {
            _npcService = npcService;
        }

        [Command("create")]
        [Alias("new")]
        public async Task CreateNewNpc(string type, string name)
        {
            try
            {
                _npcService.CreateNpc(type, name);
            }
            catch (Exception e)
            {
                await ReplyAsync(Messages.FAILURE_EMOJI + e.Message);
                return;
            }
            // used to show "Raider created" vs "raIDeR created" or whatever the user put in
            string prettyType = _npcService.IsValidNpcType(type).ToString();

            await ReplyAsync(String.Format(Messages.NPC_CREATED_SUCCESS, prettyType, name));
        }

        #region NPC Roll Commands
        [Group("roll")]
        public class NpcRollModule : ModuleBase<SocketCommandContext>
        {
            private readonly NpcService _npcService;

            public NpcRollModule(NpcService npcService)
            {
                _npcService = npcService;
            }

            #region SPECIAL Commands
            [Command("strength")]
            [Alias("str")]
            public async Task RollStrength(string name) => await ReplyAsync($"{_npcService.RollNpcSpecial(name, "Strength")}");

            [Command("perception")]
            [Alias("per")]
            public async Task RollPerception(string name) => await ReplyAsync($"{_npcService.RollNpcSpecial(name, "Perception")}");

            [Command("endurance")]
            [Alias("end")]
            public async Task RollEndurance(string name) => await ReplyAsync($"{_npcService.RollNpcSpecial(name, "Endurance")}");

            [Command("charisma")]
            [Alias("cha")]
            public async Task RollCharisma(string name) => await ReplyAsync($"{_npcService.RollNpcSpecial(name, "Charisma")}");

            [Command("intelligence")]
            [Alias("int")]
            public async Task RollIntelligence(string name) => await ReplyAsync($"{_npcService.RollNpcSpecial(name, "Intelligence")}");

            [Command("agility")]
            [Alias("agi")]
            public async Task RollAgility(string name) => await ReplyAsync($"{_npcService.RollNpcSpecial(name, "Agility")}");

            [Command("luck")]
            [Alias("luc", "lck")]
            public async Task RollLuck(string name) => await ReplyAsync($"{_npcService.RollNpcSpecial(name, "Luck")}");
            #endregion

            #region Skills Commands
            [Command("barter")]
            public async Task RollBarter(string name) => await ReplyAsync($"{_npcService.RollNpcSkill(name, "Barter")}");

            [Command("energy weapons")]
            [Alias("energy weapon", "energyweapons", "energyweapon", "energy")]
            public async Task RollEnergyWeapons(string name) => await ReplyAsync($"{_npcService.RollNpcSkill(name, "EnergyWeapons")}");

            [Command("explosives")]
            public async Task RollExplosives(string name) => await ReplyAsync($"{_npcService.RollNpcSkill(name, "Explosives")}");

            [Command("guns")]
            public async Task RollGuns(string name) => await ReplyAsync($"{_npcService.RollNpcSkill(name, "Guns")}");

            [Command("lockpick")]
            public async Task RollLockpick(string name) => await ReplyAsync($"{_npcService.RollNpcSkill(name, "Lockpick")}");

            [Command("medicine")]
            [Alias("medic", "doctor")]
            public async Task RollMedicine(string name) => await ReplyAsync($"{_npcService.RollNpcSkill(name, "Medicine")}");

            [Command("meleeweapons")]
            [Alias("melee", "meleeweapon", "melee weapons", "melee weapons")]
            public async Task RollMeleeWeapons(string name) => await ReplyAsync($"{_npcService.RollNpcSkill(name, "MeleeWeapons")}");

            [Command("repair")]
            public async Task RollRepair(string name) => await ReplyAsync($"{_npcService.RollNpcSkill(name, "Repair")}");

            [Command("science")]
            public async Task RollScience(string name) => await ReplyAsync($"{_npcService.RollNpcSkill(name, "Science")}");

            [Command("sneak")]
            public async Task RollSneak(string name) => await ReplyAsync($"{_npcService.RollNpcSkill(name, "Sneak")}");

            [Command("speech")]
            public async Task RollSpeech(string name) => await ReplyAsync($"{_npcService.RollNpcSkill(name, "Speech")}");

            [Command("survival")]
            public async Task RollSurvival(string name) => await ReplyAsync($"{_npcService.RollNpcSkill(name, "Survival")}");

            [Command("unarmed")]
            public async Task RollUnarmed(string name) => await ReplyAsync($"{_npcService.RollNpcSkill(name, "Unarmed")}");
            #endregion
        }
        #endregion
    }
}
