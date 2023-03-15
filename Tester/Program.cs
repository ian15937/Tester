using SKCOMLib;

namespace Tester
{
    public class Reply
    {
        public Reply()
        {
            //如果將下面這行註解掉，SKQuoteLib_RequestStocks就會成功
            var nCode = CapitalInit.m_pSKReply.SKReplyLib_ConnectByID(CapitalInit.Account);
        }
    }
    public class CapitalInit
    {
        public static SKCenterLib m_pSKCenter = new SKCenterLib();
        public static SKQuoteLib m_pSKQuote;
        public static SKReplyLib m_pSKReply = new SKReplyLib();
        public const string Account = "", Password = "";
        public CapitalInit()
        {
            var login = new Login();
            var reply = new Reply();
        }
    }
    public class Login
    {
        public Login()
        {
            CapitalInit.m_pSKReply.OnReplyMessage += new _ISKReplyLibEvents_OnReplyMessageEventHandler(OnAnnouncement);
            var code = CapitalInit.m_pSKCenter.SKCenterLib_Login(CapitalInit.Account, CapitalInit.Password);
        }
        private void OnAnnouncement(string strUserID, string bstrMessage, out short nConfirmCode)
        {
            nConfirmCode = -1;
        }
    }
    public class CapitalQuoteObject
    {
        bool _ready = false;
        public CapitalQuoteObject()
        {
            CapitalInit.m_pSKQuote = new();
            CapitalInit.m_pSKQuote.OnConnection += new _ISKQuoteLibEvents_OnConnectionEventHandler(m_SKQuoteLib_OnConnection);
            var code = CapitalInit.m_pSKQuote.SKQuoteLib_EnterMonitorLONG();
            while (!_ready) {
                Thread.Sleep(100);
            }
        }
        private void m_SKQuoteLib_OnConnection(int nKind, int nCode)
        {
            switch (nKind) {
                case 3001:
                    if (nCode != 0) {
                        Console.WriteLine($"nKind={nKind}, nCode={nCode}");
                    }
                    break;
                case 3002:
                    Console.WriteLine($"nKind={nKind}, nCode={nCode}");
                    break;
                case 3003:
                    _ready = true;
                    break;
                case 3036:
                    break;
                default:
                    Console.WriteLine($"nKind={nKind}, nCode={nCode}");
                    break;
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var init = new CapitalInit();
            var quoteObject = new CapitalQuoteObject();
            short sPage = -1;
            var code = CapitalInit.m_pSKQuote.SKQuoteLib_RequestStocks(ref sPage, "TX00,2330");
        }
    }
}