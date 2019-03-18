using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameList.Db.Domain;
using GameList.Db.Interfaces;
using GameList.Db.Models;

namespace GameList.Db
{
	public class DataService : IDataService
	{

		public  bool NeedInit { get; private set; }
		public async Task<int> SaveGame(Game game) 
		{
			await _rep.Save(game);
			return game.Id;
		}

		public async Task SaveDesc(GameDescription desc, int id)
		{
			desc.IdGame = id;
			await _rep.Save(desc);
		}

		public async Task<Tuple<Game, GameDescription>> GetGameData(int id)
		{
			var game = await _rep.FirstOrDefault<Game>(i => i.Id == id);
			var desc = await _rep.FirstOrDefault<GameDescription>(i => i.IdGame == id);
			var res = new Tuple<Game, GameDescription>(game, desc);
			return res;
		}

		public async Task DeleteGame(int id)
		{
			var desc = await _rep.FirstOrDefault<GameDescription>(i => i.IdGame == id);
			if(desc!=null)
			await _rep.Delete(desc);

			var game = await _rep.FirstOrDefault<Game>(i => i.Id == id);
			await _rep.Delete(game);
		}

		private readonly DataRepository _rep;
		public DataService()
		{
			_rep =  new DataRepository();
			NeedInit = true;
		}

		public async Task Init(string path)
		{
			await _rep.CreateDatabaseAsync(path);
			NeedInit = false;
		}

		public async Task<List<GameTitle>> GetTitles()
		{
			var games = await _rep.GetAll<Game>();
			var res = games.Select(i => new GameTitle
			{
				Name = i.Name,
				Id = i.Id,
				Autor = i.Author,
			}).ToList();
			var ids = res.Select(i => i.Id).Distinct();
			var gameDescs = await _rep.GetAll<GameDescription>();
			var images = gameDescs.Select(i => new
			{
				i.IdGame,
				i.Image
			}).Where(i=>ids.Contains(i.IdGame));
			foreach (var game in res)
			{
				var currImage = images.FirstOrDefault(i => i.IdGame == game.Id);
				if (currImage != null)
				{
					game.ImagePath = currImage.Image;
				}
			}
			return res;
		}
	}
}
