using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebPortal.Controllers.CustomAuthorizeAttributes;
using WebPortal.DbStuff.Models;
using WebPortal.DbStuff.Models.Tourism;
using WebPortal.DbStuff.Repositories;
using WebPortal.DbStuff.Repositories.Interfaces;
using WebPortal.Enum;
using WebPortal.Models.Tourism;
using WebPortal.Services;
using WebPortal.Services.Apis;
using WebPortal.Services.Permissions;
using static System.Net.Mime.MediaTypeNames;


namespace WebPortal.Controllers
{
    [Authorize]
    public class TourismController : Controller
    {
        private INextArticlePreviewRepository _tourPreviewRepository;
        private IToursRepository _toursRepository;
        private IUserRepositrory _userRepositrory;
        private IAuthService _authService;
        private ITourPermission _tourPermission;
        private ITourismFilesService _tourismFilesService;
        private WikiPageApi _wikiPageApi;
        private IShopCartRepository _shopCartRepository;
        private IArticlePermission _articlePermission;
        protected IWebHostEnvironment _webHostEnvironment;

        public TourismController(INextArticlePreviewRepository tourPreviewRepository,
            IToursRepository toursRepository,
            IUserRepositrory userRepositrory,
            IAuthService authService,
            ITourPermission tourPermission,
            ITourismFilesService tourismFilesService,
            WikiPageApi wikiPage,
            IWebHostEnvironment webHostEnvironment,
            IShopCartRepository shopCartRepository,
            IArticlePermission articlePermission)
        {
            _tourPreviewRepository = tourPreviewRepository;
            _toursRepository = toursRepository;
            _userRepositrory = userRepositrory;
            _authService = authService;
            _tourPermission = tourPermission;
            _tourismFilesService = tourismFilesService;
            _wikiPageApi = wikiPage;
            _webHostEnvironment = webHostEnvironment;
            _shopCartRepository = shopCartRepository;
            _articlePermission = articlePermission;
        }
        #region Main page

        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchingPlace)
        {
            var viewModel = new PersonalHomeViewModel();

            if (_authService.IsAuthenticated())
            {
                var name = _authService.GetUser().UserName;
                viewModel.Name = name;
            }
            else
            {
                viewModel.Name = "Guest";
            }
            var titleNames = _tourPreviewRepository
                .GetArticleList()
                .Select(dbData => new NextArticleViewModel
                {
                    Title = dbData.TourName,
                    URL = dbData.TourImgUrl,
                    Days = dbData.DaysToPrepareTour,
                    Id = dbData.Id,
                    CanDelete = _articlePermission.CanDelete(dbData)
                }).
                ToList();

            viewModel.NextArticlePreview = titleNames;
            viewModel.SearchingPlace = searchingPlace;

            if (!string.IsNullOrEmpty(searchingPlace))
            {
                var info = await _wikiPageApi.GetWikiPageAboutYourPlace(searchingPlace);

                if (info != null)
                {
                    viewModel.WikiPageAboutPlace = new WikiPageAboutPlaceViewModel
                    {
                        Title = info.Title,
                        Description = info.Description,
                        Url = info.ContentUrls.Desktop.Page
                    };
                }
                else
                {
                    viewModel.WikiPageAboutPlace = new WikiPageAboutPlaceViewModel
                    {
                        Title = "Nothing Found",
                        Description = $"We could not find information about {searchingPlace}",
                        Url = "/Tourism/NotFound"
                    };
                }
            }

            return View(viewModel);
        }

        [HttpGet]
        [Role(Role.Admin)]
        public IActionResult AddContent()
        {
            return View();
        }


        [HttpPost]
        [Role(Role.Admin)]
        public IActionResult AddContent(NextArticleViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var tourismBd = new NextArticlePreview()
            {
                TourName = viewModel.Title,
                DaysToPrepareTour = (int)viewModel.Days,
                TourImgUrl = viewModel.URL
            };
            _tourPreviewRepository.Add(tourismBd);
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            _tourPreviewRepository.Remove(id);
            return RedirectToAction("Index");
        }
        #endregion
        #region Shop

        [AllowAnonymous]
        public IActionResult Shop()
        {
            var viewModel = InitShopView();
            return View (viewModel);
        }

        private PersonalHomeViewModel InitShopView(PersonalHomeViewModel? model = null)
        {
            var viewModel = model?? new PersonalHomeViewModel();
            if (_authService.IsAuthenticated())
            {
                var id = _authService.GetId();
                var name = _authService.GetUser().UserName;

                viewModel.Id = id;
                viewModel.Name = name;
            }
            else
            {
                viewModel.Id = 0;
                viewModel.Name = "Guest";
            }

            var currentUserId = _authService.IsAuthenticated()
                ? _authService.GetId()
                : -1;
            var tourItems = _toursRepository
                .GetShopItemWithAuthor()
                .Select(dbData => new TourViewModel
                {
                    Id = dbData.Id,
                    TourImg = dbData.TourImgUrl,
                    TourName = dbData.TourName,
                    AuthorName = dbData.Author?.UserName ?? "NoAuthor",
                    CanDelete = _tourPermission.CanDelete(dbData),
                    Description = dbData.Description ?? "This Title does not have Description",
                    Price = dbData?.Price ?? 0
                }).
                ToList();

            viewModel.Tours = tourItems;

            return viewModel;
        }
        [HttpGet]
        [Role(Role.Admin)]
        public IActionResult AddShopItem()
        {
            var viewModel = InitAdminShopView();
            return View(viewModel);
        }

        [HttpPost]
        [Role(Role.Admin)]
        public IActionResult AddShopItem(TourCreationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                viewModel = InitAdminShopView();
                return View(viewModel);
            }
            var authorId = viewModel.AuthorId;
            var author = _userRepositrory.GetFirstById(authorId);

            var ImagePath = "";
            if (viewModel.TourImgFile != null)
            {
                ImagePath = _tourismFilesService.UploadImage(viewModel.TourImgFile);
            }
            else if (!string.IsNullOrEmpty(viewModel.TourImgUrl)) 
            {
                ImagePath = viewModel.TourImgUrl;
            }
            else 
            {
                ImagePath = "/images/tourism/grodno.jpg";
            }

            var tourismShopBd = new Tours()
            {
                TourName = viewModel.TourName,
                TourImgUrl = ImagePath,
                Author = author,
                CreatedDate = viewModel.DateTime,
                Description = viewModel.Description,
                Price = viewModel.Price
            };

            _toursRepository.Add(tourismShopBd);
            return RedirectToAction("Shop");
        }
        public IActionResult RemoveShopItem(int id)
        {
            var userId = _authService.GetId();
            var shopCartItems = _shopCartRepository
                    .GetToursAddedInAllShopCart()
                    .Where(x => x.TourInShopId == id && x.UserId == userId)
                    .ToList();

            foreach (var item in shopCartItems)
            {
                _shopCartRepository.Remove(item.Id);
            }
            _toursRepository.Remove(id);
            return RedirectToAction("Shop");
        }
        private TourCreationViewModel InitAdminShopView(TourCreationViewModel? model = null)
        {
            var users = _userRepositrory.GetAll();
            var viewModel = model ?? new TourCreationViewModel();
            if (_authService.IsAdmin())
            {
                viewModel.AllUsers = users
                   .Select(x => new SelectListItem
                   {
                       Value = x.Id.ToString(),
                       Text = x.UserName,
                   }).ToList();
            }
            else
            {
                viewModel.AuthorName = _authService.GetName();
                viewModel.AuthorId = _authService.GetId();
            }

            viewModel.ToursStatiscticModel = _toursRepository.GetTourAutor();
            return viewModel;
        }
        #endregion
        #region Link Author with Tour in Shop
        [HttpGet]
        [Role(Role.Admin)]
        public IActionResult Link()
        {
            var linkToursViewModel = new LinkTourWithAuthorViewModel();
            linkToursViewModel.AllUsers = _userRepositrory
                .GetAll()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.UserName,
                })
                .ToList();

            linkToursViewModel.AllShopItems = _toursRepository
                .GetAll()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.TourName,
                })
                .ToList();
            return View(linkToursViewModel);
        }

        [HttpPost]
        [Role(Role.Admin)]
        public IActionResult Link(LinkTourWithAuthorViewModel linkTourView)
        {
            var tourShopItem = _toursRepository.GetFirstById(linkTourView.TitleNameId);
            var user = _userRepositrory.GetFirstById(linkTourView.AuthorId);

            tourShopItem.Author = user;
            _toursRepository.Update(tourShopItem);
            return RedirectToAction("Shop");
        }
        #endregion
        #region Link User with Role
        [HttpGet]
        [Role(Role.Admin)]
        public IActionResult LinkRole()
        {
            var linkToursViewModel = new LinkUsersWithRoleViewModel();
            linkToursViewModel.AllUsers = _userRepositrory
                .GetAll()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.UserName,
                })
                .ToList();

            linkToursViewModel.AllRoles = System
                .Enum
                .GetValues<Role>()
                .Select(x => new SelectListItem
                {
                    Value = x.ToString(),
                    Text = x.ToString(),
                })
                .ToList();
            return View(linkToursViewModel);
        }

        [HttpPost]
        [Role(Role.Admin)]
        public IActionResult LinkRole(LinkUsersWithRoleViewModel linkTourView)
        {
            var user = _userRepositrory.GetFirstById(linkTourView.AuthorId);
            user.Role = linkTourView.RoleId;
            _userRepositrory.Update(user);
            return RedirectToAction("Index", "User");
        }
        #endregion
        #region Endpoint for Ajax
        public IActionResult RemoveToursInShop([FromForm] List<int> ids)
        {
            var user = _authService.GetUser();
            var admin = Role.Admin;
            if (user.Role != admin)
            {
                return Json(false);
            }

            foreach (var id in ids)
            {
                var shopCartItems = _shopCartRepository
                    .GetToursAddedInAllShopCart()
                    .Where(x => x.TourInShopId == id)
                    .ToList();

                foreach (var item in shopCartItems)
                {
                    _shopCartRepository.Remove(item.Id);
                }
               
                _toursRepository.Remove(id);

            }

            return Json(true);
        }

        public IActionResult UpdateTourName(int id, string name)
        {
            var user = _authService.GetUser();
            var admin = Role.Admin;
            if (user.Role != admin)
            {
                return Json(false);
            }

            var tour = _toursRepository.GetFirstById(id);
            tour.TourName = name;
            _toursRepository.Update(tour);
            return Json(true);
        }
        public IActionResult GetCountShopCartItems()
        {
            var userId = _authService.GetId();
            var countShopCartItems = _shopCartRepository.GetListAllTorsShopCart(userId).Count();
            return Json(new { countShopCartItems });
        }
        
        public IActionResult AddTourInShopCart(int tourId)
        {
            var userId = _authService.GetId();
            var shopCartBd = new ShopCart
            {
                TourInShopId = tourId,
                UserWhoAddTheTour = _authService.GetUser()
                
            };
            _shopCartRepository.Add(shopCartBd);
           
            var countShopCartItems = _shopCartRepository.GetListAllTorsShopCart(userId).Count();
            return Json(new { countShopCartItems });
        }
        #endregion
        #region ShopCart
        [AllowAnonymous]
            
        public IActionResult ShopCart()
        {
            var viewModel = new ShopCartViewModel();
            if (!_authService.IsAuthenticated())
            {
                viewModel.Id = 0;
                viewModel.UserName = "Guest";              
            }

            var userId = _authService.GetId();
            var name = _authService.GetUser().UserName;

            viewModel.Id = userId;
            viewModel.UserName = name;

            var cartItem = _shopCartRepository
                .GetUserShopCart(userId)
                .Select(dbData => new TourViewModel
                {
                    TourName = dbData.TourInShop.TourName,
                    TourImg = dbData.TourInShop.TourImgUrl,
                    AuthorName = dbData.TourInShop.Author?.UserName??"No Author",
                    Id = dbData.Id,
                    Price = Math.Round(dbData.TourInShop.Price, 2),
                    CountOfAddedUniqueItemInShopCart = _shopCartRepository.GetCountOfUniqueTourAddedInShopCart(dbData.TourInShopId, dbData.UserWhoAddTheTour.Id)
                });
            viewModel.Tours = cartItem.ToList();


            var finalPriceOfAllItemsInShopCart = Math.Round(viewModel.Tours.Sum(x => x.Price??0),2);
            viewModel.SumPriceOfAllItemsInShopCart = finalPriceOfAllItemsInShopCart;

       
            return View(viewModel);
        }
        public IActionResult RemoveShopCartItem(int id)
        {
            _shopCartRepository.Remove(id);
            return RedirectToAction("ShopCart");
        }
        #endregion
        public IActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}
