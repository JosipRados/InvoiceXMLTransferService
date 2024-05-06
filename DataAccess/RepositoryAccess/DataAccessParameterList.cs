using System.Collections;
using System.Data;
using static DataAccess.RepositoryAccess.TypeParameter;

namespace DataAccess.RepositoryAccess
{
    public class DataAccessParameterList : ArrayList
    {
        /// <summary>
        /// Konstruktor.
        /// </summary>
        public DataAccessParameterList()
            : base()
        {
        }

        /// <summary>
        /// Indexer.
        /// </summary>
        public new DataAccessParameter this[int index]   // deklaracija indexera
        {
            get
            {
                return (DataAccessParameter)base[index];
            }
            set
            {
                base[index] = value;
            }
        }

        /// <summary>
        /// Dodaj parametar za poziv DataAccess objekta.
        /// </summary>
        public void ParametarAdd(string name, object value, System.Data.ParameterDirection parameterDirection, TypeParametar type)
        {
            DataAccessParameter dataAccessParameter = new DataAccessParameter(name, value, parameterDirection, type);
            this.Add(dataAccessParameter);
        }

        /// <summary>
        /// Dodaj parametar za poziv DataAccess objekta.
        /// </summary>
        public void ParametarAdd(string name, object value, TypeParametar type)
        {
            DataAccessParameter dataAccessParameter = new DataAccessParameter(name, value, ParameterDirection.Input, type);
            this.Add(dataAccessParameter);
        }
    }
}
