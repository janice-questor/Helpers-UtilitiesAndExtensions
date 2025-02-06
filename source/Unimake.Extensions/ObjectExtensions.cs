using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SystemUnimake.Collections.Generic;

namespace SystemUnimake
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
        /// <param name="ignoreCase">Se verdadeiro, ignora se o nome da propriedade no objeto de destino está com o nome sensível ao caso no objeto de origem.</param>
        /// <param name="convertFrom">Função chamada quando não for possível converter o valor. Se nulo, será lançado um erro de <see cref="ArgumentException"/> </param>
        /// <typeparam name="TSource"> </typeparam>
        /// <exception cref="Exception">Exceções de forma generalizada, quando acontecer qualquer erro durante a cópia.</exception>
        /// <exception cref="ArgumentException">Lançada quando um tipo não pode ser convertido e a função <paramref name="convertFrom"/> não foi informada.</exception>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="source"/> ou <paramref name="destination"/> forem nulos.</exception>
        /// <returns>Erros, em caso de tentativas de conversões não permitidas
        /// <para>Objeto do tipo <typeparamref name="TDestination"/> com as propriedades copiadas de <typeparamref name="TSource"/></para></returns>
        public static void CopyTo<TSource, TDestination>(this TSource source, ref TDestination destination, bool ignoreCase = true, Func<(PropertyInfo Source, PropertyInfo Destination, object Value), object> convertFrom = null)
        {
            if(source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if(destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            var enumerable = from w in source.GetType().GetProperties()
                             where w.CanRead
                             select w into s
                             select s;
            var source2 = from w in destination.GetType().GetProperties()
                          where w.CanWrite
                          select w into s
                          select s;
            foreach(var sourceProperty in enumerable)
            {
                var propertyDest = source2.FirstOrDefault((PropertyInfo w) => w.Name.Equals(sourceProperty.Name, ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.CurrentCulture));

                if(!(propertyDest == null))
                {
                    var value = sourceProperty.GetValue(source);
                    try
                    {
                        propertyDest.SetValue(destination, value);
                    }
                    catch(Exception ex)
                    {
                        if(convertFrom != null)
                        {
                            value = convertFrom((sourceProperty, propertyDest, value));
                            propertyDest.SetValue(destination, value);
                            continue;
                        }

                        throw new ArgumentException($"Ocorreu um erro ao preencher a propriedade '{sourceProperty.Name}' com o valor '{value}' do tipo '{sourceProperty.PropertyType.Name}' no tipo esperado pelo objeto de destino: '{typeof(TDestination).FullName}.{propertyDest.PropertyType.Name}'. \nErro: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Copia os valores em <paramref name="source" /> para o tipo definido em <typeparamref name="TDestination" />
        /// <para>Este método faz uma copia apenas das propriedades públicas e que possam ser escritas.</para>
        /// <para>Copia apenas nomes de propriedades iguais.</para>
        /// </summary>
        /// <typeparam name="TDestination"> Tipo de destino que irá receber as cópias </typeparam>
        /// <param name="source"> Valor de origem para cópia das propriedades. </param>
        /// <typeparam name="TSource"> </typeparam>
        /// <exception cref="Exception">Exceções de forma generalizada, quando acontecer qualquer erro durante a cópia.</exception>
        /// <returns>Erros, em caso de tentativas de conversões não permitidas
        /// <para>Objeto do tipo <typeparamref name="TDestination"/> com as propriedades copiadas de <typeparamref name="TSource"/></para></returns>
        public static TDestination CopyTo<TSource, TDestination>(this TSource source)
            where TDestination : new()
        {
            var result = new TDestination();
            source.CopyTo(ref result);
            return result;
        }

        /// <summary>
        /// Retorna verdadeiro se o objeto implementa o tipo passado
        /// </summary>
        /// <param name="obj">objeto para verificação</param>
        /// <typeparam name="T">Tipo para verificação da implementação</typeparam>
        /// <returns>Verdadeiro se o objeto implementa a interface, ou falso</returns>
        public static bool IsImplementationOfInterface<T>(this object obj) => obj.IsImplementationOfInterface(typeof(T));

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

            result = type.IsGenericType
                ? lhsT.GetInterfaces().Any(x => x.IsGenericType &&
                                                       x.GetGenericTypeDefinition() == type)
                : lhsT.GetInterfaces().Any(x => x == type);

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
                _ = decimal.TryParse(value.ToString(),
                                        System.Globalization.NumberStyles.Any,
                                        System.Globalization.NumberFormatInfo.InvariantInfo,
                                        out var result);

                return result == 0;
            }

            return value is System.Collections.IEnumerable enumerable
                ? IEnumerableExtensions.IsNullOrEmpty(enumerable)
                : string.IsNullOrEmpty(value.ToString());
        }

        #endregion Public Methods
    }
}