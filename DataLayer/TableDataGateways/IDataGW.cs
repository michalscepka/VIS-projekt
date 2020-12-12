using System.Collections.Generic;

namespace DataLayer.TableDataGateways
{
	public interface IDataGW<T>
	{
		public bool LoadAll(out List<T> seznam, out string errMsg);
		public bool SaveAll(List<T> seznam, out string errMsg);
		public bool InsertOrUpdate(T entity, out string errMsg);
		public bool Delete(int id, out string errMsg);
		public bool Find(int id, out T entity, out string errMsg);
	}
}
