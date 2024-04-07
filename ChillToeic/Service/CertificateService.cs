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

	}
}
