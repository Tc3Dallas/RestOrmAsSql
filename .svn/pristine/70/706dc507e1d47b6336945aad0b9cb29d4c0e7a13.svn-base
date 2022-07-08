using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsOrm3
{
    /// <summary>
    /// Attributo per le classi modello per specificare il nome della tabella mappata
    /// Se non è utilizzato l'attributo fa fede il nome della classe modello
    /// L'attributo può contenere anche la libreria fissa per una tabella
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class DBTable : System.Attribute
    {
        public string name;

        public DBTable(string name)
        {
            this.name = name;
        }

        public DBTable()
        {
            name = null;
        }
    }

    public enum ZLibrary { SNS, CSIS, CSISSoc }

    /// <summary>
    /// Attributo per le classi modello per specificare il nome della libreria mappata
    /// Se non è utilizzato l'attributo fa fede il nome della classe modello
    /// L'attributo può contenere anche la libreria fissa per una tabella
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class DBLibrary : System.Attribute
    {
        public ZLibrary? zLibrary;

        public DBLibrary(ZLibrary zLibrary)
        {
            this.zLibrary = zLibrary;
        }

        public DBLibrary()
        {
            zLibrary = null;
        }
    }

    /// <summary>
    /// Specifica il nome nel campo mappato
    /// Se non utlizzato fa fede il nome della Property
    /// TODO: ATTENZIONE! non implementato il suo utilizzo
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class DBColumn : System.Attribute
    {
        public string name;

        public DBColumn(string name)
        {
            this.name = name;
        }

        public DBColumn()
        {
            name = null;
        }
    }

    /// <summary>
    /// Attributo per le properties di un modello per specificare che non sono campi reali del DB ma entità collegate
    /// Separare le Foreing Keys con la virgola
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class Virtual : System.Attribute
    {
        public Dictionary<string, string> foreignKeys;

        public Virtual(params string[] foreignKeys)
        {
            this.foreignKeys = new Dictionary<string, string>();
            foreignKeys.ToList().ForEach(i =>
            {
                if (i.Contains("-"))
                {
                    string[] kv = i.Split('-');
                    this.foreignKeys.Add(kv[0], kv[1]);
                }
                else
                {
                    this.foreignKeys.Add("FIX" + i, i);
                }
            });

        }
    }
}