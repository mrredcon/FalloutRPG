using Discord.Commands;
using FalloutRPG.Constants;
using FalloutRPG.Helpers;
using FalloutRPG.Services;
using FalloutRPG.Services.Roleplay;
using System;
using System.Threading.Tasks;

namespace FalloutRPG.Modules.Roleplay
{
    [Group("npc")]
    public class NpcModule : ModuleBase<SocketCommandContext>
    {
        private readonly NpcService _npcService;
        private readonly HelpService _helpService;

        public NpcModule(NpcService npcService, HelpService helpService)
        {
            _npcService = npcService;
            _helpService = helpService;
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

        [Command("skills")]
        [Alias("skill")]
        public async Task ShowSkillsAsync(string name)
        {
            var userInfo = Context.User;
            var character = _npcService.FindNpc(name);

            if (character == null)
            {
                await ReplyAsync(
                    string.Format(Messages.ERR_NPC_CHAR_NOT_FOUND, name) + $" {Context.User.Mention}");
                return;
            }

            var embed = EmbedHelper.BuildBasicEmbed("NPC Skills",
                $"**Name:** {character.FirstName}\n" +
                $"**Barter:** {character.Skills.Barter}\n" +
                $"**Energy Weapons:** {character.Skills.EnergyWeapons}\n" +
                $"**Explosives:** {character.Skills.Explosives}\n" +
                $"**Guns:** {character.Skills.Guns}\n" +
                $"**Lockpick:** {character.Skills.Lockpick}\n" +
                $"**Medicine:** {character.Skills.Medicine}\n" +
                $"**Melee Weapons:** {character.Skills.MeleeWeapons}\n" +
                $"**Repair:** {character.Skills.Repair}\n" +
                $"**Science:** {character.Skills.Science}\n" +
                $"**Sneak:** {character.Skills.Sneak}\n" +
                $"**Speech:** {character.Skills.Speech}\n" +
                $"**Survival:** {character.Skills.Survival}\n" +
                $"**Unarmed:** {character.Skills.Unarmed}");

            await ReplyAsync(userInfo.Mention, embed: embed);
        }

        [Command("special")]
        [Alias("specials")]
        public async Task ShowSpecialAsync(string name)
        {
            var userInfo = Context.User;
            var character = _npcService.FindNpc(name);

            if (character == null)
            {
                await ReplyAsync(
                    string.Format(Messages.ERR_NPC_CHAR_NOT_FOUND, name) + $" {Context.User.Mention}");
                return;
            }

            var embed = EmbedHelper.BuildBasicEmbed("NPC S.P.E.C.I.A.L.:",
                $"**Name:** {character.FirstName}\n" +
                $"**STR:** {character.Special.Strength}\n" +
                $"**PER:** {character.Special.Perception}\n" +
                $"**END:** {character.Special.Endurance}\n" +
                $"**CHA:** {character.Special.Charisma}\n" +
                $"**INT:** {character.Special.Intelligence}\n" +
                $"**AGI:** {character.Special.Agility}\n" +
                $"**LUC:** {character.Special.Luck}");

            await ReplyAsync(userInfo.Mention, embed: embed);
        }

        [Command]
        [Alias("help")]
        public async Task ShowCharacterHelpAsync()
        {
            await _helpService.ShowNpcHelpAsync(Context);
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
