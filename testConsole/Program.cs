﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var ftpTest = new StrumentiMusicali.ftpBackup.FtpManager();
            //class1.Upload(@"C:\Users\fastcode13042017\Downloads\DocFatture\b9f768fb-8f3e-4a42-b54f-11fbde23ec50\Answers-to-Difficult-Bible-Passages.pdf");
            //class1.Delete("Answers-to-Difficult-Bible-Passages.pdf");
            var list =ftpTest.FileList();
        }
    }
}
