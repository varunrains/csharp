namespace TradeBot.Helpers.MagicStrings
{
    internal static class BotCommands
    {
        //BOT LIFECYCLE COMMANDS
        //To start the BOT
        public const string START = "/START";
        //To Stop all the operations of the bot
        public const string KILL = "/KILL";

        //TRADING COMMANDS
        public const string SET_MTM = "MTM";
        public const string SET_UNDERLYING = "U";
        //Stop loss can be set based on MTM / Underlying -> SL-MTM or SL-U
        public const string SET_STOP_LOSS = "/SL-";
     
    }
}
