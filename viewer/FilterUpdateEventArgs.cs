using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MixPanelViewer
{
    public class FilterUpdateEventArgs : System.EventArgs
    {
        private string m_Property;
        public string Property
        {
            get { return m_Property; }
            set { m_Property = value; }
        }

        private string m_Operation;
        public string Operation
        {
            get { return m_Operation; }
            set { m_Operation = value; }
        }

        private string m_Value;
        public string Value
        {
            set { m_Value = value; }
            get { return m_Value; }
        }

        public FilterUpdateEventArgs(string property, string operation, string value)
        {
            m_Value = value;
            m_Property = property;
            m_Operation = operation;
        }
    }
}
