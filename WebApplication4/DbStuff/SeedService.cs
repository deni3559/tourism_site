using WebPortal.DbStuff.Repositories.Interfaces;
using WebPortal.DbStuff.Models;
using WebPortal.DbStuff.Models.Tourism;
using WebPortal.DbStuff.Repositories.Interfaces;

namespace WebPortal.DbStuff
{
    public class SeedService
    {
        private IUserRepositrory _userRepositrory;
        private IToursRepository _toursRepository;

        public const string ADMIN_NAME = "Admin";

        public SeedService(
            IUserRepositrory userRepositrory,
            IToursRepository toursRepository)
        {
            _userRepositrory = userRepositrory;
            _toursRepository = toursRepository;
        }

        public void Seed()
        {
            FillUser();
            FillTours();
        }

        private void FillUser()
        {
            var admin = _userRepositrory.GetByName(ADMIN_NAME);
            if (admin == null)
            {
                _userRepositrory.Registration(ADMIN_NAME, ADMIN_NAME);
                admin = _userRepositrory.GetByName(ADMIN_NAME);
                admin.Role = Enum.Role.Admin;
                _userRepositrory.Update(admin);
            }
        }

        private void FillTours() 
        {
            if (_toursRepository.Any())
            {
                return ;
            }
            var admin = _userRepositrory.GetByName(ADMIN_NAME);
            var tours = new List<Tours>{
            new Tours
            {
                TourName = "Tour to Vitebsk",
                TourImgUrl = "https://34travel.me/media/posts/5f5633dd2d2dc-vitebsk940.jpg",
                Author = admin,
                Price = 1,
                CreatedDate = DateTime.Now
            },

            new Tours
            {
                TourName = "Tour to Grodno",
                TourImgUrl = "https://34travel.me/media/posts/5f50a0c8afab4-hrodna-pan940.jpg",
                Author = admin,
                Price = 2,
                CreatedDate = DateTime.Now
            }
            };
            _toursRepository.AddRange(tours);
        }
    }
}
