using Discord.Commands;
using FalloutRPG.Addons;
using FalloutRPG.Constants;
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
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task RollStrength() => await ReplyAsync(await _rollService.GetSpRollAsync(Context.User, "Strength"));

        [Command("perception")]
        [Alias("per")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task RollPerception() => await ReplyAsync(await _rollService.GetSpRollAsync(Context.User, "Perception")); 

        [Command("endurance")]
        [Alias("end")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task RollEndurance() => await ReplyAsync(await _rollService.GetSpRollAsync(Context.User, "Endurance")); 

        [Command("charisma")]
        [Alias("cha")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task RollCharisma() => await ReplyAsync(await _rollService.GetSpRollAsync(Context.User, "Charisma")); 

        [Command("intelligence")]
        [Alias("int")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task RollIntelligence() => await ReplyAsync(await _rollService.GetSpRollAsync(Context.User, "Intelligence")); 

        [Command("agility")]
        [Alias("agi")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task RollAgility() => await ReplyAsync(await _rollService.GetSpRollAsync(Context.User, "Agility")); 

        [Command("luck")]
        [Alias("luc", "lck")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task RollLuck() => await ReplyAsync(await _rollService.GetSpRollAsync(Context.User, "Luck")); 
        #endregion

        #region Skills Commands
        [Command("barter")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task RollBarter() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Barter")); 

        [Command("energy weapons")]
        [Alias("energy weapon", "energyweapons", "energyweapon", "energy")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task RollEnergyWeapons() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "EnergyWeapons")); 

        [Command("explosives")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task RollExplosives() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Explosives")); 

        [Command("guns")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task RollGuns() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Guns")); 

        [Command("lockpick")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task RollLockpick() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Lockpick")); 

        [Command("medicine")]
        [Alias("medic", "doctor")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task RollMedicine() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Medicine")); 

        [Command("meleeweapons")]
        [Alias("melee", "meleeweapon", "melee weapons", "melee weapons")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task RollMeleeWeapons() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "MeleeWeapons")); 

        [Command("repair")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task RollRepair() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Repair")); 

        [Command("science")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task RollScience() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Science")); 

        [Command("sneak")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task RollSneak() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Sneak"));

        [Command("speech")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task RollSpeech() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Speech"));

        [Command("survival")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task RollSurvival() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Survival"));

        [Command("unarmed")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task RollUnarmed() => await ReplyAsync(await _rollService.GetSkillRollAsync(Context.User, "Unarmed"));
        #endregion
    }
}
