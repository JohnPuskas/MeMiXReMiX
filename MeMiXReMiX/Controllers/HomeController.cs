using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MeMiXReMiX.Models;
using MeMiXReMiX.ViewModels;
using MeMiXReMiX.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MeMiXReMiX.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext context;

        public HomeController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }


        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult MakeSongs()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult MakeSongs(AddSongViewModel addSongViewModel)
        {
            if (ModelState.IsValid)
            {
                Song newSong = new Song
                {
                    Title = addSongViewModel.Title,
                    FilePointer = addSongViewModel.FilePointer,
                    ApplicationUserUserName = addSongViewModel.ApplicationsUserUserName,
                    ApplicationUserID = addSongViewModel.ApplicationUserID
                };

                context.Songs.Add(newSong);
                context.SaveChanges();

                return Redirect(string.Format("/Home/ViewUserSongs?={0}", addSongViewModel.ApplicationsUserUserName));
            }

            return View(addSongViewModel);
        }

        [Authorize]
        public IActionResult ViewUserSongs(string userName)
        {
            string CurrentUser = userName;
            List<Song> AllSongs = context.Songs.ToList();

            var querySongs = from s in AllSongs where s.ApplicationUserUserName == CurrentUser
            select s;           
            
            /* List<Song> Songs = context.Songs.Any(s => s.ApplicationUserUserName == userName).ToList()*/;
            ViewUserSongsViewModel viewModel = new ViewUserSongsViewModel
            {
                Songs = querySongs,
                ApplicationUserUserName = CurrentUser
            };

            return View(viewModel);
        }

        [Authorize]
        public IActionResult ViewSongs(string userName)
        {
            IEnumerable<Song> AllSongs = context.Songs;

            if (userName == null)
            {
                ViewBag.Title = "All Songs";
                return View(AllSongs);
            }
            else
            {
                ViewBag.Title = (userName + "'s songs");

                var querySongs = from s in AllSongs where s.ApplicationUserUserName == userName
                select s;

                return View(querySongs);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
