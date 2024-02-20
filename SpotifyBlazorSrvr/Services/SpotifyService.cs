using MetaBrainz.MusicBrainz;
using Serilog;
using SpotifyAPI.Web;
namespace SpotifyBlazorSrvr.Services
{



	public class SpotifyService
	{



		private SpotifyClient? client;

		public SpotifyService(AuthService authService)
		{


			client = new SpotifyClient(authService.GetAccessToken());

		}


		public int GetRandomNumberRange(int min, int max)
		{

			Random random = new Random();
			int randomNumber = random.Next(min, max);
			return randomNumber;
		}

		public string GetRandomCharacter()
		{
			char[] alpha = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZñÑαβγδεζηθικλμνξοπρστυφχψωАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ!@#$%^&*?0123456789אבגדהוזחטיכלמנסעפצקרשתابتثجحخدذرزسشصضطظعغفقكلمنهويअआइईउऊएऐओऔकखगघचछजझटठडढणतथदधनपफबभमयरलवशषसहঅআইঈউঊあいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをんアイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワヲン가나다라마바사아자차카타파하爱北从到额发个好见可乐吗你哦跑去人是他我想要在ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðòóôõöøùúûüýþÿЁЂЃЄЅІЇЈЉЊЋЌЎЏёђѓєѕіїјљњћќўџॐक़ख़ग़ज़ड़ढ़फ़य़ਅਆਇਈਉਊਏਐਓਔਕਖਗਘਙਚਛਜਝਞਟਠਡਢਣਤਥਦਧਨਪਫਬਭਮਯਰਲ".ToCharArray();

			var randomIndex = GetRandomNumberRange(0, alpha.Length);

			string randomChar = alpha[randomIndex].ToString();

			return randomChar;
		}

		public string GetRandomSongTitle()
		{
			var q = new Query();


			int randomOffset = GetRandomNumberRange(1, 10000);



			var musicBrainSearchResult = q.FindRecordings(GetRandomCharacter(), 100, 0, true);

			var randomSongTitle = musicBrainSearchResult.Results.ToList()[GetRandomNumberRange(1, musicBrainSearchResult.Results.Count)].Item.Title;

			return randomSongTitle ?? "No song found";
		}
		public async Task<FullTrack?> GetRandomTrackAsync()
		{
			try
			{
				var songTitle = GetRandomSongTitle();
				if (string.IsNullOrEmpty(songTitle))
				{
					Log.Information("No song title was found.");
					return null;
				}

				var response = await client!.Search.Item(
					new SearchRequest(SearchRequest.Types.Track, songTitle)
					{
						Limit = 50
					});

				if (response.Tracks != null && response.Tracks.Items.Any())
				{
					var randomOffset = GetRandomNumberRange(0, response.Tracks.Items.Count - 1);
					var track = await client.Tracks.Get(response.Tracks.Items[randomOffset].Id);

					Log.Information("Tracks returned: {NumberOfTracks} Offset number: {randomOffset}, Track: {trackName}",
						response.Tracks.Items.Count, randomOffset, track.Name);
					return track;
				}
				else
				{
					Log.Information("No tracks were found for the given title: {SongTitle}", songTitle);
					return null;
				}
			}
			catch (Exception e)
			{
				Log.Error("An exception occurred in GetRandomTrackAsync: {ExceptionMessage}", e.Message);

				return null;
			}
		}




	}
}
