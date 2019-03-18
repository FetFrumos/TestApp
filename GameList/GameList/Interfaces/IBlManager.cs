namespace GameList.Interfaces
{
	public interface IBlManager
	{
		void Connect();
		void Disconnect();
		bool Send(string data);
		string Resive();
	}
}