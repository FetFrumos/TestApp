using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameList.Db.Domain;
using GameList.Db.Models;

namespace GameList.Db.Interfaces
{
	public interface IDataService
	{
		Task Init(string path);
		Task<List<GameTitle>> GetTitles();
		bool NeedInit { get; }

		Task<int> SaveGame(Game game);
		Task DeleteGame(int id);

		Task SaveDesc(GameDescription desc, int id);

		Task<Tuple<Game, GameDescription>> GetGameData(int id);
	}
}