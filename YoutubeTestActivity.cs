using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using YouTube.Player;

namespace YoutubeVideoPlayingTest
{
    [Activity(Label = "YoutubeTestActivity")]
    public class YoutubeTestActivity : YouTubeBaseActivity, IYouTubePlayerOnInitializedListener, IYouTubePlayerPlayerStateChangeListener
    {
        #region MyRegion
        private const int RecoveryRequest = 1;
        private YouTubePlayerView youtubeView;
        private bool IsInitialized;
        private IYouTubePlayer Player { get; set; }

        public void OnInitializationFailure(IYouTubePlayerProvider provider, YouTubeInitializationResult errorReason)
        {
            if (errorReason.IsUserRecoverableError)
            {
                errorReason.GetErrorDialog(this, RecoveryRequest).Show();
            }
            else
            {
                var error = errorReason.ToString();
                Toast.MakeText(this, error, ToastLength.Long).Show();
            }
        }

        public void OnInitializationSuccess(IYouTubePlayerProvider provider, IYouTubePlayer player, bool wasRestored)
        {
            if (!wasRestored)
            {
                this.IsInitialized = true;
                this.Player = player;
                this.Player.SetPlayerStateChangeListener(this);
                //player.CueVideo("o9P4B0iPHpI"); // Plays https://www.youtube.com/watch?v=o9P4B0iPHpI
                //player.LoadVideo("o9P4B0iPHpI");
            }
        }
        #endregion

        private ImageView imgView1;
        private ImageView imgView2;
        private View videoLayer;
        private View baseView;

        private string videoId1 = "o9P4B0iPHpI";
        private string videoId2 = "N7LJM4AJYTk";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_youtube_test);

            imgView1 = FindViewById<ImageView>(Resource.Id.imgView1);
            imgView2 = FindViewById<ImageView>(Resource.Id.imgView2);
            videoLayer = FindViewById<View>(Resource.Id.videoLayer);
            baseView = FindViewById<View>(Resource.Id.baseView);

            SetImages();

            imgView1.Click += ImgView1_Click;
            imgView2.Click += ImgView2_Click;

            videoLayer.Click += VideoLayer_Click;

            youtubeView = FindViewById<YouTubePlayerView>(Resource.Id.youtubeView);
            youtubeView.Initialize(Constant.YoutubeApiKey, this);
        }

        private void VideoLayer_Click(object sender, EventArgs e)
        {
            videoLayer.Visibility = ViewStates.Gone;
            baseView.Visibility = ViewStates.Visible;
            this.Player.Pause();
        }

        private void ImgView1_Click(object sender, EventArgs e)
        {
            videoLayer.Visibility = ViewStates.Visible;
            baseView.Visibility = ViewStates.Gone;
            PlayVideo(videoId1);
        }

        private void ImgView2_Click(object sender, EventArgs e)
        {
            videoLayer.Visibility = ViewStates.Visible;
            baseView.Visibility = ViewStates.Gone;
            PlayVideo(videoId2);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == RecoveryRequest)
            {
                //youtubeView1.Initialize(Constant.YoutubeApiKey, this);
            }
        }

        private async Task<Bitmap> GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = await webClient.DownloadDataTaskAsync(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        private async Task<Bitmap> GetYoutubeVideoThumbnail(string videoId)
        {
            var url = $"https://img.youtube.com/vi/{videoId}/0.jpg";
            var bitmap = await GetImageBitmapFromUrl(url);
            return bitmap;
        }

        private async Task SetImages()
        {
            var bm = await GetYoutubeVideoThumbnail(videoId1);
            RunOnUiThread(() => imgView1.SetImageBitmap(bm));

            bm = await GetYoutubeVideoThumbnail(videoId2);
            RunOnUiThread(() => imgView2.SetImageBitmap(bm));
        }

        private void PlayVideo(string videoId)
        {
            if (this.IsInitialized)
            {
                this.Player.LoadVideo(videoId);
                this.Player.Play();
            }
        }

        public static string GetTitle(string videoId)
        {
            var url = $"https://www.youtube.com/watch?v={videoId}";
            var api = $"http://youtube.com/get_video_info?video_id={videoId}";
            return new WebClient().DownloadString(api);
        }

        #region IYouTubePlayerPlayerStateChangeListener
        public void OnAdStarted()
        {
        }

        public void OnError(YouTubePlayerErrorReason p0)
        {
        }

        public void OnLoaded(string p0)
        {
            this.Player.SeekToMillis(30000);
        }

        public void OnLoading()
        {
        }

        public void OnVideoEnded()
        {
        }

        public void OnVideoStarted()
        {
        }

        #endregion IYouTubePlayerPlayerStateChangeListener
    }
}