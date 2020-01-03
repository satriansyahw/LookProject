using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LookDAL.Mem.IFace
{
    interface IDtMember
    {
        Task<List<BufferClass>> SearchMemberAllInfoByMemberId(int id);
    }
}
