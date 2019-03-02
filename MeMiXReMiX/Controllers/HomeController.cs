﻿using System;
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
using MeMiXReMiX.Helpers;

namespace MeMiXReMiX.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext context;

        public HomeController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View("MakeSongs");
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

                return Redirect(string.Format("/Home/ViewSongs?username={0}", addSongViewModel.ApplicationsUserUserName));
            }

            return View(addSongViewModel);
        }


        [Authorize]
        public IActionResult ViewSongs(string userName, string sortOrder)
        {
            string UserName = userName;
            IEnumerable<Song> AllSongs = context.Songs;

            switch (sortOrder)
            {
                case "name_desc":
                    AllSongs = AllSongs.OrderByDescending(s => s.ApplicationUserUserName);
                    break;
                case "name_asc":
                    AllSongs = AllSongs.OrderBy(s => s.ApplicationUserUserName);
                    break;
                case "title_desc":
                    AllSongs = AllSongs.OrderByDescending(s => s.Title);
                    break;
                case "title_asc":
                    AllSongs = AllSongs.OrderBy(s => s.Title);
                    break;
                default:
                    AllSongs = AllSongs.OrderBy(s => s.ID);
                    break;
            }

            if (userName == null)
            {
                ViewSongsViewModel viewModel = new ViewSongsViewModel
                {
                    Songs = AllSongs,
                    ApplicationUserUserName = ""
                };
                return View(viewModel);
            }
            else
            {
                var querySongs = from s in AllSongs where s.ApplicationUserUserName == userName
                select s;

                ViewSongsViewModel viewModel = new ViewSongsViewModel
                {
                    Songs = querySongs,
                    ApplicationUserUserName = UserName
                };
                return View(viewModel);
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
