using System;
using System.Collections.Generic;
using System.Text;
using GameList.Db.Domain.Base;
using SQLite;

namespace GameList.Db.Domain
{
	public class GameDescription : Entity
	{
		public string Description { get; set; }
		[MaxLength(200)]
		public string Image { get; set; }

		public int IdGame { get; set; }
	}
}
