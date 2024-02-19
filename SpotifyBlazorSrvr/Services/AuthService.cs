using Microsoft.AspNetCore.Components;
using SpotifyAPI.Web;

namespace SpotifyBlazorSrvr.Services
{
	public class AuthService
	{
		public Uri _authUri;

		public NavigationManager navManager;

		public IConfiguration Configuration;

		public event Action OnAuthenticationChanged;

		public bool _isAuthed;

		public bool IsAuthed
		{
			get => _isAuthed;
			private set
			{
				_isAuthed = value;
				OnAuthenticationChanged?.Invoke();
			}
		}






		public AuthService(NavigationManager navManager, IConfiguration configuration)
		{
			this.navManager = navManager;
			Configuration = configuration;
		}


		public void CreateLoginUri()
		{
			var baseUri = navManager.ToAbsoluteUri(navManager.BaseUri);

			var loginRequest = new LoginRequest(baseUri, Configuration["SPOTIFY_CLIENT_ID"], LoginRequest.ResponseType.Token)
			{
				Scope = new[] { Scopes.PlaylistReadPrivate, Scopes.PlaylistReadCollaborative }
			};
			_authUri = loginRequest.ToUri();

		}
		public string GetAccessToken()
		{
			var uri = new Uri(navManager.Uri);
			var maxLen = Math.Min(1, uri.Fragment.Length);
			Dictionary<string, string> fragmentParams = uri.Fragment.Substring(maxLen)?
				.Split("&", StringSplitOptions.RemoveEmptyEntries)?
				.Select(param => param.Split("=", StringSplitOptions.RemoveEmptyEntries))?
				.ToDictionary(param => param[0], param => param[1]) ?? new Dictionary<string, string>();
			IsAuthed = fragmentParams.ContainsKey("access_token");
			return IsAuthed ? fragmentParams["access_token"] : null;
		}





	}
}
