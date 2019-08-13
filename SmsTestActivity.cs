using System;
using Android.App;
using Android.Content;
using Android.Gms.Auth.Api.Phone;
using Android.OS;
using Android.Widget;
using YoutubeVideoPlayingTest.Helpers;

namespace YoutubeVideoPlayingTest
{
    [Activity(Label = "SmsTestActivity")]
    public class SmsTestActivity : Activity
    {
        private SmsBroadcastReceiver _receiver = new SmsBroadcastReceiver();
        private TextView txtCode;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.activity_sms_test);

            var txtHashKey = FindViewById<TextView>(Resource.Id.txtHashKey);
            txtHashKey.Text = AppHashKeyHelper.GetAppHashKey(this);

            var btnWaitSms = FindViewById(Resource.Id.btnWaitSms);
            btnWaitSms.Click += btnWaitSmsOnClick;

            var intentFilter = new IntentFilter();
            intentFilter.AddAction(SmsRetriever.SmsRetrievedAction);
            RegisterReceiver(_receiver, intentFilter);
            _receiver.SmsCodeReceived += receiver_SmsCodeReceived;

            txtCode = FindViewById<TextView>(Resource.Id.txtCode);
        }

        private void receiver_SmsCodeReceived(object sender, string e)
        {
            txtCode.Text = e;
        }

        private void btnWaitSmsOnClick(object sender, EventArgs e)
        {
            var service = new ListenSmsService();
            service.ListenToSmsRetriever();
        }
    }
}