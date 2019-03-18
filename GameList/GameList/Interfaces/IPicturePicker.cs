using System;
using System.IO;
using System.Threading.Tasks;

namespace GameList.Interfaces
{
	public interface IPicturePicker
	{
		Task<Tuple<Stream, string>> GetImageStreamAsync();
	}
}