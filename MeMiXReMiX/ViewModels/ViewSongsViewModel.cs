﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeMiXReMiX.Models;

namespace MeMiXReMiX.ViewModels
{
    public class ViewSongsViewModel
    {
        public IEnumerable<Song> Songs { get; set; }
        public string ApplicationUserUserName { get; set; }
        public string SearchTerm { get; set; }
    }
}
