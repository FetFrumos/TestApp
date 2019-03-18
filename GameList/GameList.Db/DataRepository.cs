using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GameList.Db.Domain;
using GameList.Db.Domain.Base;
using GameList.Db.Service;
using SQLite;

namespace GameList.Db
{
	public class DataRepository
	{
		private SQLiteAsyncConnection _db;
		private static readonly AsyncLock Mutex = new AsyncLock();

		public async Task CreateDatabaseAsync(string path)
		{
			using (await Mutex.LockAsync().ConfigureAwait(false))
			{
				_db = new SQLiteAsyncConnection(path);
				await _db.CreateTableAsync<Game>();
				await _db.CreateTableAsync<GameDescription>();
				await _db.CreateTableAsync<GameSync>();
			}
		}

		public async Task<int> CountAsync<T>() where T : Entity, new()
		{
			using (await Mutex.LockAsync().ConfigureAwait(false))
			{
				return await _db.Table<T>().CountAsync();
			}
		}

		public async Task<int> CountAsyncWithPredicate<T>(Expression<Func<T, bool>> predicate) where T : Entity, new()
		{
			using (await Mutex.LockAsync().ConfigureAwait(false))
			{
				return await _db.Table<T>()
					.Where(predicate)
					.CountAsync();
			}
		}

		public async Task Save<T>(T entity) where T : Entity, new()
		{
			using (await Mutex.LockAsync().ConfigureAwait(false))
			{
				await _db.InsertAsync(entity);
			}
		}

		public async Task Delete(Entity item)
		{
			using (await Mutex.LockAsync().ConfigureAwait(false))
			{
				await _db.DeleteAsync(item);
			}
		}

		public async Task Update<T>(T entity) where T : Entity, new()
		{
			using (await Mutex.LockAsync().ConfigureAwait(false))
			{
				await _db.UpdateAsync(entity);
			}
		}

		public async Task UpdateAll<T>(List<T> entity) where T : Entity, new()
		{
			using (await Mutex.LockAsync().ConfigureAwait(false))
			{
				await _db.UpdateAllAsync(entity);
			}
		}

		public async Task SaveAll<T>(List<T> entity) where T : Entity, new()
		{
			using (await Mutex.LockAsync().ConfigureAwait(false))
			{
				await _db.InsertAllAsync(entity);
			}
		}

		public async Task<T> ExecScalarSql<T>(string sql)
		{
			using (await Mutex.LockAsync().ConfigureAwait(false))
			{
				return await _db.ExecuteScalarAsync<T>(sql);
			}
		}

		public async Task<T> ExecuteScalarAsync<T>(string query, params object[] args)
		{
			using (await Mutex.LockAsync().ConfigureAwait(false))
			{
				return await _db.ExecuteScalarAsync<T>(query, args);
			}
		}

		public async Task<List<T>> GetAll<T>() where T : Entity, new()
		{
			using (await Mutex.LockAsync().ConfigureAwait(false))
			{
				var res = await _db.Table<T>().ToListAsync();
				return res;
			}
		}

		public async Task<List<T>> GetAll<T>(Expression<Func<T, bool>> predicate) where T : Entity, new()
		{
			try
			{
				using (await Mutex.LockAsync().ConfigureAwait(false))
				{
					return await _db.Table<T>().Where(predicate).ToListAsync();
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				throw;
			}
		}

		public async Task<T> GetById<T>(int id) where T : Entity, new()
		{
			using (await Mutex.LockAsync().ConfigureAwait(false))
			{
				return await _db.Table<T>().Where(i => i.Id == id).FirstOrDefaultAsync();
			}
		}

		public async Task<T> FirstOrDefault<T>(Expression<Func<T, bool>> predicate) where T : Entity, new()
		{
			using (await Mutex.LockAsync().ConfigureAwait(false))
			{
				return await _db.Table<T>().Where(predicate).FirstOrDefaultAsync();
			}
		}

		public async Task<T> FirstOrDefault<T>() where T : Entity, new()
		{
			using (await Mutex.LockAsync().ConfigureAwait(false))
			{
				return await _db.Table<T>().FirstOrDefaultAsync();
			}
		}

		public async Task<List<T>> SelectAsync<T>(string sql) where T : Entity, new()
		{
			using (await Mutex.LockAsync().ConfigureAwait(false))
			{
				return await _db.QueryAsync<T>(sql);
			}
		}
	}
}
