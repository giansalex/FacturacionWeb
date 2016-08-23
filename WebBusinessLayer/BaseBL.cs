namespace WebBusinessLayer
{
    /// <summary>
    /// Class BaseBl.
    /// </summary>
    public class BaseBl
    {
        /// <summary>
        /// Contiene informacion de la ultima Consulta Ejecutada.
        /// </summary>
        public OperationResult LastResult { protected set; get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseBl"/> class.
        /// </summary>
        public BaseBl()
        {
            LastResult = new OperationResult();
        }
    }
}
