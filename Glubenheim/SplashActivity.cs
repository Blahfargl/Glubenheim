namespace Glubenheim{
	using System.Threading;
	using Android.App;
	using Android.OS;

	[Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
	public class SplashActivity : Activity {
		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);
			Thread.Sleep(2000); // Simulate a long loading process on app startup.
			StartActivity(typeof(MainActivity));
		}
	}
}