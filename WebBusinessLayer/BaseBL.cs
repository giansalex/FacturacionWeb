namespace WebBusinessLayer
{
    public class BaseBl
    {
        /// <summary>
        /// Contiene informacion de la ultima Consulta Ejecutada.
        /// </summary>
        public OperationResult LastResult { protected set; get; }

        public BaseBl()
        {
            LastResult = new OperationResult();
        }
    }
}
