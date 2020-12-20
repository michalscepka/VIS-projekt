using DTO.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer.TableDataGateways
{
	/// <summary>
	/// TableDataGateway použití vzoru pro třídu zaměstnanec
	/// </summary>
	public class ZamestnanecGW : IDataGW<ZamestnanecDTO>
	{
		private static readonly object m_LockObj = new object();
		private static ZamestnanecGW m_Instance;

		public static ZamestnanecGW Instance
		{
			get
			{
				lock (m_LockObj)
				{
					return m_Instance ??= new ZamestnanecGW();
				}
			}
		}

		private ZamestnanecGW()
		{

		}

		public bool LoadAll(out List<ZamestnanecDTO> seznam, out string errMsg)
		{
			seznam = null;
			errMsg = string.Empty;

			var sql = "SELECT id, jmeno, prijmeni, email, telefon, datum_narozeni, datum_nastupu, hodinova_mzda, pobocka_id FROM zamestnanec";

			//Nacteni objektu z uloziste
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
							seznam = new List<ZamestnanecDTO>();
							if (result.HasRows)
							{
								while (result.Read())
								{
									ZamestnanecDTO zamestnanec = new ZamestnanecDTO()
									{
										Id = result.GetInt32(result.GetOrdinal("id")),
										Jmeno = result.GetString(result.GetOrdinal("jmeno")),
										Prijmeni = result.GetString(result.GetOrdinal("prijmeni")),
										Email = result.GetString(result.GetOrdinal("email")),
										Telefon = result.GetString(result.GetOrdinal("telefon")),
										DatumNarozeni = result.GetDateTime(result.GetOrdinal("datum_narozeni")),
										DatumNastupu = result.GetDateTime(result.GetOrdinal("datum_nastupu")),
										HodinovaMzda = result.GetInt32(result.GetOrdinal("hodinova_mzda")),
										PobockaId = result.GetInt32(result.GetOrdinal("pobocka_id")),
									};
									seznam.Add(zamestnanec);
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

		public bool SaveAll(List<ZamestnanecDTO> seznam, out string errMsg)
		{
			errMsg = string.Empty;

			string sqlInsert =
				"INSERT INTO zamestnanec (jmeno, prijmeni, email, telefon, datum_narozeni, datum_nastupu, hodinova_mzda, pobocka_id) " +
				"VALUES (@jmeno, @prijmeni, @email, @telefon, @datum_narozeni, @datum_nastupu, @hodinova_mzda, @pobocka_id)";
			string sqlUpdate =
				"UPDATE zamestnanec SET jmeno=@jmeno, prijmeni=@prijmeni, email=@email, telefon=@telefon, datum_narozeni=@datum_narozeni, datum_nastupu=@datum_nastupu, " +
				"hodinova_mzda=@hodinova_mzda, pobocka_id=@pobocka_id WHERE id=@id";

			//Vlozeni nebo aktualizace objektu v ulozisti
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
							sqlCmd.Parameters.AddWithValue("@jmeno", seznam[i].Jmeno);
							sqlCmd.Parameters.AddWithValue("@prijmeni", seznam[i].Prijmeni);
							sqlCmd.Parameters.AddWithValue("@email", seznam[i].Email);
							sqlCmd.Parameters.AddWithValue("@telefon", seznam[i].Telefon);
							sqlCmd.Parameters.AddWithValue("@datum_narozeni", seznam[i].DatumNarozeni);
							sqlCmd.Parameters.AddWithValue("@datum_nastupu", seznam[i].DatumNastupu);
							sqlCmd.Parameters.AddWithValue("@hodinova_mzda", seznam[i].HodinovaMzda);
							sqlCmd.Parameters.AddWithValue("@pobocka_id", seznam[i].PobockaId);
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

		public bool InsertOrUpdate(ZamestnanecDTO entity, out string errMsg)
		{
			errMsg = string.Empty;

			string sqlInsert =
				"SET NOCOUNT ON; INSERT INTO zamestnanec(jmeno, prijmeni, email, telefon, datum_narozeni, datum_nastupu, hodinova_mzda, pobocka_id) " +
				"VALUES (@jmeno, @prijmeni, @email, @telefon, @datum_narozeni, @datum_nastupu, @hodinova_mzda, @pobocka_id); SELECT SCOPE_IDENTITY(); SET NOCOUNT OFF;";
			string sqlUpdate =
				"UPDATE zamestnanec SET jmeno=@jmeno, prijmeni=@prijmeni, email=@email, telefon=@telefon, datum_narozeni=@datum_narozeni, datum_nastupu=@datum_nastupu, " +
				"hodinova_mzda=@hodinova_mzda, pobocka_id=@pobocka_id WHERE id=@id";
			var sql = entity.Id < 0 ? sqlInsert : sqlUpdate;

			//Vlozeni nebo aktualizace objektu v ulozisti
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
						sqlCmd.Parameters.AddWithValue("@jmeno", entity.Jmeno);
						sqlCmd.Parameters.AddWithValue("@prijmeni", entity.Prijmeni);
						sqlCmd.Parameters.AddWithValue("@email", entity.Email);
						sqlCmd.Parameters.AddWithValue("@telefon", entity.Telefon);
						sqlCmd.Parameters.AddWithValue("@datum_narozeni", entity.DatumNarozeni);
						sqlCmd.Parameters.AddWithValue("@datum_nastupu", entity.DatumNastupu);
						sqlCmd.Parameters.AddWithValue("@hodinova_mzda", entity.HodinovaMzda);
						sqlCmd.Parameters.AddWithValue("@pobocka_id", entity.PobockaId);
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

		public bool Delete(int id, out string errMsg)
		{
			errMsg = string.Empty;

			var sql = "DELETE FROM zamestnanec WHERE id=@id";

			//Smazani objektu z uloziste
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
							//Pokud je návratová hodnota záporná nepovedlo se smazat objekt
							if (result < 0)
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

		public bool Find(int id, out ZamestnanecDTO entity, out string errMsg)
		{
			errMsg = string.Empty;
			entity = null;

			//Nalezeni objektu podle jeho id v DB
			try
			{
				Database.Instance.Connect();
				try
				{
					string sql =
						"SELECT jmeno, prijmeni, email, telefon, datum_narozeni, datum_nastupu, hodinova_mzda, pobocka_id FROM zamestnanec WHERE id=@id";

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
									entity = new ZamestnanecDTO()
									{
										Id = (int)id,
										Jmeno = result.GetString(result.GetOrdinal("jmeno")),
										Prijmeni = result.GetString(result.GetOrdinal("prijmeni")),
										Email = result.GetString(result.GetOrdinal("email")),
										Telefon = result.GetString(result.GetOrdinal("telefon")),
										DatumNarozeni = result.GetDateTime(result.GetOrdinal("datum_narozeni")),
										DatumNastupu = result.GetDateTime(result.GetOrdinal("datum_nastupu")),
										HodinovaMzda = result.GetInt32(result.GetOrdinal("hodinova_mzda")),
										PobockaId = result.GetInt32(result.GetOrdinal("pobocka_id"))
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
