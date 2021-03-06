using AsOrm3;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebRestAs400.Models
{
    [DBLibrary(ZLibrary.SNS)]
    [DBTable("WAUSR02K")]
    public class WebAccessoriUser
    {
        /// <summary>
        ///UdB ultimo agg. type:CHAR length:3
        /// </summary>
        [StringLength(3)]
        public string PPCUDB { get; set; }
        /// <summary>
        ///Utente ultimo agg. type:CHAR length:10
        /// </summary>
        [StringLength(10)]
        public string PPCUTE { get; set; }
        /// <summary>
        ///Data ultimo agg. type:NUMERIC length:8 dec:0
        /// </summary>
        [Range(0, 99999999)]
        public decimal PPFECH { get; set; }
        /// <summary>
        ///Ora ultimo agg. type:NUMERIC length:6 dec:0
        /// </summary>
        [Range(0, 999999)]
        public decimal PPHORA { get; set; }
        /// <summary>
        ///Numero ultima az. agg. type:DECIMAL length:9 dec:0
        /// </summary>
        [Range(0, 999999999)]
        public decimal PPNJOB { get; set; }
        /// <summary>
        ///Pgm ultimo agg. type:CHAR length:10
        /// </summary>
        [StringLength(10)]
        public string PPNPGM { get; set; }
        /// <summary>
        ///Attivo/annullato type:CHAR length:1
        /// </summary>
        [StringLength(1)]
        public string A001GR { get; set; }
        /// <summary>
        ///Flag sospensione type:CHAR length:1
        /// </summary>
        [StringLength(1)]
        public string A002GR { get; set; }
        /// <summary>
        ///UdB propietaria record type:CHAR length:3
        /// </summary>
        [StringLength(3)]
        public string A003GR { get; set; }
        /// <summary>
        ///UdB Sistema 1°resident type:CHAR length:3
        /// </summary>
        [StringLength(3)]
        public string A004GR { get; set; }
        /// <summary>
        ///UdB Sistema residente type:CHAR length:3
        /// </summary>
        [StringLength(3)]
        public string A005GR { get; set; }
        /// <summary>
        ///UdB Sistema destino type:CHAR length:3
        /// </summary>
        [StringLength(3)]
        public string A006GR { get; set; }
        /// <summary>
        ///PK Codice Utente Login type:CHAR length:10
        /// </summary>
        [Key]
        [StringLength(10)]
        public string GRCUTE { get; set; }
        /// <summary>
        ///Nome Utente Login type:CHAR length:35
        /// </summary>
        [StringLength(35)]
        public string GRNUTE { get; set; }
        /// <summary>
        ///PK UdB Fornitore type:CHAR length:3
        /// </summary>
        [Key]
        [StringLength(3)]
        public string GRCUDF { get; set; }
        /// <summary>
        ///PK Codice Fornitore type:CHAR length:6
        /// </summary>
        [Key]
        [StringLength(6)]
        public string GRCFOR { get; set; }
        /// <summary>
        ///Gestione Ordini type:CHAR length:1
        /// </summary>
        [StringLength(1)]
        public string GRORDI { get; set; }
        /// <summary>
        ///Gestione Spedizioni type:CHAR length:1
        /// </summary>
        [StringLength(1)]
        public string GRSPED { get; set; }
        /// <summary>
        ///Gestione Ricezioni type:CHAR length:1
        /// </summary>
        [StringLength(1)]
        public string GRRICE { get; set; }
        /// <summary>
        ///Gestione Imp/Exp file type:CHAR length:1
        /// </summary>
        [StringLength(1)]
        public string GRIMEX { get; set; }
        /// <summary>
        ///Gestione Codice Conf. type:CHAR length:1
        /// </summary>
        [StringLength(1)]
        public string GRCDCO { get; set; }
        /// <summary>
        ///Spedizione x Cod.Conf. type:CHAR length:1
        /// </summary>
        [StringLength(1)]
        public string GRSPCC { get; set; }
        /// <summary>
        ///% Toll.Qt{ > in Sped. type:DECIMAL length:5 dec:2
        /// </summary>
        [Range(0, 99999)]
        public decimal GRPERC { get; set; }
        /// <summary>
        ///Utente Gruppo Zegna type:CHAR length:1
        /// </summary>
        [StringLength(1)]
        public string GRZEGN { get; set; }
        /// <summary>
        ///Ragione Sociale Forn. type:CHAR length:35
        /// </summary>
        [StringLength(35)]
        public string GRRAGS { get; set; }
        /// <summary>
        ///Lingua Fornitore type:CHAR length:1
        /// </summary>
        [StringLength(1)]
        public string GRCLIN { get; set; }
        /// <summary>
        ///Descrizione Lingua type:CHAR length:35
        /// </summary>
        [StringLength(35)]
        public string GRDLIN { get; set; }
        /// <summary>
        ///UdB ACQ abilitata - 1 type:CHAR length:3
        /// </summary>
        [StringLength(3)]
        public string GRUDA1 { get; set; }
        /// <summary>
        ///UdB ACQ abilitata - 2 type:CHAR length:3
        /// </summary>
        [StringLength(3)]
        public string GRUDA2 { get; set; }
        /// <summary>
        ///UdB ACQ abilitata - 3 type:CHAR length:3
        /// </summary>
        [StringLength(3)]
        public string GRUDA3 { get; set; }
        /// <summary>
        ///UdB ACQ abilitata - 4 type:CHAR length:3
        /// </summary>
        [StringLength(3)]
        public string GRUDA4 { get; set; }
        /// <summary>
        ///UdB ACQ abilitata - 5 type:CHAR length:3
        /// </summary>
        [StringLength(3)]
        public string GRUDA5 { get; set; }
        /// <summary>
        ///UdB ACQ abilitata - 6 type:CHAR length:3
        /// </summary>
        [StringLength(3)]
        public string GRUDA6 { get; set; }
        /// <summary>
        ///UdB ACQ abilitata - 7 type:CHAR length:3
        /// </summary>
        [StringLength(3)]
        public string GRUDA7 { get; set; }
        /// <summary>
        ///UdB ACQ abilitata - 8 type:CHAR length:3
        /// </summary>
        [StringLength(3)]
        public string GRUDA8 { get; set; }
        /// <summary>
        ///UdB ACQ abilitata - 9 type:CHAR length:3
        /// </summary>
        [StringLength(3)]
        public string GRUDA9 { get; set; }
        /// <summary>
        ///UdB ACQ abilitata - 10 type:CHAR length:3
        /// </summary>
        [StringLength(3)]
        public string GRUDA0 { get; set; }
        /// <summary>
        ///Flag Rich.dati DOC type:CHAR length:1
        /// </summary>
        [StringLength(1)]
        public string GRFRDD { get; set; }
        /// <summary>
        ///Abbinamento DDT/FAT type:CHAR length:1
        /// </summary>
        [StringLength(1)]
        public string GRFADF { get; set; }
        /// <summary>
        ///Flag gest.Merce Pronta type:CHAR length:1
        /// </summary>
        [StringLength(1)]
        public string GRFAMP { get; set; }
        /// <summary>
        ///% max saldo riga type:DECIMAL length:5 dec:2
        /// </summary>
        [Range(0, 99999)]
        public decimal GRPSAL { get; set; }
        /// <summary>
        ///Utente ActiveDirectory type:CHAR length:60
        /// </summary>
        [StringLength(60)]
        public string GRUTAD { get; set; }
        /// <summary>
        ///Data ultimo ingresso type:NUMERIC length:8 dec:0
        /// </summary>
        [Range(0, 99999999)]
        public decimal GRDULI { get; set; }
        /// <summary>
        ///Ora ultimo ingresso type:NUMERIC length:6 dec:0
        /// </summary>
        [Range(0, 999999)]
        public decimal GROULI { get; set; }
        /// <summary>
        ///Gior Tolleranza delay type:NUMERIC length:2 dec:0
        /// </summary>
        [Range(0, 99)]
        public decimal GRGTDL { get; set; }

        /// <summary>
        ///MAX GG in + x AutoConf NUMERIC length:3 dec:0
        /// </summary>
        [Range(0, 999)]
        public decimal GRGCON { get; set; }
      
    }
}