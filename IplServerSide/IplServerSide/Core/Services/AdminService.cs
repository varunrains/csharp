using System;
using IplServerSide.Core.Repositories;
using IplServerSide.Dtos;
using IplServerSide.Models;
using System.Collections.Generic;
using System.Linq;
using WebPush;

namespace IplServerSide.Core.Services
{
    public class AdminService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void UpdateResult(MatchDto matchDetails)
        {
            var bets = _unitOfWork.Bets.Find(bet => bet.MatchId == matchDetails.MatchId).ToList();
            if (matchDetails.IsMatchAbandoned)
            {
                UpdateResultForAbandonedMatch(bets);
                return;
            }
            UpdateMatchResult(matchDetails);
            UpdateBettingDetails(bets, matchDetails);
            UpdateUsersWallet(bets);

            _unitOfWork.Complete();
        }

        public void RemoveAllBets()
        {
            return;
            //var bets = _unitOfWork.Bets.GetAll();
            //_unitOfWork.Bets.RemoveRange(bets);
            //_unitOfWork.Complete();
        }

        public void SendNotifications()
        {
            var pushEndpoint = @"https://updates.push.services.mozilla.com/wpush/v2/gAAAAABffvNfQBFET4_UJ4BvERuieBAbQdARo_b4ITWhzt2rKfrTFZqlGvlmXhinlVPRJAh0MQ3psmdm0LxWpsX6NuG7jbUP13cVPiXrqU0SvxZt0Xzrify5j6mvj-J3SP8VirRMrRB0GOUT2MRtge4TKPaLrKNFyKPZL156UErRqNB82j47qPQ";
            var p256dh = @"BFSKNo0F7rYR8jWI1pzwh-HcKK1A9pR7rsRejutIxbuAaQSnCgypByTu29HWpqXHcSu57uPHrv-GeXmHjMqrfAE";
            var auth = @"zlYiYpf1-QRyy-uFtDFv8g";

            var subject = @"mailto:example@example.com";
            var publicKey = @"BDk-QSoBtl9BNgY5mGVzP9iiUtAdQFPsnUl3VwnZz05zO54OsS_d8yiBxrdubhd236AWcx0E2E0JFfVo1t-sc0E";
            var privateKey = @"LaUAkhjAM7G16wNevX0CJTQPzpLFrI_jR0mYpK9IXeI";

            var subscription = new PushSubscription(pushEndpoint, p256dh, auth);
            var vapidDetails = new VapidDetails(subject, publicKey, privateKey);
            //var gcmAPIKey = @"[your key here]";
            var notification = "{\"notification\":{\"title\":\"Hi Varun\",\"body\":\"Today's winner is Rama!!\",\"icon\":\"https://www.shareicon.net/data/256x256/2015/10/02/110808_blog_512x512.png \",\"vibrate\":[100,50,100],\"data\":{\"url\":\"https://iplbet.tk\"}}}";
            var webPushClient = new WebPushClient();
            try
            {
                webPushClient.SendNotification(subscription, notification, vapidDetails);
                //webPushClient.SendNotification(subscription, "payload", gcmAPIKey);
            }
            catch (WebPushException exception)
            {
                Console.WriteLine("Http STATUS code" + exception.StatusCode);
            }
        }

        #region Private methods
        private void UpdateResultForAbandonedMatch(List<Bet> bets)
        {
            bets.ForEach(bet =>
            { 
                bet.NetAmountWon = bet.BetAmount;
            });
            UpdateUsersWallet(bets);
            _unitOfWork.Complete();
        }
    

        private void UpdateMatchResult(MatchDto matchDetails)
        {
            var match = _unitOfWork.Matches.Get(matchDetails.MatchId);
            match.Result = matchDetails.Result;
        }

        private void UpdateBettingDetails(List<Bet> bets, MatchDto matchDetails)
        {
            var totalBetAmountOfMatch = (decimal) bets.Sum(bet => bet.BetAmount);
            var winnersAmount = (decimal)bets.Where(bet => bet.BettingTeamId == matchDetails.Result).Sum(bet => bet.BetAmount);
            var losersAmount = totalBetAmountOfMatch - winnersAmount;
            bets.ForEach(bet =>
            {
                bet.WinningTeamId = matchDetails.Result;
                if (bet.WinningTeamId == bet.BettingTeamId)
                {
                    bet.NetAmountWon = bet.BetAmount / winnersAmount * losersAmount;
                }
                else
                {
                    bet.NetAmountWon = -1 * bet.BetAmount;
                }

            });
        }

        private void UpdateUsersWallet(List<Bet> bets)
        {
            var bettingUserIds = bets.Select(x => x.UserId);
            var users = _unitOfWork.Users.Find(user => bettingUserIds.Contains(user.UserId)).ToList();
            users.ForEach(user =>
            {
                var amt = bets.FirstOrDefault(x => x.UserId == user.UserId && x.NetAmountWon > 0);
                if (amt != null)
                    user.UserAmount += amt.BetAmount + amt.NetAmountWon.GetValueOrDefault();
            });

        }
        #endregion



    }
}