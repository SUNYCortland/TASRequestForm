using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data;
using TASRequestForm.Core.Data.Entities;
using TASRequestForm.Core.Data.Repositories;

namespace TASRequestForm.Core.Services.Impl
{
    public class AdministratorService : IAdministratorService
    {
        private readonly IAdministratorRepository _administratorRepository;

        public AdministratorService(IAdministratorRepository administratorRepository)
        {
            _administratorRepository = administratorRepository;
        }

        public bool IsAdministratorByPidm(int Pidm)
        {
            return _administratorRepository.FindBy(x => x.Pidm == Pidm) != null;
        }

        [UnitOfWork]
        public Administrator SaveAdministrator(Administrator Admin)
        {
            return _administratorRepository.SaveOrUpdate(Admin);
        }
    }
}
