using ChillToeic.Models.Entity;
using ChillToeic.Repository;

namespace ChillToeic.Service
{
	public class CertificateService
	{
		private readonly IRepository<Certificate> _certificateRepository;

		public CertificateService(IRepository<Certificate> certificateRepository)
		{
			_certificateRepository = certificateRepository;
		}

		public IEnumerable<Certificate> GetAllCertificates()
		{
			return _certificateRepository.GetAll();
		}
        public IEnumerable<Certificate> GetCertificateByUserId(int id)
        {
            return _certificateRepository.Find(m => m.UserId == id);
        }
    }
}
