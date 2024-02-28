namespace System
{
    /// <summary>
    /// sobrecarga para a interface  System.ICloneable
    /// </summary>
    /// <typeparam name="T">Tipo de objeto a ser clonado</typeparam>
    public interface ICloneable<T> : ICloneable
    {
        #region Public Methods

        /// <summary>
        /// Retorna uma cópia deste objeto
        /// </summary>
        /// <returns></returns>
        new T Clone();

        #endregion Public Methods
    }
}