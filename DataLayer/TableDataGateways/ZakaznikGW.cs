using DTO.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataLayer.TableDataGateways
{
	public class ZakaznikGW : IDataGW<ZakaznikDTO>
	{
		private static readonly object m_LockObj = new object();
		private static ZakaznikGW m_Instance;

		public static ZakaznikGW Instance
		{
			get
			{
				lock (m_LockObj)
				{
					return m_Instance ??= new ZakaznikGW();
				}
			}
		}

		private ZakaznikGW()
		{

		}

		/// <summary>
		/// Načtení všech zaměstnanců z databáze do objektu ZakaznikDTO
		/// </summary>
		/// <param name="seznam">List načtených zaměstnanců</param>
		/// <param name="errMsg">Chybové hlášení, pokud nastala chyba</param>
		/// <returns>True - načtení proběhlo bez chyby, False - chyba při načítání</returns>
		public bool LoadAll(out List<ZakaznikDTO> seznam, out string errMsg)
		{
			seznam = null;
			errMsg = string.Empty;

			var sql = "SELECT id, jmeno, prijmeni, email, telefon, datum_narozeni, login, heslo, cislo_platebni_karty, ridicsky_prukaz_id FROM zakaznik";

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
							seznam = new List<ZakaznikDTO>();
							if (result.HasRows)
							{
								while (result.Read())
								{
									ZakaznikDTO zamestnanec = new ZakaznikDTO()
									{
										Id = result.GetInt32(result.GetOrdinal("id")),
										Jmeno = result.GetString(result.GetOrdinal("jmeno")),
										Prijmeni = result.GetString(result.GetOrdinal("prijmeni")),
										Email = result.GetString(result.GetOrdinal("email")),
										Telefon = result.GetString(result.GetOrdinal("telefon")),
										DatumNarozeni = result.GetDateTime(result.GetOrdinal("datum_narozeni")),
										Login = result.GetString(result.GetOrdinal("login")),
										Heslo = result.GetString(result.GetOrdinal("heslo")),
										CisloPlatebniKarty = result.GetString(result.GetOrdinal("cislo_platebni_karty")),
										RidicskyPrukazId = result.GetInt32(result.GetOrdinal("ridicsky_prukaz_id"))
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

		/// <summary>
		/// Uložení všech objektů zaměstnanci do Databáze. Provede jejich Insert/Update a to v DB transakci
		/// </summary>
		/// <param name="seznam">List zaměstnanců, které chcecme uložit do DB</param>
		/// <param name="errMsg">Chybové hlášení pokud nastala chyba</param>
		/// <returns>True - operace se provedla, False - nastala chyba</returns>
		public bool SaveAll(List<ZakaznikDTO> seznam, out string errMsg)
		{
			errMsg = string.Empty;

			string sqlInsert =
				"INSERT INTO zakaznik(jmeno, prijmeni, email, telefon, datum_narozeni, login, heslo, cislo_platebni_karty, ridicsky_prukaz_id) " +
				"VALUES (@jmeno, @prijmeni, @email, @telefon, @datum_narozeni, @login, @heslo, @cislo_platebni_karty, @ridicsky_prukaz_id)";
			string sqlUpdate =
				"UPDATE zakaznik SET jmeno=@jmeno, prijmeni=@prijmeni, email=@email, telefon=@telefon, datum_narozeni=@datum_narozeni, login=@login, heslo=@heslo, " +
				"cislo_platebni_karty=@cislo_platebni_karty, ridicsky_prukaz_id=@ridicsky_prukaz_id WHERE id=@id";

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
							sqlCmd.Parameters.AddWithValue("@jmeno", seznam[i].Jmeno);
							sqlCmd.Parameters.AddWithValue("@prijmeni", seznam[i].Prijmeni);
							sqlCmd.Parameters.AddWithValue("@email", seznam[i].Email);
							sqlCmd.Parameters.AddWithValue("@telefon", seznam[i].Telefon);
							sqlCmd.Parameters.AddWithValue("@datum_narozeni", seznam[i].DatumNarozeni);
							sqlCmd.Parameters.AddWithValue("@login", seznam[i].Login);
							sqlCmd.Parameters.AddWithValue("@heslo", seznam[i].Heslo);
							sqlCmd.Parameters.AddWithValue("@cislo_platebni_karty", seznam[i].CisloPlatebniKarty);
							sqlCmd.Parameters.AddWithValue("@ridicsky_prukaz_id", seznam[i].RidicskyPrukazId);

							try
							{
								var result = Database.Instance.ExecuteNonQuery(sqlCmd);
								//Pokud je návratová hodnota záporná nepovedlo se vložit/upravit
								//TODO mozna '<='
								if (result < 0)
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
		public bool InsertOrUpdate(ZakaznikDTO entity, out string errMsg)
		{
			errMsg = string.Empty;

			string sqlInsert =
				"SET NOCOUNT ON; INSERT INTO zakaznik(jmeno, prijmeni, email, telefon, datum_narozeni, login, heslo, cislo_platebni_karty, ridicsky_prukaz_id) " +
				"VALUES (@jmeno, @prijmeni, @email, @telefon, @datum_narozeni, @login, @heslo, @cislo_platebni_karty, @ridicsky_prukaz_id); SELECT SCOPE_IDENTITY(); SET NOCOUNT OFF;";
			string sqlUpdate =
				"UPDATE zakaznik SET jmeno=@jmeno, prijmeni=@prijmeni, email=@email, telefon=@telefon, datum_narozeni=@datum_narozeni, login=@login, heslo=@heslo, " +
				"cislo_platebni_karty=@cislo_platebni_karty, ridicsky_prukaz_id=@ridicsky_prukaz_id WHERE id=@id";
			var sql = entity.Id < 0 ? sqlInsert : sqlUpdate;

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
						sqlCmd.Parameters.AddWithValue("@jmeno", entity.Jmeno);
						sqlCmd.Parameters.AddWithValue("@prijmeni", entity.Prijmeni);
						sqlCmd.Parameters.AddWithValue("@email", entity.Email);
						sqlCmd.Parameters.AddWithValue("@telefon", entity.Telefon);
						sqlCmd.Parameters.AddWithValue("@datum_narozeni", entity.DatumNarozeni);
						sqlCmd.Parameters.AddWithValue("@login", entity.Login);
						sqlCmd.Parameters.AddWithValue("@heslo", entity.Heslo);
						sqlCmd.Parameters.AddWithValue("@cislo_platebni_karty", entity.CisloPlatebniKarty);
						sqlCmd.Parameters.AddWithValue("@ridicsky_prukaz_id", entity.RidicskyPrukazId);
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

			var sql = "DELETE FROM zakaznik WHERE id=@id";

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

		/// <summary>
		/// Nalezeni zaměstnance na základě jeho ID
		/// </summary>
		/// <param name="id">Hledane ID</param>
		/// <param name="entity">Nalezeny DTO zaměstnanec nebo null</param>
		/// <param name="errMsg">Chybové hlášení</param>
		/// <returns>TRUE hledání se provedlo, FALSE nastala chyba hledání</returns>
		public bool Find(int id, out ZakaznikDTO entity, out string errMsg)
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
						"SELECT jmeno, prijmeni, email, telefon, datum_narozeni, login, heslo, cislo_platebni_karty, ridicsky_prukaz_id " +
						"FROM zakaznik WHERE id=@id";

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
									entity = new ZakaznikDTO()
									{
										Id = (int)id,
										Jmeno = result.GetString(result.GetOrdinal("jmeno")),
										Prijmeni = result.GetString(result.GetOrdinal("prijmeni")),
										Email = result.GetString(result.GetOrdinal("email")),
										Telefon = result.GetString(result.GetOrdinal("telefon")),
										DatumNarozeni = result.GetDateTime(result.GetOrdinal("datum_narozeni")),
										Login = result.GetString(result.GetOrdinal("login")),
										Heslo = result.GetString(result.GetOrdinal("heslo")),
										CisloPlatebniKarty = result.GetString(result.GetOrdinal("cislo_platebni_karty")),
										RidicskyPrukazId = result.GetInt32(result.GetOrdinal("ridicsky_prukaz_id"))
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
