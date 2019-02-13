using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeMiXReMiX.Models
{
    public class Song
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string FilePointer { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserID { get; set; }
        public string ApplicationUserUserName { get; set; }
    }
}
