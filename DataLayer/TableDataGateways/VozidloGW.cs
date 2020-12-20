using DTO.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataLayer.TableDataGateways
{
	/// <summary>
	/// TableDataGateway použití vzoru pro třídu vozidlo
	/// </summary>
	public class VozidloGW : IDataGW<VozidloDTO>
	{
		private static readonly object m_LockObj = new object();
		private static VozidloGW m_Instance;

		public static VozidloGW Instance
		{
			get
			{
				lock (m_LockObj)
				{
					return m_Instance ??= new VozidloGW();
				}
			}
		}

		private VozidloGW()
		{

		}

		public bool LoadAll(out List<VozidloDTO> seznam, out string errMsg)
		{
			seznam = null;
			errMsg = string.Empty;

			string sql = "SELECT id, znacka, model, spz, cena_za_den, pocet_dveri, motor, spotreba, obrazek, aktivni, pobocka_id FROM vozidlo";

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
							seznam = new List<VozidloDTO>();
							if (result.HasRows)
							{
								while (result.Read())
								{
									VozidloDTO vozidlo = new VozidloDTO()
									{
										Id = result.GetInt32(result.GetOrdinal("id")),
										Znacka = result.GetString(result.GetOrdinal("znacka")),
										Model = result.GetString(result.GetOrdinal("model")),
										SPZ = result.GetString(result.GetOrdinal("spz")),
										CenaZaDen = result.GetInt32(result.GetOrdinal("cena_za_den")),
										PocetDveri = result.GetInt32(result.GetOrdinal("pocet_dveri")),
										Motor = result.GetString(result.GetOrdinal("motor")),
										Spotreba = result.GetDouble(result.GetOrdinal("spotreba")),
										Obrazek = result.GetString(result.GetOrdinal("obrazek")),
										Aktivni = result.GetBoolean(result.GetOrdinal("aktivni")),
										PobockaId = result.GetInt32(result.GetOrdinal("pobocka_id"))
									};
									seznam.Add(vozidlo);
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

		public bool SaveAll(List<VozidloDTO> seznam, out string errMsg)
		{
			errMsg = string.Empty;

			string sqlInsert =
				"INSERT INTO vozidlo(znacka, model, spz, cena_za_den, pocet_dveri, motor, spotreba, obrazek, aktivni, pobocka_id) " +
				"VALUES (@znacka, @model, @spz, @cena_za_den, @pocet_dveri, @motor, @spotreba, @obrazek, @aktivni, @pobocka_id)";
			string sqlUpdate =
				"UPDATE vozidlo SET znacka=@znacka, model=@model, spz=@spz, cena_za_den=@cena_za_den, pocet_dveri=@pocet_dveri, motor=@motor, spotreba=@spotreba, " +
				"obrazek=@obrazek, aktivni=@aktivni, pobocka_id=@pobocka_id WHERE id=@id";

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
							sqlCmd.Parameters.AddWithValue("@znacka", seznam[i].Znacka);
							sqlCmd.Parameters.AddWithValue("@model", seznam[i].Model);
							sqlCmd.Parameters.AddWithValue("@spz", seznam[i].SPZ);
							sqlCmd.Parameters.AddWithValue("@cena_za_den", seznam[i].CenaZaDen);
							sqlCmd.Parameters.AddWithValue("@pocet_dveri", seznam[i].PocetDveri);
							sqlCmd.Parameters.AddWithValue("@motor", seznam[i].Motor);
							sqlCmd.Parameters.AddWithValue("@spotreba", seznam[i].Spotreba);
							sqlCmd.Parameters.AddWithValue("@obrazek", seznam[i].Obrazek);
							sqlCmd.Parameters.AddWithValue("@aktivni", seznam[i].Aktivni);
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

		public bool InsertOrUpdate(VozidloDTO entity, out string errMsg)
		{
			errMsg = string.Empty;

			string sqlInsert =
				"SET NOCOUNT ON; INSERT INTO vozidlo(znacka, model, spz, cena_za_den, pocet_dveri, motor, spotreba, obrazek, aktivni, pobocka_id) " +
				"VALUES (@znacka, @model, @spz, @cena_za_den, @pocet_dveri, @motor, @spotreba, @obrazek, @aktivni, @pobocka_id); SELECT SCOPE_IDENTITY(); SET NOCOUNT OFF;";
			string sqlUpdate =
				"UPDATE vozidlo SET znacka=@znacka, model=@model, spz=@spz, cena_za_den=@cena_za_den, pocet_dveri=@pocet_dveri, motor=@motor, spotreba=@spotreba, " +
				"obrazek=@obrazek, aktivni=@aktivni, pobocka_id=@pobocka_id WHERE id=@id";
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
						sqlCmd.Parameters.AddWithValue("@znacka", entity.Znacka);
						sqlCmd.Parameters.AddWithValue("@model", entity.Model);
						sqlCmd.Parameters.AddWithValue("@spz", entity.SPZ);
						sqlCmd.Parameters.AddWithValue("@cena_za_den", entity.CenaZaDen);
						sqlCmd.Parameters.AddWithValue("@pocet_dveri", entity.PocetDveri);
						sqlCmd.Parameters.AddWithValue("@motor", entity.Motor);
						sqlCmd.Parameters.AddWithValue("@spotreba", entity.Spotreba);
						sqlCmd.Parameters.AddWithValue("@obrazek", entity.Obrazek);
						sqlCmd.Parameters.AddWithValue("@aktivni", entity.Aktivni);
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

			var sql = "DELETE FROM vozidlo WHERE id=@id";

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

		public bool Find(int id, out VozidloDTO entity, out string errMsg)
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
						"SELECT znacka, model, spz, cena_za_den, pocet_dveri, motor, spotreba, obrazek, aktivni, pobocka_id FROM vozidlo WHERE id=@id";

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
									entity = new VozidloDTO()
									{
										Id = id,
										Znacka = result.GetString(result.GetOrdinal("znacka")),
										Model = result.GetString(result.GetOrdinal("model")),
										SPZ = result.GetString(result.GetOrdinal("spz")),
										CenaZaDen = result.GetInt32(result.GetOrdinal("cena_za_den")),
										PocetDveri = result.GetInt32(result.GetOrdinal("pocet_dveri")),
										Motor = result.GetString(result.GetOrdinal("motor")),
										Spotreba = result.GetFloat(result.GetOrdinal("spotreba")),
										Obrazek = result.GetString(result.GetOrdinal("obrazek")),
										Aktivni = result.GetBoolean(result.GetOrdinal("aktivni")),
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
