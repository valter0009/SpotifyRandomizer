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
			string randomCharacter = GetRandomCharacter();
			int randomOffset = GetRandomNumberRange(1, 10000);

			var musicBrainSearchResult = q.FindRecordings(randomCharacter, 100, randomOffset, true);


			if (musicBrainSearchResult.Results.Count == 0)
			{
				return randomCharacter;
			}
			else
			{
				var randomSongTitle = musicBrainSearchResult.Results.ToList()[GetRandomNumberRange(0, musicBrainSearchResult.Results.Count)].Item.Title;
				return randomSongTitle;
			}
		}
		public async Task<FullTrack?> GetRandomTrackAsync()
		{
			int randomOffset = GetRandomNumberRange(1, 500);
			const int maxRetries = 5;
			int attempts = 0;
			while (attempts < maxRetries)
			{
				attempts++;
				try
				{
					var songTitle = GetRandomSongTitle();
					if (string.IsNullOrEmpty(songTitle))
					{
						Log.Information("No song title was found.");
						continue;
					}

					var response = await client!.Search.Item(new SearchRequest(SearchRequest.Types.Track, songTitle)
					{
						Limit = 50,
						Offset = randomOffset
					});

					if (response.Tracks != null && response.Tracks.Items.Any())
					{
						var randomIndex = GetRandomNumberRange(0, response.Tracks.Items.Count);
						var track = response.Tracks.Items[randomIndex];
						return track;
					}
					else
					{
						Log.Information("No tracks were found for the given title: {SongTitle}", songTitle);

					}
				}
				catch (Exception e)
				{
					Log.Error("An exception occurred in GetRandomTrackAsync: {ExceptionMessage}", e.Message);

				}
			}


			return null;
		}




	}
}
