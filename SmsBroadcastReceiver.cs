using System;
using Android.App;
using Android.Content;
using Android.Gms.Auth.Api.Phone;
using Android.Gms.Common.Apis;
using System.Linq;
using System.Text.RegularExpressions;

namespace YoutubeVideoPlayingTest
{
    [BroadcastReceiver(Enabled = false, Exported = true)]
    [IntentFilter(new[] {SmsRetriever.SmsRetrievedAction})]
    public class SmsBroadcastReceiver : BroadcastReceiver
    {
        private readonly string[] Keywords = { "Test App" };

        public event EventHandler<string> SmsCodeReceived;

        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                if (intent.Action != SmsRetriever.SmsRetrievedAction) return;
                var bundle = intent.Extras;
                if (bundle == null) return;
                var status = (Statuses) bundle.Get(SmsRetriever.ExtraStatus);
                switch (status.StatusCode)
                {
                    case CommonStatusCodes.Success:
                        // Get SMS message contents
                        var message = (string) bundle.Get(SmsRetriever.ExtraSmsMessage);
                        // Extract one-time code from the message and complete verification
                        // by sending the code back to your server.
                        var foundKeyword = Keywords.Any(k => message.Contains(k));
                        if (!foundKeyword) return;
                        var code = ExtractNumber(message);
                        this.SmsCodeReceived?.Invoke(this, code);
                        break;
                    case CommonStatusCodes.Timeout:
                        // Waiting for SMS timed out (5 minutes)
                        // Handle the error ...
                        break;
                }

            }
            catch
            {
                // ignored
            }
        }
        private static string ExtractNumber(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            var number = Regex.Match(text, @"\d+").Value;
            return number;
        }
    }
}