using System.Linq;

namespace System
{
    public static class ObjectCopier
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
                throw new ArgumentNullException(nameof(destination));
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

        #endregion Public Methods
    }
}