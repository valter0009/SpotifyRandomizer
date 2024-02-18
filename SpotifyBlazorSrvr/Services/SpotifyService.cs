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


			client = new SpotifyClient(authService.AuthCallback());

		}

		public int GetRandomNumberRange(int min, int max)
		{

			Random random = new Random();
			int randomNumber = random.Next(min, max);
			return randomNumber;
		}

		public string GetRandomCharacter()
		{
			char[] alpha = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZñÑαβγδεζηθικλμνξοπρστυφχψωАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ!@#$%^&*?0123456789אבגדהוזחטיכלמנסעפצקרשתابتثجحخدذرزسشصضطظعغفقكلمنهويअआइईउऊएऐओऔकखगघचछजझटठडढणतथदधनपफबभमयरलवशषसहঅআইঈউঊঋএঐওঔকখগঘঙচছজঝঞটঠডঢণতথদধনপফবভমযরলশষসহあいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをんアイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワヲン가나다라마바사아자차카타파하爱北从到额发个好见可乐吗你哦跑去人是他我想要在ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðòóôõöøùúûüýþÿЁЂЃЄЅІЇЈЉЊЋЌЎЏёђѓєѕіїјљњћќўџॐक़ख़ग़ज़ड़ढ़फ़य़ਅਆਇਈਉਊਏਐਓਔਕਖਗਘਙਚਛਜਝਞਟਠਡਢਣਤਥਦਧਨਪਫਬਭਮਯਰਲ".ToCharArray();

			var randomIndex = GetRandomNumberRange(0, alpha.Length);

			string randomChar = alpha[randomIndex].ToString();

			return randomChar;
		}

		public string GetRandomSongTitle()
		{
			var q = new Query();


			int randomOffset = GetRandomNumberRange(1, 10000);



			var musicBrainSearchResult = q.FindRecordings(GetRandomCharacter(), 1000000, 0, true);

			var randomSongTitle = musicBrainSearchResult.Results.ToList()[GetRandomNumberRange(1, musicBrainSearchResult.Results.Count)].Item.Title;

			return randomSongTitle ?? "No song found";
		}
		public async Task<FullTrack> GetRandomTrackAsync()
		{
			try
			{



				var response = await client!.Search.Item(
					new SearchRequest(SearchRequest.Types.Track, GetRandomSongTitle())
					{
						Limit = 50
					});
				if (response.Tracks != null && response.Tracks.Items.Count > 0)
				{

					var randomOffset = GetRandomNumberRange(0, response.Tracks.Items.Count);
					var track = await client.Tracks.Get(response.Tracks.Items[randomOffset].Id);

					var NumberofTracks = response.Tracks.Items.Count;

					var trackName = track.Name;

					Log.Information("Tracks returned: {NumberOfTracks} Offset number: {randomOffset}, Track: {trackName}",
						NumberofTracks, randomOffset.ToString(), trackName);
					return track;
				}
				return new FullTrack();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}


		public async Task<RecommendationGenresResponse> GetGenresAsync()
		{
			RecommendationGenresResponse genres = await client!.Browse.GetRecommendationGenres();
			return genres;
		}

	}
}
