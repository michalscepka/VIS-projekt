using System.Collections.Generic;

namespace DataLayer.TableDataGateways
{
	public interface IDataGW<T>
	{
		/// <summary>
		/// Načtení všech objektů z databáze do objektu typu DTO
		/// </summary>
		/// <param name="seznam">Seznam načtených objektů</param>
		/// <param name="errMsg">Chybové hlášení, pokud nastala chyba</param>
		/// <returns>True - načtení proběhlo bez chyby, False - chyba při načítání</returns>
		public bool LoadAll(out List<T> seznam, out string errMsg);

		/// <summary>
		/// Uložení všech objektů do Databáze, provede jejich Insert/Update a to v DB transakci
		/// </summary>
		/// <param name="seznam">Seznam objektů, které chcecem uložit do DB</param>
		/// <param name="errMsg">Chybové hlášení pokud nastala chyba</param>
		/// <returns>True - operace se provedla, False - nastala chyba</returns>
		public bool SaveAll(List<T> seznam, out string errMsg);

		/// <summary>
		/// Vložení nebo aktualizace objektu v DB
		/// </summary>
		/// <param name="entity">Objekt, který chceme insertnout/updatnout</param>
		/// <param name="errMsg">Chybové hlášení pokud nastala chyba</param>
		/// <returns>True nebyla chyba, False chyba nastala</returns>
		public bool InsertOrUpdate(T entity, out string errMsg);

		/// <summary>
		/// Smazání objektu z DB uložiště
		/// </summary>
		/// <param name="id">ID objektu</param>
		/// <param name="errMsg">Chybové hlášení</param>
		/// <returns>True smazání se povedlo, False nepovedlo</returns>
		public bool Delete(int id, out string errMsg);

		/// <summary>
		/// Nalezení objektu na základě jeho ID
		/// </summary>
		/// <param name="id">Hledané ID</param>
		/// <param name="errMsg">Chybové hlášení</param>
		/// <returns>TRUE hledání se provedlo, FALSE nastala chyba hledání</returns>
		public bool Find(int id, out T entity, out string errMsg);
	}
}
