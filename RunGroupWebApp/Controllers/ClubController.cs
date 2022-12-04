using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModel;

namespace RunGroupWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;

        public ClubController(ApplicationDbContext context, IClubRepository clubRepository, IPhotoService photoService)
        {
            _context = context;
            _clubRepository = clubRepository;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            var clubs = await _clubRepository.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Club club = await _clubRepository.GetByIdAsync(id);
            return View(club);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                //return View(club);
                var result = await _photoService.AddPhotoAsync(clubVM.Image);

                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        Street = clubVM.Address.Street,
                        State = clubVM.Address.State,
                        City = clubVM.Address.City,
                    }

                };
                _clubRepository.Add(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("","Photo upload failed");
            }

            return View(clubVM);
        }
    }
}
