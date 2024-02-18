using Microsoft.AspNetCore.Components;
using SpotifyAPI.Web;

namespace SpotifyBlazorSrvr.Services
{
	public class AuthService
	{
		public Uri _authUri;

		public bool _isAuthed;

		public NavigationManager navManager;

		public PrivateUser _me;

		public IConfiguration Configuration;




		public AuthService(NavigationManager navManager, IConfiguration configuration)
		{
			this.navManager = navManager;
			Configuration = configuration;
		}


		public void Login()
		{
			var baseUri = navManager.ToAbsoluteUri(navManager.BaseUri);

			var loginRequest = new LoginRequest(baseUri, Configuration["SPOTIFY_CLIENT_ID"], LoginRequest.ResponseType.Token)
			{
				Scope = new[] { Scopes.PlaylistReadPrivate, Scopes.PlaylistReadCollaborative }
			};
			_authUri = loginRequest.ToUri();

		}
		public string AuthCallback()
		{
			var uri = new Uri(navManager.Uri);
			var maxLen = Math.Min(1, uri.Fragment.Length);
			Dictionary<string, string> fragmentParams = uri.Fragment.Substring(maxLen)?
				.Split("&", StringSplitOptions.RemoveEmptyEntries)?
				.Select(param => param.Split("=", StringSplitOptions.RemoveEmptyEntries))?
				.ToDictionary(param => param[0], param => param[1]) ?? new Dictionary<string, string>();
			_isAuthed = fragmentParams.ContainsKey("access_token");
			return _isAuthed ? fragmentParams["access_token"] : null;
		}





	}
}
