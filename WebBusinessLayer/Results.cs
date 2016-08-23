namespace WebBusinessLayer
{
    /// <summary>
    /// Class OperationResult.
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="OperationResult"/> is success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success { get; set; }
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The erro message.</value>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Gets or sets the additional information.
        /// </summary>
        /// <value>The additional information.</value>
        public string AdditionalInformation { get; set; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Boolean"/> to <see cref="OperationResult"/>.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OperationResult(bool value)
        {
            return new OperationResult
            {
                Success = value
            };
        }
    }
}
