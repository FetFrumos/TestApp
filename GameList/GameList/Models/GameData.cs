using System;
using System.Collections.Generic;
using System.Text;
using GameList.Db.Domain;

namespace GameList.Models
{
    public class GameData
    {
	    public Game Game { get; set; }
	    public string Desc { get; set; }
	    public string  Image { get; set; }
    }
}
