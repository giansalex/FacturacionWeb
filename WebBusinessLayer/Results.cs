using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBusinessLayer
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string ErroMessage { get; set; }
        public string AdditionalInformation { get; set; }
    }
}
