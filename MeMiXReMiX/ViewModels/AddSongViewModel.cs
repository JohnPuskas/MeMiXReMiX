using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MeMiXReMiX.Models;

namespace MeMiXReMiX.ViewModels
{
    public class AddSongViewModel
    {
        [Required (ErrorMessage = "Give your song a title")]
        public string Title { get; set; }

        public string FilePointer { get; set; }

    }    
}
