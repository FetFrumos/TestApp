using System;
using System.Collections.Generic;
using System.Text;
using GameList.Db.Domain.Base;
using SQLite;

namespace GameList.Db.Domain
{
	public class GameSync :Entity
	{
		[MaxLength(50)]
		public string AppId { get; set; }
		public int IdGame { get; set; }
		public int RootIdGame { get; set; }
	}
}
