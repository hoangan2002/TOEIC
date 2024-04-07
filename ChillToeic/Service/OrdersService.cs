using ChillToeic.Models.Entity;
using ChillToeic.Repository;

namespace ChillToeic.Service
{
    public class OrdersService
    {

        private readonly IRepository<Order> _OrdersRepository;

        public OrdersService(IRepository<Order> OrdersRepository)
        {
            _OrdersRepository = OrdersRepository;
        }

        public IEnumerable<Order> GetAllCourses()
        {
            return _OrdersRepository.GetAll();
        }

        public Order GetCourseById(int id)
        {
            return _OrdersRepository.GetById(id);
        }

        public void AddCourse(Order Order)
        {
            _OrdersRepository.Add(Order);
        }

        public void UpdateCourse(Order Order)
        {
            _OrdersRepository.Update(Order);
        }

        public void DeleteCourse(int id)
        {
            _OrdersRepository.Delete(id);
        }
    }
}
