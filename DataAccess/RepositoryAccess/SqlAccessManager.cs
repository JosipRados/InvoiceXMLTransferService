using System.Data;
using System.Data.SqlClient;
using static DataAccess.RepositoryAccess.TypeParameter;

namespace DataAccess.RepositoryAccess
{
    public class SqlAccessManager
    {
        public static void SelectData(SqlConnection conn, CommandType commandType, DataTable dataTable, string commandText)
        {
            SelectData(conn, commandType, dataTable, commandText, new DataAccessParameterList());
        }

        public static async Task SelectDataAsync(SqlConnection conn, CommandType commandType, DataTable dataTable, string commandText)
        {
            await SelectDataAsync(conn, commandType, dataTable, commandText, new DataAccessParameterList());
        }
        /// <summary>Puni DataTable podacima iz baze preko upita ili storane procedure.</summary>
        /// <remarks>Podaci se spremaju u parametar tipa DataTable, nema povratne vrijednosti metode.
        /// <para>Metoda je dostupna samo unutar DataAccess projekta.</para>
        /// </remarks>
        /// <param name="dataTable">DataTable popunjen podacima iz baze podataka.</param>
        /// <param name="commandType">Vrsta sql upita (tekst, storana procedura ili direktni upit na tablicu).</param>
        /// <param name="commandText">Sadržaj upita (tekst upita, naziv storane procedure ili naziv tablice).</param>
        /// <param name="parametri">Lista parametara koji se koriste za storanu proceduru.</param>
        /// <param name="conn">Konekcija na bazu podataka.</param>
        public static void SelectData(SqlConnection conn, CommandType commandType, DataTable dataTable, string commandText, DataAccessParameterList parametri)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand(commandText, conn);
                sqlCommand.CommandType = commandType;

                SetParameters(parametri, sqlCommand);
                
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);

                dataAdapter.Fill(dataTable);

                ReturnValueParameters(parametri, sqlCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task SelectDataAsync(SqlConnection conn, CommandType commandType, DataTable dataTable, string commandText, DataAccessParameterList parametri)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand(commandText, conn);
                sqlCommand.CommandType = commandType;

                SetParameters(parametri, sqlCommand);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);

                await Task.Run(() => dataAdapter.Fill(dataTable));

                ReturnValueParameters(parametri, sqlCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>Izvršava upit ili storanu proceduru u bazi.</summary>
        /// <remarks>Nema povratne vrijednosti metode.
        /// <para>Metoda je dostupna samo unutar DataAccess projekta.</para>
        /// </remarks>
        /// <param name="commandType">Vrsta sql upita (tekst, storana procedura ili direktni upit na tablicu).</param>
        /// <param name="commandText">Sadržaj upita (tekst upita, naziv storane procedure ili naziv tablice).</param>
        /// <param name="parametri">Lista parametara koji se koriste za storanu proceduru.</param>
        /// <param name="conn">Konekcija na bazu podataka.</param>
        public static void ExecuteQuery(SqlConnection conn, CommandType commandType, string commandText, DataAccessParameterList parametri)
        {
            try
            {
                SqlCommand com = new SqlCommand(commandText, conn);
                com.CommandType = commandType;

                SetParameters(parametri, com);

                // ako veza sa bazom nije otvorena, otvori ju
                if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                    conn.Open();

                com.ExecuteNonQuery();

                ReturnValueParameters(parametri, com);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>Postavlja listu parametara za storane procedure unutar sql komande koja se izvršava.</summary>
        /// <remarks>Nema povratne vrijednosti metode.
        /// <para>Metoda je dostupna samo unutar klase SqlAccessManager.</para>
        /// </remarks>
        /// <param name="parametri">Lista parametara koji se koriste za storane procedure.</param>
        /// <param name="com">Sql comanda koja se izvršava.</param>
        private static void SetParameters(DataAccessParameterList parametri, SqlCommand com)
        {
            try
            {
                if (com.CommandType == CommandType.StoredProcedure)
                {
                    if (parametri != null)
                    {
                        SqlParameter sqlParameter;
                        foreach (DataAccessParameter dataAccessParameter in parametri)
                        {
                            sqlParameter = GetSqlParameter(dataAccessParameter);

                            sqlParameter = com.Parameters.Add(sqlParameter);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>Postavlja u listu parametara za storane procedure nove vrijednosti nakon izvođenja storane procedure.</summary>
        /// <remarks>Nema povratne vrijednosti metode.
        /// <para>Metoda je dostupna samo unutar klase SqlAccessManager.</para>
        /// </remarks>
        /// <param name="parametri">Lista parametara koji se koriste za storane procedure.</param>
        /// <param name="com">Sql komanda koja se izvršava.</param>
        private static void ReturnValueParameters(DataAccessParameterList parametri, SqlCommand com)
        {
            try
            {
                if (com.CommandType == CommandType.StoredProcedure)
                {
                    if (parametri != null)
                    {
                        for (int i = 0; i < parametri.Count; i++)
                        {
                            // Ako se vrijednost parametra promijenila, ažuriraj je
                            DataAccessParameter dataAccessParameter = parametri[i];
                            if ((dataAccessParameter.Direction == ParameterDirection.Output || dataAccessParameter.Direction == ParameterDirection.InputOutput || dataAccessParameter.Direction == ParameterDirection.ReturnValue)
                                && dataAccessParameter.Value != com.Parameters[parametri[i].ParameterName].Value)
                            {
                                parametri[i].Value = com.Parameters[parametri[i].ParameterName].Value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>Inicijalizira sql parametar koji se koristi u sql komandi.</summary>
        /// <remarks>>Metoda je dostupna samo unutar klase SqlAccessManager.</remarks>
        /// <returns>Sql parametar koji se koristi u sql komandi.</returns>
        /// <param name="dataAccessParameter">Parametar koji se inicijalizira za korištenje u sql komandi.</param>
        private static SqlParameter GetSqlParameter(DataAccessParameter dataAccessParameter)
        {
            SqlParameter sqlParameter;
            SqlDbType tip;

            try
            {
                switch (dataAccessParameter.ParameterType)
                {
                    case TypeParametar.BigInt:
                        tip = SqlDbType.BigInt;
                        break;

                    case TypeParametar.Binary:
                        tip = SqlDbType.Binary;
                        break;

                    case TypeParametar.Bit:
                        if (bool.Parse(dataAccessParameter.Value.ToString()))
                            dataAccessParameter.Value = 1;
                        else
                            dataAccessParameter.Value = 0;

                        tip = SqlDbType.Bit;
                        break;

                    case TypeParametar.Char:
                        tip = SqlDbType.Char;
                        break;

                    case TypeParametar.Date:
                        tip = SqlDbType.Date;
                        break;

                    case TypeParametar.DateTime:
                        tip = SqlDbType.DateTime;
                        break;

                    case TypeParametar.DateTime2:
                        tip = SqlDbType.DateTime2;
                        break;

                    case TypeParametar.DateTimeOffset:
                        tip = SqlDbType.DateTimeOffset;
                        break;

                    case TypeParametar.Decimal:
                        tip = SqlDbType.Decimal;
                        break;

                    case TypeParametar.Float:
                        tip = SqlDbType.Float;
                        break;

                    case TypeParametar.Image:
                        tip = SqlDbType.Image;
                        break;

                    case TypeParametar.Int:
                        tip = SqlDbType.Int;
                        break;

                    case TypeParametar.Money:
                        tip = SqlDbType.Money;
                        break;

                    case TypeParametar.NChar:
                        tip = SqlDbType.NChar;
                        break;

                    case TypeParametar.None:
                        tip = SqlDbType.NVarChar;
                        throw new Exception("Ne postoji tip parametra.");

                    case TypeParametar.NText:
                        tip = SqlDbType.NText;
                        break;

                    case TypeParametar.NVarChar:
                        tip = SqlDbType.NVarChar;
                        break;

                    case TypeParametar.Real:
                        tip = SqlDbType.Real;
                        break;

                    case TypeParametar.SmallDateTime:
                        tip = SqlDbType.SmallDateTime;
                        break;

                    case TypeParametar.SmallInt:
                        tip = SqlDbType.SmallInt;
                        break;

                    case TypeParametar.SmallMoney:
                        tip = SqlDbType.SmallMoney;
                        break;

                    case TypeParametar.Structured:
                        tip = SqlDbType.Structured;
                        break;

                    case TypeParametar.Text:
                        tip = SqlDbType.Text;
                        break;

                    case TypeParametar.Time:
                        tip = SqlDbType.Time;
                        break;

                    case TypeParametar.Timestamp:
                        tip = SqlDbType.Timestamp;
                        break;

                    case TypeParametar.TinyInt:
                        tip = SqlDbType.TinyInt;
                        break;

                    case TypeParametar.UniqueIdentifier:
                        tip = SqlDbType.UniqueIdentifier;
                        break;

                    case TypeParametar.VarBinary:
                        tip = SqlDbType.VarBinary;
                        break;

                    case TypeParametar.VarChar:
                        tip = SqlDbType.VarChar;
                        break;

                    case TypeParametar.Variant:
                        tip = SqlDbType.Variant;
                        break;

                    case TypeParametar.Xml:
                        tip = SqlDbType.Xml;
                        break;

                    default:
                        tip = SqlDbType.NVarChar;
                        break;
                }

                sqlParameter = new SqlParameter(dataAccessParameter.ParameterName, tip);

                sqlParameter.Direction = dataAccessParameter.Direction;
                sqlParameter.Value = dataAccessParameter.Value;
            }
            catch
            {
                return null;
            }

            return sqlParameter;
        }
    }
}