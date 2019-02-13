using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MeMiXReMiX.Models;
using Microsoft.AspNetCore.Identity;

//private inject UserManager<ApplcationUser> UserManager;

namespace MeMiXReMiX.ViewModels
{
    public class AddSongViewModel
    {
        [Required (ErrorMessage = "Give your song a title")]
        public string Title { get; set; }

        public string FilePointer { get; set; }

        public string ApplicationUserID { get; set; }
        public string ApplicationsUserUserName { get; set; }

    }    
}
