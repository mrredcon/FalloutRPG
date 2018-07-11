using Discord;
using Discord.Commands;
using FalloutRPG.Constants;
using FalloutRPG.Helpers;
using FalloutRPG.Services.Roleplay;
using System.Threading.Tasks;

namespace FalloutRPG.Modules
{
    [Group("admin")]
    [Alias("adm")]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireOwner]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        private readonly CharacterService _charService;
        private readonly SkillsService _skillsService;
        private readonly SpecialService _specialService;

        public AdminModule(CharacterService charService,
            SkillsService skillsService,
            SpecialService specialService)
        {
            _charService = charService;
            _skillsService = skillsService;
            _specialService = specialService;
        }

        [Command("givemoney")]
        public async Task GiveMoneyAsync(IUser user, int money)
        {
            if (money < 1) return;

            var character = await _charService.GetCharacterAsync(user.Id);
            if (character == null) return;

            character.Money += money;

            await _charService.SaveCharacterAsync(character);
            await ReplyAsync(string.Format(Messages.ADM_GAVE_MONEY, Context.User.Mention));
        }

        [Command("giveskillpoints")]
        public async Task GiveSkillPointsAsync(IUser user, int points)
        {
            if (points < 1) return;

            var character = await _charService.GetCharacterAsync(user.Id);
            if (character == null) return;

            character.SkillPoints += points;

            await _charService.SaveCharacterAsync(character);
            await ReplyAsync(string.Format(Messages.ADM_GAVE_SKILL_POINTS, Context.User.Mention));
        }

        [Command("givespecialpoints")]
        public async Task GiveSpecialPointsAsync(IUser user, int points)
        {
            if (points < 1) return;

            var character = await _charService.GetCharacterAsync(user.Id);
            if (character == null) return;

            character.SpecialPoints += points;

            await _charService.SaveCharacterAsync(character);
            await ReplyAsync(string.Format(Messages.ADM_GAVE_SPEC_POINTS, Context.User.Mention));
        }

        [Command("changename")]
        public async Task ChangeCharacterNameAsync(IUser user, string firstName, string lastName)
        {
            var character = await _charService.GetCharacterAsync(user.Id);
            if (character == null) return;

            if (!StringHelper.IsOnlyLetters(firstName) || !StringHelper.IsOnlyLetters(lastName))
                return;

            if (firstName.Length > 24 || lastName.Length > 24 || firstName.Length < 2 || lastName.Length < 2)
                return;

            character.FirstName = StringHelper.ToTitleCase(firstName);
            character.LastName = StringHelper.ToTitleCase(lastName);

            await _charService.SaveCharacterAsync(character);
            await ReplyAsync(string.Format(Messages.ADM_CHANGED_NAME, Context.User.Mention));
        }
        
        [Command("reset")]
        public async Task ResetCharacterAsync(IUser user)
        {
            var character = await _charService.GetCharacterAsync(user.Id);
            if (character == null) return;

            await _charService.ResetCharacterAsync(character);
            await ReplyAsync(string.Format(Messages.ADM_RESET, Context.User.Mention));
        }

        [Command("delete")]
        public async Task DeleteCharacterAsync(IUser user)
        {
            var character = await _charService.GetCharacterAsync(user.Id);
            if (character == null) return;

            await _charService.DeleteCharacterAsync(character);
            await ReplyAsync(string.Format(Messages.ADM_DELETE, Context.User.Mention));
        }
    }
}
