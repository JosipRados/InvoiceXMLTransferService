namespace DataAccess.RepositoryAccess
{
    public class TypeParameter
    {
        public enum TypeParametar
        {
            /// <summary>Nedefiniran tip parametra.</summary>
            None,
            /// <summary>64-bitni cijeli broj.</summary>
            BigInt,
            /// <summary>Polje byte-ova fiksne veličine.</summary>
            Binary,
            /// <summary>Bit (može biti 0, 1 ili null).</summary>
            Bit,
            /// <summary>Niz non-Unicode znakova fiksne veličine.</summary>
            Char,
            /// <summary>Datum bez sati, minuta.</summary>
            Date,
            /// <summary>Datum i vrijeme do milisekundi.</summary>
            DateTime,
            /// <summary>Datum i vrijeme do nanosekundi.</summary>
            DateTime2,
            /// <summary>Datum i vrijeme do nanosekundi, vodi računa i o vremenskoj zoni.</summary>
            DateTimeOffset,
            /// <summary>Decimalni broj fiksne s fiksnom decimalnom točkom.</summary>
            Decimal,
            /// <summary>Decimalni broj s pomičnom decimalnom točkom.</summary>
            Float,
            /// <summary>Polje byte-ova koji predstavljaju sliku.</summary>
            Image,
            /// <summary>32-bitni cijeli broj.</summary>
            Int,
            /// <summary>Decimalni broj s preciznošću od 4 decimalna mjesta.</summary>
            Money,
            /// <summary>Niz Unicode znakova fiksne veličine.</summary>
            NChar,
            /// <summary>Niz Unicode znakova varijabilne veličine, služi za velike tekstove preko 10000 znakova.</summary>
            NText,
            /// <summary>Niz Unicode znakova varijabilne veličine.</summary>
            NVarChar,
            /// <summary>Decimalni broj s pomičnom decimalnom točkom manje preciznosti nego Float tip.</summary>
            Real,
            /// <summary>Datum i vrijeme preciznosti do minute.</summary>
            SmallDateTime,
            /// <summary>16-bitni cijeli broj.</summary>
            SmallInt,
            /// <summary>Decimalni broj s preciznošću od 4 decimalna mjesta, manjeg raspona nego Money tip.</summary>
            SmallMoney,
            /// <summary>Specijalni tip strukturiranih podataka (koristi se npr. na SQL Server bazama podataka.</summary>
            Structured,
            /// <summary>Niz non-Unicode znakova varijabilne veličine, služi za velike tekstove preko 10000 znakova..</summary>
            Text,
            /// <summary>Vrijeme bez datuma do nanosekundi.</summary>
            Time,
            /// <summary>Polje od 8 byte-ova garantirano unique u tablici.</summary>
            Timestamp,
            /// <summary>8-bitni cijeli broj.</summary>
            TinyInt,
            /// <summary>Jedinstveni identifikator (GUID).</summary>
            UniqueIdentifier,
            /// <summary>Polje byte-ova varijabilne veličine.</summary>
            VarBinary,
            /// <summary>Niz non-Unicode znakova varijabilne veličine.</summary>
            VarChar,
            /// <summary>Default tip parametra, ekvivalent object tipu u C#.</summary>
            Variant,
            /// <summary>XML tip parametra, dohvaća se u obliku string-a.</summary>
            Xml
        }
    }
}
