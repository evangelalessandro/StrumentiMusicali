﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Il codice è stato generato da uno strumento.
//     Versione runtime:4.0.30319.42000
//
//     Le modifiche apportate a questo file possono provocare un comportamento non corretto e andranno perse se
//     il codice viene rigenerato.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StrumentiMusicali.Library.Properties {
    using System;
    
    
    /// <summary>
    ///   Classe di risorse fortemente tipizzata per la ricerca di stringhe localizzate e così via.
    /// </summary>
    // Questa classe è stata generata automaticamente dalla classe StronglyTypedResourceBuilder.
    // tramite uno strumento quale ResGen o Visual Studio.
    // Per aggiungere o rimuovere un membro, modificare il file con estensione ResX ed eseguire nuovamente ResGen
    // con l'opzione /str oppure ricompilare il progetto VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource1 {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource1() {
        }
        
        /// <summary>
        ///   Restituisce l'istanza di ResourceManager nella cache utilizzata da questa classe.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("StrumentiMusicali.Library.Properties.Resource1", typeof(Resource1).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Esegue l'override della proprietà CurrentUICulture del thread corrente per tutte le
        ///   ricerche di risorse eseguite utilizzando questa classe di risorse fortemente tipizzata.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a declare @name nvarchar(50) =@p0  
        ///declare @pathFile nvarchar(50)  
        ///select @pathFile = @p1 + &apos;Negozio.bak&apos;  
        ///if exists (select * from sys.backup_devices where name = @name)  
        ///begin   
        ///  EXEC sp_dropdevice @name;  
        ///end   
        ///   
        ///exec sp_addumpdevice N&apos;disk&apos;, @name, @pathFile.
        /// </summary>
        internal static string AddDeviceBackup {
            get {
                return ResourceManager.GetString("AddDeviceBackup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a /*
        ///SQL Server Maintenance Solution - SQL Server 2008, SQL Server 2008 R2, SQL Server 2012, SQL Server 2014, SQL Server 2016, SQL Server 2017, and SQL Server 2019
        ///Backup: https://ola.hallengren.com/sql-server-backup.html
        ///Integrity Check: https://ola.hallengren.com/sql-server-integrity-check.html
        ///Index and Statistics Maintenance: https://ola.hallengren.com/sql-server-index-and-statistics-maintenance.html
        ///License: https://ola.hallengren.com/license.html
        ///GitHub: https://github.com/olahallengren/sql-server [stringa troncata]&quot;;.
        /// </summary>
        internal static string IndexOptimize {
            get {
                return ResourceManager.GetString("IndexOptimize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a 
        ///ALTER  OR CREATE procedure [dbo].[sp_Backup]
        ///as
        ///
        ///SET XACT_ABORT ON
        ///declare @name nvarchar(50) =&apos;BACKUP_NegozioSM&apos;
        ///
        ///
        ///DECLARE @folder nvarchar(MAX)
        ///
        ///SELECT @folder =BackupSetting_FolderLocalServer FROM SettingBackupFtp 
        ///
        ///IF right(@folder ,1)=&apos;\&apos;
        ///BEGIN
        ///  SELECT @folder =@folder + &apos;\&apos;
        ///END
        ///
        ///
        ///DELETE EventLogs WHERE DataCreazione&lt;GETDATE()-15
        /// 
        ///SELECT @folder =@folder +&apos;Negozio.bak&apos;
        ///
        ///IF EXISTS (SELECT NULL
        ///               FROM   sys.backup_devices
        ///               WHERE  [Name] = @name
        ///      [stringa troncata]&quot;;.
        /// </summary>
        internal static string SpBackup {
            get {
                return ResourceManager.GetString("SpBackup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a IF EXISTS (SELECT * FROM sys.objects 
        ///WHERE object_id = OBJECT_ID(N&apos;[dbo].[sp_Backup]&apos;) AND type in (N&apos;P&apos;, N&apos;PC&apos;))
        ///  select 1
        ///else 
        ///  select 0.
        /// </summary>
        internal static string SpCheckExists {
            get {
                return ResourceManager.GetString("SpCheckExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a INSERT INTO CONF_CodiciABarre (CodiceABarre, Azione, Descrizione, DataCreazione, DataUltimaModifica, CodiceIva)
        ///SELECT CONCAT(N&apos;99999&apos; , a.ID), N&apos;ArticoloGenerico&apos;+ NomeReparto, N&apos;Articolo Generico &apos; + NomeReparto, GETDATE()
        ///, GETDATE(),a.CodicePerRegistratoreDiCassa
        ///FROM RegistratoreDiCassaReparti a
        ///
        ///
        ///INSERT INTO CONF_CodiciABarre (CodiceABarre, Azione, Descrizione, DataCreazione, DataUltimaModifica, CodiceIva)
        ///SELECT CONCAT(N&apos;999990&apos; , 1), N&apos;StampaScontrinoContanti&apos;, N&apos;Stampa Scontrino Contanti&apos;, G [stringa troncata]&quot;;.
        /// </summary>
        internal static string sqlCodiciABarre {
            get {
                return ResourceManager.GetString("sqlCodiciABarre", resourceCulture);
            }
        }
    }
}
