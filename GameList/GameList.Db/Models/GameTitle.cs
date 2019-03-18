using System;
using System.Collections.Generic;
using System.Text;

namespace GameList.Db.Models
{
    public class GameTitle
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Autor { get; set; }
		public string ImagePath { get; set; }
	}
}
