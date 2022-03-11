using System.Collections.Generic;
using System.Linq;

namespace System
{
    /// <summary>
    /// Extensões para o tipo Object
    /// </summary>
    public static class ObjectExtensions
    {
        #region Public Methods

        /// <summary>
        /// Copia os valores em <paramref name="source" /> para o tipo definido em <typeparamref name="TDestination" />
        /// <para>Este método faz uma copia apenas das propriedades públicas e que possam ser escritas.</para>
        /// <para>Copia apenas nomes de propriedades iguais.</para>
        /// </summary>
        /// <typeparam name="TDestination"> Tipo de destino que irá receber as cópias </typeparam>
        /// <param name="source"> Valor de origem para cópia das propriedades. </param>
        /// <param name="destination"> Objeto que vai receber as propriedades em cópia </param>
        /// <typeparam name="TSource"> </typeparam>
        /// <exception cref="Exception">Exceções de forma generalizada, quando acontecer qualquer erro durante a cópia.</exception>
        /// <returns>Erros, em caso de tentativas de conversões não permitidas
        /// <para>Objeto do tipo <typeparamref name="TDestination"/> com as propriedades copiadas de <typeparamref name="TSource"/></para></returns>
        public static void CopyTo<TSource, TDestination>(this TSource source, ref TDestination destination)
        {
            if(source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if(destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            var sourceProperties = source.GetType()
                                         .GetProperties()
                                         .Where(w => w.CanRead)
                                         .Select(s => s);

            var destinationProperties = destination.GetType()
                                                   .GetProperties()
                                                   .Where(w => w.CanWrite)
                                                   .Select(s => s);

            foreach(var sourceProperty in sourceProperties)
            {
                var destinationProperty = destinationProperties.FirstOrDefault(w => w.Name == sourceProperty.Name);
                destinationProperty?.SetValue(destination, sourceProperty.GetValue(source));
            }
        }

        /// <summary>
        /// Retorna verdadeiro se o objeto implementa o tipo passado
        /// </summary>
        /// <param name="obj">objeto para verificação</param>
        /// <typeparam name="T">Tipo para verificação da implementação</typeparam>
        /// <returns>Verdadeiro se o objeto implementa a interface, ou falso</returns>
        public static bool IsImplementationOfInterface<T>(this object obj) => IsImplementationOfInterface(obj, typeof(T));

        /// <summary>
        /// Retorna verdadeiro se o objeto implementa o tipo passado
        /// </summary>
        /// <param name="obj">objeto para verificação</param>
        /// <param name="type">Tipo esperado</param>
        /// <returns>Verdadeiro se o objeto implementa a interface, ou falso</returns>
        public static bool IsImplementationOfInterface(this object obj, Type type)
        {
            var result = false;

            if(obj == null || type == null)
            {
                return result;
            }

            var lhsT = obj.GetType();

            if(type.IsGenericType)
            {
                result = lhsT.GetInterfaces().Any(x => x.IsGenericType &&
                                                       x.GetGenericTypeDefinition() == type);
            }
            else
            {
                result = lhsT.GetInterfaces().Any(x => x == type);
            }

            return result;
        }

        /// <summary>
        /// Retorna verdadeiro se o objeto implementa o tipo passado
        /// </summary>
        /// <param name="obj">objeto para verificação</param>
        /// <param name="fullyQualifiedName">Se verdadeiro pesquisa pelo nome completo da interface</param>
        /// <param name="typeName">Nome da interface para verificação</param>
        /// <returns>Verdadeiro se o objeto implementa a interface, ou falso</returns>
        public static bool IsImplementationOfInterface(this object obj, string typeName, bool fullyQualifiedName = false)
        {
            var result = false;

            if(obj == null || string.IsNullOrWhiteSpace(typeName))
            {
                return result;
            }

            var lhsT = obj.GetType();

            result = fullyQualifiedName
                ? lhsT.GetInterfaces().Any(x =>
                                           x.FullName.Equals(typeName, StringComparison.InvariantCultureIgnoreCase))
                : lhsT.GetInterfaces().Any(x =>
                                           x.Name.Equals(typeName, StringComparison.InvariantCultureIgnoreCase));
            return result;
        }

        /// <summary>
        /// Retorna verdadeiro se o tipo é anulável
        /// </summary>
        /// <typeparam name="T">Tipo anulável</typeparam>
        /// <param name="obj">Objeto para validação</param>
        /// <returns></returns>
        public static bool IsNullable(this object obj)
        {
            if(obj == null)
            {
                return true; // ¯\(°_o)/¯
            }

            return obj.GetType().IsNullable();
        }

        public static bool IsNullable(this Type type)
        {
            if(!type.IsValueType)
            {
                return true; // reference type
            }

            if(Nullable.GetUnderlyingType(type) != null)
            {
                return true; // Nullable<T>
            }

            return false; // value-type
        }

        /// <summary>
        /// Retorna true se o objeto, array, IEnumerable ou se convertido para string for nulo ou vazio
        /// </summary>
        /// <param name="value">valor que deverá ser comparado</param>
        /// <param name="considerNumericZeroValueAsEmpty">Se verdadeiro, quando o tipo for numérico e a conversão for zero, retorna true.</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object value, bool considerNumericZeroValueAsEmpty = true)
        {
            if(value == null)
            {
                return true;
            }

            if(value is Enum e)
            {
                return e.IsValid();
            }

            if(value.GetType().IsArray)
            {
                return IEnumerableExtensions.IsNullOrEmpty(value as object[]);
            }

            if(considerNumericZeroValueAsEmpty &&
                value.GetType().IsNumeric())
            {
                decimal.TryParse(value.ToString(),
                                        Globalization.NumberStyles.Any,
                                        Globalization.NumberFormatInfo.InvariantInfo,
                                        out var result);

                return result == 0;
            }

            if(value is Collections.IEnumerable enumerable)
            {
                return IEnumerableExtensions.IsNullOrEmpty(enumerable);
            }

            return string.IsNullOrEmpty(value.ToString());
        }

        #endregion Public Methods
    }
}