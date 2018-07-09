using Discord.Commands;
using FalloutRPG.Services;
using FalloutRPG.Services.Roleplay;
using System.Threading.Tasks;

namespace FalloutRPG.Modules.Roleplay
{
    [Group("roll")]
    [Alias("r")]
    public class RollModule : ModuleBase<SocketCommandContext>
    {
        private readonly RollService _rollService;
        private readonly HelpService _helpService;

        public RollModule(RollService rollService, HelpService helpService)
        {
            _rollService = rollService;
            _helpService = helpService;
        }

        [Command]
        [Alias("help")]
        public async Task ShowRollHelpAsync()
        {
            await _helpService.ShowRollHelpAsync(Context);
        }

        #region SPECIAL Commands
        [Command("strength")]
        [Alias("str")]
        public async Task RollStrength() => await ReplyAsync(await _rollService.GetSpRollAsync(Context.User, "Strength"));

        [Command("perception")]
        [Alias("per")]
        public async Task RollPerception() => await ReplyAsync(await _rollService.GetSpRollAsync(Context.User, "Perception")); 

        [Command("endurance")]
        [Alias("end")]
        public async Task RollEndurance() => await ReplyAsync(await _rollService.GetSpRollAsync(Context.User, "Endurance")); 

        [Command("charisma")]
        [Alias("cha")]
        public async Task RollCharisma() => await ReplyAsync(await _rollService.GetSpRollAsync(Context.User, "Charisma")); 

        [Command("intelligence")]
        [Alias("int")]
        public async Task RollIntelligence() => await ReplyAsync(await _rollService.GetSpRollAsync(Context.User, "Intelligence")); 

        [Command("agility")]
        [Alias("agi")]
        public async Task RollAgility() => await ReplyAsync(await _rollService.GetSpRollAsync(Context.User, "Agility")); 

        [Command("luck")]
        [Alias("luc", "lck")]
        public async Task RollLuck() => await ReplyAsync(await _rollService.GetSpRollAsync(Context.User, "Luck")); 
        #endregion

        #region Skills Commands
        [Command("barter")]
        public async Task RollBarter() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Barter")); 

        [Command("energy weapons")]
        [Alias("energy weapon", "energyweapons", "energyweapon", "energy")]
        public async Task RollEnergyWeapons() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "EnergyWeapons")); 

        [Command("explosives")]
        public async Task RollExplosives() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Explosives")); 

        [Command("guns")]
        public async Task RollGuns() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Guns")); 

        [Command("lockpick")]
        public async Task RollLockpick() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Lockpick")); 

        [Command("medicine")]
        [Alias("medic", "doctor")]
        public async Task RollMedicine() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Medicine")); 

        [Command("meleeweapons")]
        [Alias("melee", "meleeweapon", "melee weapons", "melee weapons")]
        public async Task RollMeleeWeapons() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "MeleeWeapons")); 

        [Command("repair")]
        public async Task RollRepair() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Repair")); 

        [Command("science")]
        public async Task RollScience() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Science")); 

        [Command("sneak")]
        public async Task RollSneak() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Sneak"));

        [Command("speech")]
        public async Task RollSpeech() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Speech"));

        [Command("survival")]
        public async Task RollSurvival() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Survival"));

        [Command("unarmed")]
        public async Task RollUnarmed() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Unarmed"));
        #endregion
    }
}
