using AsOrm3;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebRestAs400.Models
{
    [DBLibrary(ZLibrary.SNS)]
    [DBTable("LMPRIXSF")]
    public class LuaMpRigaListino
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
        public string A001XS { get; set; }
        [Display(Name = "Flag sospensione", Description = "CHAR 1 ")]
        public string A002XS { get; set; }
        [Display(Name = "UdB propietaria record", Description = "CHAR 3 ")]
        public string A003XS { get; set; }
        [Display(Name = "UdB Sistema 1°resident", Description = "CHAR 3 ")]
        public string A004XS { get; set; }
        [Display(Name = "UdB Sistema residente", Description = "CHAR 3 ")]
        public string A005XS { get; set; }
        [Display(Name = "UdB Sistema destino", Description = "CHAR 3 ")]
        public string A006XS { get; set; }
        [Display(Name = "Udb listino", Description = "CHAR 3 ")]
        [Key]
        public string XSCUDB { get; set; }
        [Display(Name = "Udb fornitore", Description = "CHAR 3 ")]
        [Key]
        public string XSUDBF { get; set; }
        [Display(Name = "Codice fornitore", Description = "CHAR 6 ")]
        [Key]
        public string XSCFOR { get; set; }
        [Display(Name = "Stagione", Description = "CHAR 1 ")]
        [Key]
        public string XSCSTG { get; set; }
        [Display(Name = "Codice Listino UNI ACQ", Description = "CHAR 6 ")]
        [Key]
        public string XSCLUA { get; set; }

        //CHIAVI
        //FAM
        [Display(Name = "Famiglia", Description = "CHAR 1 ")]
        [MaxLength(1)]
        [Key]
        //[ListinoTipoChiave("XTFFAM")]
        //[DisplayInputType(css = "form-small-control")]
        public string XSCFAM { get; set; }

        //TESAN
        [Display(Name = "Codice tessuto", Description = "CHAR 6 ")]
        [Key]
        [MaxLength(6)]
        //[ListinoTipoChiave("XTFTES")]
        //[DisplayInputType()]
        public string XSCTEC { get; set; }

        //TESAN
        [Display(Name = "Variante / Colore", Description = "CHAR 3 ")]
        [Key]
        [MaxLength(3)]
        //[ListinoTipoChiave("XTFCOL")]
        //[DisplayInputType()]
        public string XSCOL3 { get; set; }

        //TESAN
        //GiaVer: cambiata lunghezza campo da 15 a 25
        [Display(Name = "Codice tess. fornitore", Description = "CHAR 25 ")]
        [Key]
        [MaxLength(25)]
        //[ListinoTipoChiave("XTFTEF")]
        //[DisplayInputType()]
        public string XSCTEF { get; set; }

        //TESAN
        //GiaVer: cambiata lunghezza campo da 6 a 15
        [Display(Name = "Codice disegno fornit", Description = "CHAR 15 ")]
        [Key]
        [MaxLength(15)]
        //[ListinoTipoChiave("XTFDIF")]
        //[DisplayInputType()]
        public string XSCDIF { get; set; }

        //TESAN
        //GiaVer: cambiata lunghezza campo da 6 a 15
        [Display(Name = "Variante fornitore", Description = "CHAR 15 ")]
        [Key]
        [MaxLength(15)]
        //[ListinoTipoChiave("XTFVAF")]
        //[DisplayInputType()]
        public string XSCVAF { get; set; }

        //????? 
        [Display(Name = "Forma", Description = "CHAR 3 ")]
        [Key]
        [MaxLength(3)]
        //[ListinoTipoChiave("XTFFRM")]
        //[DisplayInputType()]
        public string XSFORM { get; set; }

        //??????
        [Display(Name = "Macro codice", Description = "CHAR 15 ")]
        [Key]
        [MaxLength(15)]
        //[ListinoTipoChiave("XTFMAC")]
        //[DisplayInputType()]
        public string XSMCOD { get; set; }

        //RTA - zoom
        [Display(Name = "Raggr.Tipi Ordini RTA", Description = "CHAR 2 ")]
        [Key]
        [MaxLength(2)]
        //[ListinoTipoChiave("XTFRTA")]
        //[DisplayInputType()]
        public string XSCRTA { get; set; }

        //???????
        [Display(Name = "Raggruppamento misura", Description = "CHAR 1 ")]
        [Key]
        [MaxLength(1)]
        //[ListinoTipoChiave("XTFRMS")]
        //[DisplayInputType()]
        public string XSCRMI { get; set; }

        //???????
        [Display(Name = "Misura", Description = "CHAR 18 ")]
        [Key]
        [MaxLength(18)]
        //[ListinoTipoChiave("XTFRMI")]
        //[DisplayInputType()]
        public string XSMISU { get; set; }

        //??????
        [Display(Name = "Taglia", Description = "CHAR 12 ")]
        [Key]
        [MaxLength(12)]
        //[ListinoTipoChiave("XTFTAG")]
        //[DisplayInputType()]
        public string XSCTAG { get; set; }

        //?????????
        [Display(Name = "Progetto commerciale", Description = "CHAR 10 ")]
        [Key]
        [MaxLength(10)]
        //[ListinoTipoChiave("XTFPRC")]
        //[DisplayInputType()]
        public string XSPRGC { get; set; }

        [Display(Name = "Fast delivery ( Y,N )", Description = "CHAR 1 ")]
        [Key]
        [MaxLength(1)]
        //[ListinoTipoChiave("XTFFDE")]
        //[DisplayInputType("CheckBox")]
        public string XSFTDE { get; set; }

        //CCR zoom
        [Display(Name = "Tipo colore CCR", Description = "CHAR 3 ")]
        [Key]
        [MaxLength(3)]
        //[ListinoTipoChiave("XTFCCR")]
        //[DisplayInputType()]
        public string XSTCCR { get; set; }

        [Display(Name = "Qtà fino a", Description = "NUMERIC 11 2")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Key]
        //[ListinoTipoChiave("XTFRGQ")]
        //[DisplayInputType()]
        public decimal XSQLIM { get; set; }

        //BRD ??? zoom
        [Display(Name = "Brand CSIS", Description = "CHAR 4 ")]
        [Key]
        [MaxLength(4)]
        //[ListinoTipoChiave("XTFBRD")]
        //[DisplayInputType()]
        public string XSBRAN { get; set; }

        [Display(Name = "Data validit{ da", Description = "NUMERIC 8 0")]
        [Key]
        //[ListinoTipoChiave("XTFPVA")]
        public decimal XSDVAD { get; set; }

        [Display(Name = "Data validit{ a", Description = "NUMERIC 8 0")]
        [Key]
        //[ListinoTipoChiave("XTFPVA")]
        public decimal XSDVAA { get; set; }

        /// <summary>
        ///Data Validit{ Reale type:NUMERIC length:8 dec:0
        /// </summary>
        [Range(0, 99999999)]
        public decimal XSDVAR { get; set; }

        [Display(Name = "Lead time totale", Description = "NUMERIC 3 0")]
        //TODO Da sistemare
        //[Key]
        //[ListinoTipoChiave("XTFLTT")]
        //[DisplayInputType()]
        public decimal XSLTTO { get; set; }

        [Display(Name = "Lead time prenotato", Description = "NUMERIC 3 0")]
        [Key]
        //[ListinoTipoChiave("XTFLTP")]
        //[DisplayInputType()]
        public decimal XSLTPR { get; set; }

        [Display(Name = "Lead time dispo greggi", Description = "NUMERIC 3 0")]
        [Key]
        //[ListinoTipoChiave("XTFLTG")]
        //[DisplayInputType()]
        public decimal XSLTDG { get; set; }

        [Display(Name = "Minimi", Description = "NUMERIC 9 2")]
        [Key]
        //[ListinoTipoChiave("XTFMIN")]
        //[DisplayInputType()]
        public decimal XSNMIN { get; set; }

        [Display(Name = "Multipli", Description = "NUMERIC 9 2")]
        [Key]
        //[ListinoTipoChiave("XTFMUL")]
        //[DisplayInputType()]
        public decimal XSMULT { get; set; }

        //TPO zoom
        [Display(Name = "Tipo ordine", Description = "CHAR 1 ")]
        [Key]
        [MaxLength(1)]
        //[ListinoTipoChiave("XTTIPO")]
        //[DisplayInputType()]
        public string XSTORD { get; set; }

        //VEL Zoom
        [Display(Name = "Esclusività", Description = "CHAR 1 ")]
        [Key]
        [MaxLength(1)]
        //[ListinoTipoChiave("XTESCL")]
        //[DisplayInputType()]
        public string XSESCL { get; set; }

        //TTR Zoom - verifica
        [Display(Name = "Tipo trattamento", Description = "CHAR 1 ")]
        [Key]
        [MaxLength(1)]
        //[ListinoTipoChiave("XTTTRA")]
        //[DisplayInputType()]
        public string XSTTRA { get; set; }

        //TTF zoom - verifica!
        [Display(Name = "Tipo tessuto fornitore", Description = "CHAR 1 ")]
        [Key]
        [MaxLength(1)]
        //[ListinoTipoChiave("XTCTTF")]
        //[DisplayInputType()]
        public string XSCTTF { get; set; }

        [Display(Name = "Scatola ( dimensione )", Description = "NUMERIC 5 0")]
        [Key]
        //[ListinoTipoChiave("XTSCAT")]
        //[DisplayInputType()]
        public decimal XSSCAT { get; set; }

        //PCT zoom - verifica
        [Display(Name = "Pettinato/cardato", Description = "CHAR 1 ")]
        [Key]
        [MaxLength(1)]
        //[ListinoTipoChiave("XTPTCA")]
        //[DisplayInputType()]
        public string XSPTCA { get; set; }

        [Display(Name = "Marchiato", Description = "CHAR 1 ")]
        [Key]
        [MaxLength(1)]
        //[ListinoTipoChiave("XTFACM")]
        //[DisplayInputType()]
        public string XSFACM { get; set; }

        //TENVF.D3TITO se trovo la variante e il campo è valorizzato (udb, for, tesf, varf, col)
        //altrimenti TENAF.D4TITO IN BASE ALL'ARTICOLO (udb, for, tesf)
        [Display(Name = "Titolo Filato", Description = "CHAR 5 ")]
        [Key]
        [MaxLength(5)]
        //[ListinoTipoChiave("XTTITO")]
        //[DisplayInputType()]
        public string XSTITO { get; set; }

        //TENVF.D3ALTS se trovo la variante e il campo è valorizzato (udb, for, tesf, varf, col)
        //Altrimenti TENAF.D4ALTS IN BASE ALL'ARTICOLO (udb, for, tesf)
        [Display(Name = "Altezza pezza", Description = "NUMERIC 7 2")]
        [Key]
        //[ListinoTipoChiave("XTALTP")]
        //[DisplayInputType()]
        public decimal XSALTP { get; set; }

        [Display(Name = "Raggruppamento cotte", Description = "CHAR 6 ")]
        [Key]
        [MaxLength(6)]
        //[ListinoTipoChiave("XTFCOT")]
        //[DisplayInputType()]
        public string XSCOTT { get; set; }

        [Display(Name = "Descrizione Cotte", Description = "CHAR 30")]
        [MaxLength(30)]
        //[ListinoTipoChiave("XTFCOT")]
        //[DisplayInputType()]
        public string XSDECO { get; set; }

        [Display(Name = "Unita di misura", Description = "CHAR 2 ")]
        [MaxLength(2)]
        //[ListinoTipoCampoCollegato("XTUMPR")]
        //[DisplayInputType()]
        public string XSUMPR { get; set; }

        [Display(Name = "Campo descrittivo 1 ", Description = "CHAR 20")]
        [MaxLength(20)]
        //[ListinoTipoChiave("XTFRE1")]
        //[DisplayInputType()]
        public string XSFRE1 { get; set; }
        [Display(Name = "Campo descrittivo 2", Description = "CHAR 20")]
        [MaxLength(20)]
        //[ListinoTipoChiave("XTFRE2")]
        //[DisplayInputType()]
        public string XSFRE2 { get; set; }
        [Display(Name = "Campo descrittivo 3 ", Description = "CHAR 20")]
        [MaxLength(20)]
        //[ListinoTipoChiave("XTFRE3")]
        //[DisplayInputType()]
        public string XSFRE3 { get; set; }


        //[Display(Name = "Incremento Valore FastDelivery", Description = "NUMERIC 11 2")]
        //[DisplayInputType()]
        //[ListinoTipoIncremento("XTITFD","F")]
        //public decimal XSINCV { get; set; }

        //[Display(Name = "	Incremento Percentuale FastDelivery", Description = "NUMERIC 11 2")]
        //[DisplayInputType()]
        //[ListinoTipoIncremento("XTITFD", "P")]
        //public decimal XSINCP { get; set; }

        [Display(Name = "Incremento Valore Cotte", Description = "NUMERIC 11 2")]
        //[DisplayInputType()]
        //[ListinoTipoIncremento("XTITTC", "F")]
        public decimal XSICCV { get; set; }

        [Display(Name = "	Incremento Percentuale Cotte", Description = "NUMERIC 11 2")]
        //[DisplayInputType()]
        //[ListinoTipoIncremento("XTITTC", "P")]
        public decimal XSICCP { get; set; }

        [Display(Name = "Accesso", Description = "NUMERIC 6 0")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Required]
        [Key]
        //[ListinoTipoChiave("XTFACC")]
        //[DisplayInputType()]
        public decimal XSACCE { get; set; }

        //MDL 23/06/2020
        /// <summary>
        ///Unit{ Misura Prz Incr type:CHAR length:2
        /// </summary>
        [Display(Name = "UN Prz. inc.", Description = "CHAR 2")]
        //[ListinoTipoChiave("XTPINC")]
        [MaxLength(2)]
        //[DisplayInputType()]
        public string XSUMPI { get; set; }

        [Display(Name = "Prezzo base", Description = "NUMERIC 14 5")]
        //[ListinoTipoCampoCollegato("XTDECP")]
        //[DisplayInputType()]
        public decimal XSPBAS { get; set; }
        [Display(Name = "Prezzo campionario", Description = "NUMERIC 14 5")]
        //[ListinoTipoChiave("XTPCAM")]
        //[DisplayInputType()]
        public decimal XSPCAM { get; set; }

        [Display(Name = "flg Prez.rif.list.qtà", Description = "NUMERIC 6 0")]
        //[ListinoTipoChiave("XTFRGQ")]
        [MaxLength(1)]
        //[DisplayInputType(type = "checkbox")]
        public string XSFPRQ { get; set; }


        // GiaVer 03/10/2018 campi nuovi FAMA
        [Display(Name = "Raggruppamento accessori CRC ", Description = "CHAR 3")]
        [MaxLength(3)]
        //[ListinoTipoChiave("XTCCRC")]
        //[DisplayInputType()]
        public string XSCCRC { get; set; }

        [Display(Name = "Metodo Pezza Fodera ", Description = "CHAR 1")]
        [MaxLength(3)]
        //[ListinoTipoChiave("XTMETP")]
        //[DisplayInputType()]
        public string XSMETP { get; set; }

        [Display(Name = "Tipo Materiale", Description = "CHAR 3 ")]
        [MaxLength(3)]
        //[ListinoTipoChiave("XTTIPM")]
        //[DisplayInputType()]
        public string XSTIPM { get; set; }

        [Display(Name = "Caratteristiche", Description = "CHAR 3 ")]
        [MaxLength(3)]
        //[ListinoTipoChiave("XTCARA")]
        //[DisplayInputType()]
        public string XSCARA { get; set; }


        [Display(Name = "Numero Cursori", Description = "NUMERIC 3 0")]
        //[ListinoTipoChiave("XTNRCR")]
        //[DisplayInputType()]
        public decimal XSNRCR { get; set; }

        [Display(Name = "Colore Istituzionale", Description = "CHAR 3 ")]
        [MaxLength(3)]
        //[ListinoTipoChiave("XTCLIS")]
        //[DisplayInputType()]
        public string XSCLIS { get; set; }

        [Display(Name = "Formato Cono", Description = "CHAR 1 ")]
        [MaxLength(1)]
        //[ListinoTipoChiave("XTFRMC")]
        //[DisplayInputType()]
        public string XSFRMC { get; set; }

        [Display(Name = "MadeIn acc.in Fattura", Description = "CHAR 2 ")]
        [MaxLength(2)]
        //[ListinoTipoChiave("XTMDFA")]
        //[DisplayInputType()]
        public string XSMDFA { get; set; }

        [Display(Name = "Cod.Origine preferenzi", Description = "CHAR 3 ")]
        [MaxLength(3)]
        //[ListinoTipoChiave("XTCOPF")]
        //[DisplayInputType()]
        public string XSCOPF { get; set; }

        [Display(Name = "Matrice listino", Description = "CHAR 6 ")]
        [MaxLength(6)]
        //[ListinoTipoChiave("XTMALA")]
        //[DisplayInputType()]
        public string XSMALA { get; set; }

        [Display(Name = "Sconto percentuale", Description = "NUMERIC 5 2")]
        // GiaVer:  VERIFICARE SE METTERE l'annotation maxlength
        // [MaxLength(6)]
        //[ListinoTipoChiave("XTSCFO")]
        //[DisplayInputType()]
        public decimal XSSCPE { get; set; }

        [Display(Name = "Sconto valore", Description = "NUMERIC 13 2")]
        // GiaVer:  VERIFICARE SE METTERE l'annotation maxlength
        //[MaxLength(6)]
        //[ListinoTipoChiave("XTSCFO")]
        //[DisplayInputType()]
        public decimal XSSCVA { get; set; }

        [Display(Name = "Prezzo attivo Y/N", Description = "CHAR 1 ")]
        [MaxLength(1)]
        //[ListinoTipoChiave("XSPRAT")]
        //[DisplayInputType()]
        public string XSPRAT { get; set; }

        //campi accesi da XTPINC 
        [Display(Name = "UM.Incremento Misura", Description = "CHAR 2 ")]
        //[ListinoTipoChiave("XTPINC")]
        [MaxLength(2)]
        //[DisplayInputType()]
        public string XSUMIN { get; set; }

        [Display(Name = "Qt.a Increm. Misura", Description = "NUMERIC 7 2")]
        //[ListinoTipoChiave("XTPINC")]
        //[DisplayInputType()]
        public decimal XSQTIN { get; set; }

        [Display(Name = "Val.di ogni Incremen.", Description = "NUMERIC 15 6")]
        //[ListinoTipoChiave("XTPINC")]
        //[DisplayInputType()]
        public decimal XSVAIN { get; set; }

        [Display(Name = "Mat.Acq.da misurta", Description = "NUMERIC 7 2")]
        //[ListinoTipoChiave("XTPINC")]
        //[DisplayInputType()]
        public decimal XSMIDA { get; set; }

        [Display(Name = "Mat.Acq.fino a Misura", Description = "NUMERIC 7 2")]
        //[ListinoTipoChiave("XTPINC")]
        //[DisplayInputType()]
        public decimal XSMISA { get; set; }

        [Display(Name = "Mis.Abit.di Riferim.", Description = "NUMERIC 7 2")]
        //[ListinoTipoChiave("XTPINC")]
        //[DisplayInputType()]
        public decimal XSMRIF { get; set; }
        //fine campi accesi da XTPINC 

        // Giaver Fine campi nuovi FAMA


    }
}