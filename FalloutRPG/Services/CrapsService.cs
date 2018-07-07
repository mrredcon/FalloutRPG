using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FalloutRPG.Services
{
    public class CrapsService
    {
        private readonly GamblingService _gamblingService;

        private readonly Random _random;

        private List<IUser> players;
        private List<Bet> bets;

        private IUser shooter;
        private int _shooterIndex;

        private Round round;
        private int shooterPoint;

        public CrapsService(GamblingService gamblingService)
        {
            players = new List<IUser>();
            bets = new List<Bet>();

            round = Round.ComeOut;
            shooterPoint = -1;

            _shooterIndex = 0;

            _gamblingService = gamblingService;

            _random = new Random();
        }
        public string SpewGutsOut()
        {
            StringBuilder result = new StringBuilder();
            result.Append("Okay, I'll talk, just don't hurt me!\n" +
            "The shooter is: " + shooter.Mention + "\n" +
            "Shooter index is " + _shooterIndex + "\n" +
            "The state of the round is: " + round.ToString() + "\n" +
            "The shooter's point is " + shooterPoint + "\n" +
            "Players:\n");

            foreach (var player in players)
            {
                result.Append(player.Mention + "\n");
            }

            result.Append("Active bets:\n");

            foreach (var bet in bets.ToList())
            {
                result.Append(bet.User.Mention + " " + bet.BetType.ToString() + " " + bet.BetAmount + "\n");
            }

            return result.ToString();
        }
        private string AwardComeBets(int roll)
        {
            string result = "";

            foreach (var bet in bets.ToList())
            {
                if (bet.BetType == BetType.Come)
                {
                    if (roll == bet.Point) // come point achieved
                    {
                        AwardBet(bet);
                        result += bet.User.Mention + " reached their Point!\n";
                    }
                    else if (bet.Point == -1) // "come out" roll for new Come Bet
                    {
                        if (roll == 7 || roll == 11) // natural
                        {
                            AwardBet(bet);
                            result += bet.User.Mention + " got a Natural!\n";
                        }
                        else if (roll == 2 || roll == 3 || roll == 12) // crap out
                        {
                            AwardBet(bet, false);
                            result += bet.User.Mention + " crapped out!\n";
                        }
                        else // point
                        {
                            bet.Point = roll;
                            result += bet.User.Mention + "'s point is: " + roll + "\n";
                        }
                    }
                }
                else if (bet.BetType == BetType.DontCome)
                {
                    if (roll == bet.Point) // point achieved
                    {
                        AwardBet(bet, false);
                        result += bet.User.Mention + " reached their Point but they betted against it!\n";
                    }
                    else if (bet.Point != -1 && roll == 7) // seven out
                    {
                        AwardBet(bet);
                        result += bet.User.Mention + " sevened out but they were counting on it!\n";
                    }
                    else if (bet.Point == -1) // come out roll
                    {
                        if (roll == 2 || roll == 3) // crap out
                        {
                            AwardBet(bet);
                            result += bet.User.Mention + " crapped out but they were counting on it!\n";
                        }
                        else if (roll == 12) // crap out push (house always wins)
                        {
                            AwardBet(bet, false, true);
                            result += bet.User.Mention + " would've made some money, but the House Always Wins!\n";
                        }
                        else if (roll == 7 || roll == 11) // natural
                        {
                            AwardBet(bet, false);
                            result += bet.User.Mention + " got a Natural, but they betted against it!\n";
                        }
                        else
                        {
                            bet.Point = roll; // point
                            result += bet.User.Mention + "'s point is: " + roll + "\n";
                        }
                    }
                }
            }

            return result;
        }
        private string AdvanceRound(int roll)
        {
            string result = "";

            if (round == Round.ComeOut)
            {
                if (roll == 2 || roll == 3 || roll == 12) // crap out
                {
                    AwardBets(BetType.DontPass);
                    result += shooter.Mention + " crapped out!\n";
                }
                else if (roll == 7 || roll == 11) // natural
                {
                    AwardBets(BetType.Pass);
                    result += shooter.Mention + " rolled a Natural!\n";
                }
                else // point
                {
                    shooterPoint = roll;
                    round = Round.Point;
                    result += shooter.Mention + " advanced the round into the Point!\n";
                }
            }
            else if (round == Round.Point)
            {
                if (roll == 7) // seven out
                {
                    AwardBets(BetType.DontPass);
                    //AwardBets(BetType.dontcome);
                    result += shooter.Mention + " sevened out!\n";
                    round = Round.ComeOut;
                    NextShooter();
                    result += "New shooter: " + shooter.Mention + "\n";
                }
                else if (roll == shooterPoint)
                {
                    AwardBets(BetType.Pass);
                    round = Round.ComeOut;
                    result += shooter.Mention + " rolled the point!\n";
                }
            }
            result += AwardComeBets(roll);
            return result;
        }
        private void AwardBet(Bet bet, bool win = true, bool push = false)
        {
            var balances = _gamblingService.UserBalances;
            if (win)
            {
                balances[bet.User] += bet.BetAmount;
                bets.Remove(bet);
            }
            else
            {
                balances[bet.User] -= bet.BetAmount;
                bets.Remove(bet);
            }
        }
        private void AwardBets(BetType winningBet)
        {
            BetType notWinningBet = BetType.Error;
            switch (winningBet)
            {
                case BetType.Pass:
                    notWinningBet = BetType.DontPass;
                    break;
                case BetType.DontPass:
                    notWinningBet = BetType.Pass;
                    break;
                case BetType.Come:
                    notWinningBet = BetType.DontCome;
                    break;
                case BetType.DontCome:
                    notWinningBet = BetType.Come;
                    break;
                default:
                    break;
            }
            foreach (var bet in bets.ToList())
            {
                if (bet.BetType == winningBet)
                {
                    _gamblingService.UserBalances[bet.User] += bet.BetAmount;
                    bets.Remove(bet);
                }
                else if (bet.BetType == notWinningBet)
                {
                    _gamblingService.UserBalances[bet.User] -= bet.BetAmount;
                    bets.Remove(bet);
                }
            }
        }
        public string Roll(IUser user)
        {
            if (user != shooter)
                return user.Mention + ", you are not the shooter! (Join the match and wait.)";
            if (bets.Find(x => x.User.Equals(user)) == null)
                return user.Mention + ", you do not have a bet placed!";

            string result = "";

            int dice1 = _random.Next(1, 7),
                dice2 = _random.Next(1, 7),
                sum = dice1 + dice2;

            result = DiceToEmoji(dice1) + " " + DiceToEmoji(dice2) + "\n";

            return result + AdvanceRound(sum);
        }
        public bool JoinMatch(IUser user)
        {
            if (!_gamblingService.AddUserBalance(user))
            {
                return false;
            }
            players.Add(user);

            if (players.Count == 1)
                shooter = user;

            return true;
        }
        public bool NextShooter()
        {
            IUser oldShooter = shooter;

            if (players.Count > _shooterIndex + 1)
                shooter = players[++_shooterIndex];
            else
                _shooterIndex = 0;

            if (shooter != oldShooter)
                return true;
            else
                return false;
        }
        public string PlaceBet(IUser user, string betToPlace, int betAmount)
        {
            if (bets.Find(x => x.User.Equals(user)) != null) // bet already placed
                return user.Mention + ", you already have a bet!";

            if (!_gamblingService.UserBalances.ContainsKey(user)) // can't find user in dictionary
                if (!_gamblingService.AddUserBalance(user)) // failed to add user
                    return user.Mention + ", I failed to add your balance to the directory! (Do you have a character?)";

            if (betAmount > _gamblingService.UserBalances[user])
                return user.Mention + ", you don't have enough money for that bet!";

            if (betAmount <= 0)
                return user.Mention + ", you have to bet something!";

            betToPlace = betToPlace.ToLower();
            BetType betType = BetType.Error;

            if (betToPlace == "pass")
                betType = BetType.Pass;
            if (betToPlace == "dontpass")
                betType = BetType.DontPass;
            if (betToPlace == "come")
                betType = BetType.Come;
            if (betToPlace == "dontcome")
                betType = BetType.DontCome;

            if (betType != BetType.Error)
            {
                if (round == Round.ComeOut)
                {
                    // can't place a Come bet during the come out round
                    if (betType == BetType.Come || betType == BetType.DontCome)
                        return user.Mention + ", you can't place a Come bet when the Point isn't set!";
                }
                else if (round == Round.Point)
                {
                    // can't place a Pass bet during the point round
                    if (betType == BetType.Pass || betType == BetType.DontPass)
                        return user.Mention + ", you can't place a Pass or Don't Pass bet after the Point has been set!";
                }

                bets.Add(new Bet(user, betAmount, betType));

                return user.Mention + ", bet placed!";
            }
            else
            {
                // failed to parse bet type
                return user.Mention + ", valid bet types are: 'pass', 'dontpass', 'come', or 'dontcome' (without single quotes.)";
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
            Error,
            Pass,
            DontPass,
            Come,
            DontCome
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
                BetType = betType;
                Point = -1;
            }
        }
    }
}
