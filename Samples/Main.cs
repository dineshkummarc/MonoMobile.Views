namespace Samples
{
	using MonoMobile.Views;
	
	public class Application : MonoMobileApplication
	{
		public new static void Main(string[] args)
		{
		 Run("Sample", typeof(MovieListView), args);
		//	Run("AppDelegate", args);
		}	
	}
}

