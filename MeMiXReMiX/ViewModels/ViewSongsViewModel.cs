using System;
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
        public string SortOrder { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage
        {
            get
            {
                return (CurrentPage > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }
    }
}
