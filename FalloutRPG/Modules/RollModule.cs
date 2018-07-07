using Discord;
using Discord.Commands;
using FalloutRPG.Constants;
using FalloutRPG.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules
{
    [Group("roll")]
    [Alias("r")]
    public class RollModule : ModuleBase<SocketCommandContext>
    {
        private readonly RollService _rollService;

        public RollModule(RollService rollService)
        {
            _rollService = rollService;
        }

        #region SPECIAL Commands
        [Command("strength")]
        [Alias("str")]
        public async Task RollStrength() => await ReplyAsync(_rollService.GetSpRoll(Context.User, "Strength"));

        [Command("perception")]
        [Alias("per")]
        public async Task RollPerception() => await ReplyAsync(_rollService.GetSpRoll(Context.User, "Perception")); 

        [Command("endurance")]
        [Alias("end")]
        public async Task RollEndurance() => await ReplyAsync(_rollService.GetSpRoll(Context.User, "Endurance")); 

        [Command("charisma")]
        [Alias("cha")]
        public async Task RollCharisma() => await ReplyAsync(_rollService.GetSpRoll(Context.User, "Charisma")); 

        [Command("intelligence")]
        [Alias("int")]
        public async Task RollIntelligence() => await ReplyAsync(_rollService.GetSpRoll(Context.User, "Intelligence")); 

        [Command("agility")]
        [Alias("agi")]
        public async Task RollAgility() => await ReplyAsync(_rollService.GetSpRoll(Context.User, "Agility")); 

        [Command("luck")]
        [Alias("luc", "lck")]
        public async Task RollLuck() => await ReplyAsync(_rollService.GetSpRoll(Context.User, "Luck")); 
        #endregion

        #region Skills Commands
        [Command("barter")]
        public async Task RollBarter() => await ReplyAsync(_rollService.GetSkillRoll(Context.User, "Barter")); 

        [Command("energy weapons")]
        [Alias("energy weapon", "energyweapons", "energyweapon", "energy")]
        public async Task RollEnergyWeapons() => await ReplyAsync(_rollService.GetSkillRoll(Context.User, "EnergyWeapons")); 

        [Command("explosives")]
        public async Task RollExplosives() => await ReplyAsync(_rollService.GetSkillRoll(Context.User, "Explosives")); 

        [Command("guns")]
        public async Task RollGuns() => await ReplyAsync(_rollService.GetSkillRoll(Context.User, "Guns")); 

        [Command("lockpick")]
        public async Task RollLockpick() => await ReplyAsync(_rollService.GetSkillRoll(Context.User, "Lockpick")); 

        [Command("medicine")]
        [Alias("medic", "doctor")]
        public async Task RollMedicine() => await ReplyAsync(_rollService.GetSkillRoll(Context.User, "Medicine")); 

        [Command("meleeweapons")]
        [Alias("melee", "meleeweapon", "melee weapons", "melee weapons")]
        public async Task RollMeleeWeapons() => await ReplyAsync(_rollService.GetSkillRoll(Context.User, "MeleeWeapons")); 

        [Command("repair")]
        public async Task RollRepair() => await ReplyAsync(_rollService.GetSkillRoll(Context.User, "Repair")); 

        [Command("science")]
        public async Task RollScience() => await ReplyAsync(_rollService.GetSkillRoll(Context.User, "Science")); 

        [Command("sneak")]
        public async Task RollSneak() => await ReplyAsync(_rollService.GetSkillRoll(Context.User, "Sneak"));

        [Command("speech")]
        public async Task RollSpeech() => await ReplyAsync(_rollService.GetSkillRoll(Context.User, "Speech"));

        [Command("survival")]
        public async Task RollSurvival() => await ReplyAsync(_rollService.GetSkillRoll(Context.User, "Survival"));

        [Command("unarmed")]
        public async Task RollUnarmed() => await ReplyAsync(_rollService.GetSkillRoll(Context.User, "Unarmed"));
        #endregion
    }
}
