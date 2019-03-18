using System;
using System.Collections.Generic;
using System.Text;
using GameList.Db.Domain.Base;
using SQLite;

namespace GameList.Db.Domain
{
	//в этой таблице только поля для поиска +ид добавил также индекс
	//описание и путь к рисунку в отдельной таблице
	public class Game : Entity
	{
		[MaxLength(200)]
		[Indexed]
		public string Name { get; set; }
		[MaxLength(200)]
		[Indexed]
		public string Author { get; set; }
	}
}
