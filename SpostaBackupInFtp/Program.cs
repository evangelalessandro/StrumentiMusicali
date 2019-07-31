using StrumentiMusicali.ftpBackup.Backup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpostaBackupInFtp
{
    class Program
    {
        static int Main(string[] args)
        {
            using (var back = new BackupManager())
            {
                if (back.Manage())
                {
                    return (int)ExitCode.Success;
                }
                return (int)ExitCode.Error;
            }
        }
        enum ExitCode : int
        {
            Success = 0,
            Error = 1,

        }

    }
}
