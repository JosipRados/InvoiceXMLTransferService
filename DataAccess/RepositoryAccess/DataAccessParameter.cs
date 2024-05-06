using static DataAccess.RepositoryAccess.TypeParameter;
using System.Data;

namespace DataAccess.RepositoryAccess
{
    public class DataAccessParameter
    {
        /// <summary>Naziv parametra.</summary>
        public string ParameterName;

        /// <summary>Vrijednost parametra.</summary>
        public object Value;

        /// <summary>Vrsta parametra.</summary>
        public ParameterDirection Direction;

        /// <summary>Tip parametra.</summary>
        public TypeParametar ParameterType;



        /// <summary>Konstruktor klase za definiranje jednog parametra.</summary>
        /// <remarks>Glavni konstruktor klase, ostali konstruktori zovu ovaj.
        /// <para>Konstruktor je dostupan samo unutar DataAccess projekta.</para>
        /// </remarks>
        /// <param name="parameterName">Naziv parametra.</param>
        /// <param name="value">Vrijednost parametra.</param>
        /// <param name="direction">Vrsta parametra.</param>
        /// <param name="parameterType">Tip parametra.</param>
        public DataAccessParameter(string parameterName, object value, ParameterDirection direction, TypeParametar parameterType)
        {
            ParameterName = parameterName;
            Value = value;
            Direction = direction;
            ParameterType = parameterType;
        }
    }
}
