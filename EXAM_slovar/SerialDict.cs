using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EXAM_slovar
{
    
        public class SerialDictionary
        {
            [XmlElement("Key")] //атрибут для показа ключа
            public string Key { get; set; }

            [XmlArray("Values")]//атрибут для показа списка значений
        [XmlArrayItem("Value")]//атрибут для каждого слова перевода
            public List<string> Values { get; set; }
        }
    }

