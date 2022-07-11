using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlOrm
{
    /// <summary>
    /// Attributo per le classi modello per specificare il nome della tabella mappata
    /// Se non è utilizzato l'attributo fa fede il nome della classe modello
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class MsSqlTable : System.Attribute
    {
        public string name;

        public MsSqlTable(string name)
        {
            this.name = name;
        }

        public MsSqlTable()
        {
            name = null;
        }
    }

    /// <summary>
    /// Attributo per le properties di un modello per specificare che non sono campi reali del DB ma entità collegate
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class Virtual : System.Attribute
    {
        public Virtual()
        {
        }
    }
}
