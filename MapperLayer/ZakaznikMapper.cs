using BusinessLayer.BO;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MapperLayer
{
	public class ZakaznikMapper
	{
        private static readonly object m_LockObj = new object();
        private static ZakaznikMapper m_Instance;

        public static ZakaznikMapper Instance
        {
            get
            {
                lock (m_LockObj)
                {
                    return m_Instance ??= new ZakaznikMapper();
                }
            }
        }

        /// <summary>
        /// Načtení všech Knih z databáze do objektu typu seznam knih
        /// </summary>
        /// <param name="seznam">Vraci seznam BO objektu</param>
        /// <param name="errMsg">Chybové hlášení, pokud nastala chyba</param>
        /// <returns>True - načtení proběhlo bez chyby, False - chyba při načítání</returns>
        public bool LoadAll(out List<Zakaznik> seznam, out string errMsg)
        {
            seznam = new List<Zakaznik>();
            errMsg = string.Empty;

            var sql = "SELECT id, jmeno, prijmeni, email, telefon, datum_narozeni, login, heslo, cislo_plat FROM zakaznik";

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
                                    Zakaznik zakaznik = new Zakaznik()
                                    {
                                        Id = result.GetInt32(result.GetOrdinal("id")),
                                        Jmeno = result.GetString(result.GetOrdinal("adresa")),
                                        Adresa = result.GetString(result.GetOrdinal("adresa")),
                                        Adresa = result.GetString(result.GetOrdinal("adresa")),
                                        Adresa = result.GetString(result.GetOrdinal("adresa")),
                                        Adresa = result.GetString(result.GetOrdinal("adresa")),
                                        Adresa = result.GetString(result.GetOrdinal("adresa")),
                                        Telefon = result.GetString(result.GetOrdinal("telefon"))
                                    };
                                    seznam.Add(zakaznik);
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
        public bool SaveAll(List<Zakaznik> seznam, out string errMsg)
        {
            errMsg = string.Empty;

            string sqlInsert =
                "INSERT INTO zakaznik(adresa, telefon) VALUES(@adresa, @telefon);";
            string sqlUpdate =
                "UPDATE zakaznik SET adresa = @adresa, telefon = @telefon WHERE id = @id";

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
        ///<param name="zakaznik">BO Kniha určený pro uložení nebo změnu</param>
        /// <param name="errMsg">Chybové hlášení pokud nastala chyba</param>
        /// <returns>True nebyla chyba, False chyba nastala</returns>
        public bool InsertOrUpdate(Zakaznik zakaznik, out string errMsg)
        {
            errMsg = string.Empty;

            string sqlInsert =
                "SET NOCOUNT ON; INSERT INTO zakaznik(adresa, telefon) VALUES(@adresa, @telefon); SELECT SCOPE_IDENTITY(); SET NOCOUNT OFF;";
            string sqlUpdate =
                "UPDATE zakaznik SET adresa = @adresa, telefon = @telefon WHERE id = @id";

            var sql = zakaznik.Id < 0 ? sqlInsert : sqlUpdate;

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
                        sqlCmd.Parameters.AddWithValue("id", zakaznik.Id);
                        sqlCmd.Parameters.AddWithValue("adresa", zakaznik.Adresa);
                        sqlCmd.Parameters.AddWithValue("telefon", zakaznik.Telefon);
                        try
                        {
                            var result = -1;
                            if (zakaznik.Id == -1)
                            {
                                result = Database.Instance.ExecuteScalar(sqlCmd);
                            }
                            else
                            {
                                result = Database.Instance.ExecuteNonQuery(sqlCmd);
                            }

                            //Pokud je návratová hodnota záporná nepovedlo se vložit/upravit
                            if (result <= 0)
                                throw new DataException($"Nepovedlo se vložit/upravit Zakaznik ID:({zakaznik.Id})");
                            Database.Instance.EndTransaction();
                            if (zakaznik.Id < 0)
                                zakaznik.Id = result;  //id vytvořené na DB serveru a přidělené novému záznamu v DB
                        }
                        catch (Exception e)
                        {
                            //nastala chyba vykonání INSERT/UPDATE - vrátíme změny v DB
                            Database.Instance.Rollback();
                            errMsg = $"Chyba při INSERT/UPDATE objektu Zakaznik \n{e.Message}";
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

            var sql = "DELETE FROM Zakaznik WHERE (Id = @id)";

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
        /// <param name="zakaznik">Nalezeny BO kniha nebo null</param>
        /// <param name="errMsg">Chybové hlášení</param>
        /// <returns>TRUE hledání se provedlo, FALSE nastala chyba hledání</returns>
        public bool Find(long id, out Zakaznik zakaznik, out string errMsg)
        {
            errMsg = string.Empty;
            zakaznik = null;

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
                                zakaznik = new Zakaznik()
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
