using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace needle {
    class Program {
        static void Main(string[] args) {
            var tiny=new Tiny(args);
            var fileNames = new List<string>();
            var tempFile=Path.GetTempFileName();
            var hayStack=tiny.Arguments.haystack.Split(';');
            using(var tpc=TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(tiny.Arguments.tfsUrl))) {
                tpc.Authenticate();
                var vcs = (VersionControlServer)tpc.GetService(typeof(VersionControlServer));
                var projects = vcs.GetAllTeamProjects(false);
                foreach(var teamProject in projects) {
                    foreach(var hay in hayStack){
                        var strands=GetStrands(hay);
                        if(string.Compare(teamProject.Name, hay.IndexOf("/")>0?hay.Substring(0,hay.IndexOf("/")):hay, StringComparison.CurrentCultureIgnoreCase) == 0) {
                            var csFiles = vcs.GetItems("$\\" + teamProject.Name + "\\" + tiny.Arguments.files, VersionSpec.Latest, RecursionType.Full, DeletedState.NonDeleted, ItemType.File);
                            foreach(var csFile in csFiles.Items){
                                csFile.DownloadFile(tempFile);
                                var s=File.OpenText(tempFile);
                                Console.SetCursorPosition(0, Console.CursorTop);
                                Console.Write("Searching in file "+csFile.ServerItem+"...");
                                if(s.ReadToEnd().Contains(tiny.Arguments.needle)) {
                                    fileNames.Add(csFile.ServerItem);
                                }
                                s.Close();
                            }
                        }
                    }
                }
                foreach(var item in fileNames) {
                    Console.WriteLine(item);
                }
            }
            File.Delete(tempFile);
            Console.ReadKey();
        }

        private static List<Strand> GetStrands(string hay) {
            throw new NotImplementedException();
        }
    }
}
