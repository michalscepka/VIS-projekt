using DTO.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataLayer.TableDataGateways
{
	public class RezervaceGW : IDataGW<RezervaceDTO>
	{
		private static readonly object m_LockObj = new object();
		private static RezervaceGW m_Instance;

		public static RezervaceGW Instance
		{
			get
			{
				lock (m_LockObj)
				{
					return m_Instance ??= new RezervaceGW();
				}
			}
		}

		private RezervaceGW()
		{

		}

		/// <summary>
		/// Načtení všech zaměstnanců z databáze do objektu RezervaceDTO
		/// </summary>
		/// <param name="seznam">List načtených zaměstnanců</param>
		/// <param name="errMsg">Chybové hlášení, pokud nastala chyba</param>
		/// <returns>True - načtení proběhlo bez chyby, False - chyba při načítání</returns>
		public bool LoadAll(out List<RezervaceDTO> seznam, out string errMsg)
		{
			seznam = null;
			errMsg = string.Empty;

			var sql = "SELECT id, datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id FROM rezervace";

			//Nacteni zamestnance z uloziste
			try
			{
				Database.Instance.Connect();
				try
				{
					SqlCommand sqlCmd = Database.Instance.CreateCommand(sql);
					try
					{
						try
						{
							var result = Database.Instance.Select(sqlCmd);
							seznam = new List<RezervaceDTO>();
							if (result.HasRows)
							{
								while (result.Read())
								{
									RezervaceDTO rezervace = new RezervaceDTO()
									{
										Id = result.GetInt32(result.GetOrdinal("id")),
										DatumZacatkuRezervace = result.GetDateTime(result.GetOrdinal("datum_zacatku")),
										DatumKonceRezervace = result.GetDateTime(result.GetOrdinal("datum_konce")),
										Cena = result.GetInt32(result.GetOrdinal("cena")),
										Kauce = result.GetInt32(result.GetOrdinal("kauce")),
										ZakaznikId = result.GetInt32(result.GetOrdinal("zakaznik_id")),
										VozidloId = result.GetInt32(result.GetOrdinal("vozidlo_id"))
									};
									seznam.Add(rezervace);
								};
							}
						}
						catch (Exception e)
						{
							//nastala chyba vykonání SELECT
							seznam = null;
							errMsg = $"Chyba při SELECT tabulky zaměstnanců \n{e.Message}";
							return false;
						}
					}
					finally
					{
						sqlCmd.Dispose();
					}
				}
				finally
				{
					Database.Instance.Close();
				}
			}
			catch (Exception e)
			{
				errMsg = $"Chyba při Connection do DB \n{e.Message}";
				return false;
			}
			return true;
		}

		/// <summary>
		/// Uložení všech objektů zaměstnanci do Databáze. Provede jejich Insert/Update a to v DB transakci
		/// </summary>
		/// <param name="seznam">List zaměstnanců, které chcecme uložit do DB</param>
		/// <param name="errMsg">Chybové hlášení pokud nastala chyba</param>
		/// <returns>True - operace se provedla, False - nastala chyba</returns>
		public bool SaveAll(List<RezervaceDTO> seznam, out string errMsg)
		{
			errMsg = string.Empty;

			string sqlInsert =
				"INSERT INTO rezervace(datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id) " +
				"VALUES (@datum_zacatku, @datum_konce, @cena, @kauce, @zakaznik_id, @vozidlo_id)";
			string sqlUpdate =
				"UPDATE rezervace SET datum_zacatku=@datum_zacatku, datum_konce=@datum_konce, cena=@cena, kauce=@kauce, zakaznik_id=@zakaznik_id, vozidlo_id=@vozidlo_id " +
				"WHERE id=@id";

			//Vlozeni nebo aktualizace zamestnance v ulozisti
			try
			{
				Database.Instance.Connect();
				try
				{
					Database.Instance.BeginTransaction();
					for (int i = 0; i < seznam.Count; i++)
					{
						var sql = seznam[i].Id < 0 ? sqlInsert : sqlUpdate;
						SqlCommand sqlCmd = Database.Instance.CreateCommand(sql);
						try
						{
							sqlCmd.Parameters.AddWithValue("@id", seznam[i].Id);
							sqlCmd.Parameters.AddWithValue("@datum_zacatku", seznam[i].DatumZacatkuRezervace);
							sqlCmd.Parameters.AddWithValue("@datum_konce", seznam[i].DatumKonceRezervace);
							sqlCmd.Parameters.AddWithValue("@cena", seznam[i].Cena);
							sqlCmd.Parameters.AddWithValue("@kauce", seznam[i].Kauce);
							sqlCmd.Parameters.AddWithValue("@zakaznik_id", seznam[i].ZakaznikId);
							sqlCmd.Parameters.AddWithValue("@vozidlo_id", seznam[i].VozidloId);
							try
							{
								var result = Database.Instance.ExecuteNonQuery(sqlCmd);
								//Pokud je návratová hodnota záporná nepovedlo se vložit/upravit
								if (result <= 0)
									throw new DataException($"Nepovedlo se uložit Knihu ID:({seznam[i].Id})");
							}
							catch (Exception e)
							{
								//nastala chyba vykonání INSERT/UPDATE - vrátíme změny v DB
								Database.Instance.Rollback();
								errMsg = $"Chyba při ukládání objektů Kniha \n{e.Message}";
								return false;
							}
						}
						finally
						{
							sqlCmd.Dispose();
						}
					}
					Database.Instance.EndTransaction();
				}
				finally
				{
					Database.Instance.Close();
				}
			}
			catch (Exception e)
			{
				errMsg = $"Chyba při Connection do DB \n{e.Message}";
				return false;
			}
			return true;
		}

		/// <summary>
		/// Vložení nebo aktualizace zaměstnance v DB
		/// </summary>
		/// <param name="entity">Pokud je záporné ID, pak se jedná o nového zamestnance a bude vytvořen, id po návratu obsahujé přidělené ID z DB, jinak se provede update</param>
		/// <param name="errMsg">Chybové hlášení pokud nastala chyba</param>
		/// <returns>True nebyla chyba, False chyba nastala</returns>
		public bool InsertOrUpdate(RezervaceDTO entity, out string errMsg)
		{
			errMsg = string.Empty;

			string sqlInsert =
				"SET NOCOUNT ON; INSERT INTO rezervace(datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id) " +
				"VALUES (@datum_zacatku, @datum_konce, @cena, @kauce, @zakaznik_id, @vozidlo_id); SELECT SCOPE_IDENTITY(); SET NOCOUNT OFF;";
			string sqlUpdate =
				"UPDATE rezervace SET datum_zacatku=@datum_zacatku, datum_konce=@datum_konce, cena=@cena, kauce=@kauce, zakaznik_id=@zakaznik_id, vozidlo_id=@vozidlo_id " +
				"WHERE id=@id";
			string sql = entity.Id < 0 ? sqlInsert : sqlUpdate;

			//Vlozeni nebo aktualizace zamestnance v ulozisti
			try
			{
				Database.Instance.Connect();
				try
				{
					Database.Instance.BeginTransaction();
					SqlCommand sqlCmd = Database.Instance.CreateCommand(sql);
					try
					{
						sqlCmd.Parameters.AddWithValue("@id", entity.Id);
						sqlCmd.Parameters.AddWithValue("@datum_zacatku", entity.DatumZacatkuRezervace);
						sqlCmd.Parameters.AddWithValue("@datum_konce", entity.DatumKonceRezervace);
						sqlCmd.Parameters.AddWithValue("@cena", entity.Cena);
						sqlCmd.Parameters.AddWithValue("@kauce", entity.Kauce);
						sqlCmd.Parameters.AddWithValue("@zakaznik_id", entity.ZakaznikId);
						sqlCmd.Parameters.AddWithValue("@vozidlo_id", entity.VozidloId);
						try
						{
							var result = -1;
							if (entity.Id == -1)
							{
								result = Database.Instance.ExecuteScalar(sqlCmd);
							}
							else
							{
								result = Database.Instance.ExecuteNonQuery(sqlCmd);
							}

							//Pokud je návratová hodnota záporná nepovedlo se vložit/upravit
							if (result <= 0)
								throw new DataException($"Nepovedlo se vložit/upravit Uživatele ID:({entity.Id})");
							Database.Instance.EndTransaction();
							if (entity.Id < 0)
								entity.Id = result;  //id vytvořené na DB serveru a přidělené novému záznamu v DB
						}
						catch (Exception e)
						{
							//nastala chyba vykonání INSERT/UPDATE - vrátíme změny v DB
							Database.Instance.Rollback();
							errMsg = $"Chyba při INSERT/UPDATE objektu Uživatele \n{e.Message}";
							return false;
						}
					}
					finally
					{
						sqlCmd.Dispose();
					}
				}
				finally
				{
					Database.Instance.Close();
				}
			}
			catch (Exception e)
			{
				errMsg = $"Chyba při Connection do DB \n{e.Message}";
				return false;
			}
			return true;
		}

		/// <summary>
		/// Smazání zaměstnance z DB uložiště
		/// </summary>
		/// <param name="id">ID zaměstnance</param>
		/// <param name="errMsg">Chybové hlášení</param>
		/// <returns>True smazání se povedlo, False nepovedlo</returns>
		public bool Delete(int id, out string errMsg)
		{
			errMsg = string.Empty;

			string sql = "DELETE FROM rezervace WHERE id=@id";

			//Smazani zamestnance z uloziste
			try
			{
				Database.Instance.Connect();
				try
				{
					Database.Instance.BeginTransaction();
					SqlCommand sqlCmd = Database.Instance.CreateCommand(sql);
					try
					{
						sqlCmd.Parameters.AddWithValue("@id", id);
						try
						{
							var result = Database.Instance.ExecuteNonQuery(sqlCmd);
							//Pokud je návratová hodnota záporná nepovedlo se smazat uživatele
							if (result <= 0)
								throw new DataException($"Nepovedlo se smazat Uživatele ID:({id})");

							Database.Instance.EndTransaction();
						}
						catch (Exception e)
						{
							//nastala chyba vykonání DELETE - vrátíme změny v DB
							Database.Instance.Rollback();
							errMsg = $"Chyba při DELETE objektu Uživatele \n{e.Message}";
							return false;
						}
					}
					finally
					{
						sqlCmd.Dispose();
					}
				}
				finally
				{
					Database.Instance.Close();
				}
			}
			catch (Exception e)
			{
				errMsg = $"Chyba při Connection do DB \n{e.Message}";
				return false;
			}
			return true;
		}

		/// <summary>
		/// Nalezeni zaměstnance na základě jeho ID
		/// </summary>
		/// <param name="id">Hledane ID</param>
		/// <param name="entity">Nalezeny DTO zaměstnanec nebo null</param>
		/// <param name="errMsg">Chybové hlášení</param>
		/// <returns>TRUE hledání se provedlo, FALSE nastala chyba hledání</returns>
		public bool Find(int id, out RezervaceDTO entity, out string errMsg)
		{
			errMsg = string.Empty;
			entity = null;

			//Nalezeni uzivatele podle jeho id v DB
			try
			{
				Database.Instance.Connect();
				try
				{
					string sql =
						"SELECT datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id FROM rezervace WHERE id=@id";

					SqlCommand sqlCmd = Database.Instance.CreateCommand(sql);
					try
					{
						sqlCmd.Parameters.AddWithValue("@id", id);
						var result = Database.Instance.Select(sqlCmd);
						try
						{
							if (result.HasRows)
							{
								while (result.Read())
								{
									entity = new RezervaceDTO()
									{
										Id = id,
										DatumZacatkuRezervace = result.GetDateTime(result.GetOrdinal("datum_zacatku")),
										DatumKonceRezervace = result.GetDateTime(result.GetOrdinal("datum_konce")),
										Cena = result.GetInt32(result.GetOrdinal("cena")),
										Kauce = result.GetInt32(result.GetOrdinal("kauce")),
										ZakaznikId = result.GetInt32(result.GetOrdinal("zakaznik_id")),
										VozidloId = result.GetInt32(result.GetOrdinal("vozidlo_id"))
									};
								}
							}
						}
						finally
						{
							result.Close();
						}
					}
					finally
					{
						sqlCmd.Dispose();
					}
				}
				finally
				{
					Database.Instance.Close();
				}
			}
			catch (Exception e)
			{
				errMsg = $"Chyba při Connection do DB \n{e.Message}";
				return false;
			}
			return true;
		}
	}
}
