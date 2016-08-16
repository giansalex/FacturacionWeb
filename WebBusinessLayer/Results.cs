namespace WebBusinessLayer
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string ErroMessage { get; set; }
        public string AdditionalInformation { get; set; }

        public static implicit operator OperationResult(bool value)
        {
            return new OperationResult
            {
                Success = value
            };
        }
    }
}
