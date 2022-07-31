﻿using System.Threading.Tasks;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;

namespace EOffice.WebAPI.Interfaces
{
    public interface IHopThuService
    {
        Task<HopThu> Create(HopThu model);
        Task<HopThu> Update(HopThu model);
        Task Delete(string id);
        Task<PagingModel<HopThu>> GetPaging(HopThuParam param);
    }
}