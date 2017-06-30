﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Core.Services
{
    public interface IAdministratorService
    {
        bool IsAdministratorByPidm(int Pidm);

        Administrator SaveAdministrator(Administrator Admin);
    }
}
