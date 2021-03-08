using System;
using System.Collections.Generic;
using System.Text;

namespace Cpsc.Azure.GraphApi
{
    [Serializable]
    public class FieldData
    {
        public Fields fields { get; set; }
        public class Fields
        {
            public string Title { get; set; }
            public string Organization { get; set; }
            public string Phone { get; set; }
            public string Source { get; set; }
            public string Email { get; set; }
            public string Message { get; set; }
            public string Comments { get; set; }
        }

    }

}
