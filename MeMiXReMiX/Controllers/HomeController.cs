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
                    FilePointer = addSongViewModel.FilePointer
                };

                context.Songs.Add(newSong);
                context.SaveChanges();

                return Redirect("/Home/ViewSongs");
            }

            return View(addSongViewModel);
        }

        [Authorize]
        public IActionResult ViewSongs()
        {
            List<Song> songs = context.Songs.ToList();

            return View(songs);
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
