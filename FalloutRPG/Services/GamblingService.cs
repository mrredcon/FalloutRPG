using Discord;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FalloutRPG.Services
{
    public class GamblingService
    {
        private List<ulong> _gamblingChannels;
        public readonly Dictionary<IUser, long> UserBalances;

        private readonly IConfiguration _config;

        public GamblingService(IConfiguration config)
        {
            UserBalances = new Dictionary<IUser, long>();

            _config = config;
            LoadGamblingEnabledChannels();
        }

        private void LoadGamblingEnabledChannels()
        {
            try
            {
                _gamblingChannels =
                    _config
                    .GetSection("gambling:enabled-channels")
                    .GetChildren()
                    .Select(x => UInt64.Parse(x.Value))
                    .ToList();
            }
            catch (Exception)
            {
                Console.WriteLine("You have not specified any gambling enabled channels in Config.json");
            }
        }
    }
    public class CrapsService
    {
        private readonly GamblingService _gamblingService;

        private readonly Random random;

        private List<IUser> players;
        private List<Bet> bets;

        private IUser shooter;
        private int _shooterIndex;
        
        private Round round;
        private int point;

        public CrapsService(GamblingService gamblingService)
        {
            players = new List<IUser>();
            bets = new List<Bet>();
            
            round = Round.ComeOut;
            point = -1;

            _shooterIndex = 0;

            _gamblingService = gamblingService;

            random = new Random();
        }
        public void AdvanceRound(RoundResult result)
        {
            switch (result)
            {
                case RoundResult.CrapOut:
                    {

                    }
                    break;
                case RoundResult.Natural:
                    {

                    }
                    break;
                case RoundResult.SevenOut:
                    {

                    }
                    break;
                default:
                    break;
            }
            if (_shooterIndex+1 < players.Count)
            {
                shooter = players[++_shooterIndex];
            }
            point = -1;

            var losingBets = new List<Bet>();
            var winningBets = new List<Bet>();

            foreach (var bet in bets)
            {

            }

            var shooterBet = bets.Find(x => x.User.Equals(shooter));
            _gamblingService.UserBalances[shooter] -= shooterBet.BetAmount;
            result += "Crapped out!";
        }
        public string Roll(IUser user)
        {
            if (user != shooter)
                return null;

            string result = "";

            int dice1 = random.Next(1, 7),
                dice2 = random.Next(1, 7),
                sum = dice1 + dice2;

            result = DiceToEmoji(dice1) + " " + DiceToEmoji(dice2) + "\n";

            // TODO: make a method that loops through all players and award or substract losing bets (RoundEnd())
            if (point == -1) // point not set
            {
                if (sum == 2 || sum == 3 || sum == 12) // crap out
                {
                    AdvanceRound();
                    result += "Crap out!";
                }
                else if (sum == 7 || sum == 1) // natural
                {
                    AdvanceRound();
                    var shooterBet = bets.Find(x => x.User.Equals(shooter));
                    _gamblingService.UserBalances[user] += shooterBet.BetAmount;
                    result += "Natural!";
                }
                else
                {
                    point = sum;
                    return result + "\nPoint is: " + point; // [5][3]   Point is 8.
                }
            }
            else // point set
            {
                if (sum == point)
                {
                    AdvanceRound();
                    var shooterBet = bets.Find(x => x.User.Equals(shooter));
                    _gamblingService.UserBalances[user] += shooterBet.BetAmount;
                    result += "Point achieved!";
                }
                else if (sum == 7) // seven out
                {
                    AdvanceRound();
                    var shooterBet = bets.Find(x => x.User.Equals(shooter));
                    _gamblingService.UserBalances[user] -= shooterBet.BetAmount;
                    result += "Seven out!";
                }
                else
                {
                    result += "Roll again!";
                }
            }
            return result;
        }
        public void JoinMatch(IUser user)
        {
            players.Add(user);

            if (players.Count == 1)
                shooter = user;

            _gamblingService.UserBalances.Add(user, 0);
        }
        public bool PlaceBet(IUser user, string betToPlace, int betAmount)
        {
            if (Enum.TryParse(betToPlace.ToLower(), out BetType betType))
            {
                if (round == Round.ComeOut)
                {
                    // can't place a Come bet during the come out round
                    if (betType == BetType.come || betType == BetType.dontcome)
                        return false;
                }
                //else if (_round == Round.Point)
                //{
                //    // can't place a Pass bet during the point round
                //    if (betType == BetType.pass || betType == BetType.dontpass)
                //        return false;
                //}
                if (betAmount <= 0)
                    return false;

                bets.Add(new Bet(user, betAmount, betType));
                return true;
            }
            else
            {
                // failed to parse bet type
                return false;
            }
        }
        private string DiceToEmoji(int num)
        {
            switch (num)
            {
                case 1:
                    return ":one:";
                case 2:
                    return ":two:";
                case 3:
                    return ":three:";
                case 4:
                    return ":four:";
                case 5:
                    return ":five:";
                case 6:
                    return ":six:";
            }
            return null;
        }
        private enum Round
        {
            ComeOut,
            Point
        }
        private enum RoundResult
        {
            CrapOut,
            Natural,
            SevenOut
        }
        public enum BetType
        {
            pass,
            dontpass,
            come,
            dontcome
        }
        private class Bet
        {
            public IUser User { get; }
            public int BetAmount { get; }
            public BetType BetType { get; }
            public int Point { get; set; }

            public Bet(IUser user, int betAmount, BetType betType)
            {
                User = user;
                BetAmount = betAmount;
                BetType = BetType;
            }
        }
    }
}
