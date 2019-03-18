using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GameList.Db.Domain.Base
{
	public class Entity
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
	}
}
