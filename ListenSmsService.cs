using Android.App;
using Android.Gms.Auth.Api.Phone;
using Android.Gms.Tasks;
using Java.Lang;

namespace YoutubeVideoPlayingTest
{
    public class ListenSmsService
    {
        public void ListenToSmsRetriever()
        {
            SmsRetrieverClient client = SmsRetriever.GetClient(Application.Context);
            var task = client.StartSmsRetriever();
            task.AddOnSuccessListener(new SuccessListener());
            task.AddOnFailureListener(new FailureListener());
        }
        private class SuccessListener : Object, IOnSuccessListener
        {
            public void OnSuccess(Object result)
            {
            }
        }
        private class FailureListener : Object, IOnFailureListener
        {
            public void OnFailure(Exception e)
            {
            }
        }
    }
}