using System;
using System.IO;
using System.Threading.Tasks;

namespace GameList.Interfaces
{
	public interface IAppFilePicker
	{
		Task<Tuple<string, Stream>> GetFile();
	}
}