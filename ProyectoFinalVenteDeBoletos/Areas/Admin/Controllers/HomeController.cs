﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalVentaDeBoletos.Helpers;
using ProyectoFinalVentaDeBoletos.Models.ViewModels;
using System.Security.Claims;
using ProyectoFinalVentaDeBoletos.Repositories;

namespace ProyectoFinalVentaDeBoletos.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
      
        [HttpGet("/Admin")]
        [HttpGet("/Admin/Home/Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
