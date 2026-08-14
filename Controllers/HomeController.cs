using CognoDBGraph.Models;
using CognoDBGraph.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CognoDBGraph.Controllers
{
    public class HomeController : Controller
    {
        private readonly GraphService _graphService;

        public HomeController(GraphService graphService )
        {
            _graphService= graphService;    
        }

        public async Task<IActionResult> Index(string devId="dev1",string message="")
        {
            var Model = new DeveloperExplorerViewModel
            {
                SelectedDevId = devId,
                Skills=await _graphService.GetSkillsAsync(devId),
                Teammates=await _graphService.GetRecommendedTeammatesAsync(devId),
                Message=message

            };
            return View(Model);
        }

        [HttpPost]
        public async Task<IActionResult> Seed()
        {
            await _graphService.SeedDataAsync();
            return RedirectToAction("Index",new { devId="dev1",message="Database Seeded successfully with sample graph data!"});
        }

        public IActionResult Privacy()
        {
            return View(); }
        }
    }
