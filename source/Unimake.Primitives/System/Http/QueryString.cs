using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace System.Http
{
    /// <summary>
    /// Utilize para montar uma queryString para as requisições
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    public class QueryString : IEnumerable<QueryStringItem>
    {
        #region Private Fields

        private Dictionary<string, object> values = new Dictionary<string, object>();

        #endregion Private Fields

        #region Public Properties

        public int Count => values.Count;

        #endregion Public Properties

        #region Public Indexers

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Se <paramref name="key"/> for nulo, vazio ou espaços.</exception>
        /// <exception cref="KeyNotFoundException">Se a chave definida em <paramref name="key"/> não existir</exception>
        public object this[string key]
        {
            get
            {
                if(string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
                }

                if(TryGetValue(key, out var value))
                {
                    return value.Value;
                }

                throw new KeyNotFoundException();
            }
            set
            {
                if(string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
                }

                values[key] = value;
            }
        }

        #endregion Public Indexers

        #region Public Methods

        /// <summary>
        /// Converte em uma string seguindo o padrão definido em <see cref="ToString(bool, bool, string, Func{object, string})"/>
        /// </summary>
        /// <param name="value">Valor para conversão</param>
        public static implicit operator string(QueryString value) => value.ToString();

        public static bool operator !=(QueryString lhs, QueryString rhs) => !(lhs == rhs);

        public static bool operator ==(QueryString lhs, QueryString rhs)
        {
            if(lhs is null)
            {
                if(rhs is null)
                {
                    return true;
                }

                return false;
            }

            return lhs.Equals(rhs);
        }

        /// <summary>
        /// Adiciona um novo item na coleção
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentException">Se <paramref name="key"/> for nulo, vazio ou espaços.</exception>
        /// <returns></returns>
        public QueryStringItem Add(string key, object value)
        {
            if(string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            values.Add(key, value);
            return new QueryStringItem(key, value);
        }

        /// <summary>
        /// Adiciona ou atualiza um valor na query
        /// </summary>
        /// <param name="key">Chave para adicionar ou atualizar</param>
        /// <param name="value">Valor.</param>
        /// <exception cref="ArgumentException">Se <paramref name="key"/> for nulo, vazio ou espaços.</exception>
        /// <returns></returns>
        public QueryStringItem AddOrUpdateValue(string key, object value)
        {
            if(string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            if(Contains(key))
            {
                this[key] = value;
                return new QueryStringItem(key, value);
            }

            return Add(key, value);
        }

        /// <summary>
        /// Retorna verdadeiro se <paramref name="key"/> existir na coleção, caso contrário falso
        /// </summary>
        /// <param name="key">Chave para verificação</param>
        /// <returns></returns>
        public bool Contains(string key) => values.ContainsKey(key);

        public override bool Equals(object obj)
        {
            if(!(obj is QueryString other))
            {
                return false;
            }

            foreach(var item in other)
            {
                if(!TryGetValue(item.Key, out var value) ||
                    value != item)
                {
                    return false;
                }
            }

            return other.Count == Count;
        }

        /// <summary>
        /// <inheritdoc cref="IEnumerable{T}"/>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<QueryStringItem> GetEnumerator()
        {
            foreach(var item in values)
            {
                yield return new QueryStringItem(item.Key, item.Value);
            }
        }

        public override int GetHashCode()
        {
            return values.GetHashCode();
        }

        /// <summary>
        /// Remove uma <paramref name="key"/> da coleção e retorna verdadeiro se removeu com sucesso, caso contrário falso
        /// </summary>
        /// <param name="key"></param>
        /// <exception cref="ArgumentException">Se <paramref name="key"/> for nulo, vazio ou espaços.</exception>
        /// <returns></returns>
        public bool Remove(string key)
        {
            if(string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            return values.Remove(key);
        }

        /// <summary>
        /// Retorna uma query string com o sinal de interrogação no início se <paramref name="appendQuestionMark"/> for verdadeiro <br/>
        /// Utiliza o sinal de &amp; como separador de parâmetros.
        /// Para mudar o separador, altere o valor do parâmetro <paramref name="keyValueSeparator"/> pra o valor desejado
        /// <para>Padrão: ?key1=value1&amp;key2=value2&amp;keyN=valueN</para>
        /// </summary>
        /// <param name="appendQuestionMark">Se verdadeiro, a interrogação será adicionada ao início.</param>
        /// <param name="keyValueSeparator">Separador de chave/valor.</param>
        /// <param name="formatValue">Função utilizada para formatar o valora antes de inserir na query</param>
        /// <param name="urlEncodeValue">Se verdadeiro, codifica o valor para a marcação URL</param>
        /// <exception cref="ArgumentException">Se o parâmetro <paramref name="keyValueSeparator"/> for nulo ou vazio</exception>
        /// <returns></returns>
        public string ToString(bool appendQuestionMark = true,
                               bool urlEncodeValue = true,
                               string keyValueSeparator = "&",
                               Func<object, string> formatValue = null)
        {
            if(string.IsNullOrEmpty(keyValueSeparator))
            {
                throw new ArgumentException($"'{nameof(keyValueSeparator)}' cannot be null or empty.", nameof(keyValueSeparator));
            }

            formatValue = formatValue ?? new Func<object, string>(obj =>
            {
                return obj?.ToString() ?? "";
            });
            var sb = new StringBuilder();
            var separator = "";

            foreach(var value in values)
            {
                var paramValue = formatValue(value.Value);

                if(urlEncodeValue)
                {
                    paramValue = System.Web.HttpUtility.UrlEncode(paramValue);
                }

                sb.Append($"{separator}{value.Key}={paramValue}");

                if(separator == "")
                {
                    separator = keyValueSeparator;
                }
            }

            if(appendQuestionMark)
            {
                sb.Insert(0, "?");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Retorna uma query string como definido em <see cref="ToString(bool, bool, string, Func{object, string})"/>
        /// <para>Padrão: ?key1=value1&amp;key2=value2&amp;keyN=valueN</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString() => ToString(true);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentException">Se <paramref name="key"/> for nulo, vazio ou espaços.</exception>
        /// <returns></returns>
        public bool TryGetValue(string key, out QueryStringItem value)
        {
            if(string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            if(values.TryGetValue(key, out var result))
            {
                value = new QueryStringItem(key, result);
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// <inheritdoc cref="IEnumerable"/>
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion Public Methods
    }
}