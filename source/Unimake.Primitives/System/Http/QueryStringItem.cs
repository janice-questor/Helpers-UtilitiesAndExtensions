using System.Collections.Generic;
using System.Diagnostics;

namespace System.Http
{
    /// <summary>
    /// Itens de uma QueryString.
    /// <para>Utilize para montar uma requisição http com base em query string</para>
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    public struct QueryStringItem
    {
        #region Public Fields

        public string Key;
        public object Value;

        #endregion Public Fields

        #region Public Constructors

        /// <summary>
        /// Inicializa o item
        /// </summary>
        /// <param name="key">Chave</param>
        /// <param name="value">Valor</param>
        public QueryStringItem(string key, object value)
        {
            Key = key;
            Value = value;
        }

        #endregion Public Constructors

        #region Public Methods

        public static implicit operator (string Key, object Value)(QueryStringItem value)
        {
            return (value.Key, value.Value);
        }

        public static implicit operator QueryStringItem((string Key, object Value) value)
        {
            return new QueryStringItem(value.Key, value.Value);
        }

        public static implicit operator string(QueryStringItem value) => value.ToString();

        public static bool operator !=(QueryStringItem lhs, QueryStringItem rhs) => !rhs.Equals(lhs);

        public static bool operator ==(QueryStringItem lhs, QueryStringItem rhs) => rhs.Equals(lhs);

        public void Deconstruct(out string key, out object value)
        {
            key = Key;
            value = Value;
        }

        public override bool Equals(object obj) => obj is QueryStringItem other
                                                   && Key == other.Key
                                                   && EqualityComparer<object>.Default.Equals(Value, other.Value);

        public override int GetHashCode()
        {
            unchecked // Overflow ¯\_(ツ)_/¯
            {
                int hash = (int)17;

                hash = (hash * 23) ^ Value?.GetHashCode() ?? 0;
                hash = (hash * 23) ^ Value?.GetHashCode() ?? 0;
                hash = (hash * 23) ^ Value?.GetHashCode() ?? 0;
                return hash;
            }
        }

        public override string ToString() => $"{Key}={Value}";

        #endregion Public Methods
    }
}