using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Unimake.Primitives.Converters.JsontypeConverters.Abstractions
{
    /// <summary>
    /// Base de conversão para os tipos JSON.
    /// <para>Trata apenas o tipo string por padrão</para>
    /// </summary>
    public abstract class TypeConverterBase : JsonConverter
    {
        #region Protected Methods

        /// <summary>
        /// Realiza a conversão do valor para a escrita no JSON
        /// </summary>
        /// <param name="value">Valor original</param>
        /// <returns>Valor convertido para o esperado pelo JSON</returns>
        protected virtual object ConvertValue(object value) => value?.ToString() ?? "";

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// Se verdadeiro, pode realizar a leitura do mesmo
        /// </summary>
        public override bool CanRead => true;

        /// <summary>
        /// Se verdadeiro pode escrever no mesmo
        /// </summary>
        public override bool CanWrite => true;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Se verdadeiro, o valor pode ser convertido no formato esperado
        /// </summary>
        /// <param name="objectType">Tipo de objeto que será convertido</param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType) => objectType == typeof(string);

        /// <summary>
        /// Lê o JSON e prepara o valor existente para o formato esperado.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if(reader?.Value == null)
            {
                return null;
            }

            existingValue = ConvertValue(reader.Value);

            return existingValue;
        }

        /// <summary>
        /// Escreve o valor no objeto JSON
        /// </summary>
        /// <param name="writer">objeto para escrita </param>
        /// <param name="value">valor que deverá ser escrito</param>
        /// <param name="serializer">Serializador que está sendo utilizado</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var t = JToken.FromObject(ConvertValue(value));

            if(t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                var o = (JObject)t;
                IList<string> propertyNames = o.Properties().Select(p => p.Name).ToList();

                o.AddFirst(new JProperty("Keys", new JArray(propertyNames)));

                o.WriteTo(writer);
            }
        }

        #endregion Public Methods
    }
}