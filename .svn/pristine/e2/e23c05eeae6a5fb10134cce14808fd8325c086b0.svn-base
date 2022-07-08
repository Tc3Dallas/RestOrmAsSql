using AsOrm3;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebRestAs400.Models
{
    [DBLibrary(ZLibrary.SNS)]
    [DBTable("LMPTEXRF")]
    public class LuaMpTestaListino
    {
        [Display(Name = "UdB ultimo agg.", Description = "CHAR 3 ")]
        public string PPCUDB { get; set; }
        [Display(Name = "Utente ultimo agg.", Description = "CHAR 10 ")]
        public string PPCUTE { get; set; }
        [Display(Name = "Data ultimo agg.", Description = "NUMERIC 8 0")]
        public decimal PPFECH { get; set; }
        [Display(Name = "Ora ultimo agg.", Description = "NUMERIC 6 0")]
        public decimal PPHORA { get; set; }
        [Display(Name = "Numero ultima az. agg.", Description = "DECIMAL 9 0")]
        public decimal PPNJOB { get; set; }
        [Display(Name = "Pgm ultimo agg.", Description = "CHAR 10 ")]
        public string PPNPGM { get; set; }
        [Display(Name = "Attivo/annullato", Description = "CHAR 1 ")]
        public string A001XR { get; set; }
        [Display(Name = "Flag sospensione", Description = "CHAR 1 ")]
        public string A002XR { get; set; }
        [Display(Name = "UdB propietaria record", Description = "CHAR 3 ")]
        public string A003XR { get; set; }
        [Display(Name = "UdB Sistema 1°resident", Description = "CHAR 3 ")]
        public string A004XR { get; set; }
        [Display(Name = "UdB Sistema residente", Description = "CHAR 3 ")]
        public string A005XR { get; set; }
        [Display(Name = "UdB Sistema destino", Description = "CHAR 3 ")]
        public string A006XR { get; set; }
        //[Display(Name = "Codice Operation", Description = "CHAR 1 ")]
        //[Key]
        //public string XRCOPR { get; set; }
        [Display(Name = "Udb Fornitore", Description = "CHAR 3 ")]
        [Key]
        public string XRUDBF { get; set; }
        [Display(Name = "Codice fornitore", Description = "CHAR 6 ")]
        [Key]
        public string XRCFOR { get; set; }
        //GiaVer: FLAG Stagione obbligatoria: SE VALORIZZATO LA STG. NON E' OBBLIGATORIA
        [Display(Name = "Flag Senza Stagione", Description = "CHAR 1 ")]
        [Key]
        public string XRFSTG { get; set; }
        //GiaVer: Fine 
        [Display(Name = "Stagione", Description = "CHAR 1 ")]
        [Key]
        public string XRCSTG { get; set; }
        [Display(Name = "Codice Listino UNI ACQ", Description = "CHAR 6 ")]
        [Key]
        public string XRCLUA { get; set; }
        [Display(Name = "Data validita da", Description = "NUMERIC 8 0")]
        public decimal XRDVAD { get; set; }
        [Display(Name = "Data validita a", Description = "NUMERIC 8 0")]
        public decimal XRDVAA { get; set; }
        [Display(Name = "Tipo Listino UNI ACQ", Description = "CHAR 6 ")]
        public string XRTLUA { get; set; }
        [Display(Name = "Codice valuta", Description = "CHAR 3 ")]
        public string XRCVAL { get; set; }
        [Display(Name = "Udb listino", Description = "CHAR 3 ")]
        [Key]
        public string XRCUDB { get; set; }
    }
}