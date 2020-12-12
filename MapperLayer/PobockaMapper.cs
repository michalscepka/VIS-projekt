using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BusinessLayer.BO;
using BusinessLayer.Interfaces;
using DataLayer;

namespace MapperLayer
{
	public class PobockaMapper : IPobockaMapper
	{
		private static readonly object m_LockObj = new object();
		private static PobockaMapper m_Instance;

		public static PobockaMapper Instance
		{
			get
			{
				lock (m_LockObj)
				{
					return m_Instance ??= new PobockaMapper();
				}
			}
		}

        /// <summary>
        /// Načtení všech Knih z databáze do objektu typu seznam knih
        /// </summary>
        /// <param name="seznam">Vraci seznam BO objektu</param>
        /// <param name="errMsg">Chybové hlášení, pokud nastala chyba</param>
        /// <returns>True - načtení proběhlo bez chyby, False - chyba při načítání</returns>
        public bool LoadAll(out List<Pobocka> seznam, out string errMsg)
        {
            seznam = new List<Pobocka>();
            errMsg = string.Empty;

            var sql = "SELECT id, adresa, telefon FROM pobocka";

            //Nazcteni Knih z uloziste
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
                            if (result.HasRows)
                            {
                                while (result.Read())
                                {
                                    Pobocka pobocka = new Pobocka()
                                    {
                                        Id = result.GetInt32(result.GetOrdinal("id")),
                                        Adresa = result.GetString(result.GetOrdinal("adresa")),
                                        Telefon = result.GetString(result.GetOrdinal("telefon"))
                                    };
                                    seznam.Add(pobocka);
                                };
                            }
                        }
                        catch (Exception e)
                        {
                            //nastala chyba vykonání SELECT
                            errMsg = $"Chyba při SELECT tabulky Knihy \n{e.Message}";
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
        /// Uložení všech objektů BO kniha do Databáze provede jejich Insert/Update a to v DB transakci
        /// </summary>
        /// <param name="seznam">Seznam BO Kniha, které chcecem uložit/aktualizovat v DB</param>
        /// <param name="errMsg">Chybové hlášení pokud nastala chyba</param>
        /// <returns>True - operace se provedla, False - nastala chyba</returns>
        public bool SaveAll(List<Pobocka> seznam, out string errMsg)
        {
            errMsg = string.Empty;

            string sqlInsert =
                "INSERT INTO pobocka(adresa, telefon) VALUES(@adresa, @telefon);";
            string sqlUpdate =
                "UPDATE pobocka SET adresa = @adresa, telefon = @telefon WHERE id = @id";

            //Vlozeni nebo aktualizace knihy v ulozisti
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
                            sqlCmd.Parameters.AddWithValue("@adresa", seznam[i].Adresa);
                            sqlCmd.Parameters.AddWithValue("@telefon", seznam[i].Telefon);
                            try
                            {
                                var result = Database.Instance.ExecuteNonQuery(sqlCmd);
                                //Pokud je návratová hodnota záporná nepovedlo se vložit/upravit
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
        /// Vložení nebo aktualizace Knihy v DB
        /// </summary>
        ///<param name="pobocka">BO Kniha určený pro uložení nebo změnu</param>
        /// <param name="errMsg">Chybové hlášení pokud nastala chyba</param>
        /// <returns>True nebyla chyba, False chyba nastala</returns>
        public bool InsertOrUpdate(Pobocka pobocka, out string errMsg)
        {
            errMsg = string.Empty;

            string sqlInsert =
                "SET NOCOUNT ON; INSERT INTO pobocka(adresa, telefon) VALUES(@adresa, @telefon); SELECT SCOPE_IDENTITY(); SET NOCOUNT OFF;";
            string sqlUpdate =
                "UPDATE pobocka SET adresa = @adresa, telefon = @telefon WHERE id = @id";

            var sql = pobocka.Id < 0 ? sqlInsert : sqlUpdate;

            //Vlozeni nebo aktualizace uzivatele v ulozisti
            try
            {
                Database.Instance.Connect();
                try
                {
                    Database.Instance.BeginTransaction();
                    SqlCommand sqlCmd = Database.Instance.CreateCommand(sql);
                    try
                    {
                        sqlCmd.Parameters.AddWithValue("id", pobocka.Id);
                        sqlCmd.Parameters.AddWithValue("adresa", pobocka.Adresa);
                        sqlCmd.Parameters.AddWithValue("telefon", pobocka.Telefon);
                        try
                        {
                            var result = -1;
                            if (pobocka.Id == -1)
                            {
                                result = Database.Instance.ExecuteScalar(sqlCmd);
                            }
                            else
                            {
                                result = Database.Instance.ExecuteNonQuery(sqlCmd);
                            }

                            //Pokud je návratová hodnota záporná nepovedlo se vložit/upravit
                            if (result <= 0)
                                throw new DataException($"Nepovedlo se vložit/upravit Pobocka ID:({pobocka.Id})");
                            Database.Instance.EndTransaction();
                            if (pobocka.Id < 0)
                                pobocka.Id = result;  //id vytvořené na DB serveru a přidělené novému záznamu v DB
                        }
                        catch (Exception e)
                        {
                            //nastala chyba vykonání INSERT/UPDATE - vrátíme změny v DB
                            Database.Instance.Rollback();
                            errMsg = $"Chyba při INSERT/UPDATE objektu Pobocka \n{e.Message}";
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
        /// Smazání Knihy z DB uložiště
        /// </summary>
        /// <param name="id">ID uživatele</param>
        /// <param name="errMsg">Chybové hlášení</param>
        /// <returns>True smazání se povedlo, False nepovedlo</returns>
        public bool Delete(long id, out string errMsg)
        {
            errMsg = string.Empty;

            var sql = "DELETE FROM Pobocka WHERE (Id = @id)";

            //Smazani objektu Kniha z uloziste
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
                            //Pokud je návratová hodnota záporná nepodlo se smazat uživatele
                            if (result < 0)
                                throw new DataException($"Nepovedlo se smazat Uživatele ID:({id})");

                            Database.Instance.EndTransaction();
                        }
                        catch (Exception e)
                        {
                            //nastala chyba vykonání DELETE - vrátíme změny v DB
                            Database.Instance.Rollback();
                            errMsg = $"Chyba při DELETE objektu Kniha \n{e.Message}";
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
        /// Nalezeni Knihy na zaklade jeho ID
        /// </summary>
        /// <param name="id">Hledane ID </param>
        /// <param name="pobocka">Nalezeny BO kniha nebo null</param>
        /// <param name="errMsg">Chybové hlášení</param>
        /// <returns>TRUE hledání se provedlo, FALSE nastala chyba hledání</returns>
        public bool Find(long id, out Pobocka pobocka, out string errMsg)
        {
            errMsg = string.Empty;
            pobocka = null;

            //Nalezeni knihy podle jeho id v DB
            try
            {
                Database.Instance.Connect();
                try
                {
                    string sql =
                        "SELECT AutorJmeno,AutorPrijmeni,NazevKnihy,Vydavatel,RokVydani,Vydani,Jazyk FROM Knihy WHERE (Id=@id)";
                    SqlCommand sqlCmd = Database.Instance.CreateCommand(sql);
                    try
                    {
                        sqlCmd.Parameters.AddWithValue("@id", id);
                        var result = Database.Instance.Select(sqlCmd);
                        try
                        {
                            if (result.HasRows)
                            {
                                //Precteme jen prvni nalezeny, podle ID musí být jen jeden
                                result.Read();
                                pobocka = new Pobocka()
                                {
                                    /*Id = id,
                                    AutorJmeno = result.GetString(0),
                                    AutorPrijmeni = result.GetString(1),
                                    NazevKnihy = result.GetString(2),
                                    Vydavatel = result.GetString(3),
                                    RokVydani = result.GetInt32(4),
                                    Vydani = result.GetInt32(5),
                                    Jazyk = result.GetString(6)*/
                                };
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
